
using System.ComponentModel.DataAnnotations;

public record class Gad7Response
{
    [Range(1,4)]
    [Required]
    public int AnxiousNervousOnEdge { get; set; }
    [Range(1,4)]
    [Required]
    public int CantStopWorrying { get; set; }
    [Range(1,4)]
    [Required]
    public int WorryingAboutTooManyThings { get; set; }
    [Range(1,4)]
    [Required]
    public int TroubleRelaxing { get; set; }
    [Range(1,4)]
    [Required]
    public int Restless { get; set; }
    [Range(1,4)]
    [Required]
    public int Annoyed { get; set; }
    [Range(1,4)]
    [Required]
    public int Afraid { get; set; }
    
    public int Score { get; set; }
    public string Severity { get; set; }
}

public record class Phq9Response
{
    [Required]
    [Range(1,4)]
    public int LittleInterestOrPleasure { get; set; }
    [Required]
    [Range(1,4)]
    public int DownDepressedHopeless { get; set; }
    [Required]
    [Range(1,4)]
    public int TroubleSleeping { get; set; }
    [Required]
    [Range(1,4)]
    public int FeelingTired { get; set; }
    [Required]
    [Range(1,4)]
    public int Appetite { get; set; }
    [Required]
    [Range(1,4)]
    public int SelfLoathing { get; set; }
    [Required]
    [Range(1,4)]
    public int TroubleConcentrating { get; set; }
    [Required]
    [Range(1,4)]
    public int SlowOrFast { get; set; }
    [Required]
    [Range(1,4)]
    public int SelfHarm { get; set; }
    
    public int Score {get; set; }
    public string Severity { get; set; }
}

public class Summary
{
    public Gad7Response Gad7 { get; set; }
    public Phq9Response Phq9 { get; set; }
}