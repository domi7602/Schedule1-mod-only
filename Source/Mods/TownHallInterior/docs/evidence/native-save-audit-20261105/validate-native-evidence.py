"""Read-only byte/integrity checks; never loads target assemblies or reads saves."""

import argparse
from collections import Counter
from datetime import datetime, timezone
import hashlib
import json
from pathlib import Path
import re
import struct
import subprocess


def digest(path, algorithm='sha256'):
    with path.open('rb') as stream:
        return hashlib.file_digest(stream, algorithm).hexdigest()


def load(path):
    return json.loads(path.read_text(encoding='utf-8-sig'))


parser = argparse.ArgumentParser(description=__doc__)
parser.add_argument('--evidence', type=Path, required=True)
parser.add_argument('--repository', type=Path, required=True)
parser.add_argument('--output', type=Path, required=True)
args = parser.parse_args()
root = args.evidence.resolve()
repo = args.repository.resolve()
manifest = load(root / 'input-manifest.json')
game = Path(manifest['gameDirectory'])
result = {
    'schema': 1,
    'checkedUtc': datetime.now(timezone.utc).isoformat(),
    'kind': 'Read-only byte/hash/control-transfer check, not a second full decoder or semantic proof',
    'sourceRevision': subprocess.check_output(['git', '-C', str(repo), 'rev-parse', 'HEAD'], text=True).strip(),
    'sourceWorktree': 'dirty',
    'inputs': [],
    'protectedSources': [],
    'nativeWindows': [],
    'nativeWriteRouteAllowed': False,
    'targetAssemblyLoadedOrInvoked': False,
    'nativeSaveFilesRead': False,
}
assert result['sourceRevision'] == manifest['sourceRevision']
for entry in manifest['inputs']:
    path = game / entry['path']
    actual = digest(path)
    assert actual == entry['sha256'] and path.stat().st_size == entry['bytes'], entry['path']
    result['inputs'].append({'path': entry['path'], 'sha256': actual, 'unchanged': True})
for entry in manifest['protectedSources']:
    actual = digest(repo / entry['path'])
    assert actual == entry['sha256'], entry['path']
    result['protectedSources'].append({'path': entry['path'], 'sha256': actual, 'unchanged': True})

data = (game / 'GameAssembly.dll').read_bytes()
pe = struct.unpack_from('<I', data, 0x3c)[0]
assert data[:2] == b'MZ' and data[pe:pe + 4] == b'PE\0\0'
assert struct.unpack_from('<H', data, pe + 4)[0] == 0x8664
optional = pe + 24
assert struct.unpack_from('<H', data, optional)[0] == 0x20b
base = struct.unpack_from('<Q', data, optional + 24)[0]
sections = []
section_count = struct.unpack_from('<H', data, pe + 6)[0]
optional_bytes = struct.unpack_from('<H', data, pe + 20)[0]
for number in range(section_count):
    offset = optional + optional_bytes + number * 40
    virtual_size, rva, raw_size, raw_offset = struct.unpack_from('<IIII', data, offset + 8)
    sections.append((rva, raw_offset, raw_size))


def raw(rva, length):
    matches = [offset + rva - start for start, offset, size in sections
               if start <= rva and rva + length <= start + size]
    assert len(matches) == 1, hex(rva)
    return matches[0]


exception_rva, exception_bytes = struct.unpack_from('<II', data, optional + 112 + 3 * 8)
assert exception_bytes % 12 == 0
exception_offset = raw(exception_rva, exception_bytes)
pdata = {begin: (end, unwind) for begin, end, unwind in struct.iter_unpack(
    '<III', data[exception_offset:exception_offset + exception_bytes])}

config_path = game / 'MelonLoader/Dependencies/Il2CppAssemblyGenerator/Config.cfg'
configured = re.search(r'GameAssemblyHash = "([0-9A-Fa-f]+)"', config_path.read_text(encoding='utf-8-sig')).group(1).lower()
actual512 = hashlib.sha512(data).hexdigest()
assert configured == actual512
result['generatorBinding'] = {'sha512Matches': True, 'nativeSha512': actual512, 'configSha256': digest(config_path)}

mapping = load(root / 'native-map-v4.json')
method_map = {entry['Token']: entry for entry in mapping['MethodMap']}
assert len(method_map) == 70 and mapping['GlobalMetadataMethodCount'] == 149349
result['mapping'] = {'selectedMethods': len(method_map), 'globalMetadataMethods': 149349,
                     'metadataParserVersion': mapping['MetadataVersion']}
stub_checked = []
for path in sorted((root / 'native-metadata').glob('*.stub.cs')):
    for token, address in re.findall(r'\[Token\(Token = "0x([0-9A-Fa-f]+)"\)\]\s*\[Address\(RVA = "0x([0-9A-Fa-f]+)"', path.read_text(encoding='utf-8-sig')):
        key = f'0x{int(token, 16):08X}'
        assert int(address, 16) == int(method_map[key]['Rva'], 16), (path.name, key)
        stub_checked.append(key)
result['mapping']['dummyAddressTokensCrossChecked'] = len(stub_checked)

expected_fields = {
    'ScheduleOne.Persistence.SaveManager': {
        '<IsSaving>k__BackingField': 0x29, 'Saveables': 0x48, 'BaseSaveables': 0x50,
        'CompletedSaveables': 0x60, 'QueuedSaveRequests': 0x68, 'onSaveStart': 0x78, 'onSaveComplete': 0x80,
    },
    'ScheduleOne.Persistence.SaveRequest': {'Saveable': 0x10, 'ParentFolderPath': 0x18, '<SaveString>k__BackingField': 0x20},
}
for name, fields in expected_fields.items():
    found = {f['Name']: int(f['Offset'], 16) for f in mapping['FieldMap'] if f['Type'] == name}
    for field, offset in fields.items():
        assert found[field] == offset, (name, field)
assert method_map['0x06001785']['InterfaceOrVirtualSlot'] == 13
assert method_map['0x06001787']['InterfaceOrVirtualSlot'] == 15
result['mapping']['focusedFieldOffsetsChecked'] = sum(len(fields) for fields in expected_fields.values())
result['mapping']['interfaceSlotsChecked'] = {'WriteBaseData': 13, 'CompleteSave': 15}

counts = Counter()
for path in sorted((root / 'native-disassembly-v4').glob('*.json')):
    window = load(path)
    start = int(window['Method']['Rva'], 16)
    assert window['Method'] == method_map[window['Method']['Token']]
    length = window['ByteLength']
    offset = raw(start, length)
    assert offset == int(window['RawOffset'], 16)
    native_bytes = data[offset:offset + length]
    assert hashlib.sha256(native_bytes).hexdigest() == window['BytesSha256']
    assert base + start + length == int(window['WindowEndVa'], 16)
    bounds = window['PrimaryPdataBounds']
    assert bounds['BeginRva'] == start
    assert pdata[start] == (bounds['EndRva'], bounds['UnwindRva'])
    cursor = start
    branches = 0
    combined = bytearray()
    for instruction in window['Instructions']:
        chunk = bytes.fromhex(instruction['Bytes'])
        assert int(instruction['Rva'], 16) == cursor
        assert int(instruction['Va'], 16) == base + cursor
        assert data[raw(cursor, len(chunk)):raw(cursor, len(chunk)) + len(chunk)] == chunk
        combined.extend(chunk)
        target = instruction.get('DirectTarget')
        if target:
            if chunk[0] in (0xe8, 0xe9) and len(chunk) == 5:
                displacement = int.from_bytes(chunk[1:], 'little', signed=True)
            elif (chunk[0] == 0xeb or 0x70 <= chunk[0] <= 0x7f) and len(chunk) == 2:
                displacement = int.from_bytes(chunk[1:], 'little', signed=True)
            elif chunk[0] == 0x0f and 0x80 <= chunk[1] <= 0x8f and len(chunk) == 6:
                displacement = int.from_bytes(chunk[2:], 'little', signed=True)
            else:
                raise AssertionError(('Unsupported direct transfer encoding', path.name, instruction))
            assert base + cursor + len(chunk) + displacement == int(target, 16)
            branches += 1
        global_hint = instruction.get('MetadataGlobal')
        if global_hint:
            assert not global_hint.get('ResolutionError'), (path.name, instruction)
            assert int(instruction['RipAddress'], 16) % 8 == 0
            counts[global_hint['Kind']] += 1
        cursor += len(chunk)
    assert cursor == start + length and bytes(combined) == native_bytes
    result['nativeWindows'].append({'file': path.relative_to(root).as_posix(), 'token': window['Method']['Token'],
                                    'rva': hex(start), 'bytes': length, 'instructions': len(window['Instructions']),
                                    'directTransfersChecked': branches, 'bytesSha256': window['BytesSha256'],
                                    'primaryPdataMatches': True})
    counts['instructions'] += len(window['Instructions'])
    counts['directTransfers'] += branches
    counts['rawBytes'] += length

managed = load(root / 'managed-boundary-review.json')
artifact = managed['installedArtifact']
assert digest(game / artifact['relativeGamePath']) == artifact['sha256']
result['s1apiRecheck'] = {'sha256': artifact['sha256'], 'unchanged': True}
result['counts'] = dict(counts)
result['resolutionErrors'] = 0
result['status'] = 'pass-bounded-integrity-checks-no-save-or-release-approval'
result['limits'] = [
    'Instruction bytes and relative branch targets are checked independently of the PowerShell exporter; x64 mnemonics are not decoded a second time.',
    'Metadata symbols/slots/offsets rely on the fresh LibCpp2IL map; hashes and selected dummy tokens bind inputs, not every semantic annotation.',
    'Window ends at the next distinct managed-method pointer; padding/cold fragments are included, exact full function/exception/transitive coverage is not claimed.',
    'No runtime observation, no last-use proof, no success/atomicity/durability proof, no save-file comparison.',
]
with args.output.open('x', encoding='utf-8', newline='\n') as stream:
    stream.write(json.dumps(result, indent=2, ensure_ascii=False) + '\n')
print(json.dumps({'status': result['status'], 'protectedSources': len(result['protectedSources']),
                  'inputsUnchanged': len(result['inputs']), 'windowsChecked': len(result['nativeWindows']),
                  'mapping': result['mapping'], 'counts': result['counts'], 'resolutionErrors': 0}, indent=2))
