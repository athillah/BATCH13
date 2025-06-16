using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FilmAPI.DTOs;
using FilmAPI.Models;
using FilmAPI.Services;

namespace FilmAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _UserManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            UserManager<User> UserManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager,
            IJwtTokenService jwtTokenService,
            ILogger<AuthController> logger)
        {
            _UserManager = UserManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _jwtTokenService = jwtTokenService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var existingUser = await _UserManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                    return Conflict(new { message = "User with this email already exists" });

                var User = new User
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    EmailConfirmed = true
                };

                var result = await _UserManager.CreateAsync(User, model.Password);
                if (result.Succeeded)
                {
                    await _UserManager.AddToRoleAsync(User, "User");
                    _logger.LogInformation("User {Email} registered successfully", model.Email);
                    
                    return Ok(new { 
                        message = "User registered successfully",
                        email = User.Email,
                        fullName = User.FullName
                    });
                }

                return BadRequest(new { message = "User registration failed", errors = result.Errors });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during User registration");
                return StatusCode(500, new { message = "Registration failed" });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var User = await _UserManager.FindByEmailAsync(model.Email);
                if (User == null)
                    return Unauthorized(new { message = "Invalid email or password" });

                var result = await _signInManager.CheckPasswordSignInAsync(User, model.Password, lockoutOnFailure: true);
                
                if (result.IsLockedOut)
                    return StatusCode(423, new { message = "Account is locked out" });

                if (!result.Succeeded)
                    return Unauthorized(new { message = "Invalid email or password" });

                var roles = await _UserManager.GetRolesAsync(User);
                var token = await _jwtTokenService.GenerateTokenAsync(User, roles);

                _logger.LogInformation("User {Email} logged in successfully", model.Email);

                return Ok(new AuthResponseDTO
                {
                    Token = token,
                    Email = User.Email ?? "",
                    FullName = User.FullName,
                    Roles = roles.ToList(),
                    ExpiresAt = DateTime.UtcNow.AddMinutes(60)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login");
                return StatusCode(500, new { message = "Login failed" });
            }
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized();

                var appUser = await _UserManager.FindByIdAsync(userId);
                if (appUser == null)
                    return NotFound(new { message = "User not found" });

                var roles = await _UserManager.GetRolesAsync(appUser);

                return Ok(new UserProfileDTO
                {
                    Id = appUser.Id,
                    Email = appUser.Email ?? "",
                    FirstName = appUser.FirstName,
                    LastName = appUser.LastName,
                    CreatedAt = appUser.CreatedAt,
                    Roles = roles.ToList()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving User profile");
                return StatusCode(500, new { message = "Failed to retrieve profile" });
            }
        }

        [HttpGet("Users")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var Users = _UserManager.Users.ToList();
                var UserProfiles = new List<UserProfileDTO>();

                foreach (var User in Users)
                {
                    var roles = await _UserManager.GetRolesAsync(User);
                    UserProfiles.Add(new UserProfileDTO
                    {
                        Id = User.Id,
                        Email = User.Email ?? "",
                        FirstName = User.FirstName,
                        LastName = User.LastName,
                        CreatedAt = User.CreatedAt,
                        Roles = roles.ToList()
                    });
                }

                return Ok(UserProfiles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all Users");
                return StatusCode(500, new { message = "Failed to retrieve Users" });
            }
        }

        [HttpPost("assign-role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleDTO model)
        {
            try
            {
                var User = await _UserManager.FindByIdAsync(model.UserId);
                if (User == null)
                    return NotFound(new { message = "User not found" });

                if (!await _roleManager.RoleExistsAsync(model.Role))
                    return BadRequest(new { message = "Role does not exist" });

                var result = await _UserManager.AddToRoleAsync(User, model.Role);
                if (result.Succeeded)
                {
                    return Ok(new { message = $"Role {model.Role} assigned to User successfully" });
                }

                return BadRequest(new { message = "Failed to assign role", errors = result.Errors });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error assigning role");
                return StatusCode(500, new { message = "Failed to assign role" });
            }
        }

        [HttpGet("test-auth")]
        [Authorize]
        public IActionResult TestAuth()
        {
            return Ok(new { message = "You are authenticated!", User = User.Identity?.Name });
        }

        [HttpGet("test-admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult TestAdmin()
        {
            return Ok(new { message = "You have admin access!", User = User.Identity?.Name });
        }
    }

    public class AssignRoleDTO
    {
        public string UserId { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}