"""Find raw rel32 call/jump candidates. A decoded containing window must confirm each hit."""

import argparse
from datetime import datetime, timezone
from pathlib import Path
import hashlib
import json
import struct

parser = argparse.ArgumentParser(description=__doc__)
parser.add_argument('--root', type=Path, default=Path(__file__).resolve().parent)
parser.add_argument('--target', action='append', default=[])
parser.add_argument('--output', default='direct-reference-candidates.json')
args = parser.parse_args()
manifest = json.loads((args.root / 'input-manifest.json').read_text(encoding='utf-8'))
path = Path(manifest['gameDirectory']) / 'GameAssembly.dll'
data = path.read_bytes()
assert hashlib.sha256(data).hexdigest() == manifest['inputs'][0]['sha256']
base = int(manifest['pe']['imageBase'], 16)
targets = {'SaveManager.Clean': 0x5a94e0, 'SaveManager.ClearCompletedSaveable': 0x5a9a40,
           'SaveManager.DequeueSaveRequest': 0x5a9fa0, 'SaveRequest.Complete': 0x5aba60,
           'SaveRoutine.MoveNext': 0x5b0c20}
for requested in args.target:
    label, rva = requested.split('=', 1)
    targets[label] = int(rva, 0)
by_rva = {address: label for label, address in targets.items()}
hits = []
checked = 0
for section in manifest['pe']['sections']:
    if section['name'] not in ('.text', 'il2cpp'):
        continue
    raw = int(section['rawOffset'], 16)
    rva = int(section['rva'], 16)
    end = raw + section['rawBytes']
    for opcode in (b'\xe8', b'\xe9'):
        offset = raw
        while True:
            offset = data.find(opcode, offset, end - 4)
            if offset < 0:
                break
            checked += 1
            current = rva + offset - raw
            target = current + 5 + struct.unpack_from('<i', data, offset + 1)[0]
            if target in by_rva:
                hits.append({'rva': hex(current), 'va': hex(base + current), 'rawOffset': hex(offset),
                             'bytes': data[offset:offset + 5].hex(' '), 'targetRva': hex(target),
                             'target': by_rva[target], 'kind': 'raw-E8/E9-candidate-not-yet-decoder-confirmed'})
            offset += 1
record = {'schema': 1, 'capturedUtc': datetime.now(timezone.utc).isoformat(),
          'gameAssemblySha256': hashlib.sha256(data).hexdigest(),
          'scope': 'Raw E8/E9 rel32 search in both executable code sections; ignores indirect calls, pointers, external modules and runtime patches',
          'targets': {name: hex(rva) for name, rva in targets.items()},
          'rawOpcodesExamined': checked, 'candidates': sorted(hits, key=lambda row: int(row['rva'], 16)),
          'limits': 'Raw bytes can be data or mid-instruction. Each positive hit needs a containing decoded window; absence is not a complete caller proof.',
          'gameInvoked': False, 'saveFilesRead': False}
with (args.root / args.output).open('x', encoding='utf-8', newline='\n') as output:
    output.write(json.dumps(record, indent=2, ensure_ascii=False) + '\n')
print(json.dumps(record, indent=2, ensure_ascii=False))
