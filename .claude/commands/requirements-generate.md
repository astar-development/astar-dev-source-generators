---
description: Generate REQUIREMENTS.md from the current session's gathered information
allowed-tools: Read, Write
---

# Requirements: Generate

Produce the `REQUIREMENTS.md` document from everything gathered in this session.

## Instructions

1. Run the quality checklist:
   - Project name and purpose are clear
   - At least one stakeholder identified
   - At least one persona with goals and environment documented
   - MVP features listed with acceptance criteria
   - At least one user journey documented end-to-end
   - Performance and availability targets are numeric
   - Security and compliance requirements are stated
   - At least one assumption documented
   - Out-of-scope items explicitly listed
   - Open questions recorded for unresolved items

2. For any checklist item that is missing or vague, **ask the user now** before writing. Do not generate a document with placeholder content or vague language.

3. Once all checklist items are satisfied, write `REQUIREMENTS.md` to the project root using the template defined in `CLAUDE.md`.

4. After writing the file, tell the user:
   - The file path
   - How many open questions remain (and what they are)
   - Recommended next step: "Share this with your architect — they can use it to begin system design."
