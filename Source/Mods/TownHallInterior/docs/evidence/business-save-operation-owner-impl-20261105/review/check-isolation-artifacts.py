"""Read-only, narrow verification of the parent's newly completed isolated Pure evidence."""
from datetime import datetime, timezone
from pathlib import Path
import hashlib
import json
import re
import sys
import xml.etree.ElementTree as ET

review = Path(__file__).resolve().parent
root = review.parent
repo = root.parent.parent
output = review / 'isolation-artifact-check.json'
assert not output.exists()
load = lambda p: json.loads(p.read_text(encoding='utf-8-sig'))
sha = lambda p: hashlib.sha256(p.read_bytes()).hexdigest()
parent = load(root / 'isolated-verification.json')
snapshot = load(root / 'isolation/snapshot.json')
report = load(root / 'isolation/report.json')
freeze = load(root / 'candidate-v2/freeze.json')
checks = []
def check(name, actual, expected):
    checks.append(dict(check=name, passed=actual == expected, actual=actual, expected=expected))

check('same snapshot digest', snapshot['sourceDigest'], parent['sourceDigest'])
check('specified isolated snapshot', parent['sourceDigest'], '917416c62f7530dc53f803a0628f2414fda0d419152556a2efbbf98a6df64ea2')
check('isolated source matches V2', parent['sourceSha256'], freeze['sourceSha256'])
check('isolated tests match V2', parent['testSha256'], freeze['testSha256'])
by_path = {f['path']: f['sha256'] for f in snapshot['files']}
for kind in ('source', 'test'):
    check('snapshot ' + kind, by_path[freeze[kind + 'Path']], freeze[kind + 'Sha256'])
    check('live ' + kind + ' remains V2', sha(repo / freeze[kind + 'Path']), freeze[kind + 'Sha256'])
check('snapshot captured files', len(snapshot['files']), parent['manualAfterRunComparison']['files'])
check('overall failed exit retained', report['ExitCode'], 1)
check('parent overall exit', parent['overallExitCode'], 1)
check('passed steps', sum(s['Status'] == 'Passed' for s in report['Steps']), 20)
check('failed steps', sum(s['Status'] == 'Failed' for s in report['Steps']), 2)
check('failed names', [s['Name'] for s in report['Steps'] if s['Status'] == 'Failed'], ['format', 'versions'])
check('formatter exit', next(s['ExitCode'] for s in report['Steps'] if s['Name'] == 'format'), 2)
check('version drift exit', next(s['ExitCode'] for s in report['Steps'] if s['Name'] == 'versions'), 1)
tool_steps = [s for s in report['Steps'] if s['Name'].startswith('tool-')]
check('14 tool suites passed', sum(s['Status'] == 'Passed' for s in tool_steps), 14)
check('xunit projects', len(parent['tests']), 18)
check('xunit passed total', sum(s['counters']['passed'] for s in parent['tests']), 4740)
check('xunit failed total', sum(s['counters']['failed'] for s in parent['tests']), 0)
check('console passed in report', parent['consolePassed'], 265)

format_text = (root / 'isolation/format.log').read_text(encoding='utf-8-sig')
errors = [line for line in format_text.splitlines() if line.strip()]
check('nine whitespace diagnostics only', len(errors), 9)
check('all format errors target only new initializer',
      all(re.search(r'BusinessSaveOperationOwnerTests\.cs\((828|829),\d+\): error WHITESPACE:', line) for line in errors), True)
ns = {'t': 'http://microsoft.com/schemas/VisualStudio/TeamTest/2010'}
trx = root / 'isolation/townhall.trx'
trx_counters = ET.parse(trx).getroot().find('t:ResultSummary/t:Counters', ns).attrib
townhall = next(s for s in parent['tests'] if s['project'] == 'TownHallInterior.Tests')
check('isolated TownHall TRX hash', sha(trx), townhall['sha256'])
check('isolated TownHall passed', int(trx_counters['passed']), 809)
check('isolated TownHall failed', int(trx_counters['failed']), 0)

artifacts = [root / 'isolated-verification.json'] + list((root / 'isolation').iterdir())
record = dict(schema=1, checkedUtc=datetime.now(timezone.utc).isoformat(),
              reviewer='GPT-6.1 Sol, independent verifier', reranPure=False,
              scope='Counter/log/input identity check only; no complete checkout rehash or test rerun',
              reportedManualAfterRunComparison=parent['manualAfterRunComparison'],
              independentlyRehashed1910Files=False,
              inputs=[dict(path=p.relative_to(root).as_posix(), sha256=sha(p)) for p in artifacts if p.is_file()],
              checks=checks, counters=dict(total=len(checks), passed=sum(c['passed'] for c in checks),
                                          failed=sum(not c['passed'] for c in checks)),
              candidateHasRequiredWhitespaceCorrection=True, nativeSaveWriteAllowed=False)
output.write_text(json.dumps(record, ensure_ascii=False, indent=2) + '\n', encoding='utf-8')
print(json.dumps(dict(path=str(output), checkedUtc=record['checkedUtc'], counters=record['counters'],
                      actualPureStatus='failed', ownFormatDefect='initializer lines 828-829',
                      unrelatedVersionDefects=['HitmanPhone', 'Minimap', 'PoliceTestMode']), indent=2))
sys.exit(0 if all(c['passed'] for c in checks) else 1)
