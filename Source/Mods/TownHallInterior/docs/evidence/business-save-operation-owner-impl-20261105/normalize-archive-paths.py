"""Keep an archived probe csproj as inert text, preserving its bytes and the initial manifest."""

from datetime import datetime, timezone
import hashlib
import json
from pathlib import Path

root = Path(__file__).resolve().parent
repo = root.parent.parent
folders = [repo / 'Source/Mods/TownHallInterior/docs/evidence/business-save-operation-owner-impl-20261105',
           Path('C:/Users/pc/Documents/LLM-WIKI/raw/attachments/townhall-save-operation-owner-impl-2026-11-05')]
sha = lambda data: hashlib.sha256(data).hexdigest()
initial_bytes = (folders[0] / 'manifest.json').read_bytes()
assert sha(initial_bytes) == '7f3578f6ea538316f8e55821ff7eaac642a28b7f8d016dc8e13e37f853f079e8'
initial = json.loads(initial_bytes)
old_path = 'review/probe/ReviewProbes.csproj'
new_path = old_path + '.txt'
record = {'schema': 1, 'recordedUtc': datetime.now(timezone.utc).isoformat(),
    'kind': 'Archive packaging path correction only; no source/test or review-content change',
    'reason': 'Tools/gen-sln.ps1:26 recursively discovers every csproj under Source/Mods, including archived evidence. Keep this nonproduction probe artifact inert as text.',
    'originalManifestSha256': sha(initial_bytes), 'preservedOriginalManifest': 'manifest-initial.json',
    'pathBinding': {'originalReviewProbeProject': old_path, 'archivedInertProjectBytes': new_path},
    'artifactBytesUnchanged': True, 'nativeSaveWriteAllowed': False}
correction = (json.dumps(record, ensure_ascii=False, indent=2) + '\n').encode('utf-8')
helper = Path(__file__).read_bytes()
manifest = dict(initial)
manifest['capturedUtc'] = record['recordedUtc']
manifest['archivePackagingCorrection'] = record['pathBinding']
entries = [dict(entry) for entry in initial['files']]
for entry in entries:
    if entry['path'] == old_path:
        entry['path'] = new_path
for name, data in [('manifest-initial.json', initial_bytes), ('archive-path-correction.json', correction),
                   ('normalize-archive-paths.py', helper)]:
    entries.append({'path': name, 'sha256': sha(data), 'bytes': len(data)})
manifest['files'] = sorted(entries, key=lambda entry: entry['path'])
encoded = (json.dumps(manifest, ensure_ascii=False, indent=2) + '\n').encode('utf-8')
for folder in folders:
    assert (folder / 'manifest.json').read_bytes() == initial_bytes
    assert not (folder / new_path).exists() and not (folder / 'manifest-initial.json').exists()
    for entry in initial['files']:
        assert sha((folder / entry['path']).read_bytes()) == entry['sha256']
for folder in folders:
    (folder / old_path).rename(folder / new_path)
    (folder / 'manifest.json').rename(folder / 'manifest-initial.json')
    for name, data in [('archive-path-correction.json', correction), ('normalize-archive-paths.py', helper), ('manifest.json', encoded)]:
        with (folder / name).open('xb') as stream:
            stream.write(data)
    for entry in manifest['files']:
        assert sha((folder / entry['path']).read_bytes()) == entry['sha256']
print(json.dumps({'filesExcludingManifest': len(entries), 'manifestSha256': sha(encoded),
    'initialManifestPreserved': True, 'probeProjectBytesUnchanged': True,
    'allCopiesHashIdentical': True}, indent=2))
