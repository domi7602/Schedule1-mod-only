# Initial CLI verification — 2026-10-26

- `python -m unittest discover -s Tools/s1-cli/tests -v`: **22 passed**, 0 failed.
  Full actual output: [TEST-RESULTS.txt](TEST-RESULTS.txt). Python 3.14.9 on Windows.
- `pwsh -NoProfile -ExecutionPolicy Bypass -File Tools/Tests/WorldLookMcp.Tests.ps1`:
  **81 passed**, 0 failed (existing driver's synthetic suite, not live gameplay).
- Actual `s1 --help` (Windows command launcher) and direct Python help passed.
- Actual `python Tools/s1-cli/s1.py --json doctor`: **exit 3**, game executable
  present, installed `Mods/S1MCPServer-IL2CPP.dll` **absent**, loopback bridge
  connection refused. Process observation succeeded: **no Schedule I process and
  no 127.0.0.1:8765 listener** at this probe. This supersedes the earlier PID 3404
  observation; processes can change after the probe.
- Actual `python Tools/s1-cli/s1.py --json status`: **exit 3**, connection refused,
  stable JSON error with correlated request metadata. No invented successful state.
- No live capture, lighting readback or WorldLook mutation was possible/attempted.
  PNG successes in tests are labelled generated fixtures, not game screenshots.
- Standalone CLI suite also passed **22/22 in the isolated working-file snapshot**:
  `C:/Users/pc/AppData/Local/hermes/cache/scratch/s1-cli-clean-check/clean-checkout-bdddb991b84e/checkout`.
- Required repository-wide `verify-clean-checkout.ps1 -Mode Pure` ran with a closed-game
  preflight and no deployment: **19 gates passed, 3 failed**. Failures are outside the
  CLI changes: formatting in existing screenshot C# edits; existing version/inventory
  drift for HitmanPhone, Minimap and PoliceTestMode; workspace-setup fixture Git/MSYS
  path failure. These files were deliberately not repaired or overwritten.
  Full report: `C:/Users/pc/AppData/Local/hermes/cache/scratch/s1-cli-clean-check/clean-checkout-bdddb991b84e/checks/verify-89aaaccffe44/report.json`.
  Documentation paths, skills/XML checks and the game-free test gate passed.
- Static scan: four Python source/test files parse successfully; no matches for
  shell=True, os.system, eval/exec, pickle or hardcoded-secret assignment patterns.
  Scoped `git diff --check` passed. This is self-review, **not independent approval**;
  independent parent review remains pending. The Python suite is not yet included
  automatically in the repository Pure runner.
- No production mod build/deploy, game launch/load/save/time mutation, package changes,
  commit/push or change to existing screenshot implementation. Only this CLI subtree
  and the small Tools README entry are implementation changes.
