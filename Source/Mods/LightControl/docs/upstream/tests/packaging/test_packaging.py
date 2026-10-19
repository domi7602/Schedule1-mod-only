"""Adversarial packaging tests. Fixtures are not compiled mods or game test evidence."""

import copy
import importlib.util
import json
from pathlib import Path
import struct
import tempfile
import unittest
from unittest import mock
import zipfile

PROJECT = Path(__file__).resolve().parents[2]
SPEC = importlib.util.spec_from_file_location("lightcontrol_package", PROJECT / "tools" / "package.py")
packaging = importlib.util.module_from_spec(SPEC)
SPEC.loader.exec_module(packaging)


def structural_pe_fixture():
    """Only PE/CLR header bytes for the gate fixture; this cannot execute as a mod."""
    data = bytearray(1024)
    data[:2] = b"MZ"
    struct.pack_into("<I", data, 0x3C, 0x80)
    data[0x80:0x84] = b"PE\0\0"
    struct.pack_into("<H", data, 0x86, 1)
    struct.pack_into("<H", data, 0x94, 224)
    optional = 0x98
    struct.pack_into("<H", data, optional, 0x10B)
    struct.pack_into("<II", data, optional + 96 + 14 * 8, 0x2000, 72)
    struct.pack_into("<IIII", data, optional + 224 + 8, 512, 0x2000, 512, 512)
    struct.pack_into("<I", data, 512 + 8, 0x2080)
    data[640:644] = b"BSJB"
    return bytes(data)


class PackageTests(unittest.TestCase):
    def setUp(self):
        artifacts = PROJECT / "artifacts"
        artifacts.mkdir(exist_ok=True)
        self.temporary = tempfile.TemporaryDirectory(prefix="packaging-test-", dir=artifacts)
        self.root = Path(self.temporary.name)
        self.manifest_path = self.root / "manifest.json"

    def tearDown(self):
        self.temporary.cleanup()

    def write(self, relative, data):
        path = self.root / relative
        path.parent.mkdir(parents=True, exist_ok=True)
        path.write_bytes(data.encode("utf-8") if isinstance(data, str) else data)
        return path

    def json_file(self, relative, value):
        return self.write(relative, packaging.encoded_json(value))

    def entry(self, source, archive=None, role="source", **extra):
        return {"source": source, "archive": archive or source, "role": role, "sha256": packaging.digest((self.root / source).read_bytes()), **extra}

    def source_manifest(self):
        self.write("README.md", "Development source only.\n")
        self.write("src/Core.cs", "// A text fixture, no binary.\n")
        return {"schema_version": 1, "kind": "development", "files": [self.entry("README.md"), self.entry("src/Core.cs")]}

    def run_source(self, manifest, name="LightControl-DEVELOPMENT-SOURCE.zip"):
        self.json_file("manifest.json", manifest)
        return packaging.package(self.root, self.manifest_path, self.root / name, "development")

    def runtime_fixture(self):
        # Deliberately isolated synthetic schema data. No real game test is asserted here.
        self.json_file("tools/integration-status.json", {"schema_version": 1, "mod_implemented": True})
        self.write("src/LightControl.Mod/LightControl.Mod.csproj", "<Project />")
        self.write("candidate/LightControl.dll", structural_pe_fixture())
        entries = [self.entry("candidate/LightControl.dll", "Mods/LightControl.dll", "mod")]
        for name in ("README_DE.txt", "CHANGELOG.txt", "LICENSE.txt"):
            self.write("docs/" + name, "Synthetic test fixture only.\n")
            entries.append(self.entry("docs/" + name, name, "documentation"))
        manifest = {"schema_version": 1, "kind": "mod", "files": entries, "dependency_review_complete": True, "dependencies": [{"id": "MelonLoader", "version": "0.7.3", "provision": "loader"}, {"id": "S1API", "version": "3.2.1", "provision": "separate"}]}
        dll_hash = entries[0]["sha256"]
        build = {"schema_version": 1, "status": "succeeded", "artifact_kind": "mod", "build_id": "SYNTHETIC-FIXTURE", "mod_version": "1.0.0", "source_fingerprint": "a" * 64, "fresh_source_snapshot": True, "configuration": "Release", "sdk_version": "6.0.428", "sdk_expected": "6.0.428", "artifacts": [{"path": entries[0]["source"], "sha256": dll_hash, "assembly_name": "LightControl"}]}
        report = {"schema_version": 1, "candidate_id": build["build_id"], "mod_version": "1.0.0", "dll_sha256": dll_hash, "runtime_sha256": {"Mods/LightControl.dll": dll_hash}, "requirements_complete": True, "blockers": [], "compatibility": {"verified": True, "game_version": "synthetic fixture", "game_branch": "synthetic fixture", "unity_version": "synthetic fixture", "melonloader_version": "synthetic fixture", "s1api_version": "synthetic fixture"}, "tests": []}
        for identifier in packaging.TEST_IDS:
            report["tests"].append({"id": identifier, "status": "pending-archive" if identifier == "T18" else "passed", "prerequisites": "Synthetic schema input, not a game test.", "steps": "Exercise validation only.", "expected": "Reject malformed input when this fixture is changed.", "actual": "No game test executed; this is fixture text.", "mod_version": "1.0.0", "dll_sha256": dll_hash, "execution": "target-game", "evidence": "Synthetic fixture only.", "tested_at_utc": "2026-10-18T00:00:00Z"})
        return manifest, build, report

    def validate_runtime(self, manifest, build, report, release=True):
        entries, payload = packaging.load_entries(self.root, manifest, "release" if release else "candidate")
        return packaging.validate_runtime(self.root, manifest, entries, payload, build, report, release)

    def packaged_runtime_fixture(self, open_first_test=False):
        manifest, build, report = self.runtime_fixture()
        mode = "candidate" if open_first_test else "release"
        if open_first_test:
            report["tests"][0]["status"] = "open"
        archive = self.root / ("LightControl-1.0.0-IL2CPP-TEST.zip" if open_first_test else "LightControl-1.0.0-IL2CPP.zip")
        packaging.package(self.root, self.json_file("manifest.json", manifest), archive, mode, self.json_file("build.json", build), self.json_file("acceptance.json", report))
        return archive

    def rewritten_archive(self, original, mutate):
        with zipfile.ZipFile(original) as archive:
            files = {name: archive.read(name) for name in archive.namelist()}
        mutate(files)
        files.pop("SHA256SUMS.txt")
        files["SHA256SUMS.txt"] = "".join(f"{packaging.digest(data)}  {name}\n" for name, data in sorted(files.items())).encode()
        changed = self.root / "rewritten.zip"
        with zipfile.ZipFile(changed, "w") as archive:
            for name, data in files.items():
                archive.writestr(name, data)
        return changed

    def test_source_archive_roundtrip(self):
        manifest = self.source_manifest()
        result = self.run_source(manifest)
        archive = self.root / "LightControl-DEVELOPMENT-SOURCE.zip"
        self.assertEqual(result, packaging.verify_archive(archive))
        with zipfile.ZipFile(archive) as content:
            self.assertEqual(set(content.namelist()), {"README.md", "src/Core.cs", *packaging.GENERATED})
            self.assertNotIn(b"MZ", content.read("README.md"))

    def test_same_source_archive_is_deterministic(self):
        manifest = self.source_manifest()
        first = self.run_source(manifest, "first-DEVELOPMENT.zip")
        second = self.run_source(manifest, "second-DEVELOPMENT.zip")
        self.assertEqual(first["archive_sha256"], second["archive_sha256"])

    def test_source_hash_change_fails_without_zip(self):
        manifest = self.source_manifest()
        self.write("src/Core.cs", "// Changed after manifest capture.")
        with self.assertRaisesRegex(packaging.PackagingError, "SHA-256-Abweichung"):
            self.run_source(manifest)
        self.assertFalse((self.root / "LightControl-DEVELOPMENT-SOURCE.zip").exists())

    def test_path_traversal_is_rejected(self):
        manifest = self.source_manifest()
        for bad in ("../outside.txt", "/absolute.txt", "a/../../outside.txt", "a\\b.txt", "C:/secret.txt", "file.txt:stream", "NUL.txt", "a//b.txt", "trailing./file.txt"):
            with self.subTest(bad=bad), self.assertRaises(packaging.PackagingError):
                changed = copy.deepcopy(manifest)
                changed["files"][0]["archive"] = bad
                self.run_source(changed)

    def test_source_traversal_is_rejected(self):
        manifest = self.source_manifest()
        manifest["files"][0]["source"] = "../outside.txt"
        with self.assertRaisesRegex(packaging.PackagingError, "Pfadtraversal"):
            self.run_source(manifest)

    def test_windows_case_and_parent_collisions_are_rejected(self):
        for names in (("README.md", "readme.md"), ("file", "file/child.txt"), ("one.txt", "SHA256SUMS.txt")):
            manifest = self.source_manifest()
            for item, name in zip(manifest["files"], names):
                item["archive"] = name
            with self.subTest(names=names), self.assertRaises(packaging.PackagingError):
                self.run_source(manifest)

    def test_symlink_source_is_rejected(self):
        manifest = self.source_manifest()
        link = self.root / "linked.md"
        try:
            link.symlink_to(self.root / "README.md")
        except OSError:
            self.skipTest("This platform does not permit local symlinks.")
        manifest["files"][0] = self.entry("linked.md")
        with self.assertRaisesRegex(packaging.PackagingError, "Symbolischer Link"):
            self.run_source(manifest)

    def test_development_rejects_binary_and_artifacts(self):
        for name in ("src/foreign.dll", "artifacts/private.txt", "docs/Latest.log.txt"):
            manifest = self.source_manifest()
            self.write(name, "text still forbidden by its role/path")
            manifest["files"].append(self.entry(name))
            with self.subTest(name=name), self.assertRaises(packaging.PackagingError):
                self.run_source(manifest)

    def test_development_cannot_rename_text_into_installation_paths_or_binaries(self):
        for archive_name in ("Mods/LightControl.dll", "src/Fake.dll", "LightControl/Mods/README.txt", "LightControl/artifacts/private.txt", "LightControl/Latest.log.txt"):
            manifest = self.source_manifest()
            manifest["files"][0]["archive"] = archive_name
            with self.subTest(archive_name=archive_name), self.assertRaises(packaging.PackagingError):
                self.run_source(manifest)

    def test_utf8_decodable_binary_with_nuls_is_rejected(self):
        manifest = self.source_manifest()
        self.write("src/Core.cs", b"MZ\x00\x00UTF-8-decodable binary bytes")
        manifest["files"][1] = self.entry("src/Core.cs")
        with self.assertRaisesRegex(packaging.PackagingError, "NUL-Zeichen"):
            self.run_source(manifest)

    def test_existing_zip_is_not_overwritten(self):
        manifest = self.source_manifest()
        old = self.write("LightControl-DEVELOPMENT-SOURCE.zip", b"previous reviewed archive")
        with self.assertRaisesRegex(packaging.PackagingError, "existiert bereits"):
            self.run_source(manifest)
        self.assertEqual(old.read_bytes(), b"previous reviewed archive")

    def test_racing_partial_file_owned_by_other_process_is_preserved(self):
        manifest = self.source_manifest()
        partial = self.root / "LightControl-DEVELOPMENT-SOURCE.zip.partial"

        def race(path, mode, **kwargs):
            self.assertEqual(mode, "x")
            Path(path).write_bytes(b"other packaging run owns this file")
            raise FileExistsError("simulated concurrent creation")

        with mock.patch.object(packaging.zipfile, "ZipFile", side_effect=race):
            with self.assertRaises(FileExistsError):
                self.run_source(manifest)
        self.assertEqual(partial.read_bytes(), b"other packaging run owns this file")

    def test_current_integration_blocks_runtime_packaging(self):
        manifest, build, report = self.runtime_fixture()
        self.json_file("tools/integration-status.json", {"schema_version": 1, "mod_implemented": False})
        with self.assertRaisesRegex(packaging.PackagingError, "DEVELOPMENT"):
            self.validate_runtime(manifest, build, report)

    def test_core_and_diagnostics_cannot_be_mods(self):
        manifest, build, report = self.runtime_fixture()
        for kind in ("core-only", "diagnostics"):
            build["artifact_kind"] = kind
            with self.subTest(kind=kind), self.assertRaisesRegex(packaging.PackagingError, "keine installierbare"):
                self.validate_runtime(manifest, build, report)

    def test_stale_or_changed_candidate_hash_is_rejected(self):
        manifest, build, report = self.runtime_fixture()
        build["artifacts"][0]["sha256"] = "b" * 64
        with self.assertRaisesRegex(packaging.PackagingError, "geprüften Build gebunden"):
            self.validate_runtime(manifest, build, report)

    def test_native_or_invalid_dll_is_rejected(self):
        manifest, build, report = self.runtime_fixture()
        self.write("candidate/LightControl.dll", b"MZ -- not managed code")
        manifest["files"][0]["sha256"] = packaging.digest((self.root / "candidate/LightControl.dll").read_bytes())
        with self.assertRaisesRegex(packaging.PackagingError, "verwaltete PE"):
            self.validate_runtime(manifest, build, report)

    def test_game_reference_and_unassigned_foreign_dll_are_rejected(self):
        for name in ("UnityEngine.CoreModule.dll", "UnexpectedThirdParty.dll"):
            manifest, build, report = self.runtime_fixture()
            self.write("candidate/" + name, structural_pe_fixture())
            item = self.entry("candidate/" + name, "UserLibs/" + name, "runtime", dependency="undeclared")
            manifest["files"].append(item)
            build["artifacts"].append({"path": item["source"], "sha256": item["sha256"]})
            with self.subTest(name=name), self.assertRaises(packaging.PackagingError):
                self.validate_runtime(manifest, build, report)

    def test_all_eighteen_tests_must_be_present(self):
        manifest, build, report = self.runtime_fixture()
        report["tests"].pop(3)
        with self.assertRaisesRegex(packaging.PackagingError, "T01–T18"):
            self.validate_runtime(manifest, build, report, release=False)

    def test_missing_runtime_test_blocks_release_but_candidate_remains_explicit(self):
        manifest, build, report = self.runtime_fixture()
        report["tests"][0]["status"] = "open"
        with self.assertRaisesRegex(packaging.PackagingError, "T01 ist nicht bestanden"):
            self.validate_runtime(manifest, build, report)
        self.validate_runtime(manifest, build, report, release=False)

    def test_simulation_cannot_pass_runtime_acceptance(self):
        manifest, build, report = self.runtime_fixture()
        report["tests"][11]["execution"] = "core-simulation"
        with self.assertRaisesRegex(packaging.PackagingError, "ersetzen keine Laufzeitabnahme"):
            self.validate_runtime(manifest, build, report, release=False)

    def test_report_is_bound_to_dll_and_t18_stays_external(self):
        manifest, build, report = self.runtime_fixture()
        changed = copy.deepcopy(report)
        changed["dll_sha256"] = "f" * 64
        with self.assertRaisesRegex(packaging.PackagingError, "DLL-Kandidaten"):
            self.validate_runtime(manifest, build, changed)
        report["tests"][-1]["status"] = "passed"
        with self.assertRaisesRegex(packaging.PackagingError, "T18 muss intern ausstehen"):
            self.validate_runtime(manifest, build, report)

    def test_zip_traversal_is_rejected_before_extraction(self):
        archive = self.root / "bad.zip"
        with zipfile.ZipFile(archive, "w") as content:
            content.writestr("../escaped.txt", "must not extract")
        with self.assertRaisesRegex(packaging.PackagingError, "Pfadtraversal"):
            packaging.verify_archive(archive)
        self.assertFalse((self.root.parent / "escaped.txt").exists())

    def test_t18_requires_exact_final_zip_hash(self):
        manifest, build, report = self.runtime_fixture()
        manifest_path = self.json_file("manifest.json", manifest)
        build_path = self.json_file("build-record.json", build)
        report_path = self.json_file("acceptance.json", report)
        archive = self.root / "LightControl-1.0.0-IL2CPP.zip"
        result = packaging.package(self.root, manifest_path, archive, "release", build_path, report_path)
        final_report = {"schema_version": 1, "test_id": "T18", "status": "passed", "archive_sha256": "0" * 64, "dll_sha256": report["dll_sha256"], "mod_version": "1.0.0", "candidate_id": build["build_id"]}
        final_path = self.json_file("T18.json", final_report)
        with self.assertRaisesRegex(packaging.PackagingError, "anderen archive_sha256"):
            packaging.verify_release(archive, final_path)
        self.assertEqual(packaging.digest(archive.read_bytes()), result["archive_sha256"])

    def test_final_verifier_rejects_marker_without_internal_acceptance(self):
        original = self.packaged_runtime_fixture()
        changed = self.rewritten_archive(original, lambda files: files.pop("ACCEPTANCE_REPORT.json"))
        final_path = self.json_file("T18.json", {"status": "passed"})
        with self.assertRaisesRegex(packaging.PackagingError, "Pflichtunterlagen"):
            packaging.verify_release(changed, final_path)

    def test_candidate_marker_cannot_be_promoted_with_open_runtime_tests(self):
        original = self.packaged_runtime_fixture(open_first_test=True)

        def promote(files):
            metadata = json.loads(files["PACKAGE_METADATA.json"])
            metadata["kind"] = "release-candidate"
            files["PACKAGE_METADATA.json"] = packaging.encoded_json(metadata)

        changed = self.rewritten_archive(original, promote)
        final_path = self.json_file("T18.json", {"status": "passed"})
        with self.assertRaisesRegex(packaging.PackagingError, "T01 ist nicht bestanden"):
            packaging.verify_release(changed, final_path)

    def test_external_verifier_rejects_unlisted_foreign_dll_even_with_updated_checksums(self):
        original = self.packaged_runtime_fixture()
        changed = self.rewritten_archive(original, lambda files: files.update({"UserLibs/Unexpected.dll": structural_pe_fixture()}))
        with self.assertRaisesRegex(packaging.PackagingError, "Laufzeit-Allowlist"):
            packaging.verify_archive(changed)


if __name__ == "__main__":
    unittest.main(verbosity=2)
