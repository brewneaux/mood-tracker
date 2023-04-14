using System.Net;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoodTrackerAPI.Configuration;
using MoodTrackerAPI.Controllers;
using MoodTrackerAPI.Models;
using MoodTrackerAPI.Services;
using NSubstitute;
using NuGet.Common;

namespace MoodTrackerApi.Tests.Controllers;

public class MoodTrackerControllerTests
{
    [Fact]
    public void Controller_Should_Call_Calculator_and_Repo()
    {
        var calculator = Substitute.For<IResponseCalculator>();
        var gad7 = new Gad7Response
        {
            AnxiousNervousOnEdge = 3,
            Score = 3,
            Severity = "mild"
        };
        var phq9 = new Phq9Response()
        {
            LittleInterestOrPleasure = 3,
            Score = 3,
            Severity = "mild"
        };
        calculator.CalculateGad7Response(Arg.Any<Gad7ResponseDto>()).Returns(gad7);
        calculator.CalculatePhq9Response(Arg.Any<Phq9ResponseDto>()).Returns(phq9);
        var repo = Substitute.For<IResponseRepository>();
        var controller = new MoodTrackerController(calculator, repo);
        var requestObject = new QuestionnaireResponseDto
        {
            Gad7 = new Gad7ResponseDto
            {
                AnxiousNervousOnEdge = 3,
            },
            Phq9 = new Phq9ResponseDto
            {
                LittleInterestOrPleasure = 3,
            }
        };
        controller.Post(requestObject).Wait();
        repo.Received().InsertResponses(gad7, phq9);
    }

    [Fact]
    public void Controller_Should_BindModel()
    {
        Environment.SetEnvironmentVariable("POSTGRES__HOST", "testhost");
        Environment.SetEnvironmentVariable("POSTGRES__USER", "testuser");
        Environment.SetEnvironmentVariable("POSTGRES__PASSWORD", "testpassword");
        Environment.SetEnvironmentVariable("POSTGRES__DATABASENAME", "testdb");
        Environment.SetEnvironmentVariable("POSTGRES__ENABLEMIGRATIONS", "false");
        var mockCalc = Substitute.For<IResponseCalculator>();
        var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(c =>
        {
            c.ConfigureServices(s =>
            {
                s.AddTransient(_ => Substitute.For<IResponseRepository>());
                s.AddTransient<IResponseCalculator>(_ => mockCalc);
            });
        });
        var client = factory.CreateClient();
        var json = @"{
    ""Gad7"": {
        ""AnxiousNervousOnEdge"": 1,
        ""CantStopWorrying"": 2,
        ""WorryingAboutTooManyThings"": 3,
        ""TroubleRelaxing"": 4,
        ""Restless"": 1,
        ""Annoyed"": 2,
        ""Afraid"": 3
    },
    ""Phq9"": {
        ""LittleInterestOrPleasure"": 1,
        ""DownDepressedHopeless"": 2,
        ""TroubleSleeping"": 3,
        ""FeelingTired"": 4,
        ""Appetite"": 1,
        ""SelfLoathing"": 2,
        ""TroubleConcentrating"": 3,
        ""SlowOrFast"": 4,
        ""SelfHarm"": 1
    }
}";
        var response = client.PostAsync("api/mood", new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json)).Result;
        response.StatusCode.Should().Be(HttpStatusCode.Accepted);
        mockCalc.Received().CalculateGad7Response(Arg.Is<Gad7ResponseDto>(dto => dto.TroubleRelaxing == 4));
    }
    
    [Fact]
    public void Controller_Should_Not_Bind_Invalid_Model()
    {
        Environment.SetEnvironmentVariable("POSTGRES__HOST", "testhost");
        Environment.SetEnvironmentVariable("POSTGRES__USER", "testuser");
        Environment.SetEnvironmentVariable("POSTGRES__PASSWORD", "testpassword");
        Environment.SetEnvironmentVariable("POSTGRES__DATABASENAME", "testdb");
        Environment.SetEnvironmentVariable("POSTGRES__ENABLEMIGRATIONS", "false");
        var mockCalc = Substitute.For<IResponseCalculator>();
        var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(c =>
        {
            c.ConfigureServices(s =>
            {
                s.Configure<PostgresConnectionConfiguration>(opts =>
                {
                    opts.Host = "testhost";
                    opts.Port = 123;
                    opts.User = "user";
                    opts.Password = "pass";
                    opts.DatabaseName = "db";
                });
                s.AddTransient(_ => Substitute.For<IResponseRepository>());
                s.AddTransient<IResponseCalculator>(_ => mockCalc);
            });
        });
        var client = factory.CreateClient();
        var json = @"{
    ""Gad7"": {
        ""AnxiousNervousOnEdge"": 1,
        ""CantStopWorrying"": 2,
        ""WorryingAboutTooManyThings"": 3,
        ""TroubleRelaxing"": 4,
        ""Restless"": 1,
        ""Annoyed"": 2,
        ""Afraid"": 31012
    },
    ""Phq9"": {
        ""LittleInterestOrPleasure"": 1,
        ""DownDepressedHopeless"": 2,
        ""TroubleSleeping"": 3,
        ""FeelingTired"": 4,
        ""Appetite"": 1,
        ""SelfLoathing"": 2,
        ""TroubleConcentrating"": 3,
        ""SlowOrFast"": 4,
        ""SelfHarm"": 1
    }
}";
        var response = client.PostAsync("api/mood", new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json)).Result;
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var responseText = response.Content.ReadAsStringAsync().Result;
        responseText.Should().Contain("must be between 1 and 4");
    }

    [Fact]
    public async Task GetResponseSummary_Should_Return_Summary()
    {
        var repo = Substitute.For<IResponseRepository>();
        var calc = new ResponseCalculator();
        repo.GetGad7ResponseAveragesAsync().Returns(new Gad7ResponseDto
        {
            AnxiousNervousOnEdge = 1,
            CantStopWorrying = 2,
            WorryingAboutTooManyThings = 3,
            TroubleRelaxing = 4,
            Restless = 1,
            Annoyed = 2,
            Afraid = 3
        });
        repo.GetPhq9ResponseAveragesAsync().Returns(new Phq9ResponseDto
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
        });
        
        var controller = new MoodTrackerController(calc, repo);
        var summaryResponse = await controller.GetResponseSummary();
        var okSummary = summaryResponse.Result.Should().BeAssignableTo<OkObjectResult>().Subject;
        var summary = okSummary.Value.Should().BeAssignableTo<Summary>().Subject;
        summary.Gad7.Afraid.Should().Be(3);
        summary.Phq9.Appetite.Should().Be(1);
        summary.Gad7.Score.Should().Be(16);
        summary.Gad7.Severity.Should().Be("severe anxiety");
        summary.Phq9.Score.Should().Be(21);
        summary.Phq9.Severity.Should().Be("severe depression");
    }
}