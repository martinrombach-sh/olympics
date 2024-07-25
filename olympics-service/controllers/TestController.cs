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
    /*
    [HttpGet("GetSingleUser/{userId}")]
    //returns a user model instance
    public TestUser GetSingleUser(int userId)
    {

    }
    */
    [HttpGet("GetUsers")]
    //returns an array of model instances
    public IEnumerable<TestUser> GetUsers()
    {
        //@" = multiline string
        string sql = @"
            SELECT [UserId],
                [FirstName],
                [LastName],
                [Email],
                [Gender],
                [Active] 
            FROM TutorialAppSchema.Users";

        //DATA HERE: sql is fed into query method and returned as users
        IEnumerable<TestUser> users = _dapper.LoadData<TestUser>(sql);
        return users;
    }


}