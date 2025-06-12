using demoStructuredLogging.Models;

namespace demoStructuredLogging.Services
{
    public interface IUserService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request, string clientIp);
        Task<User> CreateUserAsync(CreateUserRequest request);
        Task<User?> GetUserByIdAsync(int id);
        Task<PaginatedResponse<User>> GetUsersAsync(PaginatedRequest request);
    }

    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(CreateOrderRequest request);
        Task<Order?> GetOrderByIdAsync(int id);
        Task<PaginatedResponse<Order>> GetOrdersByUserIdAsync(int userId, PaginatedRequest request);
        Task<bool> ProcessPaymentAsync(int orderId, PaymentRequest request);
        Task<bool> ProcessShippingAsync(int orderId, ShippingRequest request);
    }
}