"""Archive the bounded offline implementation evidence once, without installed-file access."""

import argparse
from datetime import datetime, timezone
import hashlib
import json
from pathlib import Path
import re
import stat

parser = argparse.ArgumentParser(description=__doc__)
parser.add_argument('--vault', type=Path, required=True)
args = parser.parse_args()
root = Path(__file__).resolve().parent
repo = root.parent.parent
docs = repo / 'Source/Mods/TownHallInterior/docs'


def payload(path):
    for parent in (path, *path.parents):
        assert not (getattr(parent.lstat(), 'st_file_attributes', 0) & stat.FILE_ATTRIBUTE_REPARSE_POINT), str(parent)
    return path.read_bytes()


def sha(path):
    return hashlib.sha256(payload(path)).hexdigest()


def load(path):
    return json.loads(payload(path).decode('utf-8-sig'))


freeze = load(root / 'input-manifest.json')
closeout = load(root / 'implementation-closeout.json')
final = load(root / closeout['finalCandidate'] / 'freeze.json')
source = payload(repo / freeze['mutableSource'])
before = payload(root / 'BusinessConstruction.before.cs')
pattern = rb'(?ms)^#region DataOnlySaveOperationOwnership\r?\n.*?^#endregion\r?\n\r?\n'
match = re.search(pattern, source)
assert match and source[:match.start()] + source[match.end():] == before
assert sha(repo / freeze['mutableSource']) == final['sourceSha256']
assert sha(repo / freeze['newTest']) == final['testSha256']
for entry in freeze['protectedSourcesBefore']:
    if entry['path'] != freeze['mutableSource']:
        assert sha(repo / entry['path']) == entry['sha256'], entry['path']
for prior in freeze['priorEvidence']:
    folder = repo / prior['root']
    assert sha(folder / 'manifest.json') == prior['manifestSha256']
    for entry in load(folder / 'manifest.json')['files']:
        assert sha(folder / entry['path']) == entry['sha256'], entry['path']
for entry in freeze['inputs']:
    if not entry['path'].endswith('/business-save-operation-owner.md'):
        assert sha(repo / entry['path']) == entry['sha256'], entry['path']
design_docs = load(docs / 'evidence/business-save-operation-owner-design-20261105/source-documents.json')
for entry in design_docs['files']:
    if entry['path'].endswith(('/native-save-completion-audit.md', '/native-world-exit-audit.md')):
        assert sha(repo / entry['path']) == entry['sha256'], entry['path']
assert sha(root / 'review/impl-review.json') == closeout['independentReviewSha256']
destinations = [docs / 'evidence/business-save-operation-owner-impl-20261105',
                args.vault.resolve() / 'raw/attachments/townhall-save-operation-owner-impl-2026-11-05']
assert all(not target.exists() for target in destinations), 'Never overwrite an evidence archive.'
selected = {}


def add(path, name=None):
    selected[name or path.relative_to(root).as_posix()] = payload(path)


for name in ('capture-before.py', 'capture-isolation.py', 'write-closeout.py', 'input-manifest.json', 'freeze-candidate.py', 'run-model-tests.ps1',
             'archive-implementation.py', 'implementation-closeout.json', 'isolated-pure.log',
             'isolated-verification.json', 'review-disposition.json'):
    add(root / name)
add(root / 'implementation-contract.md', 'implementation-contract.txt')
add(root / 'BusinessConstruction.before.cs', 'BusinessConstruction.before.txt')
for folder in sorted(root.glob('candidate-v*')):
    assert folder.is_dir()
    for entry in load(folder / 'freeze.json')['files']:
        assert sha(folder / entry['path']) == entry['sha256'], str(folder / entry['path'])
        add(folder / entry['path'])
    add(folder / 'freeze.json')
for name in closeout['testRunDirectories']:
    for filename in ('report.json', 'output.log', 'results.trx'):
        add(root / name / filename)
for folder_name in ('review', 'isolation'):
    folder = root / folder_name
    for path in sorted(folder.rglob('*')):
        if path.is_file() and not any(part in ('bin', 'obj') for part in path.relative_to(folder).parts):
            assert path.suffix.lower() in ('.json', '.txt', '.cs', '.csproj', '.ps1', '.py', '.trx', '.log', '.config'), str(path)
            add(path)
document_paths = ['Source/Mods/TownHallInterior/docs/' + name for name in (
    'business-save-operation-owner.md', 'business-completion-plan.md', 'business-manager-save-probe.md',
    'README.md', 'CHANGELOG.md', 'native-save-completion-audit.md', 'native-world-exit-audit.md')]
document_paths.append('docs/compatibility.md')
documents = {'schema': 1, 'capturedUtc': datetime.now(timezone.utc).isoformat(),
    'sourceRevision': freeze['sourceRevision'], 'sourceWorktree': 'dirty',
    'files': [{'path': path, 'sha256': sha(repo / path), 'bytes': (repo / path).stat().st_size} for path in document_paths]}
selected['source-documents.json'] = (json.dumps(documents, ensure_ascii=False, indent=2) + '\n').encode('utf-8')
selected['business-save-operation-owner-current.txt'] = payload(docs / 'business-save-operation-owner.md')
integrity = {'schema': 1, 'checkedUtc': datetime.now(timezone.utc).isoformat(),
    'sourceRevision': freeze['sourceRevision'], 'sourceWorktree': 'dirty',
    'onlyProductionChangeIsOwnershipBlock': True, 'oldSourceBytesPreservedByRemovingBlock': True,
    'sourceBeforeSha256': sha(root / 'BusinessConstruction.before.cs'), 'sourceAfterSha256': final['sourceSha256'],
    'newTestSha256': final['testSha256'], 'otherProtectedSourcesUnchanged': len(freeze['protectedSourcesBefore']) - 1,
    'priorArtifactsUnchangedExcludingManifests': sum(row['files'] for row in freeze['priorEvidence']),
    'priorManifestsUnchanged': len(freeze['priorEvidence']), 'originalNativeAuditDocumentsUnchanged': 2,
    'productionFiles': len(list((repo / 'Source/Mods/TownHallInterior/src').glob('*.cs'))) + 1,
    'nativeSaveWriteAllowed': False, 'installedFilesOrSaveDataRead': False,
    'status': 'source-and-archive-integrity-pass-not-native-proof'}
selected['integrity-closeout.json'] = (json.dumps(integrity, ensure_ascii=False, indent=2) + '\n').encode('utf-8')
records = []
for name, data in sorted(selected.items()):
    expected = hashlib.sha256(data).hexdigest()
    for destination in destinations:
        target = destination / name
        target.parent.mkdir(parents=True, exist_ok=True)
        with target.open('xb') as stream:
            stream.write(data)
        assert sha(target) == expected
    records.append({'path': name, 'sha256': expected, 'bytes': len(data)})
manifest = {'schema': 1, 'capturedUtc': datetime.now(timezone.utc).isoformat(),
    'project': 'schedule1-mod-only', 'sourceRevision': freeze['sourceRevision'], 'sourceWorktree': 'dirty',
    'scope': 'Synthetic managed ownership implementation, independent tests/review, retained initial failure and isolated offline checks',
    'finalCandidate': closeout['finalCandidate'], 'nativeSaveWriteAllowed': False,
    'installationOrNativeSaveDataCopied': False, 'files': records}
encoded = (json.dumps(manifest, ensure_ascii=False, indent=2) + '\n').encode('utf-8')
for destination in destinations:
    with (destination / 'manifest.json').open('xb') as stream:
        stream.write(encoded)
print(json.dumps({'archivedFilesExcludingManifest': len(records), 'totalBytesExcludingManifest': sum(row['bytes'] for row in records),
    'manifestSha256': hashlib.sha256(encoded).hexdigest(), 'allCopiesHashIdentical': True, **integrity}, ensure_ascii=False, indent=2))
