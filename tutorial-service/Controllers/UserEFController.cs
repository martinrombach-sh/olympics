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
public class UserEFController : ControllerBase
{
    DataContextEF _entityFramework;
    IMapper _mapper;

    IUserRepository _userRepository;
    public UserEFController(IConfiguration config, IUserRepository userRepository)
    {
        _entityFramework = new DataContextEF(config);

        _userRepository = userRepository;

        _mapper = new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<UserDto, User>();
        }));
    }

    [HttpGet("GetUsers")]
    //returns an array of model instances
    public IEnumerable<User> GetUsers()
    {
        //DATA HERE: users is populated with result of entityFramework query. 
        //Unlike dapper, the SQL is abstracted away and we 'ask' EF for what we want.
        IEnumerable<User> users = _entityFramework.Users.ToList<User>();
        return users;
    }


    [HttpGet("GetSingleUser/{userId}")]
    //returns a user model instance
    public User GetSingleUser(int userId)

    {
        //DATA HERE: user is declared from the result of EF searching the db for the user id
        User? user = _entityFramework.Users
        .Where(u => u.UserId == userId)
        .FirstOrDefault<User>();

        if (user != null)
        {

            return user;
        }

        throw new Exception("Failed to Get User");
    }



    [HttpPut("EditUser")]
    public IActionResult EditUser(User User)
    {
        //DATA HERE
        User? userDb = _entityFramework.Users
        .Where(u => u.UserId == User.UserId)
        .FirstOrDefault<User>();

        if (userDb != null)
        {
            userDb.Active = User.Active;
            userDb.FirstName = User.FirstName;
            userDb.LastName = User.LastName;
            userDb.Email = User.Email;
            userDb.Gender = User.Gender;
        }

        if (_userRepository.SaveChanges())
        {
            return Ok();
        }

        throw new Exception("Failed to Update User");
    }

    [HttpPost("AddUser")]
    public IActionResult AddUser(UserDto User)
    {
        /*User userDb = new User();
        userDb.Active = User.Active;
        userDb.FirstName = User.FirstName;
        userDb.LastName = User.LastName;
        userDb.Email = User.Email;
        userDb.Gender = User.Gender;*/

        ////DATA HERE mapper is used here to map content in add user
        User userDb = _mapper.Map<User>(User);

        //EF updates the db
        _userRepository.AddEntity<User>(userDb);
        if (_userRepository.SaveChanges())
        {
            return Ok();
        }
        throw new Exception("Failed to add user.");
    }

    [HttpDelete("DeleteUser/{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        User? userDb = _entityFramework.Users
        .Where(u => u.UserId == userId)
        .FirstOrDefault<User>();

        if (userDb != null)
        {
            _userRepository.RemoveEntity<User>(userDb);
            if (_userRepository.SaveChanges())
            {
                return Ok();
            }
            throw new Exception("Failed to delete user.");
        }

        throw new Exception("Failed to get user.");
    }

    //TROUBLESHOOTING
    //https://github.com/DomTripodi93/DotNetAPICourse/tree/main/3-APIBasics/8-DotnetAPI_UserPeripherals/Controllers 

    [HttpGet("UserSalary/{UserId}")]
    public IEnumerable<UserSalary> GetUserSalaryEF(int UserId)
    {
        return _entityFramework.UserSalary
            .Where(u => u.UserId == UserId)
            .ToList();
    }

    [HttpPost("UserSalary")]
    public IActionResult PostUserSalaryEf(UserSalary UserForInsert)
    {
        _userRepository.AddEntity<UserSalary>(UserForInsert);
        if (_userRepository.SaveChanges())
        {
            return Ok();
        }
        throw new Exception("Adding UserSalary failed on save");
    }


    [HttpPut("UserSalary")]
    public IActionResult PutUserSalaryEf(UserSalary UserForUpdate)
    {
        UserSalary? UserToUpdate = _entityFramework.UserSalary
            .Where(u => u.UserId == UserForUpdate.UserId)
            .FirstOrDefault();

        if (UserToUpdate != null)
        {
            _mapper.Map(UserForUpdate, UserToUpdate);
            if (_userRepository.SaveChanges())
            {
                return Ok();
            }
            throw new Exception("Updating UserSalary failed on save");
        }
        throw new Exception("Failed to find UserSalary to Update");
    }


    [HttpDelete("UserSalary/{UserId}")]
    public IActionResult DeleteUserSalaryEf(int UserId)
    {
        UserSalary? UserToDelete = _entityFramework.UserSalary
            .Where(u => u.UserId == UserId)
            .FirstOrDefault();

        if (UserToDelete != null)
        {
            _userRepository.RemoveEntity<UserSalary>(UserToDelete);
            if (_userRepository.SaveChanges())
            {
                return Ok();
            }
            throw new Exception("Deleting UserSalary failed on save");
        }
        throw new Exception("Failed to find UserSalary to delete");
    }


    [HttpGet("UserJobInfo/{UserId}")]
    public IEnumerable<UserJobInfo> GetUserJobInfoEF(int UserId)
    {
        return _entityFramework.UserJobInfo
            .Where(u => u.UserId == UserId)
            .ToList();
    }

    [HttpPost("UserJobInfo")]
    public IActionResult PostUserJobInfoEf(UserJobInfo UserForInsert)
    {
        _userRepository.AddEntity<UserJobInfo>(UserForInsert);
        if (_userRepository.SaveChanges())
        {
            return Ok();
        }
        throw new Exception("Adding UserJobInfo failed on save");
    }


    [HttpPut("UserJobInfo")]
    public IActionResult PutUserJobInfoEf(UserJobInfo UserForUpdate)
    {
        UserJobInfo? UserToUpdate = _entityFramework.UserJobInfo
            .Where(u => u.UserId == UserForUpdate.UserId)
            .FirstOrDefault();

        if (UserToUpdate != null)
        {
            _mapper.Map(UserForUpdate, UserToUpdate);
            if (_userRepository.SaveChanges())
            {
                return Ok();
            }
            throw new Exception("Updating UserJobInfo failed on save");
        }
        throw new Exception("Failed to find UserJobInfo to Update");
    }


    [HttpDelete("UserJobInfo/{UserId}")]
    public IActionResult DeleteUserJobInfoEf(int UserId)
    {
        UserJobInfo? UserToDelete = _entityFramework.UserJobInfo
            .Where(u => u.UserId == UserId)
            .FirstOrDefault();

        if (UserToDelete != null)
        {
            _userRepository.RemoveEntity<UserJobInfo>(UserToDelete);
            if (_userRepository.SaveChanges())
            {
                return Ok();
            }
            throw new Exception("Deleting UserJobInfo failed on save");
        }
        throw new Exception("Failed to find UserJobInfo to delete");
    }

}