# Documentation Index

| Document | Audience | Content |
|---|---|---|
| [`../README.md`](../README.md) | Players | Mod overview, compatibility, installation, download. |
| [`repository-maintenance.md`](repository-maintenance.md) | Maintainers | Current repository work, documentation ownership, local storage and WorldLook completion evidence. |
| [`history/agent-mod-history-20261024.md`](history/agent-mod-history-20261024.md) | Maintainers, researchers | Dated inherited mod details and former checklists; historical evidence, not the current backlog. |
| [`history/repository-maintenance-20261024.md`](history/repository-maintenance-20261024.md) | Maintainers, researchers | Prior repository checks, deployment fingerprints and completed maintenance history. |
| [`compatibility.md`](compatibility.md) | Players, developers | Toolchain versions and the per-mod verification matrix. |
| [`troubleshooting.md`](troubleshooting.md) | Players | Log location, installation checklist, common symptoms, how to report bugs. |
| [`../DEVELOPERS.md`](../DEVELOPERS.md) | Developers | Getting started, build, test, format, debugging, creating a mod. |
| [`../CONTRIBUTING.md`](../CONTRIBUTING.md) | Contributors | Quality gates, version bumps, definition of done, PR checklist. |
| [`architecture.md`](architecture.md) | Developers | Dependency direction, mod layout, runtime boundaries. |
| [`source-file-budget.md`](source-file-budget.md) | Developers, reviewers | 20-file production-source budget, mechanical consolidation scope and verification/deployment results. |
| [`pitfalls.md`](pitfalls.md) | Developers | IL2CPP and Unity gotchas collected from the mods in this repository. |
| [`release-process.md`](release-process.md) | Maintainers | Version bump, packaging, ZIP layout, GitHub release workflow. |
| [`working-across-devices.md`](working-across-devices.md) | Maintainers, developers | Multi-machine workflow: what syncs via git, what is machine-local, fresh-clone checklist. |
| [`../Source/Mods/README.md`](../Source/Mods/README.md) | Developers | Mod source directory: isolation, shared library, conventions. |
| [`../Tools/README.md`](../Tools/README.md) | Developers | What each PowerShell helper script does. |
| [`../Skills/README.md`](../Skills/README.md) | AI agents, developers | Index of the 23 skills (runbooks and reference material). |
| [`../AGENTS.md`](../AGENTS.md) | AI agents | Bootstrap file: setup, authoritative mod inventory, agent workflows. |
| [`../THIRD-PARTY-NOTICES.md`](../THIRD-PARTY-NOTICES.md) | Everyone | Third-party components and their licenses. |

PropertyLEDs documentation: [`README`](../Source/Mods/PropertyLEDs/docs/README.md) · [`milestones`](../Source/Mods/PropertyLEDs/docs/implementation-status.md). Version 0.1.10 was owner-confirmed 2026-10-25; Dominik explicitly confirmed the LED edges are correct, and the log does not measure the insets. The 0.1.8 confirmation does not cover 0.1.9/0.1.10. WorldLook was owner-confirmed finished on 2026-10-25.

Per-mod documentation lives next to the code: `Source/Mods/<ModName>/docs/README.md` (features, configuration, usage) and `Source/Mods/<ModName>/docs/CHANGELOG.md` (version history).
