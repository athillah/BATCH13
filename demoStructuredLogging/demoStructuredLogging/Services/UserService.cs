using demoStructuredLogging.Models;
using System.Diagnostics;

namespace demoStructuredLogging.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private static readonly List<User> _users = new List<User>();
        private static int _nextId = 1;

        public UserService(ILogger<UserService> logger)
        {
            _logger = logger;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request, string clientIp)
        {
            var correlationId = Guid.NewGuid().ToString();
            var stopwatch = Stopwatch.StartNew();

            _logger.LogInformation("User login attempt for {Username} from {ClientIP} at {Timestamp}. Correlation ID: {CorrelationId}",
                request.Username, clientIp, DateTime.UtcNow, correlationId);

            try
            {
                // Simulate async operation
                await Task.Delay(100);

                var user = _users.FirstOrDefault(u => u.Username == request.Username && u.IsActive);
                
                if (user == null)
                {
                    _logger.LogWarning("Login failed for {Username} from {ClientIP} - User not found. Correlation ID: {CorrelationId}",
                        request.Username, clientIp, correlationId);
                    
                    return new LoginResponse
                    {
                        Success = false,
                        Message = "Invalid username or password"
                    };
                }

                // Simulate password validation (never log actual passwords)
                if (request.Password != "password123")
                {
                    _logger.LogWarning("Login failed for {Username} from {ClientIP} - Invalid password. Correlation ID: {CorrelationId}",
                        request.Username, clientIp, correlationId);
                    
                    return new LoginResponse
                    {
                        Success = false,
                        Message = "Invalid username or password"
                    };
                }

                stopwatch.Stop();
                
                _logger.LogInformation("User login successful for {Username} from {ClientIP} in {ElapsedMilliseconds}ms. User ID: {UserId}, Correlation ID: {CorrelationId}",
                    request.Username, clientIp, stopwatch.ElapsedMilliseconds, user.Id, correlationId);

                return new LoginResponse
                {
                    Success = true,
                    Message = "Login successful",
                    User = user,
                    Token = $"mock-token-{user.Id}-{correlationId}"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during login for {Username} from {ClientIP}. Correlation ID: {CorrelationId}",
                    request.Username, clientIp, correlationId);
                
                return new LoginResponse
                {
                    Success = false,
                    Message = "An error occurred during login"
                };
            }
        }

        public async Task<User> CreateUserAsync(CreateUserRequest request)
        {
            var correlationId = Guid.NewGuid().ToString();
            var stopwatch = Stopwatch.StartNew();

            _logger.LogInformation("Creating new user with username {Username} and email {Email}. Correlation ID: {CorrelationId}",
                request.Username, request.Email, correlationId);

            try
            {
                // Simulate async operation
                await Task.Delay(200);

                // Check for existing user
                if (_users.Any(u => u.Username == request.Username))
                {
                    _logger.LogWarning("User creation failed - Username {Username} already exists. Correlation ID: {CorrelationId}",
                        request.Username, correlationId);
                    throw new InvalidOperationException("Username already exists");
                }

                if (_users.Any(u => u.Email == request.Email))
                {
                    _logger.LogWarning("User creation failed - Email {Email} already exists. Correlation ID: {CorrelationId}",
                        request.Email, correlationId);
                    throw new InvalidOperationException("Email already exists");
                }

                var user = new User
                {
                    Id = _nextId++,
                    Username = request.Username,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                _users.Add(user);
                stopwatch.Stop();

                _logger.LogInformation("User created successfully with ID {UserId} for username {Username} in {ElapsedMilliseconds}ms. Correlation ID: {CorrelationId}",
                    user.Id, user.Username, stopwatch.ElapsedMilliseconds, correlationId);

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user with username {Username}. Correlation ID: {CorrelationId}",
                    request.Username, correlationId);
                throw;
            }
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            _logger.LogDebug("Retrieving user with ID {UserId}", id);

            try
            {
                await Task.Delay(50); // Simulate async operation
                
                var user = _users.FirstOrDefault(u => u.Id == id);
                
                if (user == null)
                {
                    _logger.LogWarning("User with ID {UserId} not found", id);
                }
                else
                {
                    _logger.LogDebug("User with ID {UserId} retrieved successfully. Username: {Username}",
                        id, user.Username);
                }

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user with ID {UserId}", id);
                throw;
            }
        }

        public async Task<PaginatedResponse<User>> GetUsersAsync(PaginatedRequest request)
        {
            var correlationId = Guid.NewGuid().ToString();
            
            _logger.LogDebug("Querying users with parameters: Skip={Skip}, Take={Take}, Filter={Filter}. Correlation ID: {CorrelationId}",
                request.Skip, request.Take, request.Filter, correlationId);

            try
            {
                await Task.Delay(100); // Simulate async operation

                var filteredUsers = _users.AsQueryable();

                if (!string.IsNullOrEmpty(request.Filter))
                {
                    filteredUsers = filteredUsers.Where(u => 
                        u.Username.Contains(request.Filter, StringComparison.OrdinalIgnoreCase) ||
                        u.Email.Contains(request.Filter, StringComparison.OrdinalIgnoreCase) ||
                        u.FirstName.Contains(request.Filter, StringComparison.OrdinalIgnoreCase) ||
                        u.LastName.Contains(request.Filter, StringComparison.OrdinalIgnoreCase));
                }

                var totalCount = filteredUsers.Count();
                var users = filteredUsers.Skip(request.Skip).Take(request.Take).ToList();

                _logger.LogInformation("Users query completed: {TotalCount} total users, {ReturnedCount} returned. Correlation ID: {CorrelationId}",
                    totalCount, users.Count, correlationId);

                return new PaginatedResponse<User>
                {
                    Items = users,
                    TotalCount = totalCount,
                    Skip = request.Skip,
                    Take = request.Take
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error querying users with filter {Filter}. Correlation ID: {CorrelationId}",
                    request.Filter, correlationId);
                throw;
            }
        }
    }
}