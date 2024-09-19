using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Service.Services.CustomerBasketService;
using Store.Service.Services.CustomerBasketService.Dtos;
using System.Threading.Tasks;

namespace STORE.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpGet("GetBasketById")]
        public async Task<ActionResult<CustomerBasketDto>> GetBasketById(string id)
            => Ok( await _basketService.GetBasketAsync(id));

        [HttpPost("UpdaateBasket")]
        public async Task<ActionResult<CustomerBasketDto>> UpdaateBasketAsync(CustomerBasketDto BasketDto)
            => Ok(await _basketService.UpdateBasketAsync(BasketDto));

        [HttpDelete("DeleteBasket")]
        public async Task< ActionResult<CustomerBasketDto>> DeleteBasket(string id)
            => Ok(await _basketService.DeleteBasketAsync(id));


    }
}
