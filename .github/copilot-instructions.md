# GitHub Copilot Instructions

## Project Context
This repository is a **Blazor Web App** demo used to showcase GitHub Copilot cloud agent workflows.

## Primary Objectives
- Generate production-ready C# and Razor code.
- Generate automated tests for new and updated behavior.
- Generate and keep documentation in sync.

## Architecture Guidance
- Keep changes aligned with existing Blazor project structure under `BlazorWebApp/`.
- Prefer small, focused updates instead of broad refactors.
- Reuse existing patterns and naming conventions in the codebase.

## Coding Standards
- Follow existing C# style and file organization.
- Add XML/docs/comments only when they improve maintainability.
- Avoid adding dependencies unless required.

## Testing Expectations
- Add or update tests for behavior changes.
- Prioritize deterministic tests and avoid flaky external dependencies.

## Definition of Done
- Code builds successfully.
- Relevant tests pass.
- Documentation is updated when behavior changes.
