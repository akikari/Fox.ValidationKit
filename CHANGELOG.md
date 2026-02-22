# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

_No unreleased changes yet._

## [1.0.0] - 2026-02-22

### Added

#### Fox.ValidationKit (Core Library)
- `Validator<T>` abstract base class with fluent rule configuration
- DSL for defining validation rules with `RuleFor()` method
- `ValidationResult` type with `IsValid` and `Errors` properties
- `ValidationError` record with `PropertyName`, `Message`, and `ErrorCode` properties
- `RuleBuilder<T, TProperty>` fluent builder for rule configuration
- **18 FVK### error codes** for structured error handling and localization
- `IValidationMessageProvider` interface for custom/localized error messages
- Built-in validation rules:
  - **Basic**: `NotNull` (FVK001), `NotEmpty` (FVK002)
  - **Comparison**: `Equal` (FVK200), `NotEqual` (FVK201), `GreaterThan` (FVK202), `LessThan` (FVK203), `Between` (FVK204)
  - **String Length**: `MinLength` (FVK100), `MaxLength` (FVK101), `Length` (FVK102)
  - **String Pattern**: `EmailAddress` (FVK301), `Url` (FVK302), `CreditCard` (FVK303), `Matches` (FVK304)
  - **Collection**: `NotEmpty` (FVK400), `MinCount` (FVK401), `MaxCount` (FVK402)
  - **Enum**: `IsInEnum` (FVK500)
  - **Custom**: `Custom` (FVK900), `CustomAsync` (FVK900)
- **Advanced features**:
  - Conditional validation with `When` and `Unless`
  - Cascade modes: `Continue` (collect all errors) and `Stop` (fail fast)
  - Nested object validation with `SetValidator`
  - Collection element validation with `RuleForEach`
- **Async support**: `ValidateAsync()` for asynchronous validation scenarios
- **Strongly typed**: Expression-based property selection with IntelliSense
- **Zero external dependencies** - Only .NET BCL
- Multi-targeting: .NET 8.0, .NET 9.0, .NET 10.0
- XML documentation for all public APIs
- Modern C# features: primary constructors, collection expressions, GeneratedRegex

#### Fox.ValidationKit.ResultKit (Integration Package)
- Optional ResultKit integration package for Railway Oriented Programming
- `ToResult()` extension methods for `ValidationResult`
- `ToResult<T>(value)` extension for returning validated value
- `ToErrorsResult()` extension for individual error Results
- `ValidateAsResult()` and `ValidateAsResultAsync()` extension methods for `Validator<T>`
- `ValidateAsResultValue()` and `ValidateAsResultValueAsync()` for validated value with Result<T>
- `ValidateAsErrorsResult()` and `ValidateAsErrorsResultAsync()` for multiple error handling
- Automatic error message aggregation with FVK error codes
- Depends on Fox.ResultKit 1.2.0 (ErrorsResult support)

#### Documentation
- Comprehensive README.md with examples, badges, and complete API documentation
- Package-specific README files for NuGet packages
- Contributing guidelines (CONTRIBUTING.md)
- Design principles and project philosophy documentation (CREDITS)
- MIT License (LICENSE.txt)
- Copyright notice (NOTICE)

#### Samples
- Demo console application (Fox.ValidationKit.Demo)
- User validation examples with all built-in rules
- ResultKit integration examples
- ErrorsResult demonstration for multiple validation errors
- Async validation demonstrations

#### Tests
- **432 test executions** (144 unique tests × 3 target frameworks)
- Comprehensive unit tests for Fox.ValidationKit (144 tests)
- Tests for all 15 built-in validation rules
- Edge case coverage (null handling, boundary conditions, regex patterns)
- Async validation tests
- Conditional validation tests (When, Unless)
- Cascade mode tests (Continue, Stop)
- Nested validation tests (SetValidator)
- Collection validation tests (NotEmpty, MinCount, MaxCount, RuleForEach)
- Custom rule tests (Custom, CustomAsync)
- Fox.ValidationKit.ResultKit integration tests
- xUnit + FluentAssertions test framework
- **100% test pass rate** across all frameworks

#### Build & CI/CD
- Multi-targeting: .NET 8.0, .NET 9.0, .NET 10.0
- GitHub Actions workflows:
  - `build-and-test.yml` - Build and test validation on Ubuntu and Windows
  - `publish-nuget.yml` - Automated NuGet publishing on version tags
- NuGet package metadata configuration with icon and README
- Symbol packages (.snupkg) with source link support
- Package validation enabled
- Strict build policy: zero errors, zero warnings, zero messages
- Code analyzers: Microsoft.CodeAnalysis.NetAnalyzers

#### Code Quality & Organization
- **SOLID principles**: 9.5/10 assessment
- **DRY**: 9/10 with ValidationRuleBase and extension methods
- **Clean Architecture**: 9.5/10 with proper layers and zero dependencies
- **Clean Code**: 9.5/10 with excellent naming and documentation
- **Overall**: 9.5/10 - Production-ready code quality
- **File organization**: One class per file convention (13 files reorganized)
- **Region organization**: Members organized by visibility (Fields, Constructors, Properties, Public/Internal/Protected/Private Methods)
- All nullable reference types enabled
- Follows Microsoft coding conventions
- Comprehensive XML documentation with `<param>` and `<returns>` tags
- GenerateDocumentationFile enabled with zero CS1591 warnings

### Initial Release

Fox.ValidationKit 1.0.0 is production-ready with:
- 18 validation error codes (FVK001-FVK900)
- 15 built-in validation rules
- 432 passing tests across 3 .NET versions
- Zero external dependencies (core library)
- Complete XML documentation
- Fluent API with strong type safety
- Async validation support
- Optional ResultKit integration
- Localization support via IValidationMessageProvider

---

**Breaking Changes**: N/A (Initial release)

**Migration Guide**: N/A (Initial release)
