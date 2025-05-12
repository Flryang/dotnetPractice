using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly DbService _dbService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, DbService dbService)
        {
            _logger = logger;
            _dbService = dbService;
        }

        [HttpPost]
        public async Task<ActionResult> Create(WeatherForecast forecast) 
        {
            var result = await _dbService.CreateAsync(forecast);
            return result? Ok() : BadRequest(new {message = "failed to create."});
        }

        [HttpPut]
        public async Task<ActionResult> Update(WeatherForecast forecast)
        {
            var result = await _dbService.UpdateAsync(forecast);
            return result ? Ok() : BadRequest(new { message = "failed to update." });
        }

        [HttpDelete("{date}")]
        public async Task<ActionResult> Delete(string date)
        {
            if (!DateTime.TryParse(date, out var result))
            {
                return BadRequest(new { message = "wrong format." });
            }
            var response = await _dbService.DeleteAsync(result);
            return response ? Ok() : BadRequest(new {message =  "failed to delete." });
        }

        [HttpGet("{date}")]
        public async Task<ActionResult<WeatherForecast>> GetByDate(string date)
        {
            if (!DateTime.TryParse(date, out var result))
            {
                return BadRequest(new { message = "wrong format." });
            }
            var stat = await _dbService.GetByDateAsync(result);
            if (stat == null)
            {
                return BadRequest(new { message = "no record found." });
            }
            return Ok(stat);
        }
    }
}
