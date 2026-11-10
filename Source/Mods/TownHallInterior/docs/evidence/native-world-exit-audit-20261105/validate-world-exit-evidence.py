"""Independent stdlib byte/RVA/rel-transfer checks; no game execution or save access."""

import argparse
from collections import Counter
from datetime import datetime, timezone
import hashlib
import json
from pathlib import Path
import stat
import struct
import subprocess

parser = argparse.ArgumentParser(description=__doc__)
parser.add_argument('--evidence', type=Path, required=True)
parser.add_argument('--repository', type=Path, required=True)
parser.add_argument('--output', type=Path, required=True)
args = parser.parse_args()
root = args.evidence.resolve()
repo = args.repository.resolve()


def load(path):
    return json.loads(path.read_text(encoding='utf-8-sig'))


def digest(path):
    for parent in (path, *path.parents):
        assert not (getattr(parent.lstat(), 'st_file_attributes', 0) & stat.FILE_ATTRIBUTE_REPARSE_POINT), str(parent)
    with path.open('rb') as stream:
        return hashlib.file_digest(stream, 'sha256').hexdigest()


manifest = load(root / 'input-manifest.json')
result = {'schema': 1, 'checkedUtc': datetime.now(timezone.utc).isoformat(),
          'kind': 'Independent stdlib byte/hash/RVA/relative-target check, not a second x64 decoder or semantic proof',
          'sourceRevision': subprocess.check_output(['git', '-C', str(repo), 'rev-parse', 'HEAD'], text=True).strip(),
          'sourceWorktree': 'dirty', 'inputs': [], 'protectedSources': [], 'nativeWindows': [],
          'nativeSaveWriteAllowed': False, 'targetAssembliesLoadedOrInvoked': False,
          'gameInvoked': False, 'nativeSaveFilesRead': False}
assert result['sourceRevision'] == manifest['sourceRevision']
game = Path(manifest['gameDirectory'])
for entry in manifest['inputs']:
    path = game / entry['path']
    assert digest(path) == entry['sha256'] and path.stat().st_size == entry['bytes'], entry['path']
    result['inputs'].append({'path': entry['path'], 'sha256': entry['sha256'], 'unchanged': True})
for entry in manifest['protectedSources']:
    assert digest(repo / entry['path']) == entry['sha256'], entry['path']
    result['protectedSources'].append({'path': entry['path'], 'sha256': entry['sha256'], 'unchanged': True})
prior_root = repo / manifest['earlierEvidence']['root']
assert digest(prior_root / 'manifest.json') == manifest['earlierEvidence']['manifestSha256']
prior_manifest = load(prior_root / 'manifest.json')
for entry in prior_manifest['files']:
    assert digest(prior_root / entry['path']) == entry['sha256'], entry['path']
result['earlierEvidence'] = {'manifestSha256': digest(prior_root / 'manifest.json'),
                             'unchangedArtifacts': len(prior_manifest['files'])}
for entry in manifest['analysisTools']:
    assert digest(Path(manifest['analysisToolDirectory']) / entry['path']) == entry['sha256'], entry['path']
result['analysisToolsUnchanged'] = len(manifest['analysisTools'])
data = (game / 'GameAssembly.dll').read_bytes()
pe = struct.unpack_from('<I', data, 0x3c)[0]
assert data[:2] == b'MZ' and data[pe:pe + 4] == b'PE\0\0'
assert struct.unpack_from('<H', data, pe + 4)[0] == 0x8664
optional = pe + 24
assert struct.unpack_from('<H', data, optional)[0] == 0x20b
base = struct.unpack_from('<Q', data, optional + 24)[0]
assert base == int(manifest['pe']['imageBase'], 16)
sections = []
for number in range(struct.unpack_from('<H', data, pe + 6)[0]):
    offset = optional + struct.unpack_from('<H', data, pe + 20)[0] + number * 40
    virtual_size, rva, raw_size, raw_offset = struct.unpack_from('<IIII', data, offset + 8)
    sections.append((rva, raw_offset, raw_size))


def raw(rva, length):
    matches = [offset + rva - start for start, offset, size in sections
               if start <= rva and rva + length <= start + size]
    assert len(matches) == 1, hex(rva)
    return matches[0]


exception_rva, exception_size = struct.unpack_from('<II', data, optional + 112 + 3 * 8)
assert exception_size % 12 == 0
exception_offset = raw(exception_rva, exception_size)
pdata = list(struct.iter_unpack('<III', data[exception_offset:exception_offset + exception_size]))
counts = Counter()
instructions_by_rva = {}
window_by_token = {}
for tag in ('pass2', 'pass3', 'pass4'):
    mapping = load(root / ('native-map-' + tag + '.json'))
    assert mapping['GlobalMethodCount'] == 149349 and mapping['MetadataVersion'] == 31.1
    assert mapping['NativeSha256'] == manifest['inputs'][0]['sha256']
    methods = {(entry['Type'], entry['Token']): entry for entry in mapping['MethodMap']}
    expected = {entry['File']: entry for entry in mapping['Disassembled']}
    paths = sorted((root / ('native-disassembly-' + tag)).glob('*.json'))
    assert len(paths) == len(expected)
    for path in paths:
        window = load(path)
        method = window['Method']
        assert method == methods[(method['Type'], method['Token'])]
        assert expected[path.stem]['Method'] == method
        start = int(method['Rva'], 16)
        length = window['ByteLength']
        offset = raw(start, length)
        native_bytes = data[offset:offset + length]
        assert offset == int(window['RawOffset'], 16)
        assert hashlib.sha256(native_bytes).hexdigest() == window['BytesSha256']
        assert expected[path.stem]['BytesSha256'] == window['BytesSha256']
        assert base + start + length == int(window['WindowEndVa'], 16)
        bounds = window['PrimaryPdataBounds']
        containing = [row for row in pdata if row[0] <= start < row[1]]
        if bounds:
            assert containing == [(bounds['BeginRva'], bounds['EndRva'], bounds['UnwindRva'])]
        else:
            assert not containing
        combined = bytearray()
        cursor = start
        transfers = 0
        for instruction in window['Instructions']:
            chunk = bytes.fromhex(instruction['Bytes'])
            assert int(instruction['Rva'], 16) == cursor and int(instruction['Va'], 16) == base + cursor
            assert data[raw(cursor, len(chunk)):raw(cursor, len(chunk)) + len(chunk)] == chunk
            target = instruction.get('DirectTarget')
            if target:
                if chunk[0] in (0xe8, 0xe9) and len(chunk) == 5:
                    displacement = int.from_bytes(chunk[1:], 'little', signed=True)
                elif (chunk[0] == 0xeb or 0x70 <= chunk[0] <= 0x7f) and len(chunk) == 2:
                    displacement = int.from_bytes(chunk[1:], 'little', signed=True)
                elif chunk[0] == 0x0f and 0x80 <= chunk[1] <= 0x8f and len(chunk) == 6:
                    displacement = int.from_bytes(chunk[2:], 'little', signed=True)
                else:
                    raise AssertionError(('Unsupported relative transfer', path.name, instruction))
                assert base + cursor + len(chunk) + displacement == int(target, 16)
                transfers += 1
            annotation = instruction.get('MetadataGlobal')
            assert not (annotation and annotation.get('ResolutionError')), (path.name, instruction)
            combined.extend(chunk)
            instructions_by_rva[cursor] = instruction
            cursor += len(chunk)
        tail = window.get('UndecodedTail')
        if tail:
            chunk = bytes.fromhex(tail['Bytes'])
            assert int(tail['Rva'], 16) == cursor and int(tail['Va'], 16) == base + cursor
            assert len(chunk) == tail['Length']
            assert chunk == data[raw(cursor, len(chunk)):raw(cursor, len(chunk)) + len(chunk)]
            combined.extend(chunk)
            cursor += len(chunk)
            counts['undecodedSuffixBytes'] += len(chunk)
        assert cursor == start + length and bytes(combined) == native_bytes
        assert len(window['Instructions']) == expected[path.stem]['Instructions']
        window_by_token[method['Token']] = window
        result['nativeWindows'].append({'file': path.relative_to(root).as_posix(), 'token': method['Token'],
                                        'rva': hex(start), 'bytes': length,
                                        'linearDecodedRows': len(window['Instructions']),
                                        'undecodedSuffixBytes': tail['Length'] if tail else 0,
                                        'relativeTransfersChecked': transfers,
                                        'bytesSha256': window['BytesSha256'], 'pdataPresenceOrBoundsMatched': True})
        counts['linearDecodedRows'] += len(window['Instructions'])
        counts['relativeTransfers'] += transfers
        counts['rawWindowBytes'] += length

# The StartGame state dispatcher itself identifies 11 RVA table entries. Do not
# promote the 9 accidental linear-decode rows inside this data to code evidence.
table_start = 0x592560
table_bytes = data[raw(table_start, 44):raw(table_start, 44) + 44]
assert instructions_by_rva[0x590dd9]['Bytes'] == '83 F8 0A'
assert instructions_by_rva[0x590df2]['Bytes'] == '8B 8C 82 60 25 59 00'
assert instructions_by_rva[0x590dfc]['Bytes'] == 'FF E1'
table_targets = list(struct.unpack('<11I', table_bytes))
assert all(address in instructions_by_rva and 0x590a80 <= address < table_start for address in table_targets)
assert data[raw(table_start + 44, 4):raw(table_start + 44, 4) + 4] == b'\xcc' * 4
data_rows = [rva for rva in instructions_by_rva if table_start <= rva < table_start + 44]
counts['knownDataLinearRowsExcludedFromCodeClaims'] = len(data_rows)
result['startGameDataSupplement'] = {'dispatcherCompareRva': '0x590dd9', 'tableLoadRva': '0x590df2',
    'indirectJumpRva': '0x590dfc', 'tableRva': hex(table_start), 'entries': 11, 'bytes': table_bytes.hex(' '),
    'targetRvas': [hex(address) for address in table_targets], 'targetsHaveDecodedRowBoundaries': True,
    'linearRowsInsideTable': [hex(address) for address in data_rows],
    'undecodedSuffix': window_by_token['0x060017E4']['UndecodedTail'],
    'limit': 'Supplement identifies local state-switch data only; no full CFG/EH coverage or dynamic-target proof.'}
mapping = load(root / 'native-map-pass4.json')
expected_fields = {
    'ScheduleOne.Persistence.LoadManager': {'<IsGameLoaded>k__BackingField': 0x28,
        '<IsLoading>k__BackingField': 0x29, '<ActiveSaveInfo>k__BackingField': 0x40,
        'onPreSceneChange': 0x78, 'onSceneChangeDone': 0x80},
    'ScheduleOne.Persistence.SaveManager': {'<IsSaving>k__BackingField': 0x29,
        'Saveables': 0x48, 'BaseSaveables': 0x50, 'CompletedSaveables': 0x60, 'QueuedSaveRequests': 0x68},
    'ScheduleOne.PlayerScripts.Player': {'Local': 0x18, '<playerSaveRequestReturned>k__BackingField': 0x330},
    'ScheduleOne.Persistence.LoadManager+<>c__DisplayClass81_1': {'timeOnWaitStart': 0x10, 'maxWait': 0x14}}
for name, fields in expected_fields.items():
    found = {entry['Name']: int(entry['Offset'], 16) for entry in mapping['FieldMap'] if entry['Type'] == name}
    for field, offset in fields.items():
        assert found[field] == offset, (name, field)
result['focusedFieldOffsetsChecked'] = sum(len(fields) for fields in expected_fields.values())
result['genericFieldOffsetLimit'] = 'Open-generic field offsets can be unresolved/zero; no semantic claim relies on their reported offsets.'
result['floatConstants'] = []
for rva in (0x2e5cc60, 0x2e5cb40):
    chunk = data[raw(rva, 4):raw(rva, 4) + 4]
    result['floatConstants'].append({'rva': hex(rva), 'bytes': chunk.hex(' '), 'float32': struct.unpack('<f', chunk)[0]})
refs = load(root / 'world-exit-reference-candidates.json')
result['referenceCandidates'] = []
for candidate in refs['candidates']:
    site = int(candidate['rva'], 16)
    instruction = instructions_by_rva.get(site)
    confirmed = bool(instruction and instruction['Bytes'].lower() == candidate['bytes'].lower()
                     and int(instruction['DirectTarget'], 16) == base + int(candidate['targetRva'], 16))
    result['referenceCandidates'].append({'rva': hex(site), 'target': candidate['target'],
                                          'confirmedInNewWindows': confirmed,
                                          'status': 'decoded-transfer-confirmed' if confirmed else 'raw-candidate-only-not-reviewed-here'})
result['counts'] = dict(counts)
result['mapping'] = {'globalMethods': 149349, 'mappedMethodsPass4': len(mapping['MethodMap']),
                     'mappedFieldsPass4': len(mapping['FieldMap']), 'metadataVersion': mapping['MetadataVersion']}
command = "[pscustomobject]@{GameProcessCount=@(Get-Process | Where-Object { $_.ProcessName -in @('Schedule I','ScheduleI') }).Count;DotnetRoot=$env:DOTNET_ROOT}|ConvertTo-Json -Compress"
result['environment'] = json.loads(subprocess.check_output(['pwsh', '-NoProfile', '-NonInteractive', '-Command', command], text=True))
result['status'] = 'pass-bounded-byte-integrity-no-save-or-release-approval'
result['limits'] = ['No second x64 mnemonic decode; LibCpp2IL metadata labels and semantics require separate review.',
    'Linear decoding includes padding and potentially data; known StartGame table rows are expressly excluded from code claims.',
    'Next-pointer windows and primary pdata are not complete function/EH/transitive coverage.',
    'Raw xref absence is not a complete caller/abort proof; indirect calls, delegates and Unity engine internals remain outside that search.',
    'No game/runtime observation, no last-use, write-success, durability or sidecar consistency proof.']
with args.output.open('x', encoding='utf-8', newline='\n') as stream:
    stream.write(json.dumps(result, indent=2, ensure_ascii=False) + '\n')
print(json.dumps({'status': result['status'], 'inputsUnchanged': len(result['inputs']),
    'protectedSourcesUnchanged': len(result['protectedSources']), 'earlierEvidence': result['earlierEvidence'],
    'nativeWindowsChecked': len(result['nativeWindows']), 'counts': result['counts'],
    'referenceCandidates': len(result['referenceCandidates']),
    'confirmedInNewWindows': sum(row['confirmedInNewWindows'] for row in result['referenceCandidates']),
    'floatConstants': result['floatConstants'], 'environment': result['environment']}, indent=2))
