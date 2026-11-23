"""Regression tests for the POSIX deploy port (Tools/deploy_mod.py).

Run locally on Linux/macOS:

    python3 -m unittest discover -s Tools/Tests -p 'test_deploy_mod.py' -v

The fixture never touches a real install: every test builds a throwaway game
root with the MelonLoader probe the preflight requires.
"""

import json
import os
import sys
import tempfile
import unittest
from pathlib import Path
from unittest import mock

TOOLS = Path(__file__).resolve().parents[1]
sys.path.insert(0, str(TOOLS))

import deploy_mod  # noqa: E402


class DeployModTests(unittest.TestCase):
    def setUp(self):
        self.temp = tempfile.TemporaryDirectory(prefix="s1deploy-")
        self.base = Path(self.temp.name)
        self.game = self.base / "Schedule I"
        (self.game / "MelonLoader" / "Il2CppAssemblies").mkdir(parents=True)
        (self.game / "MelonLoader" / "Il2CppAssemblies" / "Assembly-CSharp.dll").write_bytes(b"asm")

        self.project = self.base / "src"
        self.project.mkdir()
        self.dll = self.project / "Demo.dll"
        self.dll.write_bytes(b"new-dll")
        self.assets = self.base / "assets"
        self.assets.mkdir()
        (self.assets / "icon.png").write_bytes(b"png")
        self.mod_json = self.base / "docs" / "mod.json"
        self.mod_json.parent.mkdir()
        self.mod_json.write_text('{"name":"Demo"}', encoding="utf-8")

        patcher = mock.patch.object(deploy_mod, "running_game_processes", return_value=[])
        self.addCleanup(patcher.stop)
        patcher.start()

    def tearDown(self):
        self.temp.cleanup()

    def deploy(self, no_deploy: bool = False):
        return deploy_mod.invoke(
            game_dir=str(self.game),
            project_name="Demo",
            project_dir=str(self.project),
            target_path=str(self.dll),
            pdb_path=None,
            assets_dir=str(self.assets),
            mod_json_path=str(self.mod_json),
            configuration="Release",
            mutex_timeout_seconds=5,
            no_deploy=no_deploy,
        )

    def receipt(self):
        return json.loads((self.game / "UserData" / ".deploy" / "receipts" / "Demo.json").read_text("utf-8"))

    def test_first_deploy_writes_plan_and_receipt(self):
        result = self.deploy()
        self.assertEqual(result["Status"], "Deployed")
        self.assertTrue((self.game / "Mods" / "Demo.dll").is_file())
        self.assertTrue((self.game / "Mods" / "icon.png").is_file())
        self.assertTrue((self.game / "UserData" / "Demo" / "mod.json").is_file())
        record = self.receipt()
        self.assertEqual(record["Schema"], 2)
        self.assertEqual(record["Status"], "Deployed")
        self.assertEqual(record["Configuration"], "Release")
        self.assertEqual(len(record["Entries"]), 3)
        self.assertTrue(all(entry["Action"] == "Copied" for entry in record["Entries"]))
        self.assertIsNone(record["BackupId"])

    def test_second_deploy_is_idempotent(self):
        self.deploy()
        second = self.deploy()
        self.assertEqual(second["Copied"], 0)
        self.assertEqual(second["Unchanged"], 3)
        backups = self.game / "UserData" / ".deploy" / "backups"
        self.assertFalse(backups.exists() and any(backups.iterdir()))

    def test_changed_file_is_backed_up_and_replaced(self):
        self.deploy()
        self.dll.write_bytes(b"next-build")
        self.deploy()
        record = self.receipt()
        self.assertEqual((self.game / "Mods" / "Demo.dll").read_bytes(), b"next-build")
        self.assertIsNotNone(record["BackupId"])
        backup = Path(record["BackupDir"])
        manifest = json.loads((backup / "manifest.json").read_text("utf-8"))
        self.assertEqual(manifest["Schema"], 1)
        self.assertEqual(len(manifest["Entries"]), 1)
        backup_file = Path(manifest["Entries"][0]["Backup"])
        self.assertEqual(backup_file.read_bytes(), b"new-dll")

    def test_duplicate_destination_is_rejected(self):
        (self.project / "icon.png").write_bytes(b"other")
        with self.assertRaises(deploy_mod.DeployError):
            self.deploy()
        self.assertFalse((self.game / "Mods").exists())

    def test_symlinked_destination_is_rejected(self):
        (self.game / "Mods").mkdir()
        outside = self.base / "outside.dll"
        outside.write_bytes(b"outside")
        (self.game / "Mods" / "Demo.dll").symlink_to(outside)
        with self.assertRaises(deploy_mod.DeployError):
            self.deploy()
        self.assertEqual(outside.read_bytes(), b"outside")
        self.assertFalse((self.game / "UserData" / ".deploy").exists())

    def test_failure_rolls_back_copied_and_new_files(self):
        self.deploy()
        (self.project / "Demo.pdb").write_bytes(b"pdb")
        self.dll.write_bytes(b"broken-build")
        (self.assets / "new.png").write_bytes(b"new")
        icon = self.game / "Mods" / "new.png"
        real_replace = os.replace

        def failing_replace(source, destination):
            if Path(destination) == icon and Path(source).name.startswith(deploy_mod.TEMP_PREFIX):
                raise OSError("injected replace failure")
            return real_replace(source, destination)

        with mock.patch.object(deploy_mod.os, "replace", side_effect=failing_replace):
            with self.assertRaises(deploy_mod.DeployError):
                self.deploy()
        record = self.receipt()
        self.assertEqual(record["Status"], "RolledBack")
        self.assertEqual(record["Rollback"]["Failures"], [])
        self.assertEqual(record["Rollback"]["Restored"], 1)
        self.assertEqual(record["Rollback"]["Removed"], 1)
        self.assertEqual((self.game / "Mods" / "Demo.dll").read_bytes(), b"new-dll")
        self.assertFalse(icon.exists())
        self.assertFalse((self.game / "UserData" / "Demo" / "Demo.pdb").exists())

    def test_no_deploy_writes_nothing(self):
        result = self.deploy(no_deploy=True)
        self.assertEqual(result["Status"], "NoDeploy")
        self.assertFalse((self.game / "Mods").exists())

    def test_game_running_blocks_deploy(self):
        with mock.patch.object(deploy_mod, "running_game_processes", return_value=["4242"]):
            with self.assertRaises(deploy_mod.DeployError) as caught:
                self.deploy()
        self.assertEqual(caught.exception.code, 4)
        self.assertFalse((self.game / "Mods").exists())

    def test_missing_install_probe_is_rejected(self):
        (self.game / "MelonLoader" / "Il2CppAssemblies" / "Assembly-CSharp.dll").unlink()
        with self.assertRaises(deploy_mod.DeployError) as caught:
            self.deploy()
        self.assertEqual(caught.exception.code, 3)

    def test_game_dir_inside_repository_is_rejected(self):
        with self.assertRaises(deploy_mod.DeployError) as caught:
            deploy_mod.resolve_game_dir(str(TOOLS.parent / "Source"))
        self.assertEqual(caught.exception.code, 2)

    def test_stale_previously_deployed_file_is_reported(self):
        self.deploy()
        (self.assets / "icon.png").unlink()
        self.deploy()
        stale = self.receipt()["Stale"]
        self.assertEqual(len(stale), 1)
        self.assertTrue(stale[0]["DestinationRelative"].endswith("icon.png"))
        self.assertTrue((self.game / "Mods" / "icon.png").is_file())


    def test_process_scan_fails_closed_without_readable_entries(self):
        proc = self.base / "proc"
        (proc / "111").mkdir(parents=True)
        with self.assertRaises(deploy_mod.DeployError) as caught:
            deploy_mod._scan_processes(proc)
        self.assertEqual(caught.exception.code, 5)

    def test_process_scan_matches_only_image_or_argv0(self):
        proc = self.base / "proc"
        (proc / "111").mkdir(parents=True)
        (proc / "111" / "comm").write_text("Schedule I\n", encoding="utf-8")
        (proc / "111" / "cmdline").write_bytes(b"/opt/Schedule I.exe\0--windowed\0")
        (proc / "222").mkdir()
        (proc / "222" / "comm").write_text("grep\n", encoding="utf-8")
        (proc / "222" / "cmdline").write_bytes(b"grep\0schedulei\0")
        self.assertEqual(deploy_mod._scan_processes(proc), ["111"])

    def test_relative_pdb_path_is_rejected(self):
        with self.assertRaises(deploy_mod.DeployError):
            deploy_mod.invoke(
                game_dir=str(self.game),
                project_name="Demo",
                project_dir=str(self.project),
                target_path=str(self.dll),
                pdb_path="Demo.pdb",
                assets_dir=str(self.assets),
                mod_json_path=str(self.mod_json),
                configuration="Release",
                mutex_timeout_seconds=5,
                no_deploy=False,
            )
        self.assertFalse((self.game / "Mods").exists())

    def test_case_distinct_icons_are_not_duplicates_on_posix(self):
        if os.path.normcase("A") != "A":
            self.skipTest("case-insensitive filesystem")
        (self.assets / "New.PNG").write_bytes(b"upper")
        (self.assets / "new.png").write_bytes(b"lower")
        self.deploy()
        self.assertEqual((self.game / "Mods" / "New.PNG").read_bytes(), b"upper")
        self.assertEqual((self.game / "Mods" / "new.png").read_bytes(), b"lower")

    def test_foreign_receipt_resolves_through_relative_path(self):
        self.deploy()
        receipt_path = self.game / "UserData" / ".deploy" / "receipts" / "Demo.json"
        record = json.loads(receipt_path.read_text(encoding="utf-8"))
        for entry in record["Entries"]:
            entry["Destination"] = "C:\\Games\\Schedule I\\" + entry["DestinationRelative"].replace("/", "\\")
        receipt_path.write_text(json.dumps(record), encoding="utf-8")
        (self.assets / "icon.png").unlink()
        self.deploy()
        stale = self.receipt()["Stale"]
        self.assertEqual(len(stale), 1)
        self.assertTrue(Path(stale[0]["Destination"]).is_relative_to(self.game))

    def test_missing_game_dir_names_schedule1_path(self):
        with mock.patch.dict(os.environ, {}, clear=True):
            with self.assertRaises(deploy_mod.DeployError) as caught:
                deploy_mod.resolve_game_dir(None)
        self.assertEqual(caught.exception.code, 2)
        self.assertIn("SCHEDULE1_PATH", str(caught.exception))


if __name__ == "__main__":
    unittest.main()
