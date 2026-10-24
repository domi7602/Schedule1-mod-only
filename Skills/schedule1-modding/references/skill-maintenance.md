# Maintain this skill collection

> UNVERIFIED locally: this file was authored in the external skills package (2026-10-21) and adopted here without re-running the source checks it describes. Treat its rules as procedure, not as reproduced evidence.

## Entry point standard

Keep the existing skill name and directory stable unless a migration is explicitly planned. Use valid YAML name/description fields and one concise description that separates the skill from neighboring domains. This package adopts short Hermes descriptions; the generic Agent Skills format allows a larger field.

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

Run the package validator after changes. It checks the package profile, names, links, inventory and selected known regressions; it does not prove API semantics. Re-run focused scenario tasks for behavior the changed instruction affects. Keep expected results outside the skill body so a test cannot pass by copying its answer.

## Evaluation cases

Use realistic user tasks covering selection and execution:
- A clear task that should select this skill.
- A nearby task that should select a different skill.
- A missing-prerequisite case.
- A failure/partial-effect case.
- A normal success case.

Record the prompt, model/harness, loaded files, response/actions, rubric result and limits. Compare baseline and revised skills when claiming improved outcomes. A few passing dry runs are regression evidence, not a benchmark guarantee.

## Distribution

Distribute this collection as a coherent set because it has sibling references. Preserve the existing repository's other skills. Check the installed Hermes source path for name shadowing after replacement. Treat the current public Hermes documentation as guidance; validate features against the installed Hermes version before changing its configuration.

## Learning from incidents

An incident belongs in a scoped case note with evidence and limits. Promote a rule only after identifying which conditions generalize. Remove duplicate rules, obsolete “verified” banners and empty section clutter instead of endlessly appending corrections.
