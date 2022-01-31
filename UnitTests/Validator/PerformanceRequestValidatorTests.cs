using System;
using System.Collections.Generic;
using Core.Validators;
using FluentAssertions;
using Xunit;

namespace UnitTests.Validator;

public class PerformanceRequestValidatorTests
{
    private readonly IPerformanceRequestValidator _validator;

    public PerformanceRequestValidatorTests() => _validator = new PerformanceRequestValidator();

    [Fact]
    public void EmptySymbolList_Return_False() => 
        _validator.Validate(new List<string>(), DateTimeOffset.Now).Should().BeFalse();
    
    [Fact]
    public void Date_In_Future_Return_False() => 
        _validator.Validate(new List<string>(), DateTimeOffset.Now.AddDays(1)).Should().BeFalse();

    [Fact]
    public void Valid_Request() => 
        _validator.Validate(new List<string> {"AAA"}, DateTimeOffset.Now.AddDays(-1)).Should().BeTrue();
}