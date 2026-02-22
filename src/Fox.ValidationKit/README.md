# Fox.ValidationKit

**Lightweight .NET validation library with fluent API and zero dependencies**

Fox.ValidationKit is a lightweight, expressive validation library for .NET with a fluent API. It provides strongly-typed validation rules with support for synchronous and asynchronous validation, custom rules, and optional ResultKit integration.

## Installation

```bash
dotnet add package Fox.ValidationKit
```

**NuGet Package Manager:**
```
Install-Package Fox.ValidationKit
```

**PackageReference:**
```xml
<PackageReference Include="Fox.ValidationKit" Version="1.0.0" />
```

## Quick Start

### 1. Define Your Model

```csharp
public sealed class User
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public int Age { get; set; }
}
```

### 2. Create a Validator

```csharp
public sealed class UserValidator : Validator<User>
{
    public UserValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty("First name is required")
            .MinLength(2)
            .MaxLength(50);

        RuleFor(x => x.LastName)
            .NotEmpty("Last name is required")
            .MinLength(2)
            .MaxLength(50);

        RuleFor(x => x.Email)
            .NotEmpty("Email is required")
            .EmailAddress("Invalid email format");

        RuleFor(x => x.Age)
            .GreaterThan(0, "Age must be positive")
            .LessThan(150, "Age must be realistic")
            .Custom((user, age) => age >= 18, "User must be at least 18 years old");
    }
}
```

### 3. Validate Your Data

```csharp
var validator = new UserValidator();
var user = new User
{
    FirstName = "John",
    LastName = "Doe",
    Email = "john.doe@example.com",
    Age = 25
};

var result = validator.Validate(user);

if (result.IsValid)
{
    Console.WriteLine("User is valid!");
}
else
{
    foreach (var error in result.Errors)
    {
        Console.WriteLine($"{error.PropertyName}: {error.Message}");
    }
}
```

## Features

- **Fluent API** - Intuitive, type-safe validation rule configuration
- **Zero Dependencies** - No external dependencies, only .NET BCL
- **Strongly Typed** - Expression-based property selection with IntelliSense
- **15+ Built-in Rules** - NotNull, NotEmpty, GreaterThan, LessThan, Between, Matches, EmailAddress, Url, CreditCard, etc.
- **Custom Rules** - Synchronous and asynchronous custom validation logic
- **Error Codes** - FVK### error codes for localization and structured error handling
- **Async Support** - ValidateAsync for asynchronous validation scenarios
- **Collection Validation** - NotEmpty, MinCount, MaxCount, RuleForEach
- **Conditional Validation** - When and Unless for conditional rules
- **Cascade Modes** - Continue or Stop validation on first failure
- **Nested Validation** - SetValidator for complex objects
- **Localization Ready** - IValidationMessageProvider for custom messages

## Documentation

Full documentation available at: https://github.com/akikari/Fox.ValidationKit

## License

Fox.ValidationKit is licensed under the MIT License.

Copyright (c) 2026 Károly Akácz

## Author

**Károly Akácz**

- GitHub: @akikari
- Project: https://github.com/akikari/Fox.ValidationKit
