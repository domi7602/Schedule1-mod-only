"""Archive selected nonbinary evidence without replacing an existing archive."""

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
vault = args.vault.resolve()


def sha(path):
    for parent in (path, *path.parents):
        assert not (getattr(parent.lstat(), 'st_file_attributes', 0) & stat.FILE_ATTRIBUTE_REPARSE_POINT), str(parent)
    return hashlib.sha256(path.read_bytes()).hexdigest()


def encode(value):
    return (json.dumps(value, ensure_ascii=False, indent=2) + '\n').encode('utf-8')


freeze = json.loads((root / 'input-manifest.json').read_text(encoding='utf-8'))
assert subprocess.check_output(['git', '-C', str(repo), 'rev-parse', 'HEAD'], text=True).strip() == freeze['sourceRevision']
source_paths = ['Source/Mods/TownHallInterior/docs/' + name for name in (
    'native-world-exit-audit.md', 'business-manager-save-probe.md', 'business-completion-plan.md',
    'README.md', 'CHANGELOG.md', 'native-save-completion-audit.md')]
source_paths.append('docs/compatibility.md')
source_record = {'schema': 1, 'capturedUtc': datetime.now(timezone.utc).isoformat(),
                 'sourceRevision': freeze['sourceRevision'], 'sourceWorktree': 'dirty',
                 'scope': 'Exact maintained repository documents after world-exit audit; prior native-save audit included unchanged',
                 'files': [{'path': name, 'sha256': sha(repo / name), 'bytes': (repo / name).stat().st_size}
                           for name in source_paths]}
selected = [root / name for name in (
    'input-manifest.json', 'inspect-inputs.py', 'scan-direct-references.py',
    'direct-reference-candidates.json', 'world-exit-reference-candidates.json',
    'map-world-exit-pass1.ps1', 'map-world-exit-pass2.ps1', 'map-world-exit.ps1',
    'selection-pass1.json', 'selection-pass3.json', 'selection-pass4.json',
    'native-map-inventory.json', 'native-map-inventory2.json',
    'native-map-pass2.json', 'native-map-pass3.json', 'native-map-pass4.json',
    'native-byte-integrity.json', 'native-integrity-final.json', 'native-integrity-closeout.json',
    'validate-world-exit-evidence.py', 'world-exit-claim-corrections.json',
    'world-exit-claims.json', 'own-lifecycle-review.json', 'native-world-exit-review.json',
    'archive-world-exit-evidence.py')]
for tag in ('inventory', 'inventory2', 'pass1', 'pass2', 'pass3', 'pass4'):
    selected.extend(root / (tag + '.' + suffix + '.txt') for suffix in ('stdout', 'stderr'))
assert all(path.is_file() for path in selected), 'Required original evidence is missing.'
for folder in ('native-disassembly-pass2', 'native-disassembly-pass3', 'native-disassembly-pass4',
               'own-review', 'native-review'):
    selected.extend(sorted((root / folder).rglob('*')) if (root / folder).is_dir() else [])
selected = sorted(set(path for path in selected if path.is_file()))
assert (root / 'native-world-exit-review.json') in selected
assert (root / 'native-integrity-final.json') in selected
assert sum(path.suffix == '.json' and path.parent.name.startswith('native-disassembly-') for path in selected) == 24
assert all(path.suffix in ('.json', '.txt', '.py', '.ps1') for path in selected)
destinations = [repo / 'Source/Mods/TownHallInterior/docs/evidence/native-world-exit-audit-20261105',
                vault / 'raw/attachments/townhall-native-world-exit-audit-2026-11-05']
assert all(not path.exists() for path in destinations), 'Never overwrite an existing evidence archive.'
with (root / 'source-documents.json').open('xb') as stream:
    stream.write(encode(source_record))
selected = sorted([*selected, root / 'source-documents.json'])
records = []
for source in selected:
    name = source.relative_to(root).as_posix()
    content = source.read_bytes()
    expected = sha(source)
    for destination in destinations:
        target = destination / name
        target.parent.mkdir(parents=True, exist_ok=True)
        with target.open('xb') as stream:
            stream.write(content)
        assert sha(target) == expected
    assert sha(source) == expected
    records.append({'path': name, 'sha256': expected, 'bytes': len(content)})
record = {'schema': 1, 'capturedUtc': datetime.now(timezone.utc).isoformat(),
          'project': 'schedule1-mod-only', 'sourceRevision': freeze['sourceRevision'], 'sourceWorktree': 'dirty',
          'scope': 'Selected original text/JSON evidence and analysis scripts for bounded static native world-exit audit',
          'nativeSaveWriteAllowed': False, 'runtimeApproval': False, 'targetBinariesMetadataOrSavesCopied': False,
          'earlierEvidenceManifestSha256': freeze['earlierEvidence']['manifestSha256'],
          'originalWorkingPathsAreProvenanceNotPermanentSourceDependencies': True,
          'archivedPathBindings': {'primaryFreeze': 'input-manifest.json',
              'ownIndependentSnapshot': 'own-review/input-snapshot.json',
              'ownFinalReadback': 'own-review/final-input-verification.json',
              'nativeClaims': 'world-exit-claims.json', 'nativeReview': 'native-world-exit-review.json',
              'byteCheck': 'native-byte-integrity.json', 'finalIntegrity': 'native-integrity-closeout.json',
              'reviewDispositions': 'world-exit-claim-corrections.json',
              'maintainedRepositoryDocuments': 'source-documents.json'},
          'files': records}
manifest_bytes = encode(record)
for destination in destinations:
    with (destination / 'manifest.json').open('xb') as stream:
        stream.write(manifest_bytes)
print(json.dumps({'archivedFilesExcludingManifest': len(records), 'totalBytesExcludingManifest': sum(row['bytes'] for row in records),
                  'manifestSha256': hashlib.sha256(manifest_bytes).hexdigest(),
                  'destinations': [str(path) for path in destinations], 'allCopiesHashIdentical': True}, indent=2))
