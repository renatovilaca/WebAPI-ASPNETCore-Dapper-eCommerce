using Microsoft.AspNetCore.Mvc;
using WebAPI_ASPNETCore_Dapper_eCommerce.Models;
using WebAPI_ASPNETCore_Dapper_eCommerce.Services.Interfaces;
using WebAPI_ASPNETCore_Dapper_eCommerce.Services;

namespace WebAPI_ASPNETCore_Dapper_eCommerce.Controllers
{
    [Route("api/Contrib/Users")]
    [ApiController]
    public class UserContribController : ControllerBase
    {

        private IUserContribService _userService;

        private readonly ILogger<UserContribController> _logger;
        public UserContribController(ILogger<UserContribController> logger)
        {
            _userService = new UserContribService();
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            try
            {
                return Ok(_userService.GetAllUsers());
            }
            catch (Exception ex)
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
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}: {ex.StackTrace}");
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        public IActionResult InsertUser([FromBody] User user)
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
        public IActionResult UpdateUser([FromBody] User user)
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
