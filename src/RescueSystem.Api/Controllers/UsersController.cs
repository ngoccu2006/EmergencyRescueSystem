using Microsoft.AspNetCore.Mvc;
using RescueSystem.Application.DTOs.User;
using RescueSystem.Application.Services;
using RescueSystem.Application.Common.Response;
using RescueSystem.Domain.Entities;

namespace RescueSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<User>>> CreateUser([FromBody] CreateUserDto createUserDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (success, message) = await _userService.CreateUserAsync(createUserDto);

            if (success)
            {
                _logger.LogInformation($"User created: {createUserDto.UserName}");
                return StatusCode(201, ApiResponse<object>.SuccessResponse(null, message, 201));
            }

            _logger.LogWarning($"Failed to create user: {message}");
            return BadRequest(new { message });
        }

        /// <summary>
        /// Get user by ID
        /// </summary>
        [HttpGet("{userId}")]
        public async Task<ActionResult<UserDto>> GetUserById(Guid userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning($"User not found: {userId}");
                return NotFound(new { message = "User not found" });
            }

            return Ok(user);
        }

        /// <summary>
        /// Get all users
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        /// <summary>
        /// Update user
        /// </summary>
        [HttpPut("{userId}")]
        public async Task<ActionResult<object>> UpdateUser(Guid userId, [FromBody] UpdateUserDto updateUserDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (success, message) = await _userService.UpdateUserAsync(userId, updateUserDto);

            if (success)
            {
                _logger.LogInformation($"User updated: {userId}");
                return Ok(new { message });
            }

            _logger.LogWarning($"Failed to update user: {message}");
            return BadRequest(new { message });
        }

        /// <summary>
        /// Delete user
        /// </summary>
        [HttpDelete("{userId}")]
        public async Task<ActionResult<object>> DeleteUser(Guid userId)
        {
            var (success, message) = await _userService.DeleteUserAsync(userId);

            if (success)
            {
                _logger.LogInformation($"User deleted: {userId}");
                return Ok(new { message });
            }

            _logger.LogWarning($"Failed to delete user: {message}");
            return BadRequest(new { message });
        }

        /// <summary>
        /// Change user password
        /// </summary>
        [HttpPost("{userId}/change-password")]
        public async Task<ActionResult<object>> ChangePassword(Guid userId, [FromBody] ChangePasswordDto changePasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (success, message) = await _userService.ChangePasswordAsync(userId, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);

            if (success)
            {
                _logger.LogInformation($"Password changed for user: {userId}");
                return Ok(new { message });
            }

            _logger.LogWarning($"Failed to change password: {message}");
            return BadRequest(new { message });
        }
    }

    public class ChangePasswordDto
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
