"""Freeze only authorized binary/source inputs; never open native save files."""

from datetime import datetime, timezone
from pathlib import Path
import hashlib
import json
import os
import stat
import struct
import subprocess

repo = Path.cwd()
work = repo / '.scratch/townhall-native-world-exit-audit-20261105'
previous = repo / 'Source/Mods/TownHallInterior/docs/evidence/native-save-audit-20261105'
old = json.loads((previous / 'input-manifest.json').read_text(encoding='utf-8'))
game = Path(os.environ.get('SCHEDULE1_PATH') or old['gameDirectory'])
tools = repo / '.scratch/townhall-native-save-audit-20261105/tool-bundle'


def sha(path):
    for parent in (path, *path.parents):
        if getattr(parent.lstat(), 'st_file_attributes', 0) & stat.FILE_ATTRIBUTE_REPARSE_POINT:
            raise RuntimeError('Reparse path refused: ' + str(parent))
    with path.open('rb') as stream:
        return hashlib.file_digest(stream, 'sha256').hexdigest()


names = [entry['path'] for entry in old['inputs'] if not entry['path'].startswith('UserData/')]
names.append('Mods/S1API.Il2Cpp.MelonLoader.dll')
known = {entry['path']: entry['sha256'] for entry in old['inputs']}
known['Mods/S1API.Il2Cpp.MelonLoader.dll'] = 'd751a3cb66003f7a616eb9f8054daf34da0d9d7f23416a5005a586738a680852'
inputs = []
for name in names:
    path = game / name
    value = sha(path)
    assert value == known[name], 'Input drift: ' + name
    inputs.append({'path': name, 'sha256': value, 'bytes': path.stat().st_size})
protected = []
for entry in old['protectedSources']:
    value = sha(repo / entry['path'])
    assert value == entry['sha256'], 'Protected-source drift: ' + entry['path']
    protected.append({'path': entry['path'], 'sha256': value, 'bytes': (repo / entry['path']).stat().st_size})
previous_manifest = json.loads((previous / 'manifest.json').read_text(encoding='utf-8'))
for entry in previous_manifest['files']:
    assert sha(previous / entry['path']) == entry['sha256'], 'Earlier evidence drift: ' + entry['path']
binary = (game / 'GameAssembly.dll').read_bytes()
pe = struct.unpack_from('<I', binary, 0x3c)[0]
assert binary[:2] == b'MZ' and binary[pe:pe + 4] == b'PE\0\0'
optional = pe + 24
section_count = struct.unpack_from('<H', binary, pe + 6)[0]
optional_bytes = struct.unpack_from('<H', binary, pe + 20)[0]
sections = []
for number in range(section_count):
    offset = optional + optional_bytes + number * 40
    virtual_size, rva, raw_size, raw_offset = struct.unpack_from('<IIII', binary, offset + 8)
    sections.append({'name': binary[offset:offset + 8].rstrip(b'\0').decode('ascii'),
                     'rva': hex(rva), 'virtualBytes': virtual_size, 'rawBytes': raw_size,
                     'rawOffset': hex(raw_offset)})
tool_rows = []
old_tools = json.loads((previous / 'native-map-v4.json').read_text(encoding='utf-8'))['AnalysisToolAssemblies']
for entry in old_tools:
    value = sha(tools / entry['Name'])
    assert value == entry['Sha256'], 'Analysis-tool drift: ' + entry['Name']
    tool_rows.append({'path': entry['Name'], 'sha256': value, 'identity': entry['Identity']})
command = "[pscustomobject]@{GameProcessCount=@(Get-Process | Where-Object { $_.ProcessName -in @('Schedule I','ScheduleI') }).Count;DotnetRoot=$env:DOTNET_ROOT}|ConvertTo-Json -Compress"
environment = json.loads(subprocess.check_output(['pwsh', '-NoProfile', '-NonInteractive', '-Command', command], text=True))
record = {'schema': 1, 'capturedUtc': datetime.now(timezone.utc).isoformat(),
          'project': 'schedule1-mod-only',
          'sourceRevision': subprocess.check_output(['git', 'rev-parse', 'HEAD'], text=True).strip(),
          'sourceWorktree': 'dirty', 'scope': 'Read-only native world-exit/save-abort follow-up; no build, installation, game control, native calls or save-file reads',
          'gameDirectory': str(game), 'analysisToolDirectory': str(tools),
          'inputs': inputs, 'protectedSources': protected, 'analysisTools': tool_rows,
          'earlierEvidence': {'root': previous.relative_to(repo).as_posix(),
                              'manifestSha256': sha(previous / 'manifest.json'),
                              'unchangedArtifacts': len(previous_manifest['files'])},
          'pe': {'imageBase': hex(struct.unpack_from('<Q', binary, optional + 24)[0]), 'sections': sections},
          'environment': environment, 'nativeSaveWriteAllowed': False,
          'nativeSaveFilesRead': False, 'targetAssembliesLoadedOrInvoked': False}
with (work / 'input-manifest.json').open('x', encoding='utf-8', newline='\n') as stream:
    stream.write(json.dumps(record, indent=2, ensure_ascii=False) + '\n')
print(json.dumps({'capturedUtc': record['capturedUtc'], 'inputsUnchanged': len(inputs),
                  'protectedSourcesUnchanged': len(protected), 'priorEvidenceUnchanged': len(previous_manifest['files']),
                  'analysisToolsUnchanged': len(tool_rows), 'environment': environment,
                  'nativeSaveWriteAllowed': False}, indent=2))
