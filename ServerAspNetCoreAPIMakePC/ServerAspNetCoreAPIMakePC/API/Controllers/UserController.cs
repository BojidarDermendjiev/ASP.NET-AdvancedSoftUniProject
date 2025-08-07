namespace ServerAspNetCoreAPIMakePC.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    
    using Domain.ValueObjects;
    using Application.DTOs.User;
    using Application.Interfaces;
    using static Domain.ErrorMessages.ErrorMessages;

    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            this._userService = userService;
        }

        /// <summary>
        /// Registers a new user.
        /// POST /api/user/register
        /// </summary>
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto userRegistrationDto)
        {
            try
            {
                await this._userService.RegisterUserAsync(userRegistrationDto);
                return Ok(new { Message = "User registered successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        /// <summary>
        /// Authenticate user (login)
        /// POST /api/user/authenticate
        /// </summary>
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<ActionResult<UserDto?>> Authenticate([FromBody] AuthenticateUserDto dto)
        {
            var user = await _userService.AuthenticateUserAsync(new Email(dto.Email), dto.Password);
            if (user == null)
            {
                return Unauthorized(new { error = "Invalid credentials." });
            }
            return Ok(user);
        }

        /// <summary>
        /// Get user by ID.
        /// GET /api/user/{id}
        /// </summary>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetById(Guid id)
        {
            var user = await this._userService.GetUserByIdAsync(id);
            if (user is null)
            {
                return NotFound(new { error = String.Format(UserNotFound) });
            }
            return Ok(user);
        }
        /// <summary>
        /// Update user by ID.
        /// PUT /api/user/{id}
        /// </summary>
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserDto dto)
        {
            try
            {
                await this._userService.UpdateUserAsync(id, dto);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
        /// <summary>
        /// Delete user by ID.
        /// DELETE /api/user/{id}
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                await this._userService.DeleteUserAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Change password for user.
        /// POST /api/user/{id}/change-password
        /// </summary>
        [Authorize]
        [HttpPost("{id}/change-password")]
        public async Task<IActionResult> ChangePassword(Guid id, [FromBody] ChangePasswordDto dto)
        {
            try
            {
                await this._userService.ChangePasswordAsync(id, dto.NewPassword);
                return Ok(new { Message = UserPasswordChangeSuccessfully });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
