"""Freeze source and independently authored tests; preserve every pre-existing byte."""

import argparse
from datetime import datetime, timezone
import hashlib
import json
from pathlib import Path
import re

parser = argparse.ArgumentParser(description=__doc__)
parser.add_argument('name')
args = parser.parse_args()
assert re.fullmatch(r'[a-z0-9-]+', args.name)
root = Path(__file__).resolve().parent
repo = root.parent.parent
freeze = json.loads((root / 'input-manifest.json').read_text(encoding='utf-8'))
sha = lambda value: hashlib.sha256(value).hexdigest()
source = (repo / freeze['mutableSource']).read_bytes()
before = (root / 'BusinessConstruction.before.cs').read_bytes()
assert sha(before) == freeze['sourceBeforeSha256']
pattern = rb'(?ms)^#region DataOnlySaveOperationOwnership\r?\n.*?^#endregion\r?\n\r?\n'
matches = list(re.finditer(pattern, source))
assert len(matches) == 1, 'Exactly one new ownership block is required.'
match = matches[0]
assert source[:match.start()] + source[match.end():] == before, 'Old source bytes changed.'
assert source[match.end():].startswith(b'public sealed record SubtypeResult')
unchanged = 0
for entry in freeze['protectedSourcesBefore']:
    if entry['path'] != freeze['mutableSource']:
        assert sha((repo / entry['path']).read_bytes()) == entry['sha256'], entry['path']
        unchanged += 1
for prior in freeze['priorEvidence']:
    folder = repo / prior['root']
    manifest = (folder / 'manifest.json').read_bytes()
    assert sha(manifest) == prior['manifestSha256']
    for entry in json.loads(manifest)['files']:
        assert sha((folder / entry['path']).read_bytes()) == entry['sha256'], entry['path']
test = (repo / freeze['newTest']).read_bytes()
directory = root / args.name
directory.mkdir(exist_ok=False)
selected = {'source.txt': source, 'tests.txt': test,
            'ownership-block.txt': match[0], 'implementation-contract.txt': (root / 'implementation-contract.md').read_bytes()}
for name, payload in selected.items():
    (directory / name).write_bytes(payload)
record = {'schema': 1, 'frozenUtc': datetime.now(timezone.utc).isoformat(),
    'sourceRevision': freeze['sourceRevision'], 'sourceWorktree': 'dirty',
    'scope': 'Pure synthetic ownership model and new tests; no runtime wiring or native proof',
    'sourcePath': freeze['mutableSource'], 'testPath': freeze['newTest'],
    'sourceSha256': sha(source), 'testSha256': sha(test), 'blockSha256': sha(match[0]),
    'sourceBeforeSha256': sha(before), 'oldSourceBytesPreservedByRemovingBlock': True,
    'otherProtectedSourcesUnchanged': unchanged,
    'priorArtifactsUnchangedExcludingManifests': sum(row['files'] for row in freeze['priorEvidence']),
    'priorManifestsUnchanged': len(freeze['priorEvidence']),
    'productionFiles': len(list((repo / 'Source/Mods/TownHallInterior/src').glob('*.cs'))) + 1,
    'nativeSaveWriteAllowed': False,
    'files': [{'path': name, 'sha256': sha(payload), 'bytes': len(payload)} for name, payload in selected.items()]}
(directory / 'freeze.json').write_text(json.dumps(record, ensure_ascii=False, indent=2) + '\n', encoding='utf-8')
print(json.dumps(record, ensure_ascii=False, indent=2))
