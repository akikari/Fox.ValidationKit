# Fox.ValidationKit.ResultKit

**ResultKit integration for Fox.ValidationKit**

Fox.ValidationKit.ResultKit provides seamless integration between Fox.ValidationKit and Fox.ResultKit, enabling Railway Oriented Programming patterns for validation workflows.

## Installation

```bash
dotnet add package Fox.ValidationKit.ResultKit
```

**NuGet Package Manager:**
```
Install-Package Fox.ValidationKit.ResultKit
```

**PackageReference:**
```xml
<PackageReference Include="Fox.ValidationKit.ResultKit" Version="1.0.0" />
```

## Prerequisites

This package requires:
- Fox.ValidationKit 1.0.0 or later
- Fox.ResultKit 1.2.0 or later

## Quick Start

### ValidateAsResult

Convert ValidationResult to Result:

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

Return the validated value with Result<T>:

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

## Features

- **ValidateAsResult** - Convert ValidationResult to Result
- **ValidateAsResultValue** - Return validated value with Result<T>
- **ValidateAsErrorsResult** - Get individual errors as ErrorsResult
- **Async Support** - All methods have async variants
- **Error Aggregation** - Automatic error message aggregation with FVK codes
- **Railway Oriented Programming** - Functional error handling patterns

## API Reference

### Extension Methods for Validator<T>

```csharp
// Synchronous
Result ValidateAsResult<T>(this Validator<T> validator, T instance);
Result<T> ValidateAsResultValue<T>(this Validator<T> validator, T instance);
ErrorsResult ValidateAsErrorsResult<T>(this Validator<T> validator, T instance);

// Asynchronous
Task<Result> ValidateAsResultAsync<T>(this Validator<T> validator, T instance, CancellationToken cancellationToken = default);
Task<Result<T>> ValidateAsResultValueAsync<T>(this Validator<T> validator, T instance, CancellationToken cancellationToken = default);
Task<ErrorsResult> ValidateAsErrorsResultAsync<T>(this Validator<T> validator, T instance, CancellationToken cancellationToken = default);
```

### Extension Methods for ValidationResult

```csharp
Result ToResult(this ValidationResult validationResult);
Result<T> ToResult<T>(this ValidationResult validationResult, T value);
ErrorsResult ToErrorsResult(this ValidationResult validationResult);
```

## Example: ASP.NET Core Integration

```csharp
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserValidator validator;

    public UsersController(UserValidator validator)
    {
        this.validator = validator;
    }

    [HttpPost]
    public IActionResult CreateUser([FromBody] User user)
    {
        var result = validator.ValidateAsResult(user);

        return result.Match(
            onSuccess: () => CreatedAtAction(nameof(GetUser), new { id = user.Id }, user),
            onFailure: error => BadRequest(new { Error = error })
        );
    }

    [HttpPut("{id}")]
    public IActionResult UpdateUser(int id, [FromBody] User user)
    {
        var result = validator.ValidateAsResultValue(user);

        return result.Match(
            onSuccess: validUser => Ok(validUser),
            onFailure: error => BadRequest(new { Error = error })
        );
    }

    [HttpPost("batch")]
    public IActionResult CreateUsers([FromBody] List<User> users)
    {
        var errorsResult = ErrorsResult.Success();

        foreach (var user in users)
        {
            var validationResult = validator.ValidateAsErrorsResult(user);
            errorsResult = errorsResult.Combine(validationResult);
        }

        if (!errorsResult.IsSuccess)
        {
            return BadRequest(new
            {
                Errors = errorsResult.Errors.Select(e => e.Error)
            });
        }

        return Ok("All users validated successfully");
    }
}
```

## Example: Async Validation

```csharp
public class UserValidator : Validator<User>
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
var result = await validator.ValidateAsResultAsync(user, cancellationToken);

return result.Match(
    onSuccess: () => Ok("User validated"),
    onFailure: error => BadRequest(error)
);
```

## Documentation

Full documentation available at:
- **Fox.ValidationKit**: https://github.com/akikari/Fox.ValidationKit
- **Fox.ResultKit**: https://github.com/akikari/Fox.ResultKit

## License

Fox.ValidationKit.ResultKit is licensed under the MIT License.

Copyright (c) 2026 Karoly Akacz

## Author

**Karoly Akacz**

- GitHub: [@akikari](https://github.com/akikari)
- Project: [Fox.ValidationKit](https://github.com/akikari/Fox.ValidationKit)
