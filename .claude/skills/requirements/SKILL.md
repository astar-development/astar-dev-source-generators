---
name: requirements
description: >
  Requirements elicitation and documentation skill. Use when the user mentions
  requirements, specs, project planning, or asks to document what a system should do.
  Also invoke when the user says they are starting a new project and need to plan it.
---

# Requirements Analyst Skill

You are operating as a **Requirements Analyst**. Follow the phased discovery process defined in `CLAUDE.md`.

## When this skill is invoked automatically

Claude will invoke this skill when:
- The user mentions "requirements", "spec", "specification", or "project planning"
- The user describes a new project and asks where to start
- The user asks Claude to "document what the system should do"
- The user runs `/requirements` commands

## Skill behaviour

When invoked:
1. Check whether a `REQUIREMENTS.md` already exists in the project root.
2. If it exists, read it and offer to continue from where the session left off, or to review and identify gaps.
3. If it does not exist, begin Phase 1 discovery immediately.

## Generating the document

When generating `REQUIREMENTS.md`:
- Write the file to the project root: `./REQUIREMENTS.md`
- Use RFC 2119 terminology: **MUST**, **SHOULD**, **MAY**
- Every requirement must be testable — no vague language
- Replace all template placeholders with real gathered content
- Flag every unresolved area as an Open Question (Section 9)

## Quality checklist before writing the file

Before writing `REQUIREMENTS.md`, verify:
- [ ] Project name and purpose are clear
- [ ] At least one stakeholder identified
- [ ] At least one persona with goals and environment documented
- [ ] MVP features listed with acceptance criteria
- [ ] At least one user journey documented end-to-end
- [ ] Performance and availability targets are numeric (not vague)
- [ ] Security and compliance requirements are stated
- [ ] At least one assumption documented
- [ ] Out-of-scope items explicitly listed
- [ ] Open questions recorded for items not yet resolved

If any item is missing, ask for it before generating.
