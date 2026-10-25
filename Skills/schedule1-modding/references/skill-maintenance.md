# Maintain this skill collection

> UNVERIFIED locally: this file was authored in the external skills package (2026-10-21) and adopted here without re-running the source checks it describes. Treat its rules as procedure, not as reproduced evidence.

## Entry point standard

Keep names/directories stable unless a migration is planned. Use YAML `name` and `description`; make the description distinguish its task from neighboring skills. Keyword catalogs and arbitrary character quotas are unnecessary.

Keep the main file focused on routing, prerequisites, decisions, failure handling and observable completion. Move detailed domain knowledge into references with a concrete read trigger. Do not add a historical incident to every entry point.

## Correct the active instruction

When a source contract changes:
1. Identify the owning reference.
2. Update or remove the incorrect active example.
3. Search the whole collection for the affected member/assumption.
4. Update call sites, quick references and troubleshooting guidance together.
5. Record source scope and run the relevant checks.

Never leave a known-bad recipe active with only a correction appended elsewhere.

## Evidence and examples

Use [evidence policy](evidence-policy.md). Label an implementation contract, a source sketch and a compiled example differently. A directly reusable code example needs exact dependencies and a real compile check. If the environment is absent, preserve useful requirements and source lookup paths without inventing an API-shaped template.

## Quality checks

Use this repository's checks, invoked as `pwsh -NoProfile -ExecutionPolicy Bypass -File Tools/<script>.ps1`: `check-skills-health.ps1` and `check-doc-paths.ps1` cover structure; `check-xml.ps1` parses tracked MSBuild files. `verify.ps1 -Mode Quick` runs the required quick gates. Follow `AGENTS.md` for isolated checkout verification after documentation/tool changes. These checks do not establish API semantics or gameplay acceptance.

If changing the checker, run `Tools/Tests/skills-health.Tests.ps1`. Use an external package validator only when producing that external distribution, with its actual path supplied.

## Evaluation cases

For changed routing or behavior, try relevant normal, neighboring-task, missing-input or partial-effect scenarios. Keep expected answers outside the skill body. Record inputs, loaded files, outcomes and limits; baseline/revision comparisons support improvement claims, while a few dry runs are only regression evidence.

## Distribution

Distribute the coherent collection because sibling references are dependencies; preserve unrelated skills. For installation work, verify the actually loaded source and name shadowing against the installed tool. Repository editing alone does not require installation changes.

## Learning from incidents

Keep incidents in scoped case notes. Generalize only supported conditions; remove duplicate explanations while retaining dated evidence, hashes, attribution and limitations. Owner-accepted work remains closed under the [evidence policy](evidence-policy.md).

## Package verification and installation

For current repository checks, use the [maintenance workflow](#quality-checks). The following describes the external 2026-10-21 distribution, not prerequisites for routine edits here.

Its outer handover contains `LIESMICH.md`, `RECHERCHE.md`, `PRUEFBERICHT.md`, a content manifest and a standard-library Python validator. Keep those files outside the active skill directory. That validator checks package integrity/structure, not compilation, gameplay or SDK compatibility.

For Hermes, verify the actual loaded source after replacement. Current documentation describes precedence as project, local user skills, configured creation directory, then external directories. A higher-priority old copy can shadow the updated repository copy. Check behavior against the installed Hermes version; do not assume changing one directory changes the active skill.

## Local adoption of the 2026-10-21 professional package

The collection in this folder was replaced on 2026-10-21 with the externally prepared professional revision
(`Skills-Professional-2026-10-21`), then adapted locally. The previous collection is retained outside every
active skill search path under `%LOCALAPPDATA%/Schedule1Tools/SkillsBackups/2026-10-21-astra-baseline/`
(full tree, ZIP and a SHA-256 manifest of all 158 files).

Local deviations from the delivered package, applied deliberately and therefore **not** matching its manifest hashes:

- **Verification provenance restored:** 106 reference files had lost the dated `> verified:` / `> UNVERIFIED:`
  line during the external edit. That line was restored verbatim from the previous collection for each file;
  no date, scope or result was reformulated. The three new shared references carry an explicit
  `> UNVERIFIED locally:` line because their checks were not re-run here.
- **Repository conventions re-applied:** every `SKILL.md` again links the workspace `AGENTS.md` pointer line
  (20 were missing) and carries the previous `Keywords:` list inside its description (23 were missing).
  <!-- skills-health: version-evidence (historical package adoption, not a dependency pin) -->
  The five `S1API 3.2.1` mentions are annotated as the requested target rather than an installed-build pin.
- **Byte hygiene:** all files are UTF-8/LF with a final newline.

Checks after adoption: `Tools/check-skills-health.ps1` → **clean (0 warnings)**,
`Tools/check-doc-paths.ps1` → exit 0, all 137 reference files carry a header, 23/23 entries link `AGENTS.md`
and carry `Keywords:`. This is document-level acceptance only: no mod was built and no game session was run.
