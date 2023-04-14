using System.Data;
using Dapper;
using MoodTrackerAPI.Models;

namespace MoodTrackerAPI.Services;

public interface IResponseRepository
{
    Task InsertResponses(Gad7Response gad7, Phq9Response phq9);
    Task<Phq9ResponseDto> GetPhq9ResponseAveragesAsync();
    Task<Gad7ResponseDto> GetGad7ResponseAveragesAsync();
}

public class ResponseRepository : IResponseRepository
{
    private readonly IDbConnection _connection;

    public ResponseRepository(IDbConnection connection)
    {
        _connection = connection;
        _connection.Open();
    }

    public async Task InsertResponses(Gad7Response gad7, Phq9Response phq9)
    {
        using var transaction = _connection.BeginTransaction();
        try
        {
            await InsertGad7(transaction, gad7);
            await InsertPhq9(transaction, phq9);
        }
        catch (Exception e)
        {
            transaction.Rollback();
            throw;
        }
        transaction.Commit();
    }

    private async Task InsertGad7(IDbTransaction transaction, Gad7Response response)
    {
        var query = @"
        INSERT INTO gad7 (
            anxious_nervous_on_edge,
            cant_stop_worrying,
            worrying_about_too_many_things,
            trouble_relaxing,
            restless,
            annoyed,
            afraid,
            score,
            severity
        )
        VALUES (
            @AnxiousNervousOnEdge,
            @CantStopWorrying,
            @WorryingAboutTooManyThings,
            @TroubleRelaxing,
            @Restless,
            @Annoyed,
            @Afraid,
            @Score,
            @Severity
        )";
        await transaction.Connection.ExecuteAsync(query, response);
    }
    
    private async Task InsertPhq9(IDbTransaction transaction, Phq9Response response)
    {
        var query = @"
        INSERT INTO phq9 (
            little_interest_or_pleasure,
            down_depressed_hopeless,
            trouble_sleeping,
            feeling_tired,
            appetite,
            self_loathing,
            trouble_concentrating,
            slow_or_fast,
            self_harm,
            score,
            severity
        )
        VALUES (
            @LittleInterestOrPleasure,
            @DownDepressedHopeless,
            @TroubleSleeping,
            @FeelingTired,
            @Appetite,
            @SelfLoathing,
            @TroubleConcentrating,
            @SlowOrFast,
            @SelfHarm,
            @Score,
            @Severity
        )";
        await transaction.Connection.ExecuteAsync(query, response);
    }

    public async Task<Gad7ResponseDto> GetGad7ResponseAveragesAsync()
    {
        var query = @"SELECT
            AVG(anxious_nervous_on_edge) AS AnxiousNervousOnEdge,
            AVG(cant_stop_worrying) AS CantStopWorrying,
            AVG(worrying_about_too_many_things) AS WorryingAboutTooManyThings,
            AVG(trouble_relaxing) AS TroubleRelaxing,
            AVG(restless) AS Restless,
            AVG(annoyed) AS Annoyed,
            AVG(afraid) AS Afraid
        FROM gad7";
        return await _connection.QueryFirstOrDefaultAsync<Gad7ResponseDto>(query);
    }

    public async Task<Phq9ResponseDto> GetPhq9ResponseAveragesAsync()
    {
        var query = @"SELECT
            AVG(little_interest_or_pleasure) AS LittleInterestOrPleasure,
            AVG(down_depressed_hopeless) AS DownDepressedHopeless,
            AVG(trouble_sleeping) AS TroubleSleeping,
            AVG(feeling_tired) AS FeelingTired,
            AVG(appetite) AS Appetite,
            AVG(self_loathing) AS SelfLoathing,
            AVG(trouble_concentrating) AS TroubleConcentrating,
            AVG(slow_or_fast) AS SlowOrFast,
            AVG(self_harm) AS SelfHarm
        FROM phq9";
        return await _connection.QueryFirstOrDefaultAsync<Phq9ResponseDto>(query);
    }
};