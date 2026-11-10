"""Validate report consistency and recheck the two live frozen inputs without a build."""
from collections import Counter
from datetime import datetime, timezone
from pathlib import Path
import hashlib
import json

review = Path(__file__).resolve().parent
root = review.parent
repo = root.parent.parent
report_path = review / 'impl-review.json'
report = json.loads(report_path.read_text(encoding='utf-8'))
sha = lambda p: hashlib.sha256(p.read_bytes()).hexdigest()
checks = []
def check(name, value):
    checks.append(dict(check=name, passed=bool(value)))

for key, count_key in (('criteria', 'criteriaCounts'), ('designCounterexamples', 'designCounterexampleCounts')):
    counts = Counter(c['status'] for c in report[key])
    check(key + ' status counts', all(counts[k] == v for k, v in report[count_key].items()))
    check(key + ' unique IDs', len({c['id'] for c in report[key]}) == len(report[key]))
check('16 criteria', len(report['criteria']) == 16)
check('15 design cases', len(report['designCounterexamples']) == 15)
check('one formatting-only required correction', len(report['requiredCorrections']) == 1 and report['requiredCorrections'][0]['category'] == 'formatting-only')
check('whole review keeps required correction visible', report['verdict'] == 'needs-correction' and report['semanticVerdict'] == 'accepted')
check('all native flags remain false', not report['nativeSaveWriteAllowed'] and not report['safeToDestroy'] and not report['safeToUnregister'])
check('all native proofs remain unknown', len(report['nativeProofs']) == 3 and set(report['nativeProofs'].values()) == {'unknown'})
for item in report['reviewedHashes']:
    check('reviewed hash remains exact: ' + item['path'], sha(repo / item['path']) == item['sha256'])
for item in report['reportArtifacts']:
    check('artifact exists: ' + item, (review / item).is_file())
pure = report['inheritedRunsNotReexecuted'][-1]
check('Pure failed not reclassified', pure['status'] == 'failed' and pure['exitCode'] == 1 and pure['passedSteps'] == 20 and pure['failedSteps'] == 2)
check('33 probe groups, not 233 tests', report['independentlyExecutedChecks'][2]['counters']['probeGroups'] == 33)
result = dict(schema=1, validatedUtc=datetime.now(timezone.utc).isoformat(), reportSha256=sha(report_path),
              counts=dict(total=len(checks), passed=sum(c['passed'] for c in checks), failed=sum(not c['passed'] for c in checks)),
              checks=checks, candidateIsStillFrozenV2=True, nativeSaveWriteAllowed=False)
output = review / 'report-validation.json'
assert not output.exists()
output.write_text(json.dumps(result, ensure_ascii=False, indent=2) + '\n', encoding='utf-8')
print(json.dumps(dict(path=str(output), validatedUtc=result['validatedUtc'], counts=result['counts'],
                      reportSha256=result['reportSha256'], verdict=report['verdict'], semanticVerdict=report['semanticVerdict']), indent=2))
raise SystemExit(0 if all(c['passed'] for c in checks) else 1)
