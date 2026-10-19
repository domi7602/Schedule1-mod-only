#!/usr/bin/env python3
"""Explicit, hash-bound packaging. This program never invokes a compiler."""

from __future__ import annotations

import argparse
import hashlib
import json
import re
import stat
import struct
import sys
import tempfile
import unicodedata
import zipfile
from pathlib import Path, PurePosixPath

SCHEMA = 1
TEST_IDS = tuple(f"T{i:02}" for i in range(1, 19))
GENERATED = {"SHA256SUMS.txt", "PACKAGE_METADATA.json", "PACKAGE_STATUS.txt", "PACKAGE_MANIFEST.json"}
RUNTIME_GENERATED = {"BUILDINFO.txt", "COMPATIBILITY.txt", "TESTREPORT.txt", "ACCEPTANCE_REPORT.json"}
FORBIDDEN_DLL = re.compile(
    r"^(?:Unity.*|Il2Cpp.*|S1API.*|MelonLoader|Assembly-CSharp.*|GameAssembly|"
    r"UnityPlayer|0Harmony|Mono\.Cecil.*|System(?:\..*)?|mscorlib|Microsoft(?:\..*)?)\.dll$",
    re.I,
)
SOURCE_EXTENSIONS = {
    ".md", ".txt", ".cs", ".csproj", ".sln", ".props", ".targets", ".json",
    ".py", ".ps1", ".cmd", ".bat", ".resx", ".config", ".editorconfig", ".xml",
}
PRIVATE_PARTS = {".git", ".vs", "bin", "obj", "artifacts", "references", "saves", "userdata", "upload", "__pycache__"}
DEVELOPMENT_FORBIDDEN_PARTS = PRIVATE_PARTS | {"mods", "plugins", "melonloader"}


class PackagingError(ValueError):
    pass


def require(condition, message):
    if not condition:
        raise PackagingError(message)


def digest(data: bytes) -> str:
    return hashlib.sha256(data).hexdigest()


def read_json(path: Path):
    try:
        return json.loads(path.read_text(encoding="utf-8-sig"))
    except (OSError, ValueError) as error:
        raise PackagingError(f"JSON kann nicht gelesen werden: {path.name}: {error}") from error


def encoded_json(value) -> bytes:
    return (json.dumps(value, ensure_ascii=False, indent=2, sort_keys=True) + "\n").encode("utf-8")


def safe_name(value: str) -> str:
    require(isinstance(value, str) and value, "Ein Dateipfad fehlt.")
    require(len(value) <= 220, f"Zu langer Archivpfad: {value!r}")
    require("\\" not in value and ":" not in value, f"Kein relativer POSIX-Pfad: {value!r}")
    require(not value.startswith("/") and not value.endswith("/"), f"Ungültiger Dateipfad: {value!r}")
    require(unicodedata.normalize("NFC", value) == value, f"Nicht normalisierter Dateiname: {value!r}")
    parts = value.split("/")
    for part in parts:
        require(part not in {"", ".", ".."}, f"Pfadtraversal oder leerer Pfadteil: {value!r}")
        require(not re.search(r'[\x00-\x1f<>"|?*]', part), f"Unsicherer Windows-Dateiname: {value!r}")
        require(not part.endswith((".", " ")), f"Unsicheres Dateiende: {value!r}")
        require(not re.fullmatch(r"(?i)(CON|PRN|AUX|NUL|COM[1-9]|LPT[1-9])(?:\..*)?", part), f"Reservierter Windows-Name: {value!r}")
    return value


def ensure_distinct(names):
    seen = set()
    for name in names:
        key = safe_name(name).casefold()
        require(key not in seen, f"Doppelter Archivpfad, auch unter Windows: {name}")
        seen.add(key)
    for key in seen:
        parts = key.split("/")
        require(not any("/".join(parts[:i]) in seen for i in range(1, len(parts))), f"Datei-/Ordnerkollision: {key}")


def checked_file(root: Path, relative: str) -> Path:
    relative = safe_name(relative)
    current = root
    for part in relative.split("/"):
        current = current / part
        require(not current.is_symlink(), f"Symbolischer Link wird nicht paketiert: {relative}")
    try:
        resolved = current.resolve(strict=True)
    except OSError as error:
        raise PackagingError(f"Datei fehlt: {relative}") from error
    require(resolved.is_relative_to(root.resolve()), f"Datei außerhalb des Projekts: {relative}")
    require(resolved.is_file(), f"Keine reguläre Datei: {relative}")
    return resolved


def managed_pe(data: bytes) -> bool:
    """Check a PE CLR directory and metadata signature; this is not a game load test."""
    try:
        if data[:2] != b"MZ":
            return False
        pe = struct.unpack_from("<I", data, 0x3C)[0]
        if data[pe:pe + 4] != b"PE\0\0":
            return False
        sections = struct.unpack_from("<H", data, pe + 6)[0]
        optional_size = struct.unpack_from("<H", data, pe + 20)[0]
        optional = pe + 24
        magic = struct.unpack_from("<H", data, optional)[0]
        directory = optional + ({0x10B: 96, 0x20B: 112}[magic])
        if directory + 15 * 8 > optional + optional_size:
            return False
        cli_rva, cli_size = struct.unpack_from("<II", data, directory + 14 * 8)
        if cli_size < 72:
            return False

        def offset(rva):
            for i in range(sections):
                section = optional + optional_size + 40 * i
                _, address, raw_size, raw = struct.unpack_from("<IIII", data, section + 8)
                if address <= rva < address + raw_size:
                    return raw + rva - address
            raise ValueError("Unmapped RVA")

        metadata_rva = struct.unpack_from("<I", data, offset(cli_rva) + 8)[0]
        meta = offset(metadata_rva)
        return data[meta:meta + 4] == b"BSJB"
    except (struct.error, KeyError, ValueError, IndexError):
        return False


def validate_development_item(item, data):
    source = item.get("source", "")
    for selected_path in (source, item.get("archive", "")):
        safe_name(selected_path)
        parts = {p.casefold() for p in PurePosixPath(selected_path).parts}
        require(not (parts & DEVELOPMENT_FORBIDDEN_PARTS), f"Entwicklungsarchiv enthält einen gesperrten Bereich: {selected_path}")
        suffix = PurePosixPath(selected_path).suffix.lower()
        require(suffix in SOURCE_EXTENSIONS or PurePosixPath(selected_path).name in {"LICENSE", ".gitignore", ".editorconfig"}, f"Keine erlaubte Textquelle: {selected_path}")
        require("latest.log" not in selected_path.casefold(), "Sitzungslogs gehören nicht ins Entwicklungsarchiv.")
    require(item.get("role") == "source", f"DEVELOPMENT darf nur Textquellen enthalten: {source}")
    require(b"\x00" not in data, f"Binäre NUL-Zeichen in einer angeblichen Textquelle: {source}")
    try:
        data.decode("utf-8-sig")
    except UnicodeDecodeError as error:
        raise PackagingError(f"Binäre Datei im Quellarchiv: {source}") from error


def validate_runtime_file_policy(item):
    archive = safe_name(item.get("archive", ""))
    filename = PurePosixPath(archive).name
    require(not ({p.casefold() for p in PurePosixPath(archive).parts} & PRIVATE_PARTS), f"Privater oder Entwicklungsbereich im Laufzeitarchiv: {archive}")
    require(not FORBIDDEN_DLL.fullmatch(filename), f"Spiel-, Loader- oder Entwicklungsreferenz ist nicht auszuliefern: {archive}")
    require(PurePosixPath(archive).suffix.lower() not in {".log", ".pdb", ".dmp", ".exe", ".zip", ".ps1", ".cmd", ".py"}, f"Unzulässige Laufzeitdatei: {archive}")
    require(item.get("role") in {"mod", "runtime", "documentation"}, f"Unbestätigte Dateirolle: {archive}")
    if filename.lower().endswith(".dll"):
        require(item.get("role") in {"mod", "runtime"}, f"DLL fälschlich als Dokumentation deklariert: {archive}")


def load_entries(root: Path, manifest: dict, mode: str):
    require(manifest.get("schema_version") == SCHEMA, "Nicht unterstütztes Manifestformat.")
    require(manifest.get("kind") == ("development" if mode == "development" else "mod"), "Manifest und Paketmodus widersprechen sich.")
    entries = manifest.get("files")
    require(isinstance(entries, list) and entries, "Eine ausdrückliche Dateiliste ist erforderlich.")
    require(all(isinstance(item, dict) for item in entries), "Ungültiger Dateilisteneintrag.")
    ensure_distinct([item.get("archive", "") for item in entries] + list(GENERATED | (RUNTIME_GENERATED if mode != "development" else set())))
    payload = {}
    for item in entries:
        source, archive = item.get("source", ""), safe_name(item["archive"])
        expected = item.get("sha256", "")
        require(re.fullmatch(r"[0-9a-f]{64}", expected) is not None, f"Ein konkreter SHA-256 fehlt: {source}")
        data = checked_file(root, source).read_bytes()
        require(digest(data) == expected, f"SHA-256-Abweichung; Kandidat oder Quelle wurde verändert: {source}")
        if mode == "development":
            validate_development_item(item, data)
        else:
            validate_runtime_file_policy(item)
        payload[archive] = data
    return entries, payload


def required_text(obj, key, message):
    value = obj.get(key)
    require(isinstance(value, str) and value.strip(), message)
    return value


def validate_distribution(manifest, entries, payload):
    require(manifest.get("schema_version") == SCHEMA and manifest.get("kind") == "mod", "Ungültiges Laufzeitmanifest.")
    require(isinstance(entries, list) and entries, "Die Laufzeit-Allowlist fehlt.")
    ensure_distinct([item.get("archive", "") for item in entries] + list(GENERATED | RUNTIME_GENERATED))
    require({item["archive"] for item in entries} == set(payload) - GENERATED - RUNTIME_GENERATED, "Archivinhalt widerspricht der Laufzeit-Allowlist.")
    for item in entries:
        validate_runtime_file_policy(item)
        require(item.get("sha256") == digest(payload[item["archive"]]), f"Laufzeit-Manifesthash weicht ab: {item['archive']}")
    mod_files = [item for item in entries if item.get("role") == "mod"]
    require(len(mod_files) == 1 and mod_files[0]["archive"] == "Mods/LightControl.dll", "Genau Mods/LightControl.dll muss als eigene Mod ausgewiesen sein.")
    require(managed_pe(payload["Mods/LightControl.dll"]), "LightControl.dll ist keine nachgewiesene verwaltete PE-Datei.")
    require({"README_DE.txt", "CHANGELOG.txt", "LICENSE.txt"}.issubset(payload), "Installations-, Änderungs- oder Lizenztext fehlt.")
    require(manifest.get("dependency_review_complete") is True, "Direkte und indirekte Laufzeitabhängigkeiten sind nicht abschließend zugeordnet.")
    dependencies = manifest.get("dependencies", [])
    require(isinstance(dependencies, list) and dependencies, "Die Abhängigkeitszuordnung fehlt.")
    bundled = {}
    dependency_ids = set()
    for dependency in dependencies:
        identity = required_text(dependency, "id", "Abhängigkeitskennung fehlt.")
        require(identity not in dependency_ids, f"Doppelte Abhängigkeit: {identity}")
        dependency_ids.add(identity)
        require(dependency.get("provision") in {"game", "loader", "separate", "bundled"}, f"Unklarer Bereitstellungsweg: {identity}")
        required_text(dependency, "version", f"Version fehlt: {identity}")
        if dependency["provision"] == "bundled":
            required_text(dependency, "origin", f"Herkunft fehlt: {identity}")
            required_text(dependency, "license", f"Lizenzbeleg fehlt: {identity}")
            paths = dependency.get("archives")
            require(isinstance(paths, list) and paths, f"Dateiliste der Abhängigkeit fehlt: {identity}")
            for path in paths:
                path = safe_name(path)
                require(path not in bundled, f"Doppelte Zuständigkeit für Laufzeitdatei: {path}")
                bundled[path] = identity
    actual_runtime = {item["archive"]: item.get("dependency") for item in entries if item.get("role") == "runtime"}
    require(actual_runtime == bundled, "Nicht zugeordnete Fremd-DLL/Laufzeitdatei oder fehlende gebündelte Abhängigkeit.")
    return {item["archive"]: item["sha256"] for item in entries if item.get("role") in {"mod", "runtime"}}


def validate_runtime(root, manifest, entries, payload, build, report, release):
    integration = read_json(checked_file(root, "tools/integration-status.json"))
    require(integration.get("mod_implemented") is True, "Aktueller Stand ist DEVELOPMENT: die produktive Mod-Integration ist nicht freigegeben.")
    checked_file(root, "src/LightControl.Mod/LightControl.Mod.csproj")
    require(build.get("schema_version") == SCHEMA and build.get("status") == "succeeded", "Kein erfolgreicher frischer Buildbeleg.")
    require(build.get("artifact_kind") == "mod", "Core-/Diagnoseassemblies sind keine installierbare LightControl-Mod.")
    require(build.get("fresh_source_snapshot") is True and build.get("configuration") == "Release", "Kein frischer Release-Build aus isoliertem Quellstand belegt.")
    require(build.get("sdk_version") == build.get("sdk_expected") and bool(build.get("sdk_version")), "Das konkret verwendete SDK ist nicht bestätigt.")
    candidate_id = required_text(build, "build_id", "Die Buildkennung fehlt.")
    version = required_text(build, "mod_version", "Die Mod-Version fehlt.")
    require(re.fullmatch(r"[0-9]+\.[0-9]+\.[0-9]+(?:-[A-Za-z0-9.]+)?", version) is not None, "Ungültige Mod-Version.")
    require(re.fullmatch(r"[0-9a-f]{64}", build.get("source_fingerprint", "")) is not None, "Quellstand nicht erfasst.")
    runtime_hashes = validate_distribution(manifest, entries, payload)
    dll_hash = runtime_hashes["Mods/LightControl.dll"]
    built_files = {item.get("path"): item for item in build.get("artifacts", [])}
    for item in entries:
        if item.get("role") in {"mod", "runtime"}:
            built = built_files.get(item["source"], {})
            require(built.get("sha256") == item["sha256"], f"Laufzeitdatei nicht an den geprüften Build gebunden: {item['archive']}")
            if item["role"] == "mod":
                require(built.get("assembly_name") == "LightControl", "Assemblyidentität stimmt nicht mit der produktiven Mod überein.")
    validate_acceptance(report, candidate_id, version, dll_hash, runtime_hashes, release)
    return candidate_id, version, dll_hash


def validate_acceptance(report, candidate_id, version, dll_hash, runtime_hashes, release):
    require(report.get("schema_version") == SCHEMA, "Ungültiges Abnahmeformat.")
    require(report.get("candidate_id") == candidate_id and report.get("mod_version") == version and report.get("dll_sha256") == dll_hash, "Abnahmebericht gehört nicht exakt zu diesem DLL-Kandidaten.")
    require(report.get("runtime_sha256") == runtime_hashes, "Der geprüfte Satz aller Laufzeitdateien stimmt nicht mit dem Archiv überein.")
    tests = report.get("tests", [])
    require(isinstance(tests, list) and len(tests) == 18, "Alle Pflichtprüfungen T01–T18 müssen ausdrücklich erfasst sein.")
    require({test.get("id") for test in tests} == set(TEST_IDS), "Pflichtprüfungen fehlen oder sind doppelt.")
    for test in tests:
        identifier = test["id"]
        require(test.get("status") in {"open", "blocked", "passed", "failed", "pending-archive"}, f"Unbekannter Prüfstatus: {identifier}")
        for key in ("prerequisites", "steps", "expected", "actual"):
            required_text(test, key, f"Prüfnachweis {key} fehlt: {identifier}")
        require(test.get("mod_version") == version and test.get("dll_sha256") == dll_hash, f"Prüfung nicht an den DLL-Stand gebunden: {identifier}")
        if identifier == "T18":
            require(test["status"] == "pending-archive", "T18 muss intern ausstehen und nach Erzeugung außerhalb der endgültigen ZIP geprüft werden.")
        else:
            require(test["status"] != "pending-archive", f"Nur T18 ist eine ausstehende Archivprüfung: {identifier}")
            if test["status"] == "passed":
                require(test.get("execution") == "target-game", f"Kern-/Simulationstests ersetzen keine Laufzeitabnahme: {identifier}")
                required_text(test, "evidence", f"Konkrete Ergebnisbelege fehlen: {identifier}")
                required_text(test, "tested_at_utc", f"Testzeitpunkt fehlt: {identifier}")
            if release:
                require(test["status"] == "passed", f"Release gesperrt: {identifier} ist nicht bestanden.")
    if release:
        require(report.get("requirements_complete") is True and not report.get("blockers", ["unbekannt"]), "Release gesperrt: Pflichtanforderungen oder Integrationsblocker sind offen.")
        require(report.get("compatibility", {}).get("verified") is True, "Die konkrete Versionsmatrix ist nicht im Zielspiel bestätigt.")
        for key in ("game_version", "game_branch", "unity_version", "melonloader_version", "s1api_version"):
            required_text(report["compatibility"], key, f"Konkrete Laufzeitversion fehlt: {key}")


def minimal_acceptance(report):
    kept = {key: report.get(key) for key in ("schema_version", "candidate_id", "mod_version", "dll_sha256", "runtime_sha256", "requirements_complete", "blockers")}
    kept["compatibility"] = {key: value for key, value in report.get("compatibility", {}).items() if key in {"verified", "game_version", "game_branch", "unity_version", "melonloader_version", "s1api_version", "limitations", "notes"}}
    fields = {"id", "status", "prerequisites", "steps", "expected", "actual", "mod_version", "dll_sha256", "execution", "evidence", "tested_at_utc"}
    kept["tests"] = [{key: value for key, value in test.items() if key in fields} for test in report["tests"]]
    return kept


def test_report_text(report):
    lines = [f"Kandidat: {report['candidate_id']}", f"Mod-Version: {report['mod_version']}", f"DLL SHA256: {report['dll_sha256']}", "T18 bleibt hier ausstehend; das externe Ergebnis bindet den finalen ZIP-Hash.", ""]
    lines += [f"Laufzeitdatei: {name}; SHA256: {value}" for name, value in sorted(report["runtime_sha256"].items())] + [""]
    for test in sorted(report["tests"], key=lambda item: item["id"]):
        lines.extend([f"{test['id']}: {test['status']}"] + [f"{key}: {test[key]}" for key in ("prerequisites", "steps", "expected", "actual")] + ([f"evidence: {test['evidence']}"] if test.get("evidence") else []) + [""])
    return ("\n".join(lines) + "\n").encode("utf-8")


def verify_archive(path: Path):
    with zipfile.ZipFile(path) as archive:
        infos = archive.infolist()
        require(len(infos) <= 10000, "Unplausible Anzahl Archiveinträge.")
        ensure_distinct([item.filename for item in infos])
        require(sum(item.file_size for item in infos) <= 512 * 1024 * 1024, "Archiv überschreitet das Größenlimit.")
        require(all(not item.is_dir() and not (item.flag_bits & 1) for item in infos), "Ordner- oder verschlüsselter Archiveintrag.")
        require(all(not stat.S_ISLNK(item.external_attr >> 16) for item in infos), "Symbolischer Link im Archiv.")
        require(archive.testzip() is None, "Die ZIP-Integritätsprüfung ist fehlgeschlagen.")
        files = {item.filename: archive.read(item) for item in infos}
    require(GENERATED.issubset(files), "Paketmetadaten oder Prüfsummen fehlen.")
    checks = {}
    for line in files["SHA256SUMS.txt"].decode("utf-8").splitlines():
        match = re.fullmatch(r"([0-9a-f]{64})  (.+)", line)
        require(match is not None, "Ungültige Prüfsummenzeile.")
        value, name = match.groups()
        safe_name(name)
        require(name not in checks and name != "SHA256SUMS.txt", "Doppelte oder zirkuläre Prüfsumme.")
        checks[name] = value
    require(set(checks) == set(files) - {"SHA256SUMS.txt"}, "Inhalt stimmt nicht mit der ausdrücklichen Prüfsummenliste überein.")
    require(all(digest(files[name]) == value for name, value in checks.items()), "Archivinhalt stimmt nicht mit den Prüfsummen überein.")
    metadata = json.loads(files["PACKAGE_METADATA.json"])
    require(metadata.get("schema_version") == SCHEMA, "Unbekannte Paketmetadaten.")
    require(metadata.get("kind") in {"development", "candidate", "release-candidate"}, "Unbekannte Paketart.")
    manifest = json.loads(files["PACKAGE_MANIFEST.json"])
    require(metadata.get("manifest_sha256") == digest(files["PACKAGE_MANIFEST.json"]), "Paketmanifest widerspricht den Metadaten.")
    if metadata.get("kind") in {"candidate", "release-candidate"}:
        require(RUNTIME_GENERATED.issubset(files), "Pflichtunterlagen oder vollständiger interner Abnahmebericht fehlen.")
        runtime_hashes = validate_distribution(manifest, manifest.get("files"), files)
        require(digest(files.get("Mods/LightControl.dll", b"")) == metadata.get("dll_sha256"), "Mod-Hash widerspricht Paketmetadaten.")
        require(metadata.get("acceptance_report_sha256") == digest(files["ACCEPTANCE_REPORT.json"]), "Der Abnahmebericht ist nicht an dieses Paket gebunden.")
        report = json.loads(files["ACCEPTANCE_REPORT.json"])
        validate_acceptance(report, metadata.get("candidate_id"), metadata.get("mod_version"), metadata.get("dll_sha256"), runtime_hashes, metadata["kind"] == "release-candidate")
        require(files["TESTREPORT.txt"] == test_report_text(report), "Lesbarer Testbericht widerspricht den geprüften Abnahmedaten.")
        require(files["COMPATIBILITY.txt"] == encoded_json(report.get("compatibility", {})), "Versionsübersicht widerspricht den geprüften Abnahmedaten.")
    else:
        require(manifest.get("schema_version") == SCHEMA and manifest.get("kind") == "development", "Ungültiges Entwicklungsmanifest im Archiv.")
        entries = manifest.get("files")
        require(isinstance(entries, list) and entries, "Die ursprüngliche Quell-Allowlist fehlt.")
        ensure_distinct([item.get("archive", "") for item in entries] + list(GENERATED))
        require({item["archive"] for item in entries} == set(files) - GENERATED, "Entwicklungsarchiv widerspricht der ursprünglichen Dateiliste.")
        for item in entries:
            data = files[item["archive"]]
            validate_development_item(item, data)
            require(item.get("sha256") == digest(data), "Quelle weicht vom archivierten Manifest ab.")
    with tempfile.TemporaryDirectory(prefix="lightcontrol-verify-", dir=path.parent) as target:
        directory = Path(target)
        for name, data in files.items():
            destination = directory.joinpath(*PurePosixPath(name).parts)
            destination.parent.mkdir(parents=True, exist_ok=True)
            destination.write_bytes(data)
            require(digest(destination.read_bytes()) == digest(data), f"Entpackprüfung fehlgeschlagen: {name}")
    return {"archive_sha256": digest(path.read_bytes()), "size_bytes": path.stat().st_size, "files": len(files), "metadata": metadata}


def package(root, manifest_path, output, mode, build_path=None, report_path=None):
    root = root.resolve(strict=True)
    require(not output.exists(), "Ausgabedatei existiert bereits; ein geprüftes Archiv wird nicht überschrieben.")
    manifest = read_json(manifest_path)
    entries, payload = load_entries(root, manifest, mode)
    metadata = {"schema_version": SCHEMA, "kind": "development", "status": "source-only; not an installable mod"}
    if mode == "development":
        require("DEVELOPMENT" in output.name and output.suffix == ".zip", "Quellarchive müssen DEVELOPMENT im ZIP-Dateinamen tragen.")
        payload["PACKAGE_STATUS.txt"] = "DEVELOPMENT / QUELLARCHIV\nKeine installierbare Mod; keine Release-DLL; keine behauptete Spielabnahme.\n".encode("utf-8")
        payload["PACKAGE_MANIFEST.json"] = encoded_json(manifest)
    else:
        require(build_path is not None and report_path is not None, "Buildbeleg und Abnahmebericht sind erforderlich.")
        build, report = read_json(build_path), read_json(report_path)
        candidate_id, version, dll_hash = validate_runtime(root, manifest, entries, payload, build, report, mode == "release")
        expected_name = f"LightControl-{version}-IL2CPP" + ("-TEST.zip" if mode == "candidate" else ".zip")
        require(output.name == expected_name, f"Vorgeschriebener Paketname: {expected_name}")
        metadata = {"schema_version": SCHEMA, "kind": "candidate" if mode == "candidate" else "release-candidate", "candidate_id": candidate_id, "mod_version": version, "dll_sha256": dll_hash, "source_fingerprint": build["source_fingerprint"], "t18_status": "pending-archive"}
        payload["PACKAGE_STATUS.txt"] = ("TESTPAKET: Nicht alle Spielprüfungen sind bestanden.\n" if mode == "candidate" else "RELEASEKANDIDAT: T01–T17 bestanden; T18 ist extern an genau dieser endgültigen ZIP zu prüfen.\n").encode("utf-8")
        payload["BUILDINFO.txt"] = (f"Mod-Version: {version}\nKandidat: {candidate_id}\nPlattform: Windows IL2CPP\nKonfiguration: {build.get('configuration', 'unknown')}\nSDK: {build.get('sdk_version', 'unknown')}\nQuellstand SHA256: {build['source_fingerprint']}\nDLL SHA256: {dll_hash}\nPaketierung ohne Neubuild. Byteidentischer Wiederholungsbuild nicht zugesichert.\n").encode("utf-8")
        report = minimal_acceptance(report)
        archived_manifest = {
            "schema_version": SCHEMA,
            "kind": "mod",
            "dependency_review_complete": manifest["dependency_review_complete"],
            "files": [{key: value for key, value in item.items() if key in {"archive", "role", "sha256", "dependency"}} for item in entries],
            "dependencies": [{key: value for key, value in dependency.items() if key in {"id", "version", "provision", "origin", "license", "archives"}} for dependency in manifest["dependencies"]],
        }
        payload["PACKAGE_MANIFEST.json"] = encoded_json(archived_manifest)
        payload["COMPATIBILITY.txt"] = encoded_json(report.get("compatibility", {}))
        payload["ACCEPTANCE_REPORT.json"] = encoded_json(report)
        payload["TESTREPORT.txt"] = test_report_text(report)
        metadata["acceptance_report_sha256"] = digest(payload["ACCEPTANCE_REPORT.json"])
    metadata["manifest_sha256"] = digest(payload["PACKAGE_MANIFEST.json"])
    payload["PACKAGE_METADATA.json"] = encoded_json(metadata)
    payload["SHA256SUMS.txt"] = "".join(f"{digest(data)}  {name}\n" for name, data in sorted(payload.items())).encode("utf-8")
    ensure_distinct(payload)
    output.parent.mkdir(parents=True, exist_ok=True)
    temporary = output.with_name(output.name + ".partial")
    require(not temporary.exists(), "Temporäres Archiv existiert; zuerst den vorigen Fehler prüfen.")
    temporary_owned = False
    try:
        with zipfile.ZipFile(temporary, "x", compression=zipfile.ZIP_DEFLATED, compresslevel=9) as archive:
            temporary_owned = True
            for name, data in sorted(payload.items()):
                info = zipfile.ZipInfo(name, date_time=(2026, 1, 1, 0, 0, 0))
                info.compress_type = zipfile.ZIP_DEFLATED
                info.external_attr = (stat.S_IFREG | 0o644) << 16
                archive.writestr(info, data)
        result = verify_archive(temporary)
        created = False
        try:
            with output.open("xb") as final:
                created = True
                final.write(temporary.read_bytes())
            require(digest(output.read_bytes()) == result["archive_sha256"], "Das endgültige Archiv weicht nach dem Schreiben ab.")
            return result
        except BaseException:
            if created:
                output.unlink(missing_ok=True)
            raise
    finally:
        if temporary_owned:
            temporary.unlink(missing_ok=True)


def verify_release(archive_path, report_path):
    result = verify_archive(archive_path)
    metadata, report = result["metadata"], read_json(report_path)
    require(metadata.get("kind") == "release-candidate", "Nur ein Paket mit bestandenem T01–T17-Gate kann abschließend freigegeben werden.")
    require(report.get("schema_version") == SCHEMA and report.get("test_id") == "T18" and report.get("status") == "passed", "T18 ist nicht als bestanden dokumentiert.")
    for key, expected in {"archive_sha256": result["archive_sha256"], "dll_sha256": metadata["dll_sha256"], "mod_version": metadata["mod_version"], "candidate_id": metadata["candidate_id"]}.items():
        require(report.get(key) == expected, f"Externer T18-Bericht gehört zu einem anderen {key}.")
    require(report.get("execution") == "clean-target-game-installation", "T18 muss in einer sauberen Zielinstallation aus genau der ZIP ausgeführt sein.")
    for key in ("prerequisites", "steps", "expected", "actual", "evidence", "tested_at_utc"):
        required_text(report, key, f"T18-Nachweis fehlt: {key}")
    require(report.get("tested_checks") == {name: "passed" for name in ("first_start", "version_and_hash", "interaction_phone_save_reload", "multiplayer", "upgrade", "data_format_handling", "uninstall")}, "T18 deckt nicht alle vorgeschriebenen Installationsabläufe ab.")
    result["release_gate"] = "passed from supplied runtime evidence; archive unchanged"
    return result


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    commands = parser.add_subparsers(dest="command", required=True)
    for mode in ("development", "candidate", "release"):
        command = commands.add_parser(mode)
        command.add_argument("--root", type=Path, required=True)
        command.add_argument("--manifest", type=Path, required=True)
        command.add_argument("--output", type=Path, required=True)
        if mode != "development":
            command.add_argument("--build-record", type=Path, required=True)
            command.add_argument("--acceptance-report", type=Path, required=True)
    command = commands.add_parser("verify")
    command.add_argument("--archive", type=Path, required=True)
    command = commands.add_parser("verify-release")
    command.add_argument("--archive", type=Path, required=True)
    command.add_argument("--t18-report", type=Path, required=True)
    args = parser.parse_args()
    try:
        if args.command == "verify":
            result = verify_archive(args.archive)
        elif args.command == "verify-release":
            result = verify_release(args.archive, args.t18_report)
        else:
            result = package(args.root, args.manifest, args.output, args.command, getattr(args, "build_record", None), getattr(args, "acceptance_report", None))
        print(json.dumps(result, ensure_ascii=False, indent=2))
        return 0
    except (PackagingError, OSError, KeyError, TypeError, ValueError, zipfile.BadZipFile) as error:
        print(f"PAKETIERUNG GESPERRT: {error}", file=sys.stderr)
        return 2


if __name__ == "__main__":
    sys.exit(main())
