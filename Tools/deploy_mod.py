#!/usr/bin/env python3
"""Deploy one mod project's file set and record a verifiable receipt.

POSIX port of `Tools/deploy-mod.ps1` for hosts without PowerShell. Windows keeps
using the PowerShell script (see Source/Mods/Directory.Build.targets). Behaviour
mirrors the reference: plan derivation, per-install mutex, fail-closed preflight,
duplicate/symlink rejection, unchanged-file skip, verified staging, backups of
exactly the overwritten files, receipts and best-effort in-process rollback.

Worksheet layout (unchanged):

    Mods/<Mod>.dll                  DLL
    UserData/<Mod>/<Mod>.pdb        debug symbols
    UserData/<Mod>/mod.json         metadata
    Mods/*.png                      icons (project dir and assets/)
    Mods/*.bundle                   asset bundles (assets/)
    Mods/<Mod>/*.glb                meshes (assets/); MelonLoader scans only DLLs

There is no cross-process atomicity: a crash or kill between two copies is not
recovered automatically; the backup directory remains for manual recovery.
"""

from __future__ import annotations

import argparse
import hashlib
import json
import os
import re
import shutil
import subprocess
import sys
import tempfile
import time
import uuid
from dataclasses import dataclass
from datetime import datetime, timezone
from pathlib import Path

try:
    import fcntl
except ImportError:  # pragma: no cover - Windows keeps the PowerShell path
    fcntl = None

MUTEX_PREFIX = "Schedule1Deploy_"
RECORD_DIR_NAME = ".deploy"
TEMP_PREFIX = ".s1deploy-tmp-"
WORKSPACE_ROOT = Path(__file__).resolve().parents[1]
GAME_PROCESS_NAMES = {"schedule i", "schedulei", "schedule i.exe", "schedulei.exe"}
DEFAULT_GAME_DIR = (
    Path("C:/Program Files (x86)/Steam/steamapps/common/Schedule I") if os.name == "nt" else None
)
ROLLBACK_NOTE = "Best-effort in-process rollback only; a crash or kill between writes is not recovered automatically."


class DeployError(Exception):
    """Deployment failure with the exit code the preflight contract uses."""

    def __init__(self, message: str, code: int = 2) -> None:
        super().__init__(message)
        self.code = code


class NoDeploy(Exception):
    """Raised when deployment is disabled for this run."""


def utc_now() -> str:
    return datetime.now(timezone.utc).isoformat(timespec="microseconds").replace("+00:00", "Z")


def sha256_hex(path: Path) -> str:
    digest = hashlib.sha256()
    with path.open("rb") as handle:
        for block in iter(lambda: handle.read(1024 * 1024), b""):
            digest.update(block)
    return digest.hexdigest()


def is_symlink(path: Path) -> bool:
    return path.is_symlink()


@dataclass
class GameInstall:
    game_dir: Path
    mods_dir: Path
    user_data_dir: Path


@dataclass
class PlanEntry:
    kind: str
    source: Path
    destination_relative: str
    destination: Path | None = None
    source_sha256: str | None = None
    destination_sha256: str | None = None
    action: str | None = None
    existed_before: bool = False
    backup_path: Path | None = None
    original_sha256: str | None = None
    state: str = "Pending"

    def receipt_row(self, recompute: bool = False) -> dict[str, object]:
        destination_hash = self.destination_sha256
        read_error: str | None = None
        if recompute:
            destination_hash = None
            assert self.destination is not None
            if self.destination.is_file() and not self.destination.is_symlink():
                try:
                    destination_hash = sha256_hex(self.destination)
                except OSError as error:
                    destination_hash = "unreadable"
                    read_error = str(error)
            elif self.destination.exists():
                destination_hash = "unreadable"
                read_error = "destination exists but is not a regular file"
        row: dict[str, object] = {
            "Kind": self.kind,
            "State": self.state,
            "Action": "Unchanged" if self.action == "Unchanged" else self.state,
            "Source": str(self.source),
            "SourceSha256": self.source_sha256,
            "DestinationRelative": _relative_text(self.destination_relative),
            "Destination": str(self.destination) if self.destination else None,
            "DestinationSha256": destination_hash,
        }
        if read_error is not None:
            row["DestinationReadError"] = read_error
        return row


def _destination_key(value: str) -> str:
    """Folded on case-insensitive targets only (Windows, default macOS APFS)."""
    return value.casefold() if sys.platform == "darwin" else os.path.normcase(value)


def _relative_text(value: str) -> str:
    return value.replace(os.sep, "/") if os.sep != "/" else value


# --- preflight -----------------------------------------------------------


def resolve_game_dir(game_dir: str | None) -> GameInstall:
    candidate = (game_dir or os.environ.get("SCHEDULE1_PATH") or "").strip()
    if not candidate and DEFAULT_GAME_DIR is not None:
        candidate = str(DEFAULT_GAME_DIR)
    if not candidate:
        raise DeployError(
            "GameDir is empty: pass -GameDir or set SCHEDULE1_PATH to the Schedule I install."
        )
    if len(candidate) >= 2 and candidate[0] == candidate[-1] and candidate[0] in "\"'":
        candidate = candidate[1:-1].strip()
    if not candidate:
        raise DeployError("GameDir is empty after trimming.")
    if not os.path.isabs(candidate):
        raise DeployError(f"GameDir must be an absolute path; got '{candidate}'.")
    full = Path(os.path.abspath(candidate))
    if str(full) == full.anchor:
        raise DeployError(f"GameDir must not be a filesystem root ('{full}').")
    workspace = Path(os.path.abspath(WORKSPACE_ROOT))
    full_real = Path(os.path.realpath(full))
    workspace_real = Path(os.path.realpath(workspace))
    if full == workspace or full_real == workspace_real:
        raise DeployError(f"GameDir points at the repository root ('{full}').")
    if workspace in full.parents or workspace_real in full_real.parents:
        raise DeployError(f"GameDir is inside the repository ('{full}'); refusing to deploy into the repo.")
    if not full.is_dir():
        raise DeployError(f"Schedule I install not found at '{full}'.", code=3)

    probe = full / "MelonLoader" / "Il2CppAssemblies" / "Assembly-CSharp.dll"
    if not probe.is_file():
        raise DeployError(f"Not a Schedule I + MelonLoader install: missing probe '{probe}'.", code=3)

    for name in ("Mods", "UserData"):
        target = full / name
        if target.exists() or target.is_symlink():
            if not target.is_dir():
                raise DeployError(f"Deploy target '{target}' exists but is not a directory.", code=3)
            if is_symlink(target):
                raise DeployError(
                    f"Deploy target '{target}' is a reparse point/symlink; refusing an ambiguous deploy target.",
                    code=3,
                )
    return GameInstall(full, full / "Mods", full / "UserData")


def _process_names(comm: str, cmdline: bytes) -> set[str]:
    """Only the image name and argv[0] count; a token inside argv is not the game."""
    names = {comm.strip().casefold()}
    parts = [part for part in cmdline.split(b"\0") if part]
    if parts:
        names.add(os.path.basename(parts[0].decode("utf-8", "replace")).casefold())
    return names


def _scan_processes(proc_root: Path = Path("/proc")) -> list[str]:
    """Fail closed: without readable process data the game is not proven closed."""
    if not proc_root.is_dir():
        raise DeployError("Cannot enumerate processes on this host; refusing to deploy (fail-closed).", code=5)
    try:
        entries = [entry for entry in proc_root.iterdir() if entry.name.isdigit()]
    except OSError as error:
        raise DeployError(f"Unable to enumerate processes; refusing to deploy (fail-closed): {error}", code=5) from error
    if not entries:
        raise DeployError("Unable to enumerate processes; refusing to deploy (fail-closed).", code=5)

    matches: list[str] = []
    readable = 0
    for entry in entries:
        try:
            comm = (entry / "comm").read_text(encoding="utf-8", errors="replace")
            cmdline = (entry / "cmdline").read_bytes()
        except OSError:
            continue
        readable += 1
        if _process_names(comm, cmdline) & GAME_PROCESS_NAMES:
            matches.append(entry.name)
    if readable == 0:
        raise DeployError(
            "Unable to read any process entry; refusing to deploy (fail-closed).", code=5
        )
    return matches


def _ps_rows(selector: str) -> dict[str, str]:
    try:
        result = subprocess.run(["ps", "-axo", selector], capture_output=True, text=True, check=False)
    except OSError as error:
        raise DeployError(f"Unable to enumerate processes; refusing to deploy (fail-closed): {error}", code=5) from error
    if result.returncode != 0 or not result.stdout.strip():
        raise DeployError("Unable to enumerate processes; refusing to deploy (fail-closed).", code=5)
    rows: dict[str, str] = {}
    for line in result.stdout.splitlines():
        match = re.match(r"\s*(\d+)\s+(.*)$", line)
        if match:
            rows[match.group(1)] = match.group(2)
    return rows


def _scan_processes_ps() -> list[str]:
    """Fallback for hosts without /proc (macOS/BSD); still fail-closed."""
    comms = _ps_rows("pid=,comm=")
    arguments = _ps_rows("pid=,args=")
    matches: list[str] = []
    for pid, comm in comms.items():
        argv = (arguments.get(pid) or "").split()
        names = {comm.strip().casefold()}
        if argv:
            names.add(os.path.basename(argv[0]).casefold())
        if names & GAME_PROCESS_NAMES:
            matches.append(pid)
    return matches


def running_game_processes() -> list[str]:
    return _scan_processes() if Path("/proc").is_dir() else _scan_processes_ps()


def preflight(game_dir: str | None, operation: str) -> GameInstall:
    install = resolve_game_dir(game_dir)
    running = running_game_processes()
    if running:
        raise DeployError(
            "Schedule I is running (PID " + ", ".join(running) + "). Close the game before deploying; no files were copied.",
            code=4,
        )
    return install


# --- plan ----------------------------------------------------------------


def _files(folder: Path, suffix: str) -> list[Path]:
    if not folder.is_dir():
        return []
    return sorted(
        (entry for entry in folder.iterdir() if entry.is_file() and entry.name.casefold().endswith(suffix)),
        key=lambda entry: entry.name.casefold(),
    )


def new_plan(
    project_name: str,
    project_dir: str,
    target_path: str,
    pdb_path: str | None,
    assets_dir: str | None,
    mod_json_path: str | None,
) -> list[PlanEntry]:
    if (
        not re.match(r"^[A-Za-z0-9][A-Za-z0-9._-]*$", project_name)
        or ".." in project_name
        or project_name.endswith(".")
        or project_name.endswith(" ")
    ):
        raise DeployError(f"ProjectName '{project_name}' is not a safe folder/file name.")
    if not project_dir or not os.path.isabs(project_dir):
        raise DeployError(f"ProjectDir must be absolute; got '{project_dir}'.")
    if not target_path or not os.path.isabs(target_path):
        raise DeployError(f"TargetPath must be absolute; got '{target_path}'.")

    project_full = Path(os.path.abspath(project_dir))
    if not project_full.is_dir():
        raise DeployError(f"ProjectDir not found: '{project_full}'.")
    dll = Path(os.path.abspath(target_path))
    if not dll.is_file():
        raise DeployError(f"Built DLL not found: '{dll}'.")

    plan: list[PlanEntry] = [PlanEntry("dll", dll, os.path.join("Mods", dll.name))]

    if pdb_path:
        if not os.path.isabs(pdb_path):
            raise DeployError(f"PdbPath must be absolute; got '{pdb_path}'.")
        candidate_pdb = Path(os.path.abspath(pdb_path))
    else:
        candidate_pdb = dll.with_suffix(".pdb")
    if candidate_pdb.is_file():
        plan.append(PlanEntry("pdb", candidate_pdb, os.path.join("UserData", project_name, candidate_pdb.name)))
    elif pdb_path:
        raise DeployError(f"PDB not found: '{candidate_pdb}'.")

    if mod_json_path:
        if not os.path.isabs(mod_json_path):
            raise DeployError(f"ModJsonPath must be absolute; got '{mod_json_path}'.")
        json_path = Path(os.path.abspath(mod_json_path))
        # Absent optional metadata is not an error: MSBuild always passes the
        # conventional path, and Shared has no docs/ folder.
        if json_path.is_file():
            plan.append(PlanEntry("metadata", json_path, os.path.join("UserData", project_name, "mod.json")))

    for icon in _files(project_full, ".png"):
        plan.append(PlanEntry("icon", icon, os.path.join("Mods", icon.name)))

    if assets_dir:
        if not os.path.isabs(assets_dir):
            raise DeployError(f"AssetsDir must be absolute; got '{assets_dir}'.")
        assets = Path(os.path.abspath(assets_dir))
        if assets.is_dir():
            for icon in _files(assets, ".png"):
                plan.append(PlanEntry("icon", icon, os.path.join("Mods", icon.name)))
            for bundle in _files(assets, ".bundle"):
                plan.append(PlanEntry("bundle", bundle, os.path.join("Mods", bundle.name)))
            for glb in _files(assets, ".glb"):
                plan.append(PlanEntry("glb", glb, os.path.join("Mods", project_name, glb.name)))

    destinations: dict[str, list[PlanEntry]] = {}
    for entry in plan:
        destinations.setdefault(_destination_key(entry.destination_relative), []).append(entry)
    duplicates = {key: items for key, items in destinations.items() if len(items) > 1}
    if duplicates:
        details = "\n".join(
            f"  {items[0].destination_relative}: " + ", ".join(str(item.source) for item in items)
            for items in duplicates.values()
        )
        raise DeployError(
            f"Ambiguous duplicate destination(s) in the plan. Refusing to guess which source should win.\n{details}"
        )
    return plan


# --- destinations, mutex, hashing ---------------------------------------


def assert_destination(game_root: Path, destination: Path, create: bool = False) -> None:
    """Validate every component below the game root; optionally create parents."""
    root = Path(os.path.abspath(game_root))
    full = Path(os.path.abspath(destination))
    try:
        relative = full.relative_to(root)
    except ValueError as error:
        raise DeployError(f"Deploy destination escaped the game directory: '{full}'.") from error
    segments = [segment for segment in relative.parts if segment not in ("", ".")]
    if not segments or ".." in segments:
        raise DeployError(f"Deploy destination is not a file under the game directory: '{full}'.")

    current = root
    for index, segment in enumerate(segments):
        current = current / segment
        is_leaf = index == len(segments) - 1
        if not current.exists() and not current.is_symlink():
            if not is_leaf and create:
                current.mkdir()
            continue
        if is_symlink(current):
            raise DeployError(
                f"Deploy target is a reparse point/symlink; refusing an ambiguous write: '{current}'."
            )
        if is_leaf:
            if current.is_dir():
                raise DeployError(f"Deploy destination exists as a directory: '{current}'.")
        elif not current.is_dir():
            raise DeployError(f"Deploy destination parent is a file, not a directory: '{current}'.")


class DeployMutex:
    """Per-install POSIX lock; mirrors the named mutex of deploy-mod.ps1."""

    def __init__(self, install: GameInstall, timeout_seconds: int) -> None:
        # realpath keeps /A/S1 and /a/s1 apart on a case-sensitive host and makes
        # two spellings of the same install share one lock.
        normalized = os.path.realpath(install.game_dir).rstrip(os.sep)
        digest = hashlib.sha256(normalized.encode("utf-8")).hexdigest()[:16]
        base = Path(os.environ.get("XDG_RUNTIME_DIR") or tempfile.gettempdir()) / "s1deploy-locks"
        self.path = base / f"{MUTEX_PREFIX}{digest}.lock"
        self.timeout_seconds = timeout_seconds
        self.handle = None

    def __enter__(self) -> "DeployMutex":
        if fcntl is None:
            raise DeployError("The Python deploy port supports POSIX hosts; use deploy-mod.ps1 on Windows.")
        base = self.path.parent
        base.mkdir(mode=0o700, parents=True, exist_ok=True)
        if base.is_symlink():
            raise DeployError(f"Deploy lock directory is a symlink; refusing: '{base}'.")
        flags = os.O_CREAT | os.O_RDWR | getattr(os, "O_NOFOLLOW", 0)
        try:
            descriptor = os.open(self.path, flags, 0o600)
        except OSError as error:
            raise DeployError(f"Cannot open the deploy lock '{self.path}' safely: {error}") from error
        self.handle = os.fdopen(descriptor, "a+")
        deadline = time.monotonic() + self.timeout_seconds
        while True:
            try:
                fcntl.flock(self.handle.fileno(), fcntl.LOCK_EX | fcntl.LOCK_NB)
                return self
            except BlockingIOError:
                if time.monotonic() >= deadline:
                    self.handle.close()
                    self.handle = None
                    raise DeployError(
                        f"Another deploy of this install still holds '{self.path}' after "
                        f"{self.timeout_seconds}s. Refusing to report success without deploying."
                    ) from None
                time.sleep(0.1)

    def __exit__(self, *exc_info: object) -> None:
        if self.handle is not None:
            try:
                if fcntl is not None:
                    fcntl.flock(self.handle.fileno(), fcntl.LOCK_UN)
            finally:
                self.handle.close()
                self.handle = None


# --- receipts ------------------------------------------------------------


def write_json_atomic(path: Path, payload: object) -> None:
    temporary = path.parent / (".tmp-" + uuid.uuid4().hex + ".json")
    try:
        temporary.write_text(json.dumps(payload, indent=2) + "\n", encoding="utf-8")
        os.replace(temporary, path)
    finally:
        if temporary.exists():
            temporary.unlink(missing_ok=True)


def read_receipt(path: Path | None) -> dict | None:
    if path is None or not path.is_file():
        return None
    try:
        data = json.loads(path.read_text(encoding="utf-8-sig"))
    except (OSError, ValueError):
        return None
    return data if isinstance(data, dict) else None


def previous_receipt(receipts_dir: Path, project_name: str, latest_path: Path) -> dict | None:
    latest = read_receipt(latest_path)
    if latest is not None and str(latest.get("Status")) == "Deployed":
        return latest
    candidates: list[dict] = []
    if receipts_dir.is_dir():
        for file in receipts_dir.glob(f"{project_name}.*.json"):
            if file.name == f"{project_name}.json":
                continue
            candidate = read_receipt(file)
            if candidate is not None and str(candidate.get("Status")) == "Deployed":
                candidates.append(candidate)
    if not candidates:
        return None
    return max(candidates, key=lambda item: str(item.get("DeployedAtUtc") or ""))


def stale_artifacts(game_root: Path, plan: list[PlanEntry], previous: dict | None) -> list[dict]:
    stale: list[dict] = []
    if previous is None:
        return stale
    current = {_destination_key(str(entry.destination)) for entry in plan}
    seen: set[str] = set()
    prior_entries = list(previous.get("Entries") or []) + list(previous.get("Stale") or [])
    for prior in prior_entries:
        if not isinstance(prior, dict):
            continue
        # Prefer the portable relative path: a receipt written on another host
        # (Windows/Proton, dual boot) carries an absolute path that does not
        # exist here, while the relative entry stays valid in this install.
        relative = str(prior.get("DestinationRelative") or "").strip()
        destination_text = str(prior.get("Destination") or "").strip()
        if relative:
            destination_text = os.path.join(game_root, relative.replace("\\", "/"))
        if not destination_text:
            continue
        destination = Path(os.path.abspath(destination_text))
        folded = _destination_key(str(destination))
        if folded in current or folded in seen:
            continue
        seen.add(folded)
        assert_destination(game_root, destination)
        if not destination.is_file():
            continue
        try:
            hash_value: str = sha256_hex(destination)
        except OSError:
            hash_value = "unreadable"
        stale.append(
            {
                "Kind": prior.get("Kind"),
                "DestinationRelative": prior.get("DestinationRelative"),
                "Destination": str(destination),
                "Sha256": hash_value,
                "PreviousAttemptId": previous.get("AttemptId"),
                "PreviousDeployedAtUtc": previous.get("DeployedAtUtc"),
            }
        )
        print(
            f"[deploy-mod] WARNING: stale previously deployed file is not in this plan (left in place): "
            f"'{prior.get('DestinationRelative')}'.",
            file=sys.stderr,
        )
    return stale


def git_state(workspace: Path) -> tuple[str | None, bool | None]:
    try:
        revision = subprocess.run(
            ["git", "-C", str(workspace), "rev-parse", "HEAD"], capture_output=True, text=True, check=False
        )
        status = subprocess.run(
            ["git", "-C", str(workspace), "status", "--porcelain"], capture_output=True, text=True, check=False
        )
    except OSError:
        return None, None
    if revision.returncode != 0:
        return None, None
    dirty = bool(status.stdout.strip()) if status.returncode == 0 else None
    return revision.stdout.strip() or None, dirty


# --- rollback ------------------------------------------------------------


def rollback(entries: list[PlanEntry], game_root: Path, backup_dir: Path | None) -> dict:
    failures: list[dict] = []
    restored = removed = 0
    backup_root = (Path(os.path.abspath(backup_dir)) if backup_dir else None)
    for entry in reversed(entries):
        if entry.state not in ("Attempted", "Copied"):
            if entry.state == "Pending":
                entry.state = "NotAttempted"
            continue
        try:
            assert entry.destination is not None
            assert_destination(game_root, entry.destination)
            if entry.existed_before:
                if entry.backup_path is None or not entry.backup_path.is_file():
                    raise DeployError(
                        f"Backup missing for '{entry.destination_relative}'; cannot restore the previous file."
                    )
                backup_full = Path(os.path.abspath(entry.backup_path))
                if backup_root is None or backup_root not in backup_full.parents:
                    raise DeployError(f"Backup '{backup_full}' is outside this run's backup directory; refusing to restore.")
                assert_destination(game_root, backup_full)
                if sha256_hex(backup_full) != entry.original_sha256:
                    raise DeployError(
                        f"Backup hash mismatch for '{entry.destination_relative}'; refusing to restore."
                    )
                staged = staged_path(entry.destination)
                try:
                    shutil.copy2(backup_full, staged)
                    if sha256_hex(staged) != entry.original_sha256:
                        raise DeployError(f"Restored copy hash mismatch for '{entry.destination_relative}'.")
                    os.replace(staged, entry.destination)
                finally:
                    if staged.exists():
                        staged.unlink(missing_ok=True)
                if sha256_hex(entry.destination) != entry.original_sha256:
                    raise DeployError(f"Restored file hash mismatch for '{entry.destination_relative}'.")
                entry.state = "Restored"
                restored += 1
            else:
                if entry.destination.exists():
                    if entry.destination.is_symlink() or entry.destination.is_dir():
                        raise DeployError(
                            f"A directory or reparse point appeared at '{entry.destination_relative}'; not removing it."
                        )
                    if sha256_hex(entry.destination) != entry.source_sha256:
                        raise DeployError(
                            f"Not removing '{entry.destination_relative}': its content is not the copy this run wrote."
                        )
                    entry.destination.unlink()
                    entry.state = "Removed"
                    removed += 1
                else:
                    entry.state = "NotAttempted"
        except (DeployError, OSError) as error:
            entry.state = "RollbackFailed"
            failures.append({"Path": entry.destination_relative, "Error": str(error)})
    return {"Restored": restored, "Removed": removed, "Failures": failures}


# --- deployment ----------------------------------------------------------


def staged_path(destination: Path) -> Path:
    return destination.parent / (TEMP_PREFIX + uuid.uuid4().hex + "-" + destination.name)


def invoke(
    game_dir: str | None,
    project_name: str,
    project_dir: str,
    target_path: str,
    pdb_path: str | None,
    assets_dir: str | None,
    mod_json_path: str | None,
    configuration: str,
    mutex_timeout_seconds: int,
    no_deploy: bool,
) -> dict:
    if no_deploy or str(os.environ.get("S1NoDeploy", "")).strip().casefold() == "true":
        print("[deploy-mod] NoDeploy active - nothing copied.")
        return {"Status": "NoDeploy", "Project": project_name, "Copied": 0, "Unchanged": 0}
    if not project_name:
        raise DeployError("ProjectName is required.")
    if not target_path:
        raise DeployError("TargetPath is required.")

    install = resolve_game_dir(game_dir)
    with DeployMutex(install, mutex_timeout_seconds):
        # Authoritative preflight: after the lock (the game may have started
        # while waiting) and before any write.
        install = preflight(game_dir, project_name)
        game_root = install.game_dir

        plan = new_plan(project_name, project_dir, target_path, pdb_path, assets_dir, mod_json_path)
        for entry in plan:
            entry.destination = Path(os.path.abspath(os.path.join(game_root, entry.destination_relative)))
            entry.destination_relative = _relative_text(entry.destination_relative)
        for entry in plan:
            assert entry.destination is not None
            assert_destination(game_root, entry.destination)

        changed: list[PlanEntry] = []
        for entry in plan:
            destination = entry.destination
            assert destination is not None
            if not entry.source.is_file():
                raise DeployError(f"Plan source disappeared before deploy: '{entry.source}'.")
            entry.source_sha256 = sha256_hex(entry.source)
            if destination.is_file() and not destination.is_symlink():
                entry.existed_before = True
                entry.original_sha256 = sha256_hex(destination)
                if entry.original_sha256 == entry.source_sha256:
                    entry.action = "Unchanged"
                    entry.destination_sha256 = entry.original_sha256
                    entry.state = "NotAttempted"
                    continue
            entry.action = "Copied"
            changed.append(entry)

        records_root = install.user_data_dir / RECORD_DIR_NAME
        receipts_dir = records_root / "receipts"
        receipt_path = receipts_dir / f"{project_name}.json"
        now = datetime.now(timezone.utc)
        stamp = now.strftime("%Y%m%dT%H%M%S") + f"{now.microsecond // 1000:03d}Z"
        attempt_id = f"{stamp}-{uuid.uuid4().hex[:12]}"
        attempt_receipt_path = receipts_dir / f"{project_name}.{attempt_id}.json"

        previous = previous_receipt(receipts_dir, project_name, receipt_path)
        stale = stale_artifacts(game_root, plan, previous)

        # The receipt directory is part of the deployment record, not a mod
        # file: create it under the same reparse/safety checks before writing.
        assert_destination(game_root, receipt_path, create=True)
        assert_destination(game_root, attempt_receipt_path, create=True)

        overwritten = [entry for entry in changed if entry.existed_before]
        backup_id = backup_dir = None
        if overwritten:
            backup_id = f"{stamp}-{project_name}-{uuid.uuid4().hex[:8]}"
            backup_dir = records_root / "backups" / backup_id
            assert_destination(game_root, backup_dir / "manifest.json", create=True)
            manifest_entries: list[dict] = []
            for index, entry in enumerate(overwritten):
                assert entry.destination is not None
                backup_file = backup_dir / f"{index:03d}-{entry.destination.name}"
                shutil.copy2(entry.destination, backup_file)
                if sha256_hex(backup_file) != entry.original_sha256:
                    raise DeployError(
                        f"Backup verification failed for '{entry.destination_relative}'; nothing was overwritten."
                    )
                entry.backup_path = backup_file
                manifest_entries.append(
                    {
                        "DestinationRelative": entry.destination_relative,
                        "Destination": str(entry.destination),
                        "Backup": str(backup_file),
                        "Sha256": entry.original_sha256,
                    }
                )
            write_json_atomic(
                backup_dir / "manifest.json",
                {
                    "Schema": 1,
                    "Project": project_name,
                    "GameDir": str(game_root),
                    "CreatedUtc": utc_now(),
                    "Entries": manifest_entries,
                },
            )

        touched: list[PlanEntry] = []
        deploy_error: DeployError | None = None
        rollback_result: dict = {"Restored": 0, "Removed": 0, "Failures": []}
        try:
            for entry in changed:
                assert entry.destination is not None
                assert_destination(game_root, entry.destination, create=True)
                touched.append(entry)
                entry.state = "Attempted"
                staged = staged_path(entry.destination)
                try:
                    shutil.copy2(entry.source, staged)
                    if sha256_hex(staged) != entry.source_sha256:
                        raise DeployError(
                            f"Staged copy hash mismatch before replacing '{entry.destination}'."
                        )
                    # The backup was verified earlier; a writer may still have
                    # changed the target since. Refuse instead of overwriting it.
                    if entry.existed_before:
                        if not entry.destination.is_file() or sha256_hex(entry.destination) != entry.original_sha256:
                            raise DeployError(
                                f"Destination changed during deploy; refusing to overwrite '{entry.destination}'."
                            )
                    elif entry.destination.exists() or entry.destination.is_symlink():
                        raise DeployError(
                            f"A file appeared at '{entry.destination}' during deploy; refusing to replace it."
                        )
                    os.replace(staged, entry.destination)
                finally:
                    if staged.exists():
                        staged.unlink(missing_ok=True)
                if sha256_hex(entry.destination) != entry.source_sha256:
                    raise DeployError(
                        f"Hash mismatch after copy: '{entry.source}' != '{entry.destination}'."
                    )
                entry.destination_sha256 = entry.source_sha256
                entry.state = "Copied"

            revision, dirty = git_state(WORKSPACE_ROOT)
            receipt = {
                "Schema": 2,
                "Status": "Deployed",
                "Project": project_name,
                "Configuration": configuration,
                "GameDir": str(game_root),
                "AttemptId": attempt_id,
                "DeployedAtUtc": utc_now(),
                "GitRevision": revision,
                "GitDirty": dirty,
                "BackupId": backup_id,
                "BackupDir": str(backup_dir) if backup_dir else None,
                "ReceiptPath": str(receipt_path),
                "AttemptReceiptPath": str(attempt_receipt_path),
                "PreviousReceipt": (
                    {
                        "AttemptId": previous.get("AttemptId"),
                        "DeployedAtUtc": previous.get("DeployedAtUtc"),
                        "Path": str(receipt_path),
                    }
                    if previous
                    else None
                ),
                "Stale": stale,
                "Note": ROLLBACK_NOTE,
                "Entries": [entry.receipt_row() for entry in plan],
                "Rollback": {"Attempted": False, "Restored": 0, "Removed": 0, "Failures": []},
            }
            try:
                write_json_atomic(attempt_receipt_path, receipt)
                write_json_atomic(receipt_path, receipt)
            except OSError as error:
                historical = (
                    f"The previous receipt at '{receipt_path}' (attempt '{previous.get('AttemptId')}' "
                    f"at '{previous.get('DeployedAtUtc')}') is historical and does not describe this run."
                    if previous
                    else "No previous successful receipt describes this install for the project."
                )
                raise DeployError(
                    f"Could not write deploy receipt '{attempt_receipt_path}' / '{receipt_path}': {error} {historical}"
                ) from error
        except (DeployError, OSError) as error:
            deploy_error = error if isinstance(error, DeployError) else DeployError(str(error))
            rollback_result = rollback(touched, game_root, backup_dir)
            for entry in plan:
                if entry.state == "Pending":
                    entry.state = "NotAttempted"
            status = "RolledBack" if not rollback_result["Failures"] else "RollbackFailed"
            failed_receipt = {
                "Schema": 2,
                "Status": status,
                "Project": project_name,
                "Configuration": configuration,
                "GameDir": str(game_root),
                "AttemptId": attempt_id,
                "FailedAtUtc": utc_now(),
                "Error": str(deploy_error),
                "Note": ROLLBACK_NOTE,
                "BackupId": backup_id,
                "BackupDir": str(backup_dir) if backup_dir else None,
                "ReceiptPath": str(receipt_path),
                "AttemptReceiptPath": str(attempt_receipt_path),
                "PreviousReceipt": (
                    {
                        "AttemptId": previous.get("AttemptId"),
                        "DeployedAtUtc": previous.get("DeployedAtUtc"),
                        "Path": str(receipt_path),
                    }
                    if previous
                    else None
                ),
                "Stale": stale,
                "Entries": [entry.receipt_row(recompute=True) for entry in plan],
                "Rollback": {
                    "Attempted": bool(touched),
                    "Restored": rollback_result["Restored"],
                    "Removed": rollback_result["Removed"],
                    "Failures": rollback_result["Failures"],
                },
            }
            try:
                write_json_atomic(attempt_receipt_path, failed_receipt)
                write_json_atomic(receipt_path, failed_receipt)
            except OSError as receipt_error:
                print(
                    f"[deploy-mod] WARNING: could not write failure receipt '{attempt_receipt_path}': {receipt_error}",
                    file=sys.stderr,
                )

        if deploy_error is not None:
            lines = [
                f"[deploy-mod] {project_name} failed: {deploy_error}",
                f"[deploy-mod] Rollback: restored {rollback_result['Restored']}, "
                f"removed {rollback_result['Removed']}, failures {len(rollback_result['Failures'])}.",
            ]
            for failure in rollback_result["Failures"]:
                lines.append(f"  FAILED {failure['Path']}: {failure['Error']}")
            if rollback_result["Failures"]:
                lines.append(
                    "[deploy-mod] Partial rollback: the install is NOT back to its previous state. "
                    "See the receipt and backup directory."
                )
            raise DeployError("\n".join(lines), code=deploy_error.code)

        copied = sum(1 for entry in plan if entry.action == "Copied")
        unchanged = sum(1 for entry in plan if entry.action == "Unchanged")
        print(f"[deploy-mod] {project_name} -> Copied {copied}, Unchanged {unchanged}; receipt {receipt_path}")
        return {
            "Status": "Deployed",
            "Project": project_name,
            "Copied": copied,
            "Unchanged": unchanged,
            "Receipt": str(receipt_path),
            "GameDir": str(game_root),
        }


def main(argv: list[str] | None = None) -> int:
    parser = argparse.ArgumentParser(description="Deploy one mod project into the Schedule I install.")
    parser.add_argument("-GameDir", "--game-dir", dest="game_dir", default=None)
    parser.add_argument("-ProjectName", "--project-name", dest="project_name", required=True)
    parser.add_argument("-ProjectDir", "--project-dir", dest="project_dir", required=True)
    parser.add_argument("-TargetPath", "--target-path", dest="target_path", required=True)
    parser.add_argument("-PdbPath", "--pdb-path", dest="pdb_path", default=None)
    parser.add_argument("-AssetsDir", "--assets-dir", dest="assets_dir", default=None)
    parser.add_argument("-ModJsonPath", "--mod-json-path", dest="mod_json_path", default=None)
    parser.add_argument("-Configuration", "--configuration", dest="configuration", default="Debug")
    parser.add_argument("-MutexTimeoutSeconds", "--mutex-timeout-seconds", dest="mutex_timeout", type=int, default=120)
    parser.add_argument("-NoDeploy", "--no-deploy", dest="no_deploy", action="store_true")
    args = parser.parse_args(argv)
    try:
        invoke(
            args.game_dir,
            args.project_name,
            args.project_dir,
            args.target_path,
            args.pdb_path,
            args.assets_dir,
            args.mod_json_path,
            args.configuration,
            args.mutex_timeout,
            args.no_deploy,
        )
    except DeployError as error:
        print(f"[deploy-mod] ERROR: {error}", file=sys.stderr)
        return error.code
    except OSError as error:
        print(f"[deploy-mod] ERROR: {error}", file=sys.stderr)
        return 2
    except Exception as error:  # contract: a reported failure, never a traceback
        print(f"[deploy-mod] ERROR: {error}", file=sys.stderr)
        return 2
    return 0


if __name__ == "__main__":
    sys.exit(main())
