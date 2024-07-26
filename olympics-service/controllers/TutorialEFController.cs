using Microsoft.AspNetCore.Mvc;
using OlympicsAPI.Data;
using OlympicsAPI.Dtos;
using OlympicsAPI.Models;

namespace OlympicsAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class TutorialEFController : ControllerBase
{
    DataContextEF _entityFramework;
    public TutorialEFController(IConfiguration config)
    {
        _entityFramework = new DataContextEF(config);
    }

    [HttpGet("GetUsers")]
    public IEnumerable<TutorialUser> GetUsers()
    {
        //DATA HERE: users is populated with result of entityFramework query. 
        //Unlike dapper, the SQL is abstracted away and we 'ask' EF for what we want.
        IEnumerable<TutorialUser> users = _entityFramework.TutorialUsers.ToList<TutorialUser>();
        return users;
    }


    [HttpGet("GetSingleUser/{userId}")]
    //returns a user model instance
    public TutorialUser GetSingleUser(int userId)

    {
        TutorialUser? user = _entityFramework.TutorialUsers
        .Where(u => u.UserId == userId)
        .FirstOrDefault<TutorialUser>();

        if (user != null)
        {
            return user;
        }

        throw new Exception("Failed to Get User");
    }



    [HttpPut("EditUser")]
    public IActionResult EditUser(TutorialUser tutorialUser)
    {
        //userDb = the user from the database
        TutorialUser? userDb = _entityFramework.TutorialUsers
        .Where(u => u.UserId == tutorialUser.UserId)
        .FirstOrDefault<TutorialUser>();

        if (userDb != null)
        {
            userDb.Active = tutorialUser.Active;
            userDb.FirstName = tutorialUser.FirstName;
            userDb.LastName = tutorialUser.LastName;
            userDb.Email = tutorialUser.Email;
            userDb.Gender = tutorialUser.Gender;
        }
        //Making a boolean of EF returning the no of rows, 0 = failure
        if (_entityFramework.SaveChanges() > 0)
        {
            return Ok();
        }

        throw new Exception("Failed to Update User");
    }

    [HttpPost("AddUser")]
    public IActionResult AddUser(TutorialUserDto tutorialUser)
    {
        TutorialUser userDb = new TutorialUser();

        userDb.Active = tutorialUser.Active;
        userDb.FirstName = tutorialUser.FirstName;
        userDb.LastName = tutorialUser.LastName;
        userDb.Email = tutorialUser.Email;
        userDb.Gender = tutorialUser.Gender;

        _entityFramework.Add(userDb);
        if (_entityFramework.SaveChanges() > 0)
        {
            return Ok();
        }
        throw new Exception("Failed to add user.");
    }

    [HttpDelete("DeleteTutorialUser/{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        TutorialUser? userDb = _entityFramework.TutorialUsers
        .Where(u => u.UserId == userId)
        .FirstOrDefault<TutorialUser>();

        if (userDb != null)
        {
            _entityFramework.TutorialUsers.Remove(userDb);
            if (_entityFramework.SaveChanges() > 0)
            {
                return Ok();
            }
            throw new Exception("Failed to delete user.");
        }
        //Making a boolean of EF returning the no of rows, 0 = failure

        throw new Exception("Failed to get user.");
    }
}