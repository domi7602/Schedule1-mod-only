"""Freeze repository design sources; no installed files, saves, builds or runtime calls."""

from datetime import datetime, timezone
import hashlib
import json
from pathlib import Path
import stat
import subprocess

root = Path(__file__).resolve().parent
repo = root.parent.parent
docs = repo / 'Source/Mods/TownHallInterior/docs'


def sha(path):
    for parent in (path, *path.parents):
        assert not (getattr(parent.lstat(), 'st_file_attributes', 0) & stat.FILE_ATTRIBUTE_REPARSE_POINT), str(parent)
    with path.open('rb') as stream:
        return hashlib.file_digest(stream, 'sha256').hexdigest()


prior_freeze = json.loads((docs / 'evidence/native-world-exit-audit-20261105/input-manifest.json').read_text(encoding='utf-8'))
protected = []
for entry in prior_freeze['protectedSources']:
    assert sha(repo / entry['path']) == entry['sha256'], entry['path']
    protected.append({'path': entry['path'], 'sha256': entry['sha256']})
prior = []
for folder, expected in (
        ('native-save-audit-20261105', '9283c0f0089391acdec90318164a56e6b9ee5c29fc7c63c88569f882d58ef795'),
        ('native-world-exit-audit-20261105', '6176543cd25dd937e5c4475775d2f18bb24c01a77430c95b196fc7dcd6363ffa')):
    directory = docs / 'evidence' / folder
    assert sha(directory / 'manifest.json') == expected
    manifest = json.loads((directory / 'manifest.json').read_text(encoding='utf-8'))
    for entry in manifest['files']:
        assert sha(directory / entry['path']) == entry['sha256'], entry['path']
    prior.append({'root': directory.relative_to(repo).as_posix(), 'manifestSha256': expected,
                  'unchangedFilesExcludingManifest': len(manifest['files'])})
source_names = ['AGENTS.md', 'Skills/schedule1-modding/SKILL.md',
    'Skills/schedule1-modding/references/working-contract.md',
    'Skills/schedule1-modding/references/evidence-policy.md',
    'Skills/schedule1-modding/references/spec-first-workflow.md',
    'Skills/schedule1-persistence/SKILL.md', 'Skills/schedule1-lifecycle-verify/SKILL.md',
    'Source/Mods/TownHallInterior/src/BusinessConstruction.cs',
    'Source/Mods/TownHallInterior/src/Mod.cs',
    'Source/Mods/TownHallInterior/src/TownHallInterior.csproj']
source_names.extend('Source/Mods/TownHallInterior/docs/' + name for name in (
    'native-save-completion-audit.md', 'native-world-exit-audit.md',
    'business-manager-save-probe.md', 'business-completion-plan.md'))
record = {'schema': 1, 'capturedUtc': datetime.now(timezone.utc).isoformat(),
    'project': 'schedule1-mod-only',
    'sourceRevision': subprocess.check_output(['git', '-C', str(repo), 'rev-parse', 'HEAD'], text=True).strip(),
    'sourceWorktree': 'dirty',
    'scope': 'Offline specification and independent design review only; no production/test edits, implementation, builds, testsuites, installed-file or save access',
    'protectedSources': protected, 'priorEvidence': prior,
    'designSources': [{'path': name, 'sha256': sha(repo / name), 'bytes': (repo / name).stat().st_size}
                      for name in source_names],
    'protectedDocuments': ['Source/Mods/TownHallInterior/docs/native-save-completion-audit.md',
                           'Source/Mods/TownHallInterior/docs/native-world-exit-audit.md'],
    'nativeSaveWriteAllowed': False, 'implementationPerformed': False,
    'installedFilesRead': False, 'nativeSaveFilesRead': False,
    'targetAssembliesLoadedOrInvoked': False}
with (root / 'input-manifest.json').open('x', encoding='utf-8', newline='\n') as stream:
    stream.write(json.dumps(record, ensure_ascii=False, indent=2) + '\n')
print(json.dumps({'capturedUtc': record['capturedUtc'], 'protectedSourcesUnchanged': len(protected),
    'priorEvidenceUnchangedExcludingManifests': sum(row['unchangedFilesExcludingManifest'] for row in prior),
    'priorManifestsUnchanged': len(prior), 'designSources': len(source_names),
    'nativeSaveWriteAllowed': False}, indent=2))
