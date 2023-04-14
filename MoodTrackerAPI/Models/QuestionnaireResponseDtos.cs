using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MoodTrackerAPI.Models;


public record class Gad7ResponseDto
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
    
}

public record class Phq9ResponseDto
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
    
}

public record class QuestionnaireResponseDto
{
    public Gad7ResponseDto Gad7 { get; set; }
    public Phq9ResponseDto Phq9 { get; set; }
}

