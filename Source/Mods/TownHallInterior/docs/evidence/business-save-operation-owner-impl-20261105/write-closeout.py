"""Record the actual bounded implementation outcome and the formatting-only review disposition."""

from datetime import datetime, timezone
import hashlib
import json
from pathlib import Path
import re
import xml.etree.ElementTree as ET

root = Path(__file__).resolve().parent
load = lambda path: json.loads(path.read_text(encoding='utf-8-sig'))
sha = lambda path: hashlib.sha256(path.read_bytes()).hexdigest()
v2 = load(root / 'candidate-v2/freeze.json')
v3 = load(root / 'candidate-v3/freeze.json')
assert v2['sourceSha256'] == v3['sourceSha256']
old_test = (root / 'candidate-v2/tests.txt').read_bytes()
new_test = (root / 'candidate-v3/tests.txt').read_bytes()
assert re.sub(rb'\s+', b'', old_test) == re.sub(rb'\s+', b'', new_test)
assert old_test != new_test
report = load(root / 'review/impl-review.json')
assert report['semanticVerdict'] == 'accepted'
assert len(report['requiredCorrections']) == 1 and report['requiredCorrections'][0]['id'] == 'R01'
final_run = load(root / 'townhall-final-v3/report.json')
assert final_run['Status'] == 'passed' and final_run['Counters']['Passed'] == 809
results = ET.parse(root / 'townhall-final-v3/results.trx').findall('.//{*}UnitTestResult')
new_results = [row for row in results if 'BusinessSaveOperationOwnerTests.' in row.get('testName', '')]
assert len(new_results) == 128 and all(row.get('outcome') == 'Passed' for row in new_results)
review_sha = sha(root / 'review/impl-review.json')
disposition = {'schema': 1, 'recordedUtc': datetime.now(timezone.utc).isoformat(),
    'kind': 'Parent disposition, not a rewritten or new independent review',
    'originalReviewSha256': review_sha, 'originalReviewCandidate': 'candidate-v2',
    'originalVerdictUnchanged': report['verdict'], 'originalSemanticVerdict': report['semanticVerdict'],
    'originalCriteriaCountsUnchanged': report['criteriaCounts'],
    'finalCandidate': 'candidate-v3', 'productionHashUnchanged': True,
    'testsDifferOnlyInWhitespaceBytes': True, 'testValuesAndAssertionsUnchanged': True,
    'requiredCorrection': {'id': 'R01', 'status': 'closed-by-parent-format-and-regression-check',
        'change': 'Only multiline with-initializer whitespace; no non-whitespace test byte changed.',
        'formatterCommand': 'dotnet format whitespace Source/Tests/TownHallInterior.Tests/TownHallInterior.Tests.csproj --no-restore --verify-no-changes --include Source/Tests/TownHallInterior.Tests/BusinessSaveOperationOwnerTests.cs --verbosity minimal',
        'formatterExitCode': 0, 'finalRegressionPassed': 809, 'includedNewModelCasesPassed': 128},
    'optionalO01': 'Not added to permanent tests in this slice; original independent probe sources and 33-group results are retained.',
    'newIndependentSemanticReviewOfV3Claimed': False,
    'fullPureRerunAfterWhitespaceCorrection': False,
    'originalPureResultRemainsFailed': True,
    'nativeSaveWriteAllowed': False}
closeout = {'schema': 1, 'recordedUtc': datetime.now(timezone.utc).isoformat(),
    'sourceRevision': 'b2c6c7e6c6ecc778da9624cdb68a6578b9db471a', 'sourceWorktree': 'dirty',
    'finalCandidate': 'candidate-v3', 'sourceSha256': v3['sourceSha256'], 'testSha256': v3['testSha256'],
    'independentReviewSha256': review_sha, 'independentReviewCandidate': 'candidate-v2',
    'independentSemanticVerdict': 'accepted', 'reviewR01Disposition': 'format-only correction verified by parent',
    'testRunDirectories': ['owner-targeted-v1', 'owner-targeted-v2', 'townhall-regression-v2', 'townhall-final-v3'],
    'initialTargeted': {'total': 121, 'passed': 120, 'failed': 1, 'kind': 'preserved initial bookkeeping defect'},
    'finalTownHall': {'total': 809, 'passed': 809, 'failed': 0, 'includedNewModelCases': 128},
    'independentProbeGroups': {'passed': 33, 'failed': 0, 'includedRaceIterations': 200},
    'isolatedPure': {'exitCode': 1, 'passedSteps': 20, 'failedSteps': 2,
        'failedStepsOriginal': ['format', 'versions'], 'xunitPassed': 4740, 'consolePassed': 265, 'toolSuitesPassed': 14,
        'ownFormattingFailureCorrectedSeparately': True, 'unrelatedVersionDriftUnchanged': True},
    'proofs': {'ReferenceProof': 'unknown', 'NativeWriteProof': 'unknown', 'ModDataProof': 'unknown'},
    'nativeSaveWriteAllowed': False, 'runtimeWiringImplemented': False,
    'installedFilesReadOrChanged': False, 'nativeOrGameOrSaveActions': False,
    'modBuildAgainstGameReferencesExecuted': False, 'commitOrPushExecuted': False}
for name, value in (('review-disposition.json', disposition), ('implementation-closeout.json', closeout)):
    with (root / name).open('x', encoding='utf-8', newline='\n') as stream:
        stream.write(json.dumps(value, ensure_ascii=False, indent=2) + '\n')
print(json.dumps(closeout, ensure_ascii=False, indent=2))
