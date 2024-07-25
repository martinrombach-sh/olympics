using Microsoft.AspNetCore.Mvc;

namespace OlympicsAPI.Controllers;

[ApiController]
[Route("[controller]")] //

//To use, we need builder.Services.AddControllers();
//and app.MapControllers(); in Program.cs

public class TutorialController : ControllerBase
{
    DataContextDapper _dapper;
    public TutorialController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
    }

    [HttpGet("TutorialConnection")]
    public DateTime TutorialConnection()
    {
        return _dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
    }

    [HttpGet("GetSingleUser/{userId}")]
    //returns a user model instance
    public TutorialUser GetSingleUser(int userId)
    {
        //id variable is stringified and attached to sql query (query must be string cannot mix)
        string sql = @"
            SELECT [UserId],
                [FirstName],
                [LastName],
                [Email],
                [Gender],
                [Active] 
            FROM TutorialAppSchema.Users
                WHERE UserId = " + userId.ToString();

        //DATA HERE: user is populated with result of dapper query, using sql above
        TutorialUser user = _dapper.LoadDataSingle<TutorialUser>(sql);
        return user;
    }


    [HttpGet("GetUsers")]
    //returns an array of model instances
    public IEnumerable<TutorialUser> GetUsers()
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

        //DATA HERE: users is populated with result of dapper query, using sql above
        IEnumerable<TutorialUser> users = _dapper.LoadData<TutorialUser>(sql);
        return users;
    }

    [HttpPut("EditUser")]
    public IActionResult EditUser(TutorialUser tutorialUser)
    {
        /*
        E.g.
        UPDATE TutorialAppSchema.Users
            SET [FirstName] = 'steve',
            [LastName] = 'jobs',
            [Email] = 'steve@apple.com',
            [Gender] = 'male',
            [Active] = 1
        WHERE UserId = 1 
        
        BUG - Invalid column name False 
        Solution -> SQL accepts 1s and 0s and strings 'false' and 'true' in boolean slots.
        If you send a boolean true or false, it will think you are sending a column name.
        [Active] = 'true'
        */

        string sql = @"
        UPDATE TutorialAppSchema.Users 
        SET [FirstName] = '" + tutorialUser.FirstName +
            "', [LastName] = '" + tutorialUser.LastName +
            "', [Email] = '" + tutorialUser.Email +
            "', [Gender] = '" + tutorialUser.Gender +
            "', [Active] = '" + tutorialUser.Active +
        "' WHERE UserId = " + tutorialUser.UserId;

        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Failed to update user.");
    }

    [HttpPost("AddUser")]
    public IActionResult AddUser(TutorialUser tutorialUser)
    {
        string sql = @"INSERT INTO TutorialAppSchema.Users(
        [FirstName],
        [LastName],
        [Email],
        [Gender],
        [Active]
        ) VALUES ('" + tutorialUser.FirstName +
            "', '" + tutorialUser.LastName +
            "', '" + tutorialUser.Email +
            "', '" + tutorialUser.Gender +
            "', '" + tutorialUser.Active +
            "')";
        //TIP! If you are having trouble with SQL in here, 
        //console log the sql in the console and then run it in your SQL client.
        //You'll see the results quickly.
        //Console.WriteLine(sql);

        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Failed to add user.");
    }
}