using System.ComponentModel.DataAnnotations;

namespace MoodTrackerAPI.Configuration;

public class PostgresConnectionConfiguration
{
    [Required]
    public string Host { get; set; }
    public int Port { get; set; } = 5432;
    [Required]
    public string User { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string DatabaseName { get; set; }

    public bool EnableMigrations { get; set; } = true;
    public string ConnectionString =>
        $"Server={Host};Port={Port};User Id={User};Password={Password};CommandTimeout=20;database={DatabaseName}";

}