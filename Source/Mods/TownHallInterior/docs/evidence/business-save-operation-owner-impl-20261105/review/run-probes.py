"""Build only the private no-package probe project; NuGet feeds are explicitly cleared."""
from datetime import datetime, timezone
from pathlib import Path
import hashlib
import json
import os
import subprocess
import sys

review = Path(__file__).resolve().parent
root = review.parent
repo = root.parent.parent
assert not os.environ.get('DOTNET_ROOT'), 'Do not overwrite ambient DOTNET_ROOT.'
output = review / 'probe-run.json'
assert not output.exists()
sha = lambda p: hashlib.sha256(p.read_bytes()).hexdigest()
freeze = json.loads((root / 'candidate-v2/freeze.json').read_text(encoding='utf-8'))
files = [root / 'candidate-v2/source.txt', root / 'candidate-v2/tests.txt',
         repo / freeze['sourcePath'], repo / freeze['testPath'],
         review / 'probe/Program.cs', review / 'probe/ReviewProbes.csproj', review / 'probe/NuGet.Config']
before = {str(p): sha(p) for p in files}
assert before[str(root / 'candidate-v2/source.txt')] == freeze['sourceSha256']
game = subprocess.run(['pwsh', '-NoProfile', '-NonInteractive', '-Command',
    '@(Get-Process -Name "Schedule I" -ErrorAction SilentlyContinue).Count'],
    capture_output=True, text=True, check=True).stdout.strip()
assert game == '0', 'Pure probe build is not run while the game is open.'

project = review / 'probe/ReviewProbes.csproj'
commands = [
    ('restore', ['dotnet', 'restore', str(project), '--configfile', str(review / 'probe/NuGet.Config'), '-p:S1NoDeploy=true', '-p:NuGetAudit=false']),
    ('build', ['dotnet', 'build', str(project), '-c', 'Release', '--no-restore', '-p:S1NoDeploy=true', '-p:UseSharedCompilation=false']),
    ('probe', ['dotnet', str(review / 'probe/bin/Release/net6.0/ReviewProbes.dll')]),
]
record = dict(schema=1, startedUtc=datetime.now(timezone.utc).isoformat(),
              reviewer='GPT-6.1 Sol, independent verifier', gameProcessCount=int(game), dotnetRoot=None,
              offlineOnly=True, mainProjectBuildOrRestore=False, commands=[], inputHashes=before)
for name, arguments in commands:
    done = subprocess.run(arguments, cwd=review, capture_output=True, text=True, timeout=90)
    (review / (name + '.log')).write_text(done.stdout + done.stderr, encoding='utf-8')
    record['commands'].append(dict(name=name, arguments=arguments, exitCode=done.returncode,
                                   log=name + '.log', logSha256=sha(review / (name + '.log'))))
    if done.returncode != 0:
        break
record['finishedUtc'] = datetime.now(timezone.utc).isoformat()
record['inputFilesUnchanged'] = all(sha(p) == before[str(p)] for p in files)
record['allCommandsPassed'] = len(record['commands']) == len(commands) and all(c['exitCode'] == 0 for c in record['commands'])
if record['allCommandsPassed']:
    record['probeResults'] = json.loads((review / 'probe.log').read_text(encoding='utf-8'))
output.write_text(json.dumps(record, ensure_ascii=False, indent=2) + '\n', encoding='utf-8')
print(json.dumps(record, ensure_ascii=False, indent=2))
sys.exit(0 if record['allCommandsPassed'] and record['inputFilesUnchanged'] else 1)
