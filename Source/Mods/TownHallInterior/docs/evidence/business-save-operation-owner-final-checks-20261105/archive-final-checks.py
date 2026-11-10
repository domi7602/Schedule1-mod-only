"""Preserve the final isolated Quick result without changing the frozen implementation archive."""

from datetime import datetime, timezone
import hashlib
import json
from pathlib import Path
import subprocess

root = Path(__file__).resolve().parent
repo = root.parent.parent
run = Path('C:/Users/pc/.cache/s1ov/clean-checkout-e0035c00d8f4')
check = run / 'checks/verify-626a10b2f4fd'
sha = lambda path: hashlib.sha256(path.read_bytes()).hexdigest()
load = lambda path: json.loads(path.read_text(encoding='utf-8-sig'))
snapshot = load(run / 'snapshot.json')
report = load(check / 'report.json')
assert report['Passed'] == 6 and report['Failed'] == 1 and report['ExitCode'] == 1
assert [row['Name'] for row in report['Steps'] if row['Status'] == 'Failed'] == ['versions']
assert next(row for row in report['Steps'] if row['Name'] == 'format')['Status'] == 'Passed'
for entry in snapshot['files']:
    assert '..' not in Path(entry['path']).parts
    assert sha(repo / entry['path']) == sha(Path(snapshot['checkout']) / entry['path']) == entry['sha256'], entry['path']
paths = set(subprocess.check_output(['git', '-C', str(repo), 'ls-files', '-z'], text=True).split('\0'))
paths.update(subprocess.check_output(['git', '-C', str(repo), 'ls-files', '--others', '--exclude-standard', '-z'], text=True).split('\0'))
assert {path for path in paths if path and (repo / path).is_file()} == {entry['path'] for entry in snapshot['files']}
for module in snapshot['submodules']:
    assert subprocess.check_output(['git', '-C', str(repo / module['path']), 'rev-parse', 'HEAD'], text=True).strip() == module['commit']
v3 = load(root / 'candidate-v3/freeze.json')
assert sha(repo / v3['sourcePath']) == v3['sourceSha256']
assert sha(repo / v3['testPath']) == v3['testSha256']
main = repo / 'Source/Mods/TownHallInterior/docs/evidence/business-save-operation-owner-impl-20261105'
assert sha(main / 'manifest.json') == '40d09c5a399f85710692cdd260582a0622a0998ed16da0a129e3b31179de2484'
for entry in load(main / 'manifest.json')['files']:
    assert sha(main / entry['path']) == entry['sha256']
for entry in load(main / 'source-documents.json')['files']:
    assert sha(repo / entry['path']) == entry['sha256']
summary = {'schema': 1, 'recordedUtc': datetime.now(timezone.utc).isoformat(),
    'scope': 'Final isolated Quick after whitespace fix, canonical documentation and primary evidence archive',
    'sourceRevision': snapshot['head'], 'sourceWorktree': 'dirty', 'snapshotDigest': snapshot['sourceDigest'],
    'snapshotCapturedUtc': snapshot['capturedAtUtc'], 'overallStatus': 'failed', 'exitCode': 1,
    'passedSteps': 6, 'failedSteps': 1, 'onlyFailedStep': 'versions',
    'format': 'passed', 'solutionDeterminism': 'passed-44-projects', 'documentationPaths': 'passed',
    'skills': 'passed-with-existing-CRLF-warning', 'xml': 'passed-71-files',
    'remainingFailure': 'Existing unrelated HitmanPhone/Minimap/PoliceTestMode version drift, not repaired.',
    'sourceSha256': v3['sourceSha256'], 'testSha256': v3['testSha256'],
    'allCapturedFileHashesStillMatchSourceAndCheckout': True, 'currentNonignoredFileSetMatches': True,
    'capturedFiles': len(snapshot['files']), 'pinnedSubmoduleHeadsMatch': len(snapshot['submodules']),
    'primaryArchiveUnchanged': True, 'primaryArchivePayloadFiles': 69,
    'primaryArchiveManifestSha256': sha(main / 'manifest.json'),
    'canonicalDocumentHashesStillMatchPrimaryArchive': True,
    'nativeSaveWriteAllowed': False, 'noFullPureRerunClaimed': True,
    'actualInstallationOrNativeSaveAccess': False,
    'laterDeltaToSnapshot': 'Only this generated final-checks evidence supplement; code, tests and canonical docs unchanged.'}
selected = {'summary.json': (json.dumps(summary, ensure_ascii=False, indent=2) + '\n').encode('utf-8'),
    'snapshot.json': (run / 'snapshot.json').read_bytes(), 'report.json': (check / 'report.json').read_bytes(),
    'format-step.json': (json.dumps({'originalStep': next(row for row in report['Steps'] if row['Name'] == 'format'),
        'originalLogExists': (check / 'format.log').exists(), 'note': 'Successful formatter produced no output, so Tee created no format.log.'},
        ensure_ascii=False, indent=2) + '\n').encode('utf-8'),
    'versions.log': (check / 'versions.log').read_bytes(),
    'docs.log': (check / 'docs.log').read_bytes(), 'runner.log': (root / 'final-quick.log').read_bytes(),
    'archive-final-checks.py': Path(__file__).read_bytes()}
folders = [repo / 'Source/Mods/TownHallInterior/docs/evidence/business-save-operation-owner-final-checks-20261105',
           Path('C:/Users/pc/Documents/LLM-WIKI/raw/attachments/townhall-save-operation-owner-final-checks-2026-11-05')]
assert all(not folder.exists() for folder in folders)
records = []
for name, data in sorted(selected.items()):
    expected = hashlib.sha256(data).hexdigest()
    for folder in folders:
        folder.mkdir(parents=True, exist_ok=True)
        with (folder / name).open('xb') as stream:
            stream.write(data)
        assert sha(folder / name) == expected
    records.append({'path': name, 'sha256': expected, 'bytes': len(data)})
manifest = {'schema': 1, 'capturedUtc': summary['recordedUtc'], 'project': 'schedule1-mod-only',
    'sourceRevision': snapshot['head'], 'sourceWorktree': 'dirty', 'scope': summary['scope'],
    'overallStatus': 'failed-only-unrelated-version-drift', 'nativeSaveWriteAllowed': False, 'files': records}
encoded = (json.dumps(manifest, ensure_ascii=False, indent=2) + '\n').encode('utf-8')
for folder in folders:
    with (folder / 'manifest.json').open('xb') as stream:
        stream.write(encoded)
print(json.dumps({'archivedFilesExcludingManifest': len(records), 'manifestSha256': hashlib.sha256(encoded).hexdigest(),
    'allCopiesHashIdentical': True, **summary}, ensure_ascii=False, indent=2))
