using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using demoStructuredLogging.Models;
using demoStructuredLogging.Services;

namespace demoStructuredLogging.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserService _userService;

        public UsersController(ILogger<UsersController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<LoginResponse>>> Login([FromBody] LoginRequest request)
        {
            var correlationId = Guid.NewGuid().ToString();
            var stopwatch = Stopwatch.StartNew();
            var clientIp = GetClientIpAddress();

            _logger.LogInformation("Login request received for username {Username} from {ClientIP}. Correlation ID: {CorrelationId}",
                request.Username, clientIp, correlationId);

            try
            {
                var result = await _userService.LoginAsync(request, clientIp);
                stopwatch.Stop();

                var response = new ApiResponse<LoginResponse>
                {
                    Success = result.Success,
                    Message = result.Message,
                    Data = result,
                    CorrelationId = correlationId
                };

                if (result.Success)
                {
                    _logger.LogInformation("Login request completed successfully for username {Username} in {ElapsedMilliseconds}ms. Correlation ID: {CorrelationId}",
                        request.Username, stopwatch.ElapsedMilliseconds, correlationId);
                    return Ok(response);
                }
                else
                {
                    _logger.LogWarning("Login request failed for username {Username} in {ElapsedMilliseconds}ms. Correlation ID: {CorrelationId}",
                        request.Username, stopwatch.ElapsedMilliseconds, correlationId);
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing login request for username {Username}. Correlation ID: {CorrelationId}",
                    request.Username, correlationId);

                return StatusCode(500, new ApiResponse<LoginResponse>
                {
                    Success = false,
                    Message = "An error occurred while processing the login request",
                    CorrelationId = correlationId
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<User>>> CreateUser([FromBody] CreateUserRequest request)
        {
            var correlationId = Guid.NewGuid().ToString();
            var stopwatch = Stopwatch.StartNew();

            _logger.LogInformation("Create user request received for username {Username}. Correlation ID: {CorrelationId}",
                request.Username, correlationId);

            try
            {
                var user = await _userService.CreateUserAsync(request);
                stopwatch.Stop();

                _logger.LogInformation("Create user request completed successfully for username {Username} in {ElapsedMilliseconds}ms. User ID: {UserId}, Correlation ID: {CorrelationId}",
                    request.Username, stopwatch.ElapsedMilliseconds, user.Id, correlationId);

                var response = new ApiResponse<User>
                {
                    Success = true,
                    Message = "User created successfully",
                    Data = user,
                    CorrelationId = correlationId
                };

                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, response);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Create user request failed for username {Username} - {ErrorMessage}. Correlation ID: {CorrelationId}",
                    request.Username, ex.Message, correlationId);

                return BadRequest(new ApiResponse<User>
                {
                    Success = false,
                    Message = ex.Message,
                    CorrelationId = correlationId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing create user request for username {Username}. Correlation ID: {CorrelationId}",
                    request.Username, correlationId);

                return StatusCode(500, new ApiResponse<User>
                {
                    Success = false,
                    Message = "An error occurred while creating the user",
                    CorrelationId = correlationId
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<User>>> GetUser(int id)
        {
            var correlationId = Guid.NewGuid().ToString();

            _logger.LogDebug("Get user request received for ID {UserId}. Correlation ID: {CorrelationId}",
                id, correlationId);

            try
            {
                var user = await _userService.GetUserByIdAsync(id);

                if (user == null)
                {
                    _logger.LogWarning("Get user request failed - User {UserId} not found. Correlation ID: {CorrelationId}",
                        id, correlationId);

                    return NotFound(new ApiResponse<User>
                    {
                        Success = false,
                        Message = "User not found",
                        CorrelationId = correlationId
                    });
                }

                _logger.LogDebug("Get user request completed successfully for ID {UserId}. Correlation ID: {CorrelationId}",
                    id, correlationId);

                return Ok(new ApiResponse<User>
                {
                    Success = true,
                    Message = "User retrieved successfully",
                    Data = user,
                    CorrelationId = correlationId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing get user request for ID {UserId}. Correlation ID: {CorrelationId}",
                    id, correlationId);

                return StatusCode(500, new ApiResponse<User>
                {
                    Success = false,
                    Message = "An error occurred while retrieving the user",
                    CorrelationId = correlationId
                });
            }
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PaginatedResponse<User>>>> GetUsers([FromQuery] PaginatedRequest request)
        {
            var correlationId = Guid.NewGuid().ToString();

            _logger.LogDebug("Get users request received with parameters: Skip={Skip}, Take={Take}, Filter={Filter}. Correlation ID: {CorrelationId}",
                request.Skip, request.Take, request.Filter, correlationId);

            try
            {
                var result = await _userService.GetUsersAsync(request);

                _logger.LogDebug("Get users request completed successfully. Returned {Count} users. Correlation ID: {CorrelationId}",
                    result.Items.Count, correlationId);

                return Ok(new ApiResponse<PaginatedResponse<User>>
                {
                    Success = true,
                    Message = "Users retrieved successfully",
                    Data = result,
                    CorrelationId = correlationId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing get users request. Correlation ID: {CorrelationId}",
                    correlationId);

                return StatusCode(500, new ApiResponse<PaginatedResponse<User>>
                {
                    Success = false,
                    Message = "An error occurred while retrieving users",
                    CorrelationId = correlationId
                });
            }
        }

        private string GetClientIpAddress()
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            if (string.IsNullOrEmpty(ipAddress) || ipAddress == "::1")
                ipAddress = "127.0.0.1";
            return ipAddress;
        }
    }
}