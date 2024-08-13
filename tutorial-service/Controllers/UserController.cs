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

public class UserController : ControllerBase
{
    DataContextDapper _dapper;
    public UserController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
    }

    [HttpGet("/")]
    public DateTime GetDate()
    {
        return _dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
    }

    [HttpGet("GetUsers")]
    //returns an array of model instances
    public IEnumerable<User> GetUsers()
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
        IEnumerable<User> users = _dapper.LoadData<User>(sql);
        return users;
    }


    [HttpGet("GetSingleUser/{userId}")]
    //returns a user model instance
    public User GetSingleUser(int userId)
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
        User user = _dapper.LoadDataSingle<User>(sql);
        return user;
    }



    [HttpPut("EditUser")]
    public IActionResult EditUser(User user)
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
        SET [FirstName] = '" + user.FirstName +
            "', [LastName] = '" + user.LastName +
            "', [Email] = '" + user.Email +
            "', [Gender] = '" + user.Gender +
            "', [Active] = '" + user.Active +
        "' WHERE UserId = " + user.UserId;

        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Failed to update user.");
    }

    [HttpPost("AddUser")]
    public IActionResult AddUser(UserDto yser)
    {
        string sql = @"INSERT INTO TutorialAppSchema.Users(
        [FirstName],
        [LastName],
        [Email],
        [Gender],
        [Active]
        ) VALUES ('" + yser.FirstName +
            "', '" + yser.LastName +
            "', '" + yser.Email +
            "', '" + yser.Gender +
            "', '" + yser.Active +
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

    [HttpDelete("DeleteUser/{userId}")]
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



    [HttpGet("UserSalary/{UserId}")]
    public IEnumerable<UserSalary> GetUserSalary(int userId)
    {
        return _dapper.LoadData<UserSalary>(@"
            SELECT UserSalary.UserId
                    , UserSalary.Salary
            FROM  AppSchema.UserSalary
                WHERE UserId = " + userId.ToString());
    }

    [HttpPost("UserSalary")]
    public IActionResult PostUserSalary(UserSalary userSalaryForInsert)
    {
        string sql = @"
            INSERT INTO TutorialAppSchema.UserSalary (
                userId,
                Salary
            ) VALUES (" + userSalaryForInsert.UserId.ToString()
                + ", " + userSalaryForInsert.Salary
                + ")";

        if (_dapper.ExecuteSqlWithRowCount(sql) > 0)
        {
            return Ok(userSalaryForInsert);
        }
        throw new Exception("Adding user Salary failed on save");
    }

    [HttpPut("UserSalary")]
    public IActionResult PutUserSalary(UserSalary userSalaryForUpdate)
    {
        string sql = "UPDATE TutorialAppSchema.userSalary SET Salary="
            + userSalaryForUpdate.Salary
            + " WHERE userId=" + userSalaryForUpdate.UserId.ToString();

        if (_dapper.ExecuteSql(sql))
        {
            return Ok(userSalaryForUpdate);
        }
        throw new Exception("Updating user Salary failed on save");
    }

    [HttpDelete("UserSalary/{UserId}")]
    public IActionResult DeleteuserSalary(int userId)
    {
        string sql = "DELETE FROM TutorialAppSchema.UserSalary WHERE userId=" + userId.ToString();

        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }
        throw new Exception("Deleting User Salary failed on save");
    }

    [HttpGet("UserJobInfo/{UserId}")]
    public IEnumerable<UserJobInfo> GetuserJobInfo(int userId)
    {
        return _dapper.LoadData<UserJobInfo>(@"
            SELECT  UserJobInfo.userId
                    , UserJobInfo.JobTitle
                    , UserJobInfo.Department
            FROM  TutorialAppSchema.userJobInfo
                WHERE userId = " + userId.ToString());
    }

    [HttpPost("UserJobInfo")]
    public IActionResult PostuserJobInfo(UserJobInfo userJobInfoForInsert)
    {
        string sql = @"
            INSERT INTO TutorialAppSchema.UserJobInfo (
                UserId,
                Department,
                JobTitle
            ) VALUES (" + userJobInfoForInsert.UserId
                + ", '" + userJobInfoForInsert.Department
                + "', '" + userJobInfoForInsert.JobTitle
                + "')";

        if (_dapper.ExecuteSql(sql))
        {
            return Ok(userJobInfoForInsert);
        }
        throw new Exception("Adding user Job Info failed on save");
    }

    [HttpPut("UserJobInfo")]
    public IActionResult PutuserJobInfo(UserJobInfo userJobInfoForUpdate)
    {
        string sql = "UPDATE TutorialAppSchema.UserJobInfo SET Department='"
            + userJobInfoForUpdate.Department
            + "', JobTitle='"
            + userJobInfoForUpdate.JobTitle
            + "' WHERE UserId=" + userJobInfoForUpdate.UserId.ToString();

        if (_dapper.ExecuteSql(sql))
        {
            return Ok(userJobInfoForUpdate);
        }
        throw new Exception("Updating user Job Info failed on save");
    }

    // [HttpDelete("userJobInfo/{userId}")]
    // public IActionResult DeleteuserJobInfo(int userId)
    // {
    //     string sql = "DELETE FROM TutorialAppSchema.userJobInfo  WHERE userId=" + userId;

    //     if (_dapper.ExecuteSql(sql))
    //     {
    //         return Ok();
    //     }
    //     throw new Exception("Deleting user Job Info failed on save");
    // }

    [HttpDelete("UserJobInfo/{UserId}")]
    public IActionResult DeleteuserJobInfo(int userId)
    {
        string sql = @"
            DELETE FROM TutorialAppSchema.UserJobInfo 
                WHERE UserId = " + userId.ToString();

        Console.WriteLine(sql);

        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Failed to Delete user");
    }
}
