using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace services.Controllers
{
    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;
        public BasketController(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasketById(string Id)
        {
            var basket = await _basketRepository.GetBasketAsync(Id);
            return Ok(basket ?? new CustomerBasket(Id));
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdateBasket(CustomerBasket basket) {
            return await _basketRepository.CreateOrUpdateBasketAsync(basket);        
        }

        [HttpDelete]
        public async Task DeleteBasket(string Id) {
            await _basketRepository.DeleteBasket(Id);
        }
    }
}