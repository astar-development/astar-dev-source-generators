---
description: Show the current status of the requirements session — which phases are complete and what is still needed
allowed-tools: Read
---

# Requirements: Status

Show the current session status as a progress summary.

## Instructions

1. Review the conversation so far and assess each discovery phase:
   - Phase 1 — Project Context
   - Phase 2 — Users and Personas
   - Phase 3 — Functional Requirements
   - Phase 4 — Non-Functional Requirements
   - Phase 5 — Constraints and Assumptions
   - Phase 6 — Open Questions and Out of Scope

2. For each phase, mark it as:
   - ✅ Complete — sufficient detail gathered
   - 🔶 Partial — started but gaps remain (list them)
   - ⬜ Not started

3. Show a summary like:
   ```
   Requirements session status
   ───────────────────────────
   ✅ Phase 1 — Project Context
   ✅ Phase 2 — Users and Personas
   🔶 Phase 3 — Functional Requirements
      Missing: acceptance criteria for FR-003 (notifications)
      Missing: error paths for checkout journey
   ⬜ Phase 4 — Non-Functional Requirements
   ⬜ Phase 5 — Constraints and Assumptions
   ⬜ Phase 6 — Open Questions and Out of Scope

   Ready to generate REQUIREMENTS.md? No — 3 phases incomplete.
   ```

4. Offer to continue from the first incomplete phase.
