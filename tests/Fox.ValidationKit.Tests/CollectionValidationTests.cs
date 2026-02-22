//==================================================================================================
// Unit tests for collection validation features.
// Tests cover NotEmpty, MinCount, MaxCount, and ForEach validation rules.
//==================================================================================================

using FluentAssertions;

namespace Fox.ValidationKit.Tests;

//==================================================================================================
/// <summary>
/// Tests for collection validation rules.
/// </summary>
//==================================================================================================
public sealed class CollectionValidationTests
{
    //==============================================================================================
    /// <summary>
    /// Test model for collection validation.
    /// </summary>
    //==============================================================================================
    private sealed class TodoList
    {
        public IEnumerable<string>? Items { get; set; }
        public IEnumerable<int>? Priorities { get; set; }
    }

    //==============================================================================================
    /// <summary>
    /// Validator for TodoList with collection rules.
    /// </summary>
    //==============================================================================================
    private sealed class TodoListValidator : Validator<TodoList>
    {
        public TodoListValidator()
        {
            RuleFor(x => x.Items).NotEmpty();
            RuleFor(x => x.Items).MinCount(1).MaxCount(10);
            RuleFor(x => x.Priorities).ForEach(p => p > 0, "Priority must be greater than 0");
        }
    }

    [Fact]
    public void NotEmpty_should_fail_for_empty_collection()
    {
        var validator = new TodoListValidator();
        var list = new TodoList { Items = [] };

        var result = validator.Validate(list);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Items");
    }

    [Fact]
    public void NotEmpty_should_fail_for_null_collection()
    {
        var validator = new TodoListValidator();
        var list = new TodoList { Items = null };

        var result = validator.Validate(list);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Items");
    }

    [Fact]
    public void NotEmpty_should_pass_for_non_empty_collection()
    {
        var validator = new TodoListValidator();
        var list = new TodoList { Items = ["Item 1"] };

        var result = validator.Validate(list);

        result.Errors.Should().NotContain(e => e.PropertyName == "Items" && e.Message.Contains("empty"));
    }

    [Fact]
    public void MinCount_should_fail_when_below_minimum()
    {
        var validator = new TodoListValidator();
        var list = new TodoList { Items = [] };

        var result = validator.Validate(list);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Message.Contains("at least"));
    }

    [Fact]
    public void MinCount_should_pass_when_at_minimum()
    {
        var validator = new TodoListValidator();
        var list = new TodoList { Items = ["Item 1"] };

        var result = validator.Validate(list);

        result.Errors.Should().NotContain(e => e.Message.Contains("at least"));
    }

    [Fact]
    public void MaxCount_should_fail_when_above_maximum()
    {
        var validator = new TodoListValidator();
        var list = new TodoList { Items = Enumerable.Range(1, 15).Select(i => $"Item {i}") };

        var result = validator.Validate(list);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Message.Contains("not exceed"));
    }

    [Fact]
    public void MaxCount_should_pass_when_at_maximum()
    {
        var validator = new TodoListValidator();
        var list = new TodoList { Items = Enumerable.Range(1, 10).Select(i => $"Item {i}") };

        var result = validator.Validate(list);

        result.Errors.Should().NotContain(e => e.Message.Contains("not exceed"));
    }

    [Fact]
    public void ForEach_should_validate_all_elements()
    {
        var validator = new TodoListValidator();
        var list = new TodoList { Items = ["Valid"], Priorities = [-1, 5, 0] };

        var result = validator.Validate(list);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName.StartsWith("Priorities["));
    }

    [Fact]
    public void ForEach_should_report_correct_index_for_invalid_elements()
    {
        var validator = new TodoListValidator();
        var list = new TodoList { Items = ["Valid"], Priorities = [-1, 5, 0] };

        var result = validator.Validate(list);

        result.Errors.Should().Contain(e => e.PropertyName == "Priorities[0]");
        result.Errors.Should().Contain(e => e.PropertyName == "Priorities[2]");
        result.Errors.Should().NotContain(e => e.PropertyName == "Priorities[1]");
    }

    [Fact]
    public void ForEach_should_pass_when_all_elements_valid()
    {
        var validator = new TodoListValidator();
        var list = new TodoList { Items = ["Valid"], Priorities = [1, 5, 10] };

        var result = validator.Validate(list);

        result.Errors.Should().NotContain(e => e.PropertyName.StartsWith("Priorities["));
    }
}
