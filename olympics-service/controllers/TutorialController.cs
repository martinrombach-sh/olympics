using Microsoft.AspNetCore.Mvc;
using OlympicsAPI.Data;
using OlympicsAPI.Dtos;
using OlympicsAPI.Models;

//if you don't add the sub name space (the suffix Controllers), your file will waste memory loading the entire namespace
namespace OlympicsAPI.Controllers;

[ApiController]
[Route("[controller]")]

//To use, we need builder.Services.AddControllers();
//and app.MapControllers(); in Program.cs

public class TutorialController : ControllerBase
{
    DataContextDapper _dapper;
    public TutorialController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
    }

    [HttpGet("/")]
    public DateTime TutorialConnection()
    {
        return _dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
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
    public IActionResult AddUser(TutorialUserDto tutorialUser)
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

    [HttpDelete("DeleteTutorialUser/{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        string sql = "DELETE FROM TutorialAppSchema.Users WHERE UserId = " + userId.ToString();

        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Failed to update user.");

    }


    //TROUBLESHOOTING
    //https://github.com/DomTripodi93/DotNetAPICourse/tree/main/3-APIBasics/8-DotnetAPI_UserPeripherals/Controllers 



    [HttpGet("TutorialUserSalary/{TutorialUserId}")]
    public IEnumerable<TutorialUserSalary> GetTutorialUserSalary(int TutorialUserId)
    {
        return _dapper.LoadData<TutorialUserSalary>(@"
            SELECT TutorialUserSalary.TutorialUserId
                    , TutorialUserSalary.Salary
            FROM  TutorialAppSchema.TutorialUserSalary
                WHERE TutorialUserId = " + TutorialUserId.ToString());
    }

    [HttpPost("TutorialUserSalary")]
    public IActionResult PostTutorialUserSalary(TutorialUserSalary TutorialUserSalaryForInsert)
    {
        string sql = @"
            INSERT INTO TutorialAppSchema.TutorialUserSalary (
                TutorialUserId,
                Salary
            ) VALUES (" + TutorialUserSalaryForInsert.UserId.ToString()
                + ", " + TutorialUserSalaryForInsert.Salary
                + ")";

        if (_dapper.ExecuteSqlWithRowCount(sql) > 0)
        {
            return Ok(TutorialUserSalaryForInsert);
        }
        throw new Exception("Adding TutorialUser Salary failed on save");
    }

    [HttpPut("TutorialUserSalary")]
    public IActionResult PutTutorialUserSalary(TutorialUserSalary TutorialUserSalaryForUpdate)
    {
        string sql = "UPDATE TutorialAppSchema.TutorialUserSalary SET Salary="
            + TutorialUserSalaryForUpdate.Salary
            + " WHERE TutorialUserId=" + TutorialUserSalaryForUpdate.UserId.ToString();

        if (_dapper.ExecuteSql(sql))
        {
            return Ok(TutorialUserSalaryForUpdate);
        }
        throw new Exception("Updating TutorialUser Salary failed on save");
    }

    [HttpDelete("TutorialUserSalary/{TutorialUserId}")]
    public IActionResult DeleteTutorialUserSalary(int TutorialUserId)
    {
        string sql = "DELETE FROM TutorialAppSchema.TutorialUserSalary WHERE TutorialUserId=" + TutorialUserId.ToString();

        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }
        throw new Exception("Deleting TutorialUser Salary failed on save");
    }

    [HttpGet("TutorialUserJobInfo/{TutorialUserId}")]
    public IEnumerable<TutorialUserJobInfo> GetTutorialUserJobInfo(int TutorialUserId)
    {
        return _dapper.LoadData<TutorialUserJobInfo>(@"
            SELECT  TutorialUserJobInfo.TutorialUserId
                    , TutorialUserJobInfo.JobTitle
                    , TutorialUserJobInfo.Department
            FROM  TutorialAppSchema.TutorialUserJobInfo
                WHERE TutorialUserId = " + TutorialUserId.ToString());
    }

    [HttpPost("TutorialUserJobInfo")]
    public IActionResult PostTutorialUserJobInfo(TutorialUserJobInfo TutorialUserJobInfoForInsert)
    {
        string sql = @"
            INSERT INTO TutorialAppSchema.TutorialUserJobInfo (
                TutorialUserId,
                Department,
                JobTitle
            ) VALUES (" + TutorialUserJobInfoForInsert.UserId
                + ", '" + TutorialUserJobInfoForInsert.Department
                + "', '" + TutorialUserJobInfoForInsert.JobTitle
                + "')";

        if (_dapper.ExecuteSql(sql))
        {
            return Ok(TutorialUserJobInfoForInsert);
        }
        throw new Exception("Adding TutorialUser Job Info failed on save");
    }

    [HttpPut("TutorialUserJobInfo")]
    public IActionResult PutTutorialUserJobInfo(TutorialUserJobInfo TutorialUserJobInfoForUpdate)
    {
        string sql = "UPDATE TutorialAppSchema.TutorialUserJobInfo SET Department='"
            + TutorialUserJobInfoForUpdate.Department
            + "', JobTitle='"
            + TutorialUserJobInfoForUpdate.JobTitle
            + "' WHERE TutorialUserId=" + TutorialUserJobInfoForUpdate.UserId.ToString();

        if (_dapper.ExecuteSql(sql))
        {
            return Ok(TutorialUserJobInfoForUpdate);
        }
        throw new Exception("Updating TutorialUser Job Info failed on save");
    }

    // [HttpDelete("TutorialUserJobInfo/{TutorialUserId}")]
    // public IActionResult DeleteTutorialUserJobInfo(int TutorialUserId)
    // {
    //     string sql = "DELETE FROM TutorialAppSchema.TutorialUserJobInfo  WHERE TutorialUserId=" + TutorialUserId;

    //     if (_dapper.ExecuteSql(sql))
    //     {
    //         return Ok();
    //     }
    //     throw new Exception("Deleting TutorialUser Job Info failed on save");
    // }

    [HttpDelete("TutorialUserJobInfo/{TutorialUserId}")]
    public IActionResult DeleteTutorialUserJobInfo(int TutorialUserId)
    {
        string sql = @"
            DELETE FROM TutorialAppSchema.TutorialUserJobInfo 
                WHERE TutorialUserId = " + TutorialUserId.ToString();

        Console.WriteLine(sql);

        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Failed to Delete TutorialUser");
    }
}
