using Microsoft.AspNetCore.Mvc;
using MoodTrackerAPI.Models;
using MoodTrackerAPI.Services;

namespace MoodTrackerAPI.Controllers
{
    [Route("api/mood")]
    [ApiController]
    public class MoodTrackerController : ControllerBase
    {
        private readonly IResponseCalculator _calculator;
        private readonly IResponseRepository _responseRepo;

        public MoodTrackerController(IResponseCalculator calculator, IResponseRepository responseRepo)
        {
            _calculator = calculator;
            _responseRepo = responseRepo;
        }
        
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] QuestionnaireResponseDto response)
        {
            var mappedGad7 = _calculator.CalculateGad7Response(response.Gad7);
            var mappedPhq9 = _calculator.CalculatePhq9Response(response.Phq9);
            await _responseRepo.InsertResponses(mappedGad7, mappedPhq9);
            return new AcceptedResult();
        }

        [HttpGet]
        [Route("summary")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Summary))]
        public async Task<ActionResult<Summary>> GetResponseSummary()
        {
            var gad7 = await _responseRepo.GetGad7ResponseAveragesAsync();
            var phq9 = await _responseRepo.GetPhq9ResponseAveragesAsync();
            var summary = new Summary()
            {
                Gad7 = _calculator.CalculateGad7Response(gad7),
                Phq9 = _calculator.CalculatePhq9Response(phq9)
            };
            return Ok(summary);
        }
    }
}
