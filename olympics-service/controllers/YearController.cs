
using Microsoft.AspNetCore.Mvc;

namespace olympics_service.controllers
{

    [ApiController]
    [Route("[controller]")]
    public class YearController : ControllerBase
    {
        public YearController()
        {

        }

        [HttpGet("/year")]
        //public IActionResult Test()
        public string Year(string year)
        {
            string result = year;
            return result;
        }

        //Get argument example
        [HttpGet("/year/{range}")]
        //public IActionResult Test()
        public string YearRange(string range)
        {
            string result = range;
            return result;
        }

    }
}
