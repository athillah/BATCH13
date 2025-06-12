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
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly IOrderService _orderService;

        public OrdersController(ILogger<OrdersController> logger, IOrderService orderService)
        {
            _logger = logger;
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<Order>>> CreateOrder([FromBody] CreateOrderRequest request)
        {
            var correlationId = Guid.NewGuid().ToString();
            var stopwatch = Stopwatch.StartNew();

            _logger.LogInformation("Create order request received for user {UserId} with {ItemCount} items. Correlation ID: {CorrelationId}",
                request.UserId, request.Items.Count, correlationId);

            try
            {
                var order = await _orderService.CreateOrderAsync(request);
                stopwatch.Stop();

                _logger.LogInformation("Order creation completed in {ElapsedMilliseconds}ms for user {UserId}. Order ID: {OrderId}, Correlation ID: {CorrelationId}",
                    stopwatch.ElapsedMilliseconds, request.UserId, order.Id, correlationId);

                var response = new ApiResponse<Order>
                {
                    Success = true,
                    Message = "Order created successfully",
                    Data = order,
                    CorrelationId = correlationId
                };

                return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, response);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Create order request failed for user {UserId} - {ErrorMessage}. Correlation ID: {CorrelationId}",
                    request.UserId, ex.Message, correlationId);

                return BadRequest(new ApiResponse<Order>
                {
                    Success = false,
                    Message = ex.Message,
                    CorrelationId = correlationId
                });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Create order request failed for user {UserId} - {ErrorMessage}. Correlation ID: {CorrelationId}",
                    request.UserId, ex.Message, correlationId);

                return BadRequest(new ApiResponse<Order>
                {
                    Success = false,
                    Message = ex.Message,
                    CorrelationId = correlationId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing create order request for user {UserId}. Correlation ID: {CorrelationId}",
                    request.UserId, correlationId);

                return StatusCode(500, new ApiResponse<Order>
                {
                    Success = false,
                    Message = "An error occurred while creating the order",
                    CorrelationId = correlationId
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Order>>> GetOrder(int id)
        {
            var correlationId = Guid.NewGuid().ToString();

            _logger.LogDebug("Get order request received for ID {OrderId}. Correlation ID: {CorrelationId}",
                id, correlationId);

            try
            {
                var order = await _orderService.GetOrderByIdAsync(id);

                if (order == null)
                {
                    _logger.LogWarning("Get order request failed - Order {OrderId} not found. Correlation ID: {CorrelationId}",
                        id, correlationId);

                    return NotFound(new ApiResponse<Order>
                    {
                        Success = false,
                        Message = "Order not found",
                        CorrelationId = correlationId
                    });
                }

                _logger.LogDebug("Get order request completed successfully for ID {OrderId}. Correlation ID: {CorrelationId}",
                    id, correlationId);

                return Ok(new ApiResponse<Order>
                {
                    Success = true,
                    Message = "Order retrieved successfully",
                    Data = order,
                    CorrelationId = correlationId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing get order request for ID {OrderId}. Correlation ID: {CorrelationId}",
                    id, correlationId);

                return StatusCode(500, new ApiResponse<Order>
                {
                    Success = false,
                    Message = "An error occurred while retrieving the order",
                    CorrelationId = correlationId
                });
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<ApiResponse<PaginatedResponse<Order>>>> GetOrdersByUser(int userId, [FromQuery] PaginatedRequest request)
        {
            var correlationId = Guid.NewGuid().ToString();

            _logger.LogDebug("Get orders by user request received for user {UserId}. Correlation ID: {CorrelationId}",
                userId, correlationId);

            try
            {
                var result = await _orderService.GetOrdersByUserIdAsync(userId, request);

                _logger.LogDebug("Get orders by user request completed successfully for user {UserId}. Returned {Count} orders. Correlation ID: {CorrelationId}",
                    userId, result.Items.Count, correlationId);

                return Ok(new ApiResponse<PaginatedResponse<Order>>
                {
                    Success = true,
                    Message = "Orders retrieved successfully",
                    Data = result,
                    CorrelationId = correlationId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing get orders by user request for user {UserId}. Correlation ID: {CorrelationId}",
                    userId, correlationId);

                return StatusCode(500, new ApiResponse<PaginatedResponse<Order>>
                {
                    Success = false,
                    Message = "An error occurred while retrieving orders",
                    CorrelationId = correlationId
                });
            }
        }

        [HttpPost("{id}/pay")]
        public async Task<ActionResult<ApiResponse<bool>>> ProcessPayment(int id, [FromBody] PaymentRequest request)
        {
            var correlationId = Guid.NewGuid().ToString();
            var stopwatch = Stopwatch.StartNew();

            _logger.LogInformation("Process payment request received for order {OrderId} using method {PaymentMethod}. Correlation ID: {CorrelationId}",
                id, request.PaymentMethod, correlationId);

            try
            {
                var result = await _orderService.ProcessPaymentAsync(id, request);
                stopwatch.Stop();

                if (result)
                {
                    _logger.LogInformation("Payment processing completed successfully for order {OrderId} in {ElapsedMilliseconds}ms. Correlation ID: {CorrelationId}",
                        id, stopwatch.ElapsedMilliseconds, correlationId);

                    return Ok(new ApiResponse<bool>
                    {
                        Success = true,
                        Message = "Payment processed successfully",
                        Data = result,
                        CorrelationId = correlationId
                    });
                }
                else
                {
                    _logger.LogWarning("Payment processing failed for order {OrderId} in {ElapsedMilliseconds}ms. Correlation ID: {CorrelationId}",
                        id, stopwatch.ElapsedMilliseconds, correlationId);

                    return BadRequest(new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Payment processing failed",
                        Data = result,
                        CorrelationId = correlationId
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment for order {OrderId}. Correlation ID: {CorrelationId}",
                    id, correlationId);

                return StatusCode(500, new ApiResponse<bool>
                {
                    Success = false,
                    Message = "An error occurred while processing payment",
                    CorrelationId = correlationId
                });
            }
        }

        [HttpPost("{id}/ship")]
        public async Task<ActionResult<ApiResponse<bool>>> ProcessShipping(int id, [FromBody] ShippingRequest request)
        {
            var correlationId = Guid.NewGuid().ToString();
            var stopwatch = Stopwatch.StartNew();

            _logger.LogInformation("Process shipping request received for order {OrderId} to {ShippingAddress}. Correlation ID: {CorrelationId}",
                id, request.ShippingAddress, correlationId);

            try
            {
                var result = await _orderService.ProcessShippingAsync(id, request);
                stopwatch.Stop();

                if (result)
                {
                    _logger.LogInformation("Shipping processing completed successfully for order {OrderId} in {ElapsedMilliseconds}ms. Correlation ID: {CorrelationId}",
                        id, stopwatch.ElapsedMilliseconds, correlationId);

                    return Ok(new ApiResponse<bool>
                    {
                        Success = true,
                        Message = "Shipping processed successfully",
                        Data = result,
                        CorrelationId = correlationId
                    });
                }
                else
                {
                    _logger.LogWarning("Shipping processing failed for order {OrderId} in {ElapsedMilliseconds}ms. Correlation ID: {CorrelationId}",
                        id, stopwatch.ElapsedMilliseconds, correlationId);

                    return BadRequest(new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Shipping processing failed",
                        Data = result,
                        CorrelationId = correlationId
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing shipping for order {OrderId}. Correlation ID: {CorrelationId}",
                    id, correlationId);

                return StatusCode(500, new ApiResponse<bool>
                {
                    Success = false,
                    Message = "An error occurred while processing shipping",
                    CorrelationId = correlationId
                });
            }
        }
    }
}