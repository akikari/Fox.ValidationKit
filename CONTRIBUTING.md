# Contributing to Fox.ValidationKit

Thank you for your interest in contributing to Fox.ValidationKit! This document provides guidelines and instructions for contributing to the project.

## Code of Conduct

By participating in this project, you agree to maintain a respectful and inclusive environment for all contributors.

## How to Contribute

### Reporting Issues

If you find a bug or have a feature request:

1. Check if the issue already exists in the [GitHub Issues](https://github.com/akikari/Fox.ValidationKit/issues)
2. If not, create a new issue with:
   - Clear, descriptive title
   - Detailed description of the problem or feature
   - Steps to reproduce (for bugs)
   - Expected vs actual behavior
   - Code samples if applicable
   - Environment details (.NET version, OS, etc.)

### Submitting Changes

1. **Fork the repository** and create a new branch from `main`
2. **Make your changes** following the coding guidelines below
3. **Write or update tests** for your changes
4. **Update documentation** if needed (README, XML comments)
5. **Ensure all tests pass** (`dotnet test`)
6. **Ensure build succeeds** (`dotnet build`)
7. **Submit a pull request** with:
   - Clear description of changes
   - Reference to related issues
   - Summary of testing performed

## Coding Guidelines

Fox.ValidationKit follows strict coding standards. Please review the [Copilot Instructions](.github/copilot-instructions.md) for detailed guidelines.

### Key Standards

#### General
- **Language**: All code, comments, and documentation must be in English
- **Line Endings**: CRLF
- **Indentation**: 4 spaces (no tabs)
- **Namespaces**: File-scoped (`namespace MyNamespace;`)
- **Nullable**: Enabled
- **Language Version**: latest

#### Naming Conventions
- **Private Fields**: camelCase without underscore prefix (e.g., `value`, not `_value`)
- **Public Members**: PascalCase
- **Local Variables**: camelCase

#### Code Style
- Use expression-bodied members for simple properties and methods
- Use auto-properties where possible
- Prefer `var` only when type is obvious
- Maximum line length: 100 characters
- Add blank line after closing brace UNLESS next line is also `}`

#### Documentation
- **XML Comments**: Required for all public APIs
- **Language**: English
- **Decorators**: 98 characters width using `//======` (no space after prefix)
- **File Headers**: 3-line header (purpose + technical description + decorators)

Example:
```csharp
//==================================================================================================
// Validates that a string property is not null or empty.
// Provides validation for string properties.
//==================================================================================================

namespace Fox.ValidationKit.Rules;

//==================================================================================================
/// <summary>
/// Validates that a string property is not null or empty.
/// </summary>
/// <typeparam name="T">The type of object being validated.</typeparam>
//==================================================================================================
internal sealed class NotEmptyRule<T>(string propertyName, string? errorMessage = null) : ValidationRuleBase<T, string?>(propertyName, errorMessage)
{
    //==============================================================================================
    /// <summary>
    /// Validates that the string value is not null, empty, or whitespace.
    /// </summary>
    /// <param name="instance">The object instance being validated.</param>
    /// <param name="value">The string value to validate.</param>
    /// <returns>A validation error if the value is empty, otherwise empty collection.</returns>
    //==============================================================================================
    public override IEnumerable<ValidationError> Validate(T instance, string? value)
    {
        if (value == null || string.IsNullOrWhiteSpace(value))
        {
            return CreateError(ValidationErrorCodes.NotEmpty, $"{propertyName} must not be empty.");
        }

        return Success();
    }
}
```

## Testing Requirements

- **Framework**: xUnit
- **Assertions**: FluentAssertions
- **Test Naming**: `MethodName_should_expected_behavior`
- **Coverage**: Aim for 100% coverage of new code
- **Test Structure**:
  - Arrange: Setup test data
  - Act: Execute the method under test
  - Assert: Verify expected behavior

Example:
```csharp
[Fact]
public void NotEmpty_should_fail_when_value_is_empty()
{
    // Arrange
    var user = new User { Name = string.Empty };
    var validator = new UserValidator();

    // Act
    var result = validator.Validate(user);

    // Assert
    result.IsValid.Should().BeFalse();
    result.Errors.Should().ContainSingle(e => e.PropertyName == "Name");
}

[Fact]
public void Validator_should_validate_multiple_properties()
{
    // Arrange
    var user = new User
    {
        FirstName = "John",
        LastName = "Doe",
        Email = "john.doe@example.com",
        Age = 25
    };
    var validator = new UserValidator();

    // Act
    var result = validator.Validate(user);

    // Assert
    result.IsValid.Should().BeTrue();
    result.Errors.Should().BeEmpty();
}
```

## Architecture Principles

Fox.ValidationKit follows Clean Architecture and SOLID principles:

- **Single Responsibility**: Each validation rule has one clear purpose
- **Open/Closed**: Open for extension (via custom rules), closed for modification
- **Liskov Substitution**: All validation rule implementations are substitutable
- **Interface Segregation**: Small, focused interfaces (IValidationRule<T>)
- **Dependency Inversion**: Depend on abstractions, not concretions

### Design Guidelines

- **Fluent API**: Intuitive, chainable validation rules
- **Explicit Validation**: Make validation rules clear and discoverable
- **Type-Safe**: Leverage C# type system for compile-time safety
- **Zero Dependencies**: No external dependencies, only .NET BCL
- **Flexible**: Support for sync, async, and custom validation rules
- **Developer-Friendly**: Clear error messages, excellent IntelliSense

## Project Structure

```
Fox.ValidationKit/
├── src/
│   ├── Fox.ValidationKit/                      # Core package
│   │   ├── Validator.cs                        # Abstract base validator
│   │   ├── ValidationResult.cs                 # Result type
│   │   ├── ValidationError.cs                  # Error record
│   │   ├── RuleBuilder.cs                      # Fluent builder API
│   │   ├── RuleBuilderExtensions.cs            # Extension methods for rules
│   │   └── Rules/                              # Rule implementations
│   │       ├── ValidationRuleBase.cs           # Base class for rules
│   │       ├── NotNullRule.cs                  # NotNull validation
│   │       ├── NotEmptyRule.cs                 # NotEmpty validation
│   │       ├── GreaterThanRule.cs              # Comparison validation
│   │       └── ...                             # 15+ built-in rules
│   └── Fox.ValidationKit.ResultKit/            # ResultKit integration
│       ├── ValidatorExtensions.cs              # ValidateAsResult extensions
│       └── ValidationResultExtensions.cs       # ToResult/ToErrorsResult extensions
├── tests/
│   ├── Fox.ValidationKit.Tests/                # Core tests (144 tests × 3 frameworks)
│   └── Fox.ValidationKit.ResultKit.Tests/      # ResultKit integration tests
└── samples/
    └── Fox.ValidationKit.Demo/                 # Console demo application
```

## Pull Request Process

1. **Update tests**: Ensure your changes are covered by tests
2. **Update documentation**: Keep README and XML comments up to date
3. **Follow coding standards**: Use provided `.editorconfig` and copilot instructions
4. **Keep commits clean**: 
   - Use clear, descriptive commit messages
   - Squash commits if needed before merging
5. **Update CHANGELOG.md**: Add entry under `[Unreleased]` section
6. **Ensure CI passes**: All tests must pass and build must succeed

### Commit Message Format

Use clear, imperative commit messages in English:

```
Add InRange validation rule for TimeSpan values

- Implement TimeSpan-specific InRange validation
- Add unit tests for TimeSpan boundary conditions
- Update documentation with TimeSpan examples
```

## Feature Requests

When proposing new features, please consider:

1. **Scope**: Does this fit the focused nature of Fox.ValidationKit?
2. **Complexity**: Does this add unnecessary complexity?
3. **Dependencies**: Does this require new external dependencies?
4. **Breaking Changes**: Will this break existing code?
5. **Use Cases**: What real-world scenarios does this address?

Fox.ValidationKit aims to be lightweight and focused. Features should align with validation principles and fluent API design.

## Development Setup

### Prerequisites
- .NET 8 SDK or later
- Visual Studio 2022+ or Rider (recommended)
- Git

### Getting Started

1. Clone the repository:
```bash
git clone https://github.com/akikari/Fox.ValidationKit.git
cd Fox.ValidationKit
```

2. Restore dependencies:
```bash
dotnet restore
```

3. Build the solution:
```bash
dotnet build
```

4. Run tests:
```bash
dotnet test
```

5. Run the sample application:
```bash
dotnet run --project samples/Fox.ValidationKit.Demo/Fox.ValidationKit.Demo.csproj
```

## Questions?

If you have questions about contributing, feel free to:
- Open a [GitHub Discussion](https://github.com/akikari/Fox.ValidationKit/discussions)
- Create an issue labeled `question`
- Reach out to the maintainers

## License

By contributing to Fox.ValidationKit, you agree that your contributions will be licensed under the MIT License.

Thank you for contributing to Fox.ValidationKit! 🎉
