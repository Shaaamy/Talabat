using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.Core;
using Talabat.Core.Entities.OrderAggregation;
using Talabat.Core.Services;
using Talabat.Service;

namespace Talabat.APIs.Controllers
{
    public class OrdersController : APIBaseController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public OrdersController(IOrderService orderService, IMapper mapper ,IUnitOfWork unitOfWork)
        {
            _orderService = orderService;
            _mapper = mapper;
            this._unitOfWork = unitOfWork;
        }
        //Create Order
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var MappedAddress = _mapper.Map<Address>(orderDto.ShippingAddress);            //there are an error here in address inside the map function
            var Order = await _orderService.CreateOrderAsync(Email, orderDto.BasketId, orderDto.DeliveryMethodId, MappedAddress);
            if (Order is null)
                return BadRequest(new ApiResponse(400, "There is a problem with your order"));
            return Ok(Order);
        }
        [ProducesResponseType(typeof(IReadOnlyList<Order>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IReadOnlyList<Order>>> GetOrderForUserAsync()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            if (Email is null) return Unauthorized(new ApiResponse(401));
            var Orders = await _orderService.GetOrderForSpecificUserAsync(Email);
            if (Orders is null) return NotFound(new ApiResponse(404, "No Orders Were Found For This User"));
            return Ok(Orders);
        }
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{OrderId}")]
        [Authorize]
        public async Task<ActionResult<Order>> GetOrderByIdForUserAsync(int OrderId)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            if (Email is null) return Unauthorized(new ApiResponse(401));
            var Order = await _orderService.GetOrderByIdForSpecificUserAsync(Email, OrderId);
            if (Order is null) return NotFound(new ApiResponse(404, "No Order Was Found"));
            return Ok(Order);
        }
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethod()
        {
            var DeliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
            if (DeliveryMethod is null) return NotFound(new ApiResponse(404, "No Delivery Methods Was Found"));
            return Ok(DeliveryMethod);
        }
    }
}
