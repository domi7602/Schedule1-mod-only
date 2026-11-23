"""Regression tests for the ThirdParty allowlist guard (Tools/deploy_thirdparty.py)."""

import contextlib
import io
import os
import sys
from pathlib import Path
import tempfile
import unittest
from unittest import mock

TOOLS = Path(__file__).resolve().parents[1]
sys.path.insert(0, str(TOOLS))

import deploy_thirdparty  # noqa: E402


class ThirdPartyGuardTests(unittest.TestCase):
    def setUp(self):
        self.temp = tempfile.TemporaryDirectory(prefix="s1thirdparty-")
        self.base = Path(self.temp.name)
        self.allowlist = self.base / ".deployallow"

    def tearDown(self):
        self.temp.cleanup()

    def run_guard(self, *extra):
        stdout, stderr = io.StringIO(), io.StringIO()
        with contextlib.redirect_stdout(stdout), contextlib.redirect_stderr(stderr):
            code = deploy_thirdparty.main(["-AllowlistPath", str(self.allowlist), *extra])
        return code, stdout.getvalue(), stderr.getvalue()

    def test_empty_allowlist_is_a_noop(self):
        self.allowlist.write_text("# nothing here\n\n", encoding="utf-8")
        code, out, _ = self.run_guard()
        self.assertEqual(code, 0)
        self.assertIn("empty", out)

    def test_bom_prefixed_comment_only_allowlist_is_a_noop(self):
        self.allowlist.write_bytes(b"\xef\xbb\xbf# only a comment\n")
        code, out, _ = self.run_guard()
        self.assertEqual(code, 0, out)
        self.assertIn("empty", out)

    def test_patterns_are_refused(self):
        self.allowlist.write_text("**/*.dll\n", encoding="utf-8")
        code, _, err = self.run_guard()
        self.assertEqual(code, 6)
        self.assertIn("**/*.dll", err)

    def test_missing_allowlist_is_an_error(self):
        code, _, err = self.run_guard()
        self.assertEqual(code, 2)
        self.assertIn("not found", err)

    def test_unreadable_allowlist_reports_instead_of_raising(self):
        self.allowlist.write_bytes(b"\xff\xfe\x00")
        code, _, err = self.run_guard()
        self.assertEqual(code, 2)
        self.assertIn("cannot read", err)

    def test_no_deploy_switch_wins(self):
        self.allowlist.write_text("**/*.dll\n", encoding="utf-8")
        code, out, _ = self.run_guard("-NoDeploy")
        self.assertEqual(code, 0)
        self.assertIn("NoDeploy", out)

    def test_no_deploy_environment_wins(self):
        self.allowlist.write_text("**/*.dll\n", encoding="utf-8")
        with mock.patch.dict(os.environ, {"S1NoDeploy": "true"}):
            code, out, _ = self.run_guard()
        self.assertEqual(code, 0)
        self.assertIn("NoDeploy", out)


if __name__ == "__main__":
    unittest.main()
