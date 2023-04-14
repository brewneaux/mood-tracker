using FluentAssertions;
using MoodTrackerAPI.Models;
using MoodTrackerAPI.Services;

namespace MoodTrackerApi.Tests.Services;

public class ResponseCalculatorTests
{
    [Fact]
    public void CalculateGad7Response_Should_Sum_Scores()
    {
        var gad7dto = new Gad7ResponseDto
        {
            AnxiousNervousOnEdge = 1,
            CantStopWorrying = 2,
            WorryingAboutTooManyThings = 3,
            TroubleRelaxing = 4,
            Restless = 1,
            Annoyed = 2,
            Afraid = 3
        };
        var calculator = new ResponseCalculator();
        var calculated = calculator.CalculateGad7Response(gad7dto);
        calculated.Score.Should().Be(16);
        calculated.Severity.Should().Be("severe anxiety");
    }

    [Fact]
    public void CalculatedGad7Response_Should_Have_Severities()
    {
        var gad7dto = new Gad7ResponseDto
        {
            AnxiousNervousOnEdge = 0,
            CantStopWorrying = 0,
            WorryingAboutTooManyThings = 0,
            TroubleRelaxing = 0,
            Restless = 0,
            Annoyed = 0,
            Afraid = 0
        };
        var calculator = new ResponseCalculator();
        var calculated = calculator.CalculateGad7Response(gad7dto);
        calculated.Severity.Should().Be("minimal anxiety");

        gad7dto.AnxiousNervousOnEdge = 4;
        gad7dto.CantStopWorrying = 2;
        calculated = calculator.CalculateGad7Response(gad7dto);
        calculated.Severity.Should().Be("mild anxiety");

        
        gad7dto.AnxiousNervousOnEdge = 4;
        gad7dto.CantStopWorrying = 4;
        gad7dto.WorryingAboutTooManyThings = 4;
        calculated = calculator.CalculateGad7Response(gad7dto);
        calculated.Severity.Should().Be("moderate anxiety");
    }
    
    [Fact]
    public void CalculatePhq9Response_Should_Sum_Scores()
    {
        var phq9dto = new Phq9ResponseDto()
        {
            LittleInterestOrPleasure = 1,
            DownDepressedHopeless = 2,
            TroubleSleeping = 3,
            FeelingTired = 4,
            Appetite = 1,
            SelfLoathing = 2,
            TroubleConcentrating = 3,
            SlowOrFast = 4,
            SelfHarm = 1
        };
        var calculator = new ResponseCalculator();
        var calculated = calculator.CalculatePhq9Response(phq9dto);
        calculated.Score.Should().Be(21);
        calculated.Severity.Should().Be("severe depression");
    }

}