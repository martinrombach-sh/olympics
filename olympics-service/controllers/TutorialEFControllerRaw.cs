using AutoMapper;
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
public class TutorialEFControllerRaw : ControllerBase
{
    DataContextEF _entityFramework;
    IMapper _mapper;
    public TutorialEFControllerRaw(IConfiguration config)
    {
        _entityFramework = new DataContextEF(config);

        _mapper = new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<TutorialUserDto, TutorialUser>();
        }));
    }

    [HttpGet("GetUsers")]
    //returns an array of model instances
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
        //DATA HERE: user is declared from the result of EF searching the db for the user id
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
        //DATA HERE
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

        if (_entityFramework.SaveChanges() > 0)
        {
            return Ok();
        }

        throw new Exception("Failed to Update User");
    }

    [HttpPost("AddUser")]
    public IActionResult AddUser(TutorialUserDto tutorialUser)
    {
        /*TutorialUser userDb = new TutorialUser();
        userDb.Active = tutorialUser.Active;
        userDb.FirstName = tutorialUser.FirstName;
        userDb.LastName = tutorialUser.LastName;
        userDb.Email = tutorialUser.Email;
        userDb.Gender = tutorialUser.Gender;*/

        ////DATA HERE mapper is used here to map content in add user
        TutorialUser userDb = _mapper.Map<TutorialUser>(tutorialUser);

        //EF updates the db
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

        throw new Exception("Failed to get user.");
    }

    //TROUBLESHOOTING
    //https://github.com/DomTripodi93/DotNetAPICourse/tree/main/3-APIBasics/8-DotnetAPI_UserPeripherals/Controllers 

    [HttpGet("TutorialUserSalary/{TutorialUserId}")]
    public IEnumerable<TutorialUserSalary> GetTutorialUserSalaryEF(int TutorialUserId)
    {
        return _entityFramework.TutorialUserSalary
            .Where(u => u.UserId == TutorialUserId)
            .ToList();
    }

    [HttpPost("TutorialUserSalary")]
    public IActionResult PostTutorialUserSalaryEf(TutorialUserSalary TutorialUserForInsert)
    {
        _entityFramework.TutorialUserSalary.Add(TutorialUserForInsert);
        if (_entityFramework.SaveChanges() > 0)
        {
            return Ok();
        }
        throw new Exception("Adding TutorialUserSalary failed on save");
    }


    [HttpPut("TutorialUserSalary")]
    public IActionResult PutTutorialUserSalaryEf(TutorialUserSalary TutorialUserForUpdate)
    {
        TutorialUserSalary? TutorialUserToUpdate = _entityFramework.TutorialUserSalary
            .Where(u => u.UserId == TutorialUserForUpdate.UserId)
            .FirstOrDefault();

        if (TutorialUserToUpdate != null)
        {
            _mapper.Map(TutorialUserForUpdate, TutorialUserToUpdate);
            if (_entityFramework.SaveChanges() > 0)
            {
                return Ok();
            }
            throw new Exception("Updating TutorialUserSalary failed on save");
        }
        throw new Exception("Failed to find TutorialUserSalary to Update");
    }


    [HttpDelete("TutorialUserSalary/{TutorialUserId}")]
    public IActionResult DeleteTutorialUserSalaryEf(int TutorialUserId)
    {
        TutorialUserSalary? TutorialUserToDelete = _entityFramework.TutorialUserSalary
            .Where(u => u.UserId == TutorialUserId)
            .FirstOrDefault();

        if (TutorialUserToDelete != null)
        {
            _entityFramework.TutorialUserSalary.Remove(TutorialUserToDelete);
            if (_entityFramework.SaveChanges() > 0)
            {
                return Ok();
            }
            throw new Exception("Deleting TutorialUserSalary failed on save");
        }
        throw new Exception("Failed to find TutorialUserSalary to delete");
    }


    [HttpGet("TutorialUserJobInfo/{TutorialUserId}")]
    public IEnumerable<TutorialUserJobInfo> GetTutorialUserJobInfoEF(int TutorialUserId)
    {
        return _entityFramework.TutorialUserJobInfo
            .Where(u => u.UserId == TutorialUserId)
            .ToList();
    }

    [HttpPost("TutorialUserJobInfo")]
    public IActionResult PostTutorialUserJobInfoEf(TutorialUserJobInfo TutorialUserForInsert)
    {
        _entityFramework.TutorialUserJobInfo.Add(TutorialUserForInsert);
        if (_entityFramework.SaveChanges() > 0)
        {
            return Ok();
        }
        throw new Exception("Adding TutorialUserJobInfo failed on save");
    }


    [HttpPut("TutorialUserJobInfo")]
    public IActionResult PutTutorialUserJobInfoEf(TutorialUserJobInfo TutorialUserForUpdate)
    {
        TutorialUserJobInfo? TutorialUserToUpdate = _entityFramework.TutorialUserJobInfo
            .Where(u => u.UserId == TutorialUserForUpdate.UserId)
            .FirstOrDefault();

        if (TutorialUserToUpdate != null)
        {
            _mapper.Map(TutorialUserForUpdate, TutorialUserToUpdate);
            if (_entityFramework.SaveChanges() > 0)
            {
                return Ok();
            }
            throw new Exception("Updating TutorialUserJobInfo failed on save");
        }
        throw new Exception("Failed to find TutorialUserJobInfo to Update");
    }


    [HttpDelete("TutorialUserJobInfo/{TutorialUserId}")]
    public IActionResult DeleteTutorialUserJobInfoEf(int TutorialUserId)
    {
        TutorialUserJobInfo? TutorialUserToDelete = _entityFramework.TutorialUserJobInfo
            .Where(u => u.UserId == TutorialUserId)
            .FirstOrDefault();

        if (TutorialUserToDelete != null)
        {
            _entityFramework.TutorialUserJobInfo.Remove(TutorialUserToDelete);
            if (_entityFramework.SaveChanges() > 0)
            {
                return Ok();
            }
            throw new Exception("Deleting TutorialUserJobInfo failed on save");
        }
        throw new Exception("Failed to find TutorialUserJobInfo to delete");
    }

}