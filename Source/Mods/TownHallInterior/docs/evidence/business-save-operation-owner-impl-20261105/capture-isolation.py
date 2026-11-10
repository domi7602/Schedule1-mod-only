"""Preserve the actual isolated Pure result, including failures and exact source identities."""

import argparse
from datetime import datetime, timezone
import hashlib
import json
from pathlib import Path
import re
import subprocess
import xml.etree.ElementTree as ET

parser = argparse.ArgumentParser(description=__doc__)
parser.add_argument('run', type=Path)
args = parser.parse_args()
root = Path(__file__).resolve().parent
repo = root.parent.parent
run = args.run.resolve()
sha = lambda path: hashlib.sha256(path.read_bytes()).hexdigest()
load = lambda path: json.loads(path.read_text(encoding='utf-8-sig'))
snapshot = load(run / 'snapshot.json')
checks = list((run / 'checks').glob('verify-*/report.json'))
assert len(checks) == 1
report = load(checks[0])
check_root = checks[0].parent
candidate = load(root / 'candidate-v2/freeze.json')
captured = {row['path']: row for row in snapshot['files']}
for path, expected in ((candidate['sourcePath'], candidate['sourceSha256']), (candidate['testPath'], candidate['testSha256'])):
    assert captured[path]['sha256'] == expected
    assert sha(repo / path) == sha(Path(snapshot['checkout']) / path) == expected
for path, entry in captured.items():
    assert '..' not in Path(path).parts
    assert sha(repo / path) == sha(Path(snapshot['checkout']) / path) == entry['sha256'], path
paths = set(subprocess.check_output(['git', '-C', str(repo), 'ls-files', '-z'], text=True).split('\0'))
paths.update(subprocess.check_output(['git', '-C', str(repo), 'ls-files', '--others', '--exclude-standard', '-z'], text=True).split('\0'))
paths = {path for path in paths if path and (repo / path).is_file()}
assert paths == set(captured), 'New/deleted nonignored source file after snapshot.'
for module in snapshot['submodules']:
    actual = subprocess.check_output(['git', '-C', str(repo / module['path']), 'rev-parse', 'HEAD'], text=True).strip()
    assert actual == module['commit']
tests = []
for trx in sorted((check_root / 'tests').rglob('*.trx')):
    node = ET.parse(trx).find('.//{*}Counters')
    assert node is not None
    counters = {key: int(node.get(key, '0')) for key in ('total', 'executed', 'passed', 'failed')}
    assert counters['executed'] > 0 and counters['failed'] == 0
    tests.append({'project': trx.parent.name, 'sha256': sha(trx), 'counters': counters, 'path': str(trx)})
townhall = [row for row in tests if row['project'] == 'TownHallInterior.Tests']
assert len(townhall) == 1 and townhall[0]['counters']['passed'] == 809
test_log = (check_root / 'tests.log').read_text(encoding='utf-8-sig')
console = re.search(r'RESULT:\s*(\d+)/(\d+) passed;\s*(\d+) failed', test_log)
assert console and console[1] == console[2] and console[3] == '0'
destination = root / 'isolation'
destination.mkdir(exist_ok=False)
for source, name in [(run / 'snapshot.json', 'snapshot.json'), (checks[0], 'report.json'),
                     (check_root / 'format.log', 'format.log'), (check_root / 'versions.log', 'versions.log'),
                     (check_root / 'tests.log', 'tests.log'), (check_root / 'docs.log', 'docs.log'),
                     (Path(townhall[0]['path']), 'townhall.trx')]:
    (destination / name).write_bytes(source.read_bytes())
record = {'schema': 1, 'checkedUtc': datetime.now(timezone.utc).isoformat(),
    'scope': 'Isolated working-file Pure run before final documentation; actual failed overall result retained',
    'sourceDigest': snapshot['sourceDigest'], 'snapshotStatus': snapshot['status'], 'snapshotError': snapshot['error'],
    'sourceRevision': snapshot['head'], 'sourceWorktree': 'dirty',
    'sourceSha256': candidate['sourceSha256'], 'testSha256': candidate['testSha256'],
    'manualAfterRunComparison': {'allCapturedFileHashesStillMatchSourceAndCheckout': True,
        'currentNonignoredFileSetMatches': True, 'files': len(captured), 'pinnedSubmoduleHeadsMatch': len(snapshot['submodules'])},
    'overallExitCode': report['ExitCode'], 'passedSteps': report['Passed'], 'failedSteps': report['Failed'],
    'failedStepNames': [step['Name'] for step in report['Steps'] if step['Status'] == 'Failed'],
    'ownFailure': 'Formatting only: new test initializer lines 828-829. Correct separately without weakening assertions.',
    'unrelatedFailure': 'Existing HitmanPhone/Minimap/PoliceTestMode version drift; not repaired in this task.',
    'xunitProjects': len(tests), 'xunitPassed': sum(row['counters']['passed'] for row in tests),
    'consolePassed': int(console[1]), 'tests': tests,
    'gameReferencesAvailable': False, 'gameDirectoryOverriddenToAbsentSyntheticPath': True,
    'submodulesInitializedFromLocalRepositoriesViaProcessOnlyUrlMapping': True,
    'nativeSaveWriteAllowed': False, 'actualDeploymentOrNativeCalls': False}
(root / 'isolated-verification.json').write_text(json.dumps(record, ensure_ascii=False, indent=2) + '\n', encoding='utf-8')
print(json.dumps({key: value for key, value in record.items() if key != 'tests'}, ensure_ascii=False, indent=2))
