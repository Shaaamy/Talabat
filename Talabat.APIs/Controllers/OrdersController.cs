using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.OrderAggregation;
using Talabat.Core.Services;
using Talabat.Service;

namespace Talabat.APIs.Controllers
{
    public class OrdersController : APIBaseController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService ,IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }
        //Create Order
        [ProducesResponseType(typeof(Order) , StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse) , StatusCodes.Status400BadRequest)]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var MappedAddress = _mapper.Map<Address>(orderDto.ShippingAddress);            //there are an error here in address inside the map function
            var Order = await _orderService.CreateOrderAsync(Email, orderDto.BasketId,orderDto.DeliveryMethodId, MappedAddress);
            if (Order is null)
                return BadRequest(new ApiResponse(400,"There is a problem with your order"));
            return Ok(Order);
        }
    }
}
