"""Make archived log bytes checkout-visible as text; retain original manifests unchanged."""

from datetime import datetime, timezone
import hashlib
import json
from pathlib import Path

root = Path(__file__).resolve().parent
repo = root.parent.parent
repo_evidence = repo / 'Source/Mods/TownHallInterior/docs/evidence'
vault_evidence = Path('C:/Users/pc/Documents/LLM-WIKI/raw/attachments')
sha = lambda data: hashlib.sha256(data).hexdigest()
helper = Path(__file__).read_bytes()
results = []
for repo_name, vault_name in (
    ('business-save-operation-owner-impl-20261105', 'townhall-save-operation-owner-impl-2026-11-05'),
    ('business-save-operation-owner-final-checks-20261105', 'townhall-save-operation-owner-final-checks-2026-11-05')):
    folders = [repo_evidence / repo_name, vault_evidence / vault_name]
    original = (folders[0] / 'manifest.json').read_bytes()
    old_manifest = json.loads(original)
    mapping = {entry['path']: entry['path'] + '.txt' for entry in old_manifest['files'] if entry['path'].endswith('.log')}
    assert mapping
    correction = {'schema': 1, 'recordedUtc': datetime.now(timezone.utc).isoformat(),
        'scope': 'Post-check evidence filename normalization only; code, tests, canonical docs and original report bytes unchanged',
        'reason': 'Repository ignores *.log. Store the same original output bytes as *.log.txt for a future ordinary checkout.',
        'originalManifestSha256': sha(original), 'preservedManifest': 'manifest-before-log-paths.json',
        'archivePathBindings': mapping, 'allArtifactBytesUnchanged': True, 'nativeSaveWriteAllowed': False}
    correction_bytes = (json.dumps(correction, ensure_ascii=False, indent=2) + '\n').encode('utf-8')
    entries = [{**entry, 'path': mapping.get(entry['path'], entry['path'])} for entry in old_manifest['files']]
    added = [('manifest-before-log-paths.json', original), ('log-path-correction.json', correction_bytes),
             ('normalize-log-artifacts.py', helper)]
    for name, data in added:
        entries.append({'path': name, 'sha256': sha(data), 'bytes': len(data)})
    manifest = {**old_manifest, 'capturedUtc': correction['recordedUtc'],
        'logArtifactPathBindings': mapping, 'files': sorted(entries, key=lambda entry: entry['path'])}
    encoded = (json.dumps(manifest, ensure_ascii=False, indent=2) + '\n').encode('utf-8')
    for folder in folders:
        assert (folder / 'manifest.json').read_bytes() == original
        assert not (folder / 'manifest-before-log-paths.json').exists()
        for entry in old_manifest['files']:
            assert sha((folder / entry['path']).read_bytes()) == entry['sha256']
        assert all(not (folder / name).exists() for name in mapping.values())
    for folder in folders:
        for old, new in mapping.items():
            (folder / old).rename(folder / new)
        (folder / 'manifest.json').rename(folder / 'manifest-before-log-paths.json')
        for name, data in added[1:] + [('manifest.json', encoded)]:
            with (folder / name).open('xb') as stream:
                stream.write(data)
        for entry in manifest['files']:
            assert sha((folder / entry['path']).read_bytes()) == entry['sha256']
    results.append({'archive': repo_name, 'renamedOriginalLogs': len(mapping),
        'filesExcludingManifest': len(entries), 'manifestSha256': sha(encoded), 'allCopiesHashIdentical': True})
print(json.dumps(results, indent=2))
