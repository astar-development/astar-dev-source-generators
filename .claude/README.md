# Requirements Analyst — Claude Code Setup

Configure Claude Code as a structured **Requirements Analyst** that interviews you about your project and produces a `REQUIREMENTS.md` ready for your architect.

---

## What this does

Claude Code will:
- Work through six discovery phases with you, one question at a time
- Probe vague answers and challenge assumptions
- Summarise and confirm understanding before moving on
- Produce a complete, architect-ready `REQUIREMENTS.md` with testable requirements, user personas, journeys, non-functional requirements, constraints, and open questions

Claude Code will **not** write code, suggest implementations, or make architectural decisions during this session.

---

## Installation

### Option A — Project-scoped (recommended for a specific project)

Copy files into your project root:

```bash
# Copy the system prompt
cp CLAUDE.md your-project/CLAUDE.md

# Copy the skill
mkdir -p your-project/.claude/skills/requirements
cp .claude/skills/requirements/SKILL.md your-project/.claude/skills/requirements/SKILL.md

# Copy the slash commands
mkdir -p your-project/.claude/commands
cp .claude/commands/requirements-*.md your-project/.claude/commands/
```

Then open Claude Code in your project directory:

```bash
cd your-project
claude
```

### Option B — Global (available in all projects)

```bash
# Copy the skill globally
mkdir -p ~/.claude/skills/requirements
cp .claude/skills/requirements/SKILL.md ~/.claude/skills/requirements/SKILL.md

# Copy commands globally
mkdir -p ~/.claude/commands
cp .claude/commands/requirements-*.md ~/.claude/commands/
```

> **Note:** For global use, add the CLAUDE.md content to your global `~/.claude/CLAUDE.md` file, or pass it with `--append-system-prompt` when starting a session.

---

## Usage

### Start a session

```bash
# Open Claude Code in your project directory
claude

# Then type:
/requirements-start
```

Claude will introduce itself and begin Phase 1 immediately.

### Check your progress

```
/requirements-status
```

Shows which phases are complete and what gaps remain.

### Generate the document

```
/requirements-generate
```

Runs a quality checklist, asks for any missing information, then writes `REQUIREMENTS.md` to your project root.

---

## Discovery Phases

| Phase | Topics |
|---|---|
| 1 — Project Context | Problem, stakeholders, success criteria, timeline, compliance |
| 2 — Users and Personas | Who uses it, their goals, devices, technical proficiency |
| 3 — Functional Requirements | MVP features, user journeys, business rules, integrations |
| 4 — Non-Functional Requirements | Performance, scalability, availability, security, data, accessibility |
| 5 — Constraints and Assumptions | Tech stack, team, budget, known risks |
| 6 — Open Questions | Unresolved decisions, explicit out-of-scope items |

---

## Output

`REQUIREMENTS.md` is structured for architects:

- RFC 2119 priority language (MUST / SHOULD / MAY)
- Testable acceptance criteria on every functional requirement
- Numeric non-functional targets (not "fast" — `< 200ms p95`)
- Explicit assumptions table with risk if wrong
- Open questions table for the architect to resolve
- Glossary of domain terms

---

## Customising

The system prompt (`CLAUDE.md`) is plain markdown — edit it freely to:
- Add domain-specific question areas to any phase
- Adjust the `REQUIREMENTS.md` template sections
- Change the RFC 2119 language to your team's convention
- Add a phase for your specific compliance framework (e.g. ISO 27001, SOC 2)
