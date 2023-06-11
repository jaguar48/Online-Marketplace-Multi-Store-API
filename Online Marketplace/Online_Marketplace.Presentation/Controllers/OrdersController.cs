using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Online_Marketplace.BLL.Interface.IMarketServices;
using Online_Marketplace.DAL.Enums;
using Online_Marketplace.Logger.Logger;
using Online_Marketplace.Shared.DTOs;
using Swashbuckle.AspNetCore.Annotations;
using System.Text;

namespace Online_Marketplace.Presentation.Controllers
{
    [ApiController]
    [Route("marketplace/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILoggerManager _logger;

        public OrdersController(IOrderService orderService, ILoggerManager logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        [Authorize(Roles = "Buyer")]
        [HttpGet("buyer-order")]
        [SwaggerOperation(Summary = "Get order history for the current buyer", Description = "Requires buyer authorization.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Order history retrieved successfully.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<IActionResult> BuyerOrderHistory()
        {
            var orders = await _orderService.GetBuyerOrderHistoryAsync();
            return Ok(orders);


        }

        [Authorize(Roles = "Seller")]
        [HttpGet("seller-orders")]
        [SwaggerOperation(Summary = "Get order history for the current seller", Description = "Requires seller authorization.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Order history retrieved successfully.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<IActionResult> SellerOrderHistory()
        {

            var orders = await _orderService.GetSellerOrderHistoryAsync();
            return Ok(orders);

        }

        [HttpGet("{id}/status")]
        [SwaggerOperation(Summary = "Get the status of a specific order", Description = "No authorization required.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Order status retrieved successfully.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<IActionResult> GetOrderStatus(int id)
        {
            
            var orderStatuses = await _orderService.GetOrderStatusAsync(id);
            return Ok(orderStatuses);
            
        }

        [Authorize(Roles = "Seller")]
        [HttpGet("order/{orderid}")]
        [SwaggerOperation(Summary = "Get a specific order", Description = "No authorization required.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Order  retrieved successfully.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<IActionResult> GetOrder(int orderid)
        {

            var orderStatuses = await _orderService.GetOrderByIdAsync(orderid);
            return Ok(orderStatuses);

        }


        [Authorize(Roles = "Buyer")]
        [HttpPost("checkout")]
        [SwaggerOperation(Summary = "Checkout a cart.", Description = "Requires buyer authorization.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Cart checked out successfully.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Failed to checkout cart.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<IActionResult> Checkout(int cartId, ShippingMethod shippingMethod)
        {
            var authorizationUrl = await _orderService.CheckoutAsync(cartId, shippingMethod);
            return Redirect(authorizationUrl);

        }

    



        [Authorize(Roles = "Seller")]
        [HttpGet("{orderId}/receipt")]
        [SwaggerOperation(Summary = "Generate a receipt for an order.", Description = "Requires seller authorization.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns the receipt as a PDF file.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "An error occurred while generating the receipt.")]
        public async Task<FileResult> GenerateReceiptAsync(int orderId)
        {
            var receipt = await _orderService.GenerateReceiptAsync(orderId);

           
            return File(receipt, "application/pdf", $"receipt_{orderId}.pdf");
            
        }


        [Authorize(Roles = "Seller")]
        [HttpPost("updateOrderStatus")]
        [SwaggerOperation(Summary = "Update the status of an order.", Description = "Requires seller authorization.")]
        [SwaggerResponse(StatusCodes.Status200OK, "The order status was updated successfully.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The request was invalid or the order status could not be updated.")]
        public async Task<IActionResult> UpdateOrderStatus(int OrderId, string Status)
        {
            await _orderService.UpdateOrderStatusAsync(OrderId, Status);
            return Ok();
        }


    }
}