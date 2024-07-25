using Microsoft.AspNetCore.Mvc;

namespace OlympicsAPI.Controllers;

[ApiController]
[Route("[controller]")] //
//[ApiVersion] could be here -> what's that for?

//Lecture 58 -> FirstCustomController
//need builder.Services.AddControllers();
//and app.MapControllers(); in Program.cs

public class TestController : ControllerBase
{
    DataContextDapper _dapper;
    public TestController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
    }

    [HttpGet("TestConnection")]
    public DateTime TestConnection()
    {
        return _dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
    }

    //Get argument example
    [HttpGet("endpoint/{testValue}")]
    //public IActionResult Test()
    public string[] Test(string testValue)
    {
        string[] result = new string[] { "test1", "test2", "test3", testValue };
        return result;
    }
}