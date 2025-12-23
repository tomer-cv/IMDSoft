# Copilot Instructions

These instructions are automatically applied when using GitHub Copilot in this repository.

## Target Framework
- All code must target .NET Framework 4.8
- Use .NET Framework-compatible APIs and patterns

## Code Style & Formatting
- Indentation: 4 spaces (never tabs)
- Always use braces for control blocks (if, else, for, foreach, while), even for single-line statements
- Place opening braces on new lines (Allman style)
- Sort `using` directives alphabetically with `System` namespaces first
- Separate `using` directive groups with a blank line
- Add a final newline to all files
- Do not leave trailing whitespace

## Variable Declarations
- Use `var` only when the type is immediately apparent from the right-hand side
- Use explicit types for built-in types (e.g., `string`, `int`, `bool`) unless the type is obvious
- Avoid `var` when the type is not clear from context

## Naming Conventions
- Use `PascalCase` for class names, method names, and properties
- Use `camelCase` for local variables and parameters
- Do not use `this.` qualifier unless required for disambiguation

## Database Access & Security
- **CRITICAL**: Always use parameterized queries with `SqlParameter` for all database operations
- Never concatenate user input into SQL strings (prevents SQL injection)
- Wrap `SqlConnection`, `SqlCommand`, and `SqlDataReader` in `using` statements for proper disposal
- Validate and sanitize all external input before use
- Do not commit connection strings, API keys, or any secrets to source control

## Error Handling
- Catch specific exception types rather than generic `Exception` when possible
- Provide meaningful error messages to users (avoid exposing internal details)
- Log errors with sufficient context for debugging
- Do not silently swallow exceptions

## Resource Management
- Always dispose of `IDisposable` objects using `using` statements or explicit `Dispose()` calls
- Prefer `using` statements over try-finally for disposal

## Code Quality
- Avoid magic numbers and strings; use named constants or configuration
- Validate user input before processing
- Use meaningful variable and method names that convey intent
- Keep methods focused and reasonably sized