using Microsoft.AspNetCore.Mvc;

namespace OlympicsAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class YearController : ControllerBase
    {
        public YearController()
        {

        }

        [HttpGet("/")]
        //public IActionResult Test()
        public string Year(string year)
        {
            string result = year;
            return result;
        }

        //Get argument example
        [HttpGet("/{range}")]
        //public IActionResult Test()
        public string YearRange(string range)
        {
            string result = range;
            return result;
        }
    }
}