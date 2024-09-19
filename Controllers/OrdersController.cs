using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Store.Data.Entities;
using Store.Service.Services.OrderService;
using Store.Service.Services.OrderService.DTOS;
using System.Security.Claims;

namespace STORE.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderservice _orderservice;

        public OrdersController(IOrderservice orderservice)
        {
            _orderservice = orderservice;
        }

        [HttpPost("CreateOrder")]
        public async Task<ActionResult<OrderResultDto>> CreateOrderAsync(OrderDto input)
        {
            var order  = await _orderservice.CreateOrderAsync(input);
            if (order == null)
            {
                return BadRequest();
            }
            return Ok(order);
        }

        [HttpGet]
        [Route("user-orders")]

        public async Task<ActionResult<IReadOnlyList<OrderResultDto>>> GetAllUsersForUserAsync()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var orders = await _orderservice.GetAllOrdersForUserAsync(email);
            return Ok(orders);
        }
        [HttpGet("{id}")]

        public async Task<ActionResult<OrderResultDto>> GetOrderByIdAsync(Guid id)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var order = await _orderservice.GetOrderByIdAsync(id,email);
            return Ok(order);
        }
        [HttpGet("delivery-methods")]
        public async Task<ActionResult<IReadOnlyList<DileveryMethod>>> GetAllDeliveryMethodsAsync()
        { 
         return Ok(await _orderservice.GetAllDeliveryMethodsAsync());
        }


    }
}
