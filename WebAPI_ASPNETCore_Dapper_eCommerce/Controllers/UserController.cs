using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebAPI_ASPNETCore_Dapper_eCommerce.Models;
using WebAPI_ASPNETCore_Dapper_eCommerce.Services;
using WebAPI_ASPNETCore_Dapper_eCommerce.Services.Interfaces;

namespace WebAPI_ASPNETCore_Dapper_eCommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService;

        private readonly ILogger<UserController> _logger;
        public UserController(ILogger<UserController> logger)
        {
            _userService = new UserService();
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            try
            {
                return Ok(_userService.GetAllUsers());
            }
            catch(Exception ex)
            {
                _logger.LogError($"{ex.Message}: {ex.StackTrace}");
                return BadRequest(ex.Message);
            }
            
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            try
            {
                var user = _userService.GetUserById(id);

                if (user == null)
                    return NotFound();

                return Ok(user);
            }
            catch(Exception ex)
            {
                _logger.LogError($"{ex.Message}: {ex.StackTrace}");
                return BadRequest(ex.Message);
            }
               
        }

        [HttpPost]
        public IActionResult InsertUser([FromBody]User user)
        {
            try
            {
                _userService.InsertUser(user);
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}: {ex.StackTrace}");
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult UpdateUser([FromBody]User user)
        {
            try
            {
                var searchUser = _userService.GetUserById(user.Id);

                if (searchUser == null)
                    return NotFound();

                _userService.UpdateUser(user);

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}: {ex.StackTrace}");
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                var searchUser = _userService.GetUserById(id);

                if (searchUser == null)
                    return NotFound();

                _userService.DeleteUser(id);

                return Ok("USER DELETED");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}: {ex.StackTrace}");
                return BadRequest(ex.Message);
            }
        }


    }
}
