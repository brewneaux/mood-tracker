using FluentAssertions;
using MoodTrackerAPI.Configuration;

namespace MoodTrackerApi.Tests.Configuration;


public class PostgresConnectionConfigurationTests
{
    [Fact]
    public void PostgresConnectionConfig_Should_HaveConnectionString()
    {
        var config = new PostgresConnectionConfiguration()
        {
            Host = "unittests",
            User = "testuser",
            Password = "testpassword",
            DatabaseName = "testdb"
        };
        config.ConnectionString.Should()
            .Be("Server=unittests;Port=5432;User Id=testuser;Password=testpassword;CommandTimeout=20;database=testdb");
    }
}