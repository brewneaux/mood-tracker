using MoodTrackerAPI.Models;

namespace MoodTrackerAPI.Services;

public interface IResponseCalculator
{
    public Gad7Response CalculateGad7Response(Gad7ResponseDto responseDto);
    public Phq9Response CalculatePhq9Response(Phq9ResponseDto responseDto);
}
public class ResponseCalculator : IResponseCalculator
{
    public Gad7Response CalculateGad7Response(Gad7ResponseDto responseDto)
    {
        var score =
            responseDto.Afraid + responseDto.Annoyed + responseDto.Restless +
            responseDto.TroubleRelaxing + responseDto.CantStopWorrying +
            responseDto.AnxiousNervousOnEdge + responseDto.WorryingAboutTooManyThings;
        string severity;
        switch (score)
        {
            case <= 4:
                severity = "minimal anxiety";
                break;
            case > 4 and <= 9:
                severity = "mild anxiety";
                break;
            case > 9 and <= 14:
                severity = "moderate anxiety";
                break;
            case >= 14:
                severity = "severe anxiety";
                break;
        }
        return new Gad7Response()
        {
            Afraid = responseDto.Afraid,
            Annoyed = responseDto.Annoyed,
            AnxiousNervousOnEdge = responseDto.AnxiousNervousOnEdge,
            CantStopWorrying = responseDto.CantStopWorrying,
            Restless = responseDto.Restless,
            TroubleRelaxing = responseDto.TroubleRelaxing,
            WorryingAboutTooManyThings = responseDto.WorryingAboutTooManyThings,
            Score = score,
            Severity = severity
        };
    }

    public Phq9Response CalculatePhq9Response(Phq9ResponseDto responseDto)
    {
        var score =
            responseDto.LittleInterestOrPleasure +
            responseDto.DownDepressedHopeless +
            responseDto.TroubleSleeping +
            responseDto.FeelingTired +
            responseDto.Appetite +
            responseDto.SelfLoathing +
            responseDto.TroubleConcentrating +
            responseDto.SlowOrFast +
            responseDto.SelfHarm;
        string severity;
        switch (score)
        {
            case <= 4:
                severity = "minimal depression";
                break;
            case > 4 and <= 9:
                severity = "mild depression";
                break;
            case > 9 and <= 14:
                severity = "moderate depression";
                break;
            case > 14 and <= 19:
                severity = "moderately severe depression";
                break;
            case > 19:
                severity = "severe depression";
                break;
        }

        return new Phq9Response()
        {
            LittleInterestOrPleasure = responseDto.LittleInterestOrPleasure,
            DownDepressedHopeless = responseDto.DownDepressedHopeless,
            TroubleSleeping = responseDto.TroubleSleeping,
            FeelingTired = responseDto.FeelingTired,
            Appetite = responseDto.Appetite,
            SelfLoathing = responseDto.SelfLoathing,
            TroubleConcentrating = responseDto.TroubleConcentrating,
            SlowOrFast = responseDto.SlowOrFast,
            SelfHarm = responseDto.SelfHarm,
            Score = score,
            Severity = severity
        };
    }
}