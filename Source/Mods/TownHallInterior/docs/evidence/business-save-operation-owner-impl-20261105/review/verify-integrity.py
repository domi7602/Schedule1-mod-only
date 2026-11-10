"""Independent, read-only comparison of the frozen V2 inputs and protected files."""
from datetime import datetime, timezone
from pathlib import Path
import difflib
import hashlib
import json
import re
import subprocess
import sys
import xml.etree.ElementTree as ET

review = Path(__file__).resolve().parent
root = review.parent
repo = root.parent.parent
output = review / sys.argv[1]
assert output.parent == review and not output.exists()
sha = lambda data: hashlib.sha256(data).hexdigest()
checks = []
def compare(label, actual, expected):
    checks.append(dict(check=label, accepted=actual == expected, actual=actual, expected=expected))

baseline = json.loads((root / 'input-manifest.json').read_text(encoding='utf-8'))
freezes = {}
for version in ('candidate-v1', 'candidate-v2'):
    directory = root / version
    freeze = json.loads((directory / 'freeze.json').read_text(encoding='utf-8'))
    freezes[version] = freeze
    for item in freeze['files']:
        payload = (directory / item['path']).read_bytes()
        compare(f'{version}/{item["path"]} SHA256', sha(payload), item['sha256'])
        compare(f'{version}/{item["path"]} bytes', len(payload), item['bytes'])

v2 = freezes['candidate-v2']
before = (root / 'BusinessConstruction.before.cs').read_bytes()
source = (repo / v2['sourcePath']).read_bytes()
tests = (repo / v2['testPath']).read_bytes()
compare('live source SHA256', sha(source), v2['sourceSha256'])
compare('live tests SHA256', sha(tests), v2['testSha256'])
compare('before SHA256', sha(before), v2['sourceBeforeSha256'])
compare('contract SHA256', sha((root / 'implementation-contract.md').read_bytes()),
        next(i['sha256'] for i in v2['files'] if i['path'] == 'implementation-contract.txt'))

# Deliberately do not reuse the maker's block-removal regex.
lines = source.splitlines(keepends=True)
starts = [i for i, line in enumerate(lines) if line.rstrip(b'\r\n') == b'#region DataOnlySaveOperationOwnership']
compare('single marked ownership region', len(starts), 1)
start = starts[0]
end = next(i for i in range(start + 1, len(lines)) if lines[i].rstrip(b'\r\n') == b'#endregion')
compare('one empty separator after region', lines[end + 1].rstrip(b'\r\n'), b'')
removed = b''.join(lines[:start] + lines[end + 2:])
block = b''.join(lines[start:end + 2])
compare('removing region reproduces old bytes', sha(removed), sha(before))
compare('insert precedes SubtypeResult', lines[end + 2].startswith(b'public sealed record SubtypeResult'), True)
compare('region hash', sha(block), v2['blockSha256'])

protected = []
for item in baseline['protectedSourcesBefore']:
    if item['path'] == baseline['mutableSource']:
        continue
    actual = sha((repo / item['path']).read_bytes())
    compare('protected ' + item['path'], actual, item['sha256'])
    protected.append(dict(path=item['path'], sha256=actual))
inputs = []
for item in baseline['inputs']:
    actual = sha((repo / item['path']).read_bytes())
    compare('input ' + item['path'], actual, item['sha256'])
    inputs.append(dict(path=item['path'], sha256=actual))

prior_count = 0
prior_manifests = []
for item in baseline['priorEvidence']:
    folder = repo / item['root']
    data = (folder / 'manifest.json').read_bytes()
    compare('prior manifest ' + item['root'], sha(data), item['manifestSha256'])
    prior_manifests.append(dict(path=item['root'] + '/manifest.json', sha256=sha(data)))
    manifest = json.loads(data)
    compare('prior artifact count ' + item['root'], len(manifest['files']), item['files'])
    for artifact in manifest['files']:
        compare('prior artifact ' + item['root'] + '/' + artifact['path'],
                sha((folder / artifact['path']).read_bytes()), artifact['sha256'])
        prior_count += 1

production = sorted(p.name for p in (repo / 'Source/Mods/TownHallInterior/src').iterdir()
                    if p.suffix in ('.cs', '.csproj'))
compare('production files', len(production), 20)
head = subprocess.check_output(['git', 'rev-parse', 'HEAD'], cwd=repo, text=True).strip()
compare('repository HEAD', head, v2['sourceRevision'])

source_delta = list(difflib.unified_diff(
    (root / 'candidate-v1/source.txt').read_text(encoding='utf-8').splitlines(),
    (root / 'candidate-v2/source.txt').read_text(encoding='utf-8').splitlines(),
    fromfile='candidate-v1/source.txt', tofile='candidate-v2/source.txt', lineterm=''))
test_delta = list(difflib.unified_diff(
    (root / 'candidate-v1/tests.txt').read_text(encoding='utf-8').splitlines(),
    (root / 'candidate-v2/tests.txt').read_text(encoding='utf-8').splitlines(),
    fromfile='candidate-v1/tests.txt', tofile='candidate-v2/tests.txt', lineterm=''))
deleted_tests = [line for line in test_delta if line.startswith('-') and not line.startswith('---')]
compare('V1 test lines removed or weakened', len(deleted_tests), 0)

test_records = []
ns = {'t': 'http://microsoft.com/schemas/VisualStudio/TeamTest/2010'}
for folder in ('owner-targeted-v1', 'owner-targeted-v2', 'townhall-regression-v2'):
    report = json.loads((root / folder / 'report.json').read_text(encoding='utf-8-sig'))
    trx = ET.parse(root / folder / 'results.trx').getroot()
    counters = trx.find('t:ResultSummary/t:Counters', ns).attrib
    for key in ('total', 'executed', 'passed', 'failed'):
        compare(f'{folder} {key} TRX/report', int(counters[key]), report['Counters'][key.capitalize()])
    test_records.append(dict(folder=folder, attribution='parent-authored run; not independently executed',
                             exitCode=report['ExitCode'], counters=counters,
                             artifacts=[dict(path=f'{folder}/{name}', sha256=sha((root / folder / name).read_bytes()))
                                        for name in ('report.json', 'results.trx', 'output.log')]))

forbidden = re.findall(r'\b(?:IBusinessSavePort|IntPtr|UnityEngine|RuntimePort|BusinessManagerSaveTrial|File|Directory|Task|Timer)\b',
                       block.decode('utf-8'))
compare('forbidden dependency identifiers in new region', forbidden, [])

result = dict(schema=1, checkedUtc=datetime.now(timezone.utc).isoformat(),
              reviewer='GPT-6.1 Sol, independent code/test verifier',
              sourceRevision=head, sourceWorktree='dirty',
              checkedFrozenFiles=freezes, sourceBeforeSha256=sha(before),
              checkedRange=dict(firstLine=start + 1, lastLine=end + 1),
              protectedSources=protected, checkedInputs=inputs,
              priorManifests=prior_manifests, priorArtifactsChecked=prior_count,
              productionFiles=production, inheritedTestRuns=test_records,
              sourceV1V2Delta=source_delta, testsV1V2Delta=test_delta,
              checks=checks, counts=dict(total=len(checks), passed=sum(c['accepted'] for c in checks),
                                        failed=sum(not c['accepted'] for c in checks)),
              installedBytesRead=False, nativeBytesDecoded=False, savesUserDataSidecarsRead=False)
output.write_text(json.dumps(result, ensure_ascii=False, indent=2, default=lambda v: v.decode()) + '\n', encoding='utf-8')
print(json.dumps(dict(path=str(output), checkedUtc=result['checkedUtc'], counts=result['counts'],
                      protectedSources=len(protected), priorArtifacts=prior_count,
                      priorManifests=len(prior_manifests), productionFiles=len(production)), indent=2))
sys.exit(0 if all(c['accepted'] for c in checks) else 1)
