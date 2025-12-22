# Contributing

## Coding standards

This repository follows the standards defined in `.editorconfig`. When using GitHub Copilot, generated code must comply with these rules.

### C# guidelines

- Target framework: .NET Framework 4.8.
- Indentation: 4 spaces.
- Use braces for all control blocks.
- Prefer `var` only when the type is apparent.
- Place `System` `using` directives first and sort `using` directives.
- Add a final newline; do not leave trailing whitespace.

## Pull requests

- Include a clear description of the change.
- Add/adjust tests when behavior changes.
- Keep commits focused and readable.

## Security

- Do not commit secrets.
- Validate and sanitize any external input.