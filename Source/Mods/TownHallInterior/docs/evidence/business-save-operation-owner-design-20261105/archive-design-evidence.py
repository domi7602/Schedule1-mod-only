"""Hash-only source closeout and one-time archive; never reads installed game files."""

import argparse
from datetime import datetime, timezone
import hashlib
import json
from pathlib import Path
import stat
import subprocess

parser = argparse.ArgumentParser(description=__doc__)
parser.add_argument('--vault', type=Path, required=True)
args = parser.parse_args()
root = Path(__file__).resolve().parent
repo = root.parent.parent
docs = repo / 'Source/Mods/TownHallInterior/docs'


def sha(path):
    for parent in (path, *path.parents):
        assert not (getattr(parent.lstat(), 'st_file_attributes', 0) & stat.FILE_ATTRIBUTE_REPARSE_POINT), str(parent)
    return hashlib.sha256(path.read_bytes()).hexdigest()


def load(path):
    return json.loads(path.read_text(encoding='utf-8-sig'))


def write_new(path, value):
    with path.open('x', encoding='utf-8', newline='\n') as stream:
        stream.write(json.dumps(value, ensure_ascii=False, indent=2) + '\n')


freeze = load(root / 'input-manifest.json')
assert subprocess.check_output(['git', '-C', str(repo), 'rev-parse', 'HEAD'], text=True).strip() == freeze['sourceRevision']
for entry in freeze['protectedSources']:
    assert sha(repo / entry['path']) == entry['sha256'], entry['path']
for previous in freeze['priorEvidence']:
    directory = repo / previous['root']
    assert sha(directory / 'manifest.json') == previous['manifestSha256']
    for entry in load(directory / 'manifest.json')['files']:
        assert sha(directory / entry['path']) == entry['sha256'], entry['path']
mutable_inputs = {'Source/Mods/TownHallInterior/docs/business-manager-save-probe.md',
                  'Source/Mods/TownHallInterior/docs/business-completion-plan.md'}
source_changes = []
for entry in freeze['designSources']:
    actual = sha(repo / entry['path'])
    if actual != entry['sha256']:
        assert entry['path'] in mutable_inputs, entry['path']
        source_changes.append({'path': entry['path'], 'beforeSha256': entry['sha256'],
                               'afterSha256': actual, 'reason': 'Authorized design navigation/next-step update, not original audit rewrite'})
draft = load(root / 'design-freeze.json')
assert sha(root / draft['snapshot']) == draft['sha256']
disposition = load(root / 'owner-design-disposition.json')
assert sha(root / 'owner-design-review.json') == disposition['originalReviewSha256']
final_source = docs / 'business-save-operation-owner.md'
final_payload = final_source.read_bytes()
assert hashlib.sha256(final_payload).hexdigest() == disposition['finalDesignSha256']
for filename in ('source-documents.json', 'integrity-closeout.json'):
    assert not (root / filename).exists(), filename
destinations = [docs / 'evidence/business-save-operation-owner-design-20261105',
                args.vault.resolve() / 'raw/attachments/townhall-save-operation-owner-design-2026-11-05']
assert all(not path.exists() for path in destinations), 'Never overwrite an evidence archive.'
required = ['capture-inputs.py', 'input-manifest.json', 'design-freeze.json',
            'owner-design-review.json', 'owner-design-disposition.json', 'archive-design-evidence.py']
assert all((root / name).is_file() for name in required)
snapshot_paths = ['Source/Mods/TownHallInterior/docs/' + name for name in (
    'business-save-operation-owner.md', 'business-manager-save-probe.md',
    'business-completion-plan.md', 'README.md', 'CHANGELOG.md',
    'native-save-completion-audit.md', 'native-world-exit-audit.md')]
snapshot_paths.append('docs/compatibility.md')
write_new(root / 'source-documents.json', {'schema': 1, 'capturedUtc': datetime.now(timezone.utc).isoformat(),
    'sourceRevision': freeze['sourceRevision'], 'sourceWorktree': 'dirty',
    'files': [{'path': name, 'sha256': sha(repo / name), 'bytes': (repo / name).stat().st_size} for name in snapshot_paths]})
write_new(root / 'integrity-closeout.json', {'schema': 1, 'checkedUtc': datetime.now(timezone.utc).isoformat(),
    'sourceRevision': freeze['sourceRevision'], 'sourceWorktree': 'dirty',
    'protectedSourcesUnchanged': len(freeze['protectedSources']),
    'priorEvidenceUnchangedExcludingManifests': sum(row['unchangedFilesExcludingManifest'] for row in freeze['priorEvidence']),
    'priorManifestsUnchanged': len(freeze['priorEvidence']), 'protectedAuditDocumentsUnchanged': len(freeze['protectedDocuments']),
    'unchangedDesignInputs': len(freeze['designSources']) - len(source_changes), 'authorizedDocumentationInputChanges': source_changes,
    'draftV1Sha256': draft['sha256'], 'originalReviewSha256': disposition['originalReviewSha256'],
    'finalDesignSha256': disposition['finalDesignSha256'], 'nativeSaveWriteAllowed': False,
    'installedFilesRead': False, 'targetAssembliesLoadedOrInvoked': False,
    'productionOrTestChanged': False, 'buildOrTestsuiteExecuted': False,
    'status': 'pass-source-and-archive-preservation-not-native-proof'})
selected = {name: (root / name).read_bytes() for name in required + ['source-documents.json', 'integrity-closeout.json']}
# Keep original Markdown bytes as .txt: its original relative links retain their
# source-document meaning and must not become new maintained archive link targets.
selected['business-save-operation-owner-v1.txt'] = (root / draft['snapshot']).read_bytes()
selected['business-save-operation-owner-final.txt'] = final_payload
if (root / 'design-review').is_dir():
    for path in sorted((root / 'design-review').rglob('*')):
        if path.is_file():
            assert path.suffix in ('.json', '.txt', '.py'), path.name
            selected[path.relative_to(root).as_posix()] = path.read_bytes()
for path in sorted(root.glob('owner-design-review-followup*.json')):
    selected[path.name] = path.read_bytes()
records = []
for name, payload in sorted(selected.items()):
    expected = hashlib.sha256(payload).hexdigest()
    for destination in destinations:
        target = destination / name
        target.parent.mkdir(parents=True, exist_ok=True)
        with target.open('xb') as stream:
            stream.write(payload)
        assert sha(target) == expected
    records.append({'path': name, 'sha256': expected, 'bytes': len(payload)})
manifest = {'schema': 1, 'capturedUtc': datetime.now(timezone.utc).isoformat(),
    'project': 'schedule1-mod-only', 'sourceRevision': freeze['sourceRevision'], 'sourceWorktree': 'dirty',
    'scope': 'Offline ownership design, immutable reviewed draft, independent review, disposition and source integrity; no implementation or native approval',
    'archivedPathBindings': {'originalReviewedMarkdownSnapshot': 'business-save-operation-owner-v1.txt',
        'finalMaintainedDesignSnapshot': 'business-save-operation-owner-final.txt',
        'primaryFreeze': 'input-manifest.json', 'independentReview': 'owner-design-review.json',
        'reviewDisposition': 'owner-design-disposition.json', 'finalSourceIntegrity': 'integrity-closeout.json'},
    'nativeSaveWriteAllowed': False, 'installedBinariesMetadataSavesOrConfigurationCopied': False, 'files': records}
for destination in destinations:
    write_new(destination / 'manifest.json', manifest)
print(json.dumps({'archivedFilesExcludingManifest': len(records),
    'totalBytesExcludingManifest': sum(row['bytes'] for row in records),
    'manifestSha256': sha(destinations[0] / 'manifest.json'),
    'protectedSourcesUnchanged': len(freeze['protectedSources']),
    'priorEvidenceUnchangedExcludingManifests': sum(row['unchangedFilesExcludingManifest'] for row in freeze['priorEvidence']),
    'maintainedDocumentHashes': len(snapshot_paths), 'allCopiesHashIdentical': True}, indent=2))
