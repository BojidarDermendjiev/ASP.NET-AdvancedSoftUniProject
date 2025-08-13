namespace ServerAspNetCoreAPIMakePC.API.Controllers
{
    using Application.DTOs.User;
    using Application.Interfaces;
    using Domain.ValueObjects;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using ServerAspNetCoreAPIMakePC.Domain.Entities;
    using static Domain.ErrorMessages.ErrorMessages;

    [ApiController]
    [Route("api/user")]
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
                var user = await this._userService.RegisterUserAsync(userRegistrationDto);
                var token = _userService.GenerateJwtToken(user);
                return Ok(new
                {
                    user,
                    token
                });
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
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateUserDto dto)
        {
            var user = await _userService.AuthenticateUserAsync(new Email(dto.Email), dto.Password);
            if (user == null)
            {
                return Unauthorized(new { error = "Invalid credentials." });
            }
            var token = _userService.GenerateJwtToken(user);
            return Ok(new { token, user });
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
                await this._userService.ChangePasswordAsync(id, dto.OldPassword, dto.NewPassword);
                return Ok(new { Message = UserPasswordChangeSuccessfully });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Upload or update user avatar.
        /// PUT /api/user/{id}/avatar
        /// </summary>
        [Authorize]
        [HttpPut("{id}/avatar")]
        public async Task<IActionResult> UploadAvatar(Guid id)
        {
            if (!Request.HasFormContentType || !Request.Form.Files.Any())
                return BadRequest(new { error = "No file uploaded." });

            var file = Request.Form.Files[0];
            if (file.Length == 0)
                return BadRequest(new { error = "Uploaded file is empty." });

            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            var avatarBytes = ms.ToArray();
            var fileName = file.FileName;

            await _userService.UpdateAvatarAsync(id, avatarBytes, fileName);

            return Ok(new { message = "Avatar updated successfully." });
        }

        /// <summary>
        /// Get all users (or clients).
        /// GET /api/user/clients
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("clients")]
        public async Task<IActionResult> GetClients()
        {
            var clients = await _userService.GetClientsAsync();
            return Ok(clients);
        }
    }
}