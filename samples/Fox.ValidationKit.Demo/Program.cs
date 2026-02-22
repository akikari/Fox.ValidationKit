//==================================================================================================
// Demo application showcasing Fox.ValidationKit and Fox.ValidationKit.ResultKit usage.
// Demonstrates basic validation, custom rules, ResultKit integration, and ErrorsResult for multiple errors.
//==================================================================================================
using Fox.ValidationKit.ResultKit;

namespace Fox.ValidationKit.Demo;

//==================================================================================================
/// <summary>
/// Demo application entry point.
/// </summary>
//==================================================================================================
public static class Program
{
    public static void Main()
    {
        Console.WriteLine("===========================================");
        Console.WriteLine("Fox.ValidationKit Demo");
        Console.WriteLine("===========================================\n");

        DemoBasicValidation();
        Console.WriteLine();

        DemoValidationFailures();
        Console.WriteLine();

        DemoResultKitIntegration();
        Console.WriteLine();

        DemoAsyncValidation();

        Console.WriteLine("\n===========================================");
        Console.WriteLine("Demo completed!");
        Console.WriteLine("===========================================");
    }

    //==============================================================================================
    /// <summary>
    /// Demonstrates basic validation with a valid user.
    /// </summary>
    //==============================================================================================
    private static void DemoBasicValidation()
    {
        Console.WriteLine("--- Basic Validation (Valid User) ---");

        var user = new User
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Age = 30,
            PhoneNumber = "+1-555-0123"
        };

        var validator = new UserValidator();
        var result = validator.Validate(user);

        if (result.IsValid)
        {
            Console.WriteLine("[OK] Validation succeeded!");
            Console.WriteLine($"  User: {user.FirstName} {user.LastName}");
        }
        else
        {
            Console.WriteLine("[FAIL] Validation failed:");

            foreach (var error in result.Errors)
            {
                Console.WriteLine($"  - {error.PropertyName}: {error.Message}");
            }
        }
    }

    //==============================================================================================
    /// <summary>
    /// Demonstrates validation failures with an invalid user.
    /// </summary>
    //==============================================================================================
    private static void DemoValidationFailures()
    {
        Console.WriteLine("--- Validation Failures (Invalid User) ---");

        var user = new User
        {
            FirstName = "J",
            LastName = "",
            Email = "invalid-email",
            Age = 15,
            PhoneNumber = "invalid!@#"
        };

        var validator = new UserValidator();
        var result = validator.Validate(user);

        if (result.IsValid)
        {
            Console.WriteLine("[OK] Validation succeeded!");
        }
        else
        {
            Console.WriteLine($"[FAIL] Validation failed with {result.Errors.Count} error(s):");

            foreach (var error in result.Errors)
            {
                Console.WriteLine($"  - {error.PropertyName}: {error.Message}");
            }
        }
    }

    //==============================================================================================
    /// <summary>
    /// Demonstrates ResultKit integration.
    /// </summary>
    //==============================================================================================
    private static void DemoResultKitIntegration()
    {
        Console.WriteLine("--- ResultKit Integration ---");

        var validUser = new User
        {
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane.smith@example.com",
            Age = 25,
            PhoneNumber = "+1-555-9876"
        };

        var invalidUser = new User
        {
            FirstName = "",
            Email = "invalid",
            Age = 0
        };

        var validator = new UserValidator();

        Console.WriteLine("\n1. Valid user with Result:");
        var validResult = validator.ValidateAsResult(validUser);

        if (validResult.IsSuccess)
        {
            Console.WriteLine("   [OK] Result: Success");
        }
        else
        {
            Console.WriteLine($"   [FAIL] Result: Failure - {validResult.Error}");
        }

        Console.WriteLine("\n2. Invalid user with Result:");
        var invalidResult = validator.ValidateAsResult(invalidUser);

        if (invalidResult.IsSuccess)
        {
            Console.WriteLine("   [OK] Result: Success");
        }
        else
        {
            Console.WriteLine($"   [FAIL] Result: Failure");
            Console.WriteLine($"   Error: {invalidResult.Error}");
        }

        Console.WriteLine("\n3. Invalid user with ErrorsResult (all errors):");
        var errorsResult = validator.ValidateAsErrorsResult(invalidUser);

        if (errorsResult.IsSuccess)
        {
            Console.WriteLine("   [OK] ErrorsResult: Success");
        }
        else
        {
            Console.WriteLine($"   [FAIL] ErrorsResult: Failure with {errorsResult.Errors.Count} error(s):");

            foreach (var error in errorsResult.Errors)
            {
                Console.WriteLine($"      - {error}");
            }
        }

        Console.WriteLine("\n4. Valid user with Result<T>:");
        var validResultWithValue = validator.ValidateAsResultValue(validUser);

        if (validResultWithValue.IsSuccess)
        {
            Console.WriteLine($"   [OK] Result: Success with value");
            Console.WriteLine($"   User: {validResultWithValue.Value.FirstName} {validResultWithValue.Value.LastName}");
        }
    }

    //==============================================================================================
    /// <summary>
    /// Demonstrates asynchronous validation.
    /// </summary>
    //==============================================================================================
    private static void DemoAsyncValidation()
    {
        Console.WriteLine("\n--- Async Validation ---");

        var user = new User
        {
            FirstName = "Alice",
            LastName = "Johnson",
            Email = "alice.johnson@example.com",
            Age = 28,
            PhoneNumber = "+1-555-4567"
        };

        var validator = new UserValidator();
        var resultTask = validator.ValidateAsync(user);
        var result = resultTask.GetAwaiter().GetResult();

        if (result.IsValid)
        {
            Console.WriteLine("[OK] Async validation succeeded!");
            Console.WriteLine($"  User: {user.FirstName} {user.LastName}");
        }
        else
        {
            Console.WriteLine("[FAIL] Async validation failed:");

            foreach (var error in result.Errors)
            {
                Console.WriteLine($"  - {error.PropertyName}: {error.Message}");
            }
        }
    }
}
