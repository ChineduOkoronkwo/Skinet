using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IBasketRepository
    {
         Task<CustomerBasket> GetBasketAsync(string basketId);
         Task<CustomerBasket> CreateOrUpdateBasketAsync(CustomerBasket basket);
         Task<bool> DeleteBasket(string basketId);
    }
}