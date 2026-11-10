"""Narrow independent counterreview checks. Stdlib only; targets are byte inputs."""

from datetime import datetime, timezone
import hashlib
import json
import os
from pathlib import Path
import re
import stat
import struct
import subprocess

root = Path(__file__).resolve().parent.parent
repo = root.parent.parent
out = Path(__file__).with_name('independent-byte-check.json')


def read(path):
    for item in (path, *path.parents):
        assert not (getattr(item.lstat(), 'st_file_attributes', 0) &
                    stat.FILE_ATTRIBUTE_REPARSE_POINT), str(item)
    return path.read_bytes()


def sha(data):
    return hashlib.sha256(data).hexdigest()


def load(path):
    return json.loads(read(path).decode('utf-8-sig'))


started = datetime.now(timezone.utc).isoformat()
artifact_paths = sorted(p for p in root.iterdir() if p.is_file() and
                        p.name != 'native-world-exit-review.json')
for tag in ('pass1', 'pass2', 'pass3', 'pass4'):
    artifact_paths.extend(sorted((root / ('native-disassembly-' + tag)).glob('*.*')))
artifact_hashes = [{'path': p.relative_to(root).as_posix(), 'sha256': sha(read(p)),
                    'bytes': p.stat().st_size} for p in artifact_paths]
manifest = load(root / 'input-manifest.json')
game = Path(manifest['gameDirectory'])
inputs = []
native = None
metadata = None
for entry in manifest['inputs']:
    data = read(game / entry['path'])
    actual = sha(data)
    assert actual == entry['sha256'] and len(data) == entry['bytes'], entry['path']
    inputs.append({'path': entry['path'], 'sha256': actual, 'bytes': len(data), 'match': True})
    if entry['path'] == 'GameAssembly.dll':
        native = data
    elif entry['path'].endswith('global-metadata.dat'):
        metadata = data
protected = []
for entry in manifest['protectedSources']:
    data = read(repo / entry['path'])
    assert sha(data) == entry['sha256'] and len(data) == entry['bytes'], entry['path']
    protected.append({'path': entry['path'], 'sha256': sha(data), 'bytes': len(data), 'match': True})

pe = struct.unpack_from('<I', native, 0x3c)[0]
assert native[:2] == b'MZ' and native[pe:pe + 4] == b'PE\0\0'
assert struct.unpack_from('<H', native, pe + 4)[0] == 0x8664
opt = pe + 24
assert struct.unpack_from('<H', native, opt)[0] == 0x20b
base = struct.unpack_from('<Q', native, opt + 24)[0]
assert base == int(manifest['pe']['imageBase'], 16)
sections = []
section_start = opt + struct.unpack_from('<H', native, pe + 20)[0]
for n in range(struct.unpack_from('<H', native, pe + 6)[0]):
    pos = section_start + n * 40
    name = native[pos:pos + 8].rstrip(b'\0').decode('ascii')
    vsize, rva, size, raw = struct.unpack_from('<IIII', native, pos + 8)
    section = {'name': name, 'rva': hex(rva), 'rawOffset': hex(raw),
               'virtualBytes': vsize, 'rawBytes': size}
    assert section == manifest['pe']['sections'][n]
    sections.append(section)


def raw(rva, count):
    hits = [int(s['rawOffset'], 16) + rva - int(s['rva'], 16) for s in sections
            if int(s['rva'], 16) <= rva and
            rva + count <= int(s['rva'], 16) + s['rawBytes']]
    assert len(hits) == 1, hex(rva)
    return hits[0]


def native_at(rva, count):
    pos = raw(rva, count)
    return native[pos:pos + count]


rows = {}
windows = []
mapping_records = []
for tag in ('pass2', 'pass3', 'pass4'):
    mapping = load(root / ('native-map-' + tag + '.json'))
    assert mapping['NativeSha256'] == inputs[0]['sha256']
    method_map = {(m['Type'], m['Token']): m for m in mapping['MethodMap']}
    exports = {m['File']: m for m in mapping['Disassembled']}
    mapping_records.append({'path': 'native-map-' + tag + '.json',
                            'capturedUtc': mapping['CapturedUtc'],
                            'selectedMethods': len(exports),
                            'metadataVersionAsReported': mapping['MetadataVersion'],
                            'globalMethodCountAsReported': mapping['GlobalMethodCount']})
    paths = sorted((root / ('native-disassembly-' + tag)).glob('*.json'))
    assert len(paths) == len(exports)
    for path in paths:
        w = load(path)
        method = w['Method']
        assert method == method_map[(method['Type'], method['Token'])]
        assert method == exports[path.stem]['Method']
        rva = int(method['Rva'], 16)
        count = w['ByteLength']
        original = native_at(rva, count)
        assert int(w['RawOffset'], 16) == raw(rva, count)
        assert sha(original) == w['BytesSha256'] == exports[path.stem]['BytesSha256']
        assert int(w['WindowEndVa'], 16) == base + rva + count
        # Byte concatenation and text/JSON coherence, deliberately not another decoder.
        payload = b''.join(bytes.fromhex(row['Bytes']) for row in w['Instructions'])
        tail = w['UndecodedTail']
        if tail:
            assert int(tail['Rva'], 16) == rva + len(payload)
            payload += bytes.fromhex(tail['Bytes'])
        assert payload == original
        text = read(path.with_suffix('.txt')).decode('utf-8-sig').splitlines()
        text_rows = []
        for line in text:
            if line.startswith('0x'):
                match = re.fullmatch(r'(0x[0-9A-F]+)\s+((?:[0-9A-F]{2} )*[0-9A-F]{2})\s+(.*)', line)
                assert match, (path.name, line)
                text_rows.append(match.groups())
        assert len(text_rows) == len(w['Instructions'])
        cursor = rva
        for row, (trva, tb, tt) in zip(w['Instructions'], text_rows):
            assert cursor == int(row['Rva'], 16) == int(trva, 16)
            assert int(row['Va'], 16) == base + cursor
            assert bytes.fromhex(tb) == bytes.fromhex(row['Bytes'])
            assert tt == row['Text'] or tt.startswith(row['Text'] + ' ; ')
            rows[cursor] = row
            cursor += len(bytes.fromhex(row['Bytes']))
        windows.append({'path': path.relative_to(root).as_posix(), 'token': method['Token'],
                        'type': method['Type'], 'method': method['Name'], 'rva': hex(rva),
                        'bytes': count, 'bytesSha256': sha(original),
                        'linearRowsByteAndTextMatched': len(w['Instructions']),
                        'suffixBytes': tail['Length'] if tail else 0})

# Manually selected claim anchors. All compare to original bytes, not parent prose.
anchors = {
    0x5aad59: '48 8b 79 78', 0x5aad69: '4c 8b 05 48 36 3f 03',
    0x5aad73: '48 8b d3', 0x5aad7c: 'e8 6f a8 e7 ff', 0x5aad8f: 'e8 9c bf 5f 02',
    0x5a9511: '48 8b 4b 48', 0x5a9521: 'c7 41 18 00 00 00 00',
    0x5a9536: 'e8 c5 4f 60 01', 0x5a953b: '48 8b 4b 50',
    0x5a954b: 'c7 41 18 00 00 00 00', 0x5a9565: 'e9 96 4f 60 01',
    0x58690c: '48 8b f9', 0x586a16: '80 7f 28 00', 0x586bcb: 'c6 47 28 00',
    0x586bd1: '48 c7 47 40 00 00 00 00', 0x586be8: 'c6 47 29 01',
    0x586c5c: 'e9 af 2b 5f 02', 0x5ae414: '48 8b 48 78',
    0x5ae418: 'e8 d3 8a 5f 02', 0x5ae947: 'e8 24 6f 6f 00',
    0x5ae94e: '74 43', 0x5ae962: 'f3 0f 10 0d f6 e2 8a 02',
    0x5ae96d: 'e8 0e e6 5d 02', 0x5aea07: 'e8 14 95 07 00',
    0x5aea11: 'c7 43 14 00 00 40 40', 0x5aea1a: 'e8 41 5a 5d 02',
    0x5aea1f: 'f3 0f 11 43 10', 0x5aea36: '4c 8b 05 3b 3d 44 03',
    0x5aea60: 'e8 eb 1b e9 ff', 0x5ae91a: 'eb 34',
    0x5b3a8b: '48 8b 48 18', 0x5b3a94: '80 b9 30 03 00 00 00',
    0x5b3a9b: '74 08', 0x5b3a9d: 'b0 01', 0x5b3aa7: 'e8 b4 09 5d 02',
    0x5b3aac: 'f3 0f 5c 43 10', 0x5b3ab1: '0f 2f 43 14', 0x5b3ab5: '0f 97 c0',
    0x5a8fe1: '48 8b 15 58 5b 41 03', 0x5a8feb: 'e8 c0 79 ac 00',
    0x10709f2: 'e8 d9 4d 0c 00', 0x10709f7: '80 7b 20 00',
    0x10709fb: '75 48', 0x1070a0f: '33 d2', 0x1070a14: 'e8 e7 65 b1 01',
    0x1070a40: 'e8 1b a0 b0 01', 0x1135873: '0f 85 f8 00 00 00',
    0x11359c4: 'c6 43 20 01', 0x11359fc: 'e9 ef 4f a4 01',
    0x1135ae9: 'e8 b2 65 a4 01', 0x1135af0: '74 2c', 0x1135b1b: '48 89 08',
    0xa6809c: '45 33 c9', 0xa680a8: '45 33 c0', 0xa680ab: '33 d2',
    0xa680ad: 'e8 3e e8 b1 ff', 0x7dfc47: '45 33 c9', 0x7dfc53: '45 33 c0',
    0x7dfc56: '33 d2', 0x7dfc58: 'e8 93 6c da ff', 0xa681b6: 'f3 0f 10 71 2c',
    0x5881d0: '41 0f b6 e8', 0x5882ad: '80 7f 28 00', 0x5882b1: '74 33',
    0x5882b3: '40 84 ed', 0x5882b6: '75 2e', 0x588385: '48 89 57 40',
    0x5883aa: '48 89 57 38', 0x5884e7: 'e9 24 13 5f 02',
    0x5ae798: '45 33 c0', 0x5ae79f: 'e8 1c 9a fd ff',
    0x5ae883: '45 33 c0', 0x5ae88a: 'e8 31 99 fd ff',
    0x591153: 'e8 98 5d 61 02', 0x59116f: 'e8 dc 50 ff ff',
    0x591335: 'e8 d6 15 61 02', 0x590dd9: '83 f8 0a',
    0x590deb: '48 8d 15 0e f2 a6 ff', 0x590df2: '8b 8c 82 60 25 59 00',
    0x590dfc: 'ff e1', 0x43fec0: 'c2 00 00',
    0x2b79b56: '48 8b 05 8b 55 09 01', 0x2b79b65: '48 8d 0d dc e5 3f 00',
    0x2b79b6c: 'e8 7f e0 79 fd', 0x2b79b71: '48 89 05 70 55 09 01',
    0x2b79b80: '48 ff e0',
}
focused = []
for rva, expected_hex in anchors.items():
    expected = bytes.fromhex(expected_hex)
    assert native_at(rva, len(expected)) == expected == bytes.fromhex(rows[rva]['Bytes'])
    row = rows[rva]
    entry = {'rva': hex(rva), 'rawOffset': hex(raw(rva, len(expected))),
             'bytes': expected.hex(' '), 'textAsExported': row['Text']}
    if expected[0] in (0xe8, 0xe9) and len(expected) == 5:
        target = base + rva + 5 + int.from_bytes(expected[1:], 'little', signed=True)
        assert target == int(row['DirectTarget'], 16)
        entry['independentRelativeTargetVa'] = hex(target)
    elif expected[0] in (0x74, 0x75, 0xeb) and len(expected) == 2:
        target = base + rva + 2 + int.from_bytes(expected[1:], 'little', signed=True)
        assert target == int(row['DirectTarget'], 16)
        entry['independentRelativeTargetVa'] = hex(target)
    elif expected[:2] == b'\x0f\x85' and len(expected) == 6:
        target = base + rva + 6 + int.from_bytes(expected[2:], 'little', signed=True)
        assert target == int(row['DirectTarget'], 16)
        entry['independentRelativeTargetVa'] = hex(target)
    if row['RipAddress']:
        # These manually selected RIP-relative forms all end in disp32.
        target = base + rva + len(expected) + int.from_bytes(expected[-4:], 'little', signed=True)
        assert target == int(row['RipAddress'], 16)
        entry['independentRipTargetVa'] = hex(target)
        entry['metadataLabelAsExportedNotIndependentlyRemapped'] = row['MetadataGlobal']
    focused.append(entry)

table = native_at(0x592560, 44)
table_targets = list(struct.unpack('<11I', table))
assert all(v in rows and 0x590a80 <= v < 0x592560 for v in table_targets)
data_rows = [v for v in sorted(rows) if 0x592560 <= v < 0x59258c]
assert len(data_rows) == 9
assert native_at(0x59258c, 4) == b'\xcc' * 4

refs = load(root / 'world-exit-reference-candidates.json')
reference_results = []
for entry in refs['candidates']:
    rva = int(entry['rva'], 16)
    b = native_at(rva, 5)
    assert b == bytes.fromhex(entry['bytes']) and b[0] in (0xe8, 0xe9)
    target = rva + 5 + int.from_bytes(b[1:], 'little', signed=True)
    assert target == int(entry['targetRva'], 16)
    row = rows.get(rva)
    matched = bool(row and bytes.fromhex(row['Bytes']) == b and
                   int(row['DirectTarget'], 16) == base + target)
    reference_results.append({'rva': hex(rva), 'targetRva': hex(target),
                              'existingDecoderRowMatched': matched,
                              'status': 'existing-decoder-row-plus-independent-byte-target' if matched
                              else 'raw-byte-candidate-only-not-code-or-owner-proof'})
assert sum(r['existingDecoderRowMatched'] for r in reference_results) == 6

magic, version = struct.unpack_from('<II', metadata)
assert magic == 0xfab11baf and version == 31
strings, strings_size = struct.unpack_from('<II', metadata, 24)
methods, methods_size = struct.unpack_from('<II', metadata, 48)
params, params_size = struct.unpack_from('<II', metadata, 88)
assert methods_size % 36 == 0 and methods_size // 36 == 149349


def meta_string(index):
    assert 0 <= index < strings_size
    start = strings + index
    return metadata[start:metadata.index(b'\0', start)].decode('utf-8')


parameter_records = []
for token, name in ((0x060017b5, 'StartGame'), (0x060017bc, 'ExitToMenu')):
    pos = methods + ((token & 0xffffff) - 1) * 36
    ni, dt, rt, rpt, first, generic, actual_token = struct.unpack_from('<7I', metadata, pos)
    count = struct.unpack_from('<H', metadata, pos + 34)[0]
    assert actual_token == token and meta_string(ni) == name and count == 3
    parameters = []
    for j in range(count):
        p = params + (first + j) * 12
        pni, ptoken, typ = struct.unpack_from('<III', metadata, p)
        parameters.append({'name': meta_string(pni), 'token': hex(ptoken),
                           'typeIndex': typ, 'rawOffset': hex(p),
                           'bytes': metadata[p:p + 12].hex(' ')})
    parameter_records.append({'token': hex(token), 'name': name, 'declaringTypeIndex': dt,
                              'rawOffset': hex(pos), 'recordBytes': metadata[pos:pos + 36].hex(' '),
                              'parameters': parameters})
assert [p['name'] for p in parameter_records[0]['parameters']] == [
    'info', 'allowLoadStacking', 'allowSaveBackup']
assert [p['name'] for p in parameter_records[1]['parameters']] == [
    'autoLoadSave', 'mainMenuPopup', 'preventLeaveLobby']

field_map = load(root / 'native-map-pass4.json')['FieldMap']
field_expectations = {
    'ScheduleOne.Persistence.LoadManager': {'<IsGameLoaded>k__BackingField': 0x28,
        '<IsLoading>k__BackingField': 0x29, '<ActiveSaveInfo>k__BackingField': 0x40,
        '<LoadedGameFolderPath>k__BackingField': 0x38, 'onPreSceneChange': 0x78},
    'ScheduleOne.Persistence.SaveManager': {'<IsSaving>k__BackingField': 0x29,
        'Saveables': 0x48, 'BaseSaveables': 0x50, 'CompletedSaveables': 0x60,
        'QueuedSaveRequests': 0x68, '<SecondsSinceLastSave>k__BackingField': 0x2c},
    'ScheduleOne.PlayerScripts.Player': {'Local': 0x18,
        '<playerSaveRequestReturned>k__BackingField': 0x330},
    'ScheduleOne.Persistence.LoadManager+<>c__DisplayClass81_1': {
        'timeOnWaitStart': 0x10, 'maxWait': 0x14}}
checked_fields = []
for typ, fields in field_expectations.items():
    for name, offset in fields.items():
        matches = [f for f in field_map if f['Type'] == typ and f['Name'] == name]
        assert len(matches) == 1 and int(matches[0]['Offset'], 16) == offset, (typ, name)
        checked_fields.append(matches[0])

icall_raw = raw(0x2f78148, 128)
icall = native[icall_raw:native.index(b'\0', icall_raw)].decode('ascii')
assert 'MonoBehaviour::StopAllCoroutines' in icall
for entry in artifact_hashes:
    assert sha(read(root / entry['path'])) == entry['sha256'], entry['path']

result = {
    'schema': 1, 'startedUtc': started, 'completedUtc': datetime.now(timezone.utc).isoformat(),
    'kind': 'Independently authored stdlib PE/hash/payload/selected-operand checks; no x64 decoder rerun',
    'sourceRevision': subprocess.check_output(['git', '-C', str(repo), 'rev-parse', 'HEAD'], text=True).strip(),
    'dotnetRoot': os.environ.get('DOTNET_ROOT'), 'inputs': inputs, 'protectedSources': protected,
    'artifactSnapshot': artifact_hashes, 'artifactSnapshotUnchangedAtEnd': True,
    'snapshotLimit': 'Snapshot made after initial human/model reading; no pre-reading integrity snapshot claimed.',
    'sectionsParsedFromOriginalPe': sections, 'mappingRecordConsistency': mapping_records,
    'nativeWindows': windows, 'focusedOriginalByteAnchors': focused,
    'startGameDataTable': {'rva': '0x592560', 'bytes': table.hex(' '),
        'targets': [hex(v) for v in table_targets], 'excludedLinearRowRvas': [hex(v) for v in data_rows],
        'limit': '11 RVA values, not code; local switch supplement only, not full CFG/EH or indirect-call coverage.'},
    'referenceCandidates': reference_results, 'targetedOriginalMetadataRecords': parameter_records,
    'fieldMapEntriesCorrelatedWithNativeOperands': checked_fields,
    'genericFieldMapLimit': 'Open generic Destroyed offset can be zero/unresolved. Only native +0x20 read/write path used, not inferred generic field layout.',
    'floatConstants': [{'rva': hex(v), 'bytes': native_at(v, 4).hex(' '),
        'float32': struct.unpack('<f', native_at(v, 4))[0]} for v in (0x2e5cc60, 0x2e5cb40)],
    'maxWaitImmediateFloat32': struct.unpack('<f', native_at(0x5aea14, 4))[0],
    'stopAllCoroutinesIcallString': {'rva': '0x2f78148', 'value': icall},
    'counts': {'inputsHashed': len(inputs), 'protectedSourcesHashed': len(protected),
        'windowsPayloadCompared': len(windows), 'windowBytesCompared': sum(w['bytes'] for w in windows),
        'linearRowsByteAndTextMatchedNotRedecoded': sum(w['linearRowsByteAndTextMatched'] for w in windows),
        'suffixBytesMatchedNotDecoded': sum(w['suffixBytes'] for w in windows),
        'focusedByteAnchors': len(focused),
        'focusedRelativeTargetsRecomputed': sum('independentRelativeTargetVa' in a for a in focused),
        'focusedRipTargetsRecomputed': sum('independentRipTargetVa' in a for a in focused),
        'rawReferenceByteTargetsRecomputed': len(reference_results),
        'existingDecoderReferenceRowsMatched': sum(r['existingDecoderRowMatched'] for r in reference_results)},
    'targetAssembliesLoadedOrInvoked': False, 'nativeSaveFilesRead': False,
    'gameStartedOrControlled': False, 'nativeSaveWriteAllowed': False,
    'notRepeated': ['LibCpp2IL full method remap', 'Iced x64 decoding',
        'all 1416 relative-transfer checks', 'pdata/EH validation', '135-artifact prior V4 audit',
        'builds/testsuites/live baseline'],
    'status': 'pass-bounded-independent-integrity-check-not-a-release-proof'}
assert result['sourceRevision'] == manifest['sourceRevision']
with out.open('x', encoding='utf-8', newline='\n') as stream:
    stream.write(json.dumps(result, indent=2, ensure_ascii=False) + '\n')
print(json.dumps({'status': result['status'], 'counts': result['counts'],
                  'floatConstants': result['floatConstants'],
                  'maxWaitImmediateFloat32': result['maxWaitImmediateFloat32'],
                  'metadataParameters': parameter_records,
                  'stopAllCoroutinesIcallString': result['stopAllCoroutinesIcallString'],
                  'outputSha256': sha(read(out))}, indent=2, ensure_ascii=False))
