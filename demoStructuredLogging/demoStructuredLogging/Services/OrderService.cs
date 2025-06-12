using demoStructuredLogging.Models;
using System.Diagnostics;

namespace demoStructuredLogging.Services
{
    public class OrderService : IOrderService
    {
        private readonly ILogger<OrderService> _logger;
        private readonly IUserService _userService;
        private static readonly List<Order> _orders = new List<Order>();
        private static int _nextId = 1;

        public OrderService(ILogger<OrderService> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        public async Task<Order> CreateOrderAsync(CreateOrderRequest request)
        {
            var correlationId = Guid.NewGuid().ToString();
            var stopwatch = Stopwatch.StartNew();

            _logger.LogInformation("Creating order for user {UserId} with {ItemCount} items. Correlation ID: {CorrelationId}",
                request.UserId, request.Items.Count, correlationId);

            try
            {
                // Validate user exists
                var user = await _userService.GetUserByIdAsync(request.UserId);
                if (user == null)
                {
                    _logger.LogWarning("Order creation failed - User {UserId} not found. Correlation ID: {CorrelationId}",
                        request.UserId, correlationId);
                    throw new InvalidOperationException("User not found");
                }

                // Validate items
                if (!request.Items.Any())
                {
                    _logger.LogWarning("Order creation failed - No items provided for user {UserId}. Correlation ID: {CorrelationId}",
                        request.UserId, correlationId);
                    throw new ArgumentException("Order must contain at least one item");
                }

                var totalAmount = request.Items.Sum(item => item.Subtotal);

                var order = new Order
                {
                    Id = _nextId++,
                    UserId = request.UserId,
                    TotalAmount = totalAmount,
                    Status = OrderStatus.Pending,
                    CreatedAt = DateTime.UtcNow,
                    Items = request.Items.Select(item => new OrderItem
                    {
                        Id = item.Id,
                        ProductName = item.ProductName,
                        Price = item.Price,
                        Quantity = item.Quantity
                    }).ToList()
                };

                _orders.Add(order);
                stopwatch.Stop();

                _logger.LogInformation("Order {OrderId} created successfully for user {UserId} with total amount {TotalAmount:C} in {ElapsedMilliseconds}ms. Correlation ID: {CorrelationId}",
                    order.Id, order.UserId, order.TotalAmount, stopwatch.ElapsedMilliseconds, correlationId);

                // Log individual items for audit trail
                foreach (var item in order.Items)
                {
                    _logger.LogDebug("Order {OrderId} contains item: {ProductName} x{Quantity} at {Price:C} each. Correlation ID: {CorrelationId}",
                        order.Id, item.ProductName, item.Quantity, item.Price, correlationId);
                }

                return order;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order for user {UserId}. Correlation ID: {CorrelationId}",
                    request.UserId, correlationId);
                throw;
            }
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            _logger.LogDebug("Retrieving order with ID {OrderId}", id);

            try
            {
                await Task.Delay(50); // Simulate async operation
                
                var order = _orders.FirstOrDefault(o => o.Id == id);
                
                if (order == null)
                {
                    _logger.LogWarning("Order with ID {OrderId} not found", id);
                }
                else
                {
                    _logger.LogDebug("Order {OrderId} retrieved successfully. Status: {OrderStatus}, Amount: {TotalAmount:C}",
                        id, order.Status, order.TotalAmount);
                }

                return order;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving order with ID {OrderId}", id);
                throw;
            }
        }

        public async Task<PaginatedResponse<Order>> GetOrdersByUserIdAsync(int userId, PaginatedRequest request)
        {
            var correlationId = Guid.NewGuid().ToString();
            
            _logger.LogDebug("Querying orders for user {UserId} with parameters: Skip={Skip}, Take={Take}. Correlation ID: {CorrelationId}",
                userId, request.Skip, request.Take, correlationId);

            try
            {
                await Task.Delay(100); // Simulate async operation

                var userOrders = _orders.Where(o => o.UserId == userId).ToList();
                var totalCount = userOrders.Count;
                var orders = userOrders.Skip(request.Skip).Take(request.Take).ToList();

                _logger.LogInformation("Orders query completed for user {UserId}: {TotalCount} total orders, {ReturnedCount} returned. Correlation ID: {CorrelationId}",
                    userId, totalCount, orders.Count, correlationId);

                return new PaginatedResponse<Order>
                {
                    Items = orders,
                    TotalCount = totalCount,
                    Skip = request.Skip,
                    Take = request.Take
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error querying orders for user {UserId}. Correlation ID: {CorrelationId}",
                    userId, correlationId);
                throw;
            }
        }

        public async Task<bool> ProcessPaymentAsync(int orderId, PaymentRequest request)
        {
            var correlationId = Guid.NewGuid().ToString();
            var stopwatch = Stopwatch.StartNew();

            _logger.LogInformation("Processing payment for order {OrderId} using method {PaymentMethod}. Correlation ID: {CorrelationId}",
                orderId, request.PaymentMethod, correlationId);

            try
            {
                var order = _orders.FirstOrDefault(o => o.Id == orderId);
                if (order == null)
                {
                    _logger.LogWarning("Payment processing failed - Order {OrderId} not found. Correlation ID: {CorrelationId}",
                        orderId, correlationId);
                    return false;
                }

                if (order.Status != OrderStatus.Pending)
                {
                    _logger.LogWarning("Payment processing failed - Order {OrderId} is not in pending status. Current status: {OrderStatus}. Correlation ID: {CorrelationId}",
                        orderId, order.Status, correlationId);
                    return false;
                }

                // Simulate payment processing
                await Task.Delay(500);

                // Simulate random payment failure (10% chance)
                var random = new Random();
                if (random.Next(1, 11) == 1)
                {
                    _logger.LogWarning("Payment processing failed for order {OrderId} with amount {Amount:C} - Simulated payment gateway error. Correlation ID: {CorrelationId}",
                        orderId, order.TotalAmount, correlationId);
                    return false;
                }

                order.Status = OrderStatus.Paid;
                order.PaidAt = DateTime.UtcNow;
                stopwatch.Stop();

                _logger.LogInformation("Payment processed successfully for order {OrderId} with amount {Amount:C} in {ElapsedMilliseconds}ms. Correlation ID: {CorrelationId}",
                    orderId, order.TotalAmount, stopwatch.ElapsedMilliseconds, correlationId);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment for order {OrderId} with amount {Amount:C}. Correlation ID: {CorrelationId}",
                    orderId, request, correlationId);
                throw;
            }
        }

        public async Task<bool> ProcessShippingAsync(int orderId, ShippingRequest request)
        {
            var correlationId = Guid.NewGuid().ToString();
            var stopwatch = Stopwatch.StartNew();

            _logger.LogInformation("Processing shipping for order {OrderId} to address {ShippingAddress} via {ShippingMethod}. Correlation ID: {CorrelationId}",
                orderId, request.ShippingAddress, request.ShippingMethod, correlationId);

            try
            {
                var order = _orders.FirstOrDefault(o => o.Id == orderId);
                if (order == null)
                {
                    _logger.LogWarning("Shipping processing failed - Order {OrderId} not found. Correlation ID: {CorrelationId}",
                        orderId, correlationId);
                    return false;
                }

                if (order.Status != OrderStatus.Paid)
                {
                    _logger.LogWarning("Shipping processing failed - Order {OrderId} is not paid. Current status: {OrderStatus}. Correlation ID: {CorrelationId}",
                        orderId, order.Status, correlationId);
                    return false;
                }

                // Simulate shipping processing
                await Task.Delay(300);

                order.Status = OrderStatus.Shipped;
                order.ShippedAt = DateTime.UtcNow;
                stopwatch.Stop();

                _logger.LogInformation("Shipping processed successfully for order {OrderId} in {ElapsedMilliseconds}ms. Correlation ID: {CorrelationId}",
                    orderId, stopwatch.ElapsedMilliseconds, correlationId);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing shipping for order {OrderId}. Correlation ID: {CorrelationId}",
                    orderId, correlationId);
                throw;
            }
        }
    }
}