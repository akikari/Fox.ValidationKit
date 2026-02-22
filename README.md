# 🎯 Fox.ValidationKit

[![.NET](https://img.shields.io/badge/.NET-8%20%7C%209%20%7C%2010-512BD4)](https://dotnet.microsoft.com/)
[![Build and Test](https://img.shields.io/github/actions/workflow/status/akikari/Fox.ValidationKit/build-and-test.yml?branch=main&label=build%20and%20test&color=darkgreen)](https://github.com/akikari/Fox.ValidationKit/actions/workflows/build-and-test.yml)
[![NuGet](https://img.shields.io/nuget/v/Fox.ValidationKit.svg)](https://www.nuget.org/packages/Fox.ValidationKit/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Fox.ValidationKit?label=downloads&color=darkgreen)](https://www.nuget.org/packages/Fox.ValidationKit/)
[![License: MIT](https://img.shields.io/badge/license-MIT-orange.svg)](https://opensource.org/licenses/MIT)

> **Lightweight .NET validation library with fluent API and zero dependencies**

Fox.ValidationKit is a lightweight, expressive validation library for .NET with a fluent API. It provides strongly-typed validation rules with support for synchronous and asynchronous validation, custom rules, and optional ResultKit integration.

## 📋 Table of Contents

- [Why Fox.ValidationKit?](#-why-foxvalidationkit)
- [Features](#-features)
- [Installation](#-installation)
- [Quick Start](#-quick-start)
- [Validation Rules](#-validation-rules)
- [Advanced Scenarios](#-advanced-scenarios)
- [ResultKit Integration](#-resultkit-integration)
- [Error Codes](#-error-codes)
- [Design Principles](#-design-principles)
- [Requirements](#-requirements)
- [Contributing](#-contributing)
- [License](#-license)

## 🤔 Why Fox.ValidationKit?

**Traditional approach:**
```csharp
// ❌ Manual validation with boilerplate code
public class UserService
{
    public void RegisterUser(User user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));
        if (string.IsNullOrEmpty(user.Email)) throw new ArgumentException("Email is required");
        if (user.Age < 18) throw new ArgumentException("Must be 18 or older");
        if (user.Age > 150) throw new ArgumentException("Age is unrealistic");
        // ... more validation logic
    }
}
```

**Fox.ValidationKit approach:**
```csharp
// ✅ Clean, fluent validation with reusable validator
public class UserValidator : Validator<User>
{
    public UserValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MinLength(2).MaxLength(50);
        RuleFor(x => x.LastName).NotEmpty().MinLength(2).MaxLength(50);
        RuleFor(x => x.Email).NotEmpty().Matches(@"^[^@]+@[^@]+\.[^@]+$");
        RuleFor(x => x.Age).GreaterThan(0).LessThan(150).Custom((user, age) => 
            age >= 18, "User must be at least 18 years old");
    }
}

// Usage
var validator = new UserValidator();
var result = validator.Validate(user);

if (!result.IsValid)
{
    foreach (var error in result.Errors)
    {
        Console.WriteLine($"{error.PropertyName}: {error.Message}");
    }
}
```

## ✨ Features

- ✅ **Fluent API** - Intuitive, type-safe validation rule configuration
- ✅ **Zero Dependencies** - No external dependencies, only .NET BCL
- ✅ **Strongly Typed** - Expression-based property selection with IntelliSense support
- ✅ **Rich Rule Set** - 15+ built-in validation rules (NotNull, NotEmpty, GreaterThan, LessThan, Between, Matches, etc.)
- ✅ **Custom Rules** - Support for synchronous and asynchronous custom validation logic
- ✅ **Error Codes** - FVK### error codes for localization and structured error handling
- ✅ **Async Support** - `ValidateAsync` for asynchronous validation scenarios
- ✅ **Collection Validation** - Validate collections with `NotEmpty`, `MinCount`, `MaxCount`, `RuleForEach`
- ✅ **Conditional Validation** - Apply rules conditionally with `When` and `Unless`
- ✅ **Cascade Modes** - Continue or stop validation on first failure
- ✅ **Nested Validation** - Validate complex objects with `SetValidator`
- ✅ **ResultKit Integration** - Optional integration with Fox.ResultKit for Railway Oriented Programming
- ✅ **Localization Ready** - `IValidationMessageProvider` for custom error messages
- ✅ **Multi-Targeting** - Supports .NET 8.0, .NET 9.0, and .NET 10.0

## 📦 Installation

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

### Optional: ResultKit Integration

For Railway Oriented Programming support:

```bash
dotnet add package Fox.ValidationKit.ResultKit
```

## 🚀 Quick Start

### 1. Define Your Model

```csharp
public sealed class User
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public int Age { get; set; }
    public string? PhoneNumber { get; set; }
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
            .Matches(@"^[^@]+@[^@]+\.[^@]+$", "Email format is invalid");

        RuleFor(x => x.Age)
            .GreaterThan(0, "Age must be positive")
            .LessThan(150, "Age must be realistic")
            .Custom((user, age) => age >= 18, "User must be at least 18 years old");

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^\+?[\d\s\-\(\)]+$", "Phone number contains invalid characters");
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
    Age = 25,
    PhoneNumber = "+1-555-123-4567"
};

var result = validator.Validate(user);

if (result.IsValid)
{
    Console.WriteLine("✓ User is valid!");
}
else
{
    Console.WriteLine("✗ Validation failed:");
    foreach (var error in result.Errors)
    {
        Console.WriteLine($"  - {error.PropertyName}: {error.Message}");
        if (error.ErrorCode != null)
        {
            Console.WriteLine($"    Error Code: {error.ErrorCode}");
        }
    }
}
```

## 📚 Validation Rules

### Basic Validation

| Method | Description | Error Code |
|--------|-------------|------------|
| `NotNull(message)` | Ensures value is not null | FVK001 |
| `NotEmpty(message)` | Ensures string is not null, empty, or whitespace | FVK002 |

**Example:**

```csharp
RuleFor(x => x.FirstName)
    .NotEmpty("First name is required");

RuleFor(x => x.Address)
    .NotNull("Address is required");
```

### Comparison Validation

| Method | Description | Error Code |
|--------|-------------|------------|
| `GreaterThan(value, message)` | Ensures value is greater than specified value | FVK202 |
| `LessThan(value, message)` | Ensures value is less than specified value | FVK203 |
| `Between(min, max, message)` | Ensures value is between min and max (inclusive) | FVK204 |
| `Equal(value, message)` | Ensures value equals another property value | FVK200 |
| `NotEqual(value, message)` | Ensures value does not equal another property value | FVK201 |

**Example:**

```csharp
RuleFor(x => x.Age)
    .GreaterThan(0, "Age must be positive")
    .LessThan(150, "Age must be realistic")
    .Between(18, 65, "Age must be between 18 and 65");

RuleFor(x => x.Price)
    .GreaterThan(0.01m);

RuleFor(x => x.Password)
    .NotEqual(u => u.Username, "Password must be different from username");
```

### String Validation

| Method | Description | Error Code |
|--------|-------------|------------|
| `MinLength(length, message)` | Ensures string has minimum length | FVK100 |
| `MaxLength(length, message)` | Ensures string has maximum length | FVK101 |
| `Length(min, max, message)` | Ensures string length is within range | FVK102 |
| `Matches(regex, message)` | Ensures string matches regex pattern | FVK304 |
| `EmailAddress(message)` | Validates email address format | FVK301 |
| `Url(message)` | Validates URL format (HTTP/HTTPS) | FVK302 |
| `CreditCard(message)` | Validates credit card number (Luhn algorithm) | FVK303 |

**Example:**

```csharp
RuleFor(x => x.Username)
    .NotEmpty()
    .MinLength(3)
    .MaxLength(20)
    .Matches(@"^[a-zA-Z0-9_]+$", "Username can only contain letters, numbers, and underscores");

RuleFor(x => x.Email)
    .EmailAddress("Invalid email address");

RuleFor(x => x.Website)
    .Url("Invalid URL format");

RuleFor(x => x.CardNumber)
    .CreditCard("Invalid credit card number");
```

### Collection Validation

| Method | Description | Error Code |
|--------|-------------|------------|
| `NotEmpty(message)` | Ensures collection is not null or empty | FVK400 |
| `MinCount(count, message)` | Ensures collection has minimum count | FVK401 |
| `MaxCount(count, message)` | Ensures collection has maximum count | FVK402 |
| `RuleForEach(predicate, message)` | Validates each element in collection | - |

**Example:**

```csharp
RuleFor(x => x.Tags)
    .NotEmpty("At least one tag is required")
    .MinCount(1)
    .MaxCount(10)
    .RuleForEach(tag => tag.Length >= 2, "Each tag must be at least 2 characters");

RuleFor(x => x.Addresses)
    .NotEmpty("At least one address is required")
    .MaxCount(5, "Maximum 5 addresses allowed");
```

### Enum Validation

| Method | Description | Error Code |
|--------|-------------|------------|
| `IsInEnum(message)` | Ensures value is a valid enum value | FVK500 |

**Example:**

```csharp
public enum UserRole
{
    Admin,
    User,
    Guest
}

RuleFor(x => x.Role)
    .IsInEnum("Invalid user role");
```

### Custom Validation

| Method | Description | Error Code |
|--------|-------------|------------|
| `Custom(predicate, message)` | Custom synchronous validation | FVK900 |
| `CustomAsync(predicate, message)` | Custom asynchronous validation | FVK900 |

**Example:**

```csharp
RuleFor(x => x.Age)
    .Custom((user, age) => age >= 18, "User must be at least 18 years old");

RuleFor(x => x.Email)
    .CustomAsync(async (user, email, ct) =>
    {
        var exists = await emailService.ExistsAsync(email, ct);
        return !exists;
    }, "Email is already registered");
```

## 🔥 Advanced Scenarios

### Conditional Validation

Apply validation rules conditionally:

```csharp
public sealed class OrderValidator : Validator<Order>
{
    public OrderValidator()
    {
        RuleFor(x => x.ShippingAddress)
            .NotNull()
            .When(order => order.RequiresShipping);

        RuleFor(x => x.PickupLocation)
            .NotEmpty()
            .Unless(order => order.RequiresShipping);
    }
}
```

### Cascade Modes

Control validation behavior when rules fail:

```csharp
public sealed class UserValidator : Validator<User>
{
    public UserValidator()
    {
        // Stop validation on first failure for this property
        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty("Email is required")
            .EmailAddress("Invalid email format")
            .CustomAsync(async (user, email, ct) =>
            {
                return await emailService.IsUniqueAsync(email, ct);
            }, "Email is already registered");

        // Continue validation even if rules fail (default)
        RuleFor(x => x.Password)
            .Cascade(CascadeMode.Continue)
            .NotEmpty()
            .MinLength(8)
            .Matches(@"[A-Z]", "Password must contain uppercase letter")
            .Matches(@"[a-z]", "Password must contain lowercase letter")
            .Matches(@"\d", "Password must contain digit");
    }
}
```

### Nested Object Validation

Validate complex objects with nested validators:

```csharp
public sealed class AddressValidator : Validator<Address>
{
    public AddressValidator()
    {
        RuleFor(x => x.Street).NotEmpty();
        RuleFor(x => x.City).NotEmpty();
        RuleFor(x => x.ZipCode).Matches(@"^\d{5}$");
    }
}

public sealed class UserValidator : Validator<User>
{
    public UserValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();

        // Validate nested object
        RuleFor(x => x.Address).SetValidator(new AddressValidator());
    }
}
```

### Asynchronous Validation

Support for async validation:

```csharp
public sealed class UserValidator : Validator<User>
{
    private readonly IUserRepository repository;

    public UserValidator(IUserRepository repository)
    {
        this.repository = repository;

        RuleFor(x => x.Email)
            .NotEmpty()
            .CustomAsync(async (user, email, cancellationToken) =>
            {
                var exists = await repository.EmailExistsAsync(email, cancellationToken);
                return !exists;
            }, "Email is already registered");
    }
}

// Usage
var result = await validator.ValidateAsync(user, cancellationToken);
```

## 🎨 ResultKit Integration

Fox.ValidationKit.ResultKit provides seamless integration with [Fox.ResultKit](https://github.com/akikari/Fox.ResultKit) for Railway Oriented Programming:

### Installation

```bash
dotnet add package Fox.ValidationKit.ResultKit
```

### ValidateAsResult

Convert `ValidationResult` to `Result`:

```csharp
using Fox.ValidationKit.ResultKit;

var validator = new UserValidator();
var result = validator.ValidateAsResult(user);

return result.Match(
    onSuccess: () => Ok("User validated successfully"),
    onFailure: error => BadRequest(error)
);
```

### ValidateAsResultValue

Return the validated value with `Result<T>`:

```csharp
var result = validator.ValidateAsResultValue(user);

return result.Match(
    onSuccess: validUser => Ok(validUser),
    onFailure: error => BadRequest(error)
);
```

### ValidateAsErrorsResult

Get individual validation errors as separate Results:

```csharp
var errorsResult = validator.ValidateAsErrorsResult(user);

if (!errorsResult.IsSuccess)
{
    foreach (var error in errorsResult.Errors)
    {
        Console.WriteLine($"- {error.Error}");
    }
}
```

### Async Variants

All methods have async versions:

```csharp
var result = await validator.ValidateAsResultAsync(user, cancellationToken);
var resultValue = await validator.ValidateAsResultValueAsync(user, cancellationToken);
var errorsResult = await validator.ValidateAsErrorsResultAsync(user, cancellationToken);
```

## 🏷️ Error Codes

Fox.ValidationKit provides structured error codes for all built-in validation rules:

| Category | Error Codes | Description |
|----------|-------------|-------------|
| **Null/Empty** | FVK001-003 | NotNull, NotEmpty, CollectionNotEmpty |
| **Length** | FVK100-102 | MinLength, MaxLength, Length |
| **Comparison** | FVK200-204 | Equal, NotEqual, GreaterThan, LessThan, Between |
| **Pattern** | FVK301-304 | EmailAddress, Url, CreditCard, Matches |
| **Collection** | FVK400-402 | NotEmpty, MinCount, MaxCount |
| **Enum** | FVK500 | IsInEnum |
| **Custom** | FVK900 | Custom validation rules |

### Localization with Message Provider

Implement `IValidationMessageProvider` for custom error messages:

```csharp
public class LocalizedMessageProvider : IValidationMessageProvider
{
    public string GetMessage(string errorCode, string propertyName, params object[] args)
    {
        return errorCode switch
        {
            ValidationErrorCodes.NotEmpty => $"{propertyName} nem lehet üres",
            ValidationErrorCodes.MinLength => $"{propertyName} legalább {args[0]} karakter kell legyen",
            ValidationErrorCodes.EmailAddress => $"{propertyName} nem érvényes e-mail cím",
            _ => $"{propertyName} érvénytelen"
        };
    }
}

// Usage
var validator = new UserValidator();
validator.UseMessageProvider(new LocalizedMessageProvider());
```

## 💡 Design Principles

Fox.ValidationKit follows these design principles:

- **Explicit Validation** - All validation rules are explicitly declared using a fluent API
- **Type Safety First** - Leverage C#'s strong type system (nullable reference types, expression trees, generic constraints)
- **Zero External Dependencies** - No external dependencies, relies only on .NET BCL
- **Developer Experience** - Clear error messages, IntelliSense-friendly APIs, comprehensive XML documentation
- **Flexible** - Support for sync, async, custom validation rules, conditional validation, nested objects

## 📋 Requirements

- .NET 8.0, .NET 9.0, or .NET 10.0
- C# 12.0 or later (for modern language features)

## 🤝 Contributing

Contributions are welcome! Please see [CONTRIBUTING.md](CONTRIBUTING.md) for guidelines.

Fox.ValidationKit follows strict coding standards:
- Comprehensive unit tests required (xUnit + FluentAssertions)
- XML documentation for all public APIs
- Follow Microsoft coding conventions
- Zero warnings, zero errors build policy

## 📄 License

Fox.ValidationKit is licensed under the [MIT License](LICENSE.txt).

Copyright (c) 2026 Károly Akácz

---

## 👤 Author

**Károly Akácz**

- GitHub: [@akikari](https://github.com/akikari)
- Project: [Fox.ValidationKit](https://github.com/akikari/Fox.ValidationKit)

---
