#!/usr/bin/env python3
"""Guard for the opt-in ThirdParty deployment on hosts without PowerShell.

ThirdParty deployment is allowlist-driven and ships empty, so the default build
step is a clean no-op. A non-empty allowlist must not be skipped silently:
`Tools/deploy-thirdparty.ps1` owns the copy, overwrite and receipt rules, so this
guard refuses with exit code 6 and names the patterns instead.
"""

from __future__ import annotations

import argparse
import sys
from pathlib import Path

WORKSPACE_ROOT = Path(__file__).resolve().parents[1]


def main(argv: list[str] | None = None) -> int:
    parser = argparse.ArgumentParser(description="Check the ThirdParty allowlist without PowerShell.")
    parser.add_argument("-GameDir", "--game-dir", dest="game_dir", default=None)
    parser.add_argument("-NoDeploy", "--no-deploy", dest="no_deploy", action="store_true")
    parser.add_argument("-AllowlistPath", "--allowlist-path", dest="allowlist_path", default=None)
    args = parser.parse_args(argv)

    if args.no_deploy:
        print("[deploy-thirdparty] NoDeploy active - nothing copied.")
        return 0

    allowlist = Path(args.allowlist_path) if args.allowlist_path else WORKSPACE_ROOT / "ThirdParty" / ".deployallow"
    if not allowlist.is_file():
        print(f"[deploy-thirdparty] ERROR: allowlist not found: '{allowlist}'.", file=sys.stderr)
        return 2

    patterns = [
        line.strip()
        for line in allowlist.read_text(encoding="utf-8").splitlines()
        if line.strip() and not line.lstrip().startswith("#")
    ]
    if not patterns:
        print("[deploy-thirdparty] Allowlist is empty - no third-party DLL is deployed.")
        return 0

    print(
        "[deploy-thirdparty] ERROR: the allowlist is not empty, but third-party deployment is not "
        "automated on this host. Use Tools/deploy-thirdparty.ps1 on Windows or copy the DLLs manually. "
        "Patterns: " + ", ".join(patterns),
        file=sys.stderr,
    )
    return 6


if __name__ == "__main__":
    sys.exit(main())
