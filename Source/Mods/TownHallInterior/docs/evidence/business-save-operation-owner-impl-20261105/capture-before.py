"""Preserve current work before the authorized data-only implementation."""

from datetime import datetime, timezone
import hashlib
import json
from pathlib import Path
import subprocess

root = Path(__file__).resolve().parent
repo = root.parent.parent
evidence = repo / 'Source/Mods/TownHallInterior/docs/evidence'
old = json.loads((evidence / 'business-save-operation-owner-design-20261105/input-manifest.json').read_text(encoding='utf-8'))
sha = lambda path: hashlib.sha256(path.read_bytes()).hexdigest()
for entry in old['protectedSources']:
    assert sha(repo / entry['path']) == entry['sha256'], entry['path']
previous = []
for folder in ('native-save-audit-20261105', 'native-world-exit-audit-20261105', 'business-save-operation-owner-design-20261105'):
    directory = evidence / folder
    manifest = json.loads((directory / 'manifest.json').read_text(encoding='utf-8'))
    for entry in manifest['files']:
        assert sha(directory / entry['path']) == entry['sha256'], entry['path']
    previous.append({'root': directory.relative_to(repo).as_posix(), 'manifestSha256': sha(directory / 'manifest.json'), 'files': len(manifest['files'])})
source = repo / 'Source/Mods/TownHallInterior/src/BusinessConstruction.cs'
with (root / 'BusinessConstruction.before.cs').open('xb') as stream:
    stream.write(source.read_bytes())
paths = ['Source/Tests/TownHallInterior.Tests/TownHallInterior.Tests.csproj',
         'Source/Mods/TownHallInterior/src/Mod.cs',
         'Source/Mods/TownHallInterior/docs/business-save-operation-owner.md',
         'Source/Mods/Directory.Build.props', 'Source/Mods/Directory.Build.targets']
record = {'schema': 1, 'capturedUtc': datetime.now(timezone.utc).isoformat(),
    'sourceRevision': subprocess.check_output(['git', '-C', str(repo), 'rev-parse', 'HEAD'], text=True).strip(),
    'sourceWorktree': 'dirty', 'scope': 'Implement data-only ownership model and targeted tests; no runtime port, Mod wiring, installation or game/save actions',
    'protectedSourcesBefore': old['protectedSources'], 'priorEvidence': previous,
    'mutableSource': 'Source/Mods/TownHallInterior/src/BusinessConstruction.cs',
    'newTest': 'Source/Tests/TownHallInterior.Tests/BusinessSaveOperationOwnerTests.cs',
    'productionFileCountBefore': len(list((repo / 'Source/Mods/TownHallInterior/src').glob('*.cs'))) + 1,
    'inputs': [{'path': name, 'sha256': sha(repo / name)} for name in paths],
    'sourceBeforeSha256': sha(source), 'nativeSaveWriteAllowed': False,
    'installedFilesRead': False, 'nativeSaveFilesRead': False}
with (root / 'input-manifest.json').open('x', encoding='utf-8', newline='\n') as stream:
    stream.write(json.dumps(record, ensure_ascii=False, indent=2) + '\n')
print(json.dumps({'capturedUtc': record['capturedUtc'], 'protectedSourcesBefore': len(old['protectedSources']),
    'priorArtifactsUnchanged': sum(row['files'] for row in previous), 'priorManifests': len(previous),
    'productionFiles': record['productionFileCountBefore'], 'sourceBeforeSha256': record['sourceBeforeSha256']}, indent=2))
