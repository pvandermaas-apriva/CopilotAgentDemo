# Test Generation Prompt

You are working in this repository.

## Goal
Create or update tests for the specified implementation.

## Inputs
- Changed behavior: `<describe behavior>`
- Files changed: `<list files>`
- Existing test project/file: `<path>`

## Requirements
- Cover happy path, edge cases, and error handling where applicable.
- Keep tests deterministic and readable.
- Reuse existing testing style and fixtures.

## Output Format
1. Test strategy (brief).
2. Test files created/modified.
3. Test cases covered.
4. How to run the tests.
