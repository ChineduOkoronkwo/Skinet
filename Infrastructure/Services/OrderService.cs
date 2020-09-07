using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketRepository _basketRepo;

        public OrderService(IUnitOfWork unitOfWork, IBasketRepository basketRepo)
        {
            _unitOfWork = unitOfWork;
            _basketRepo = basketRepo;
        }

        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, 
            Address shippingAddress)
        {
            // get basket from the repo
            var basket = await _basketRepo.GetBasketAsync(basketId);

            // create ordered items by getting items in basket from product repo
            var items = new List<OrderItem>();
            foreach(var item in basket.Items) 
            {
                var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                var itemOrdered = new ProductItemOrdered(product.Id, product.Name, product.PictureUrl);
                var orderedItem = new OrderItem(itemOrdered, product.Price, item.Quantity);
                items.Add(orderedItem);
            }

            // get delivery method from repo
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>()
                .GetByIdAsync(deliveryMethodId);

            // calculate subtotal
            var subtotal = items.Sum(items => items.Price * items.Quantity);

            // create order
            var order = new Order (buyerEmail, shippingAddress, deliveryMethod, items, subtotal);
            _unitOfWork.Repository<Order>().Add(order);

            //save to db
            var result = await _unitOfWork.Complete();

            if (result <= 0) 
            {
                return null;
            }

            // delete basket
            await _basketRepo.DeleteBasket(basketId);

            // return order
            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            return await _unitOfWork.Repository<DeliveryMethod>().ListAllAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var spec = new OrdersWithItemsAndOrderingSpecification(id, buyerEmail);
            return await _unitOfWork.Repository<Order>().GetEntitiesWithSpecAsync(spec);
        }

        public async Task<IReadOnlyList<Order>> GetUserOrderAsync(string buyerEmail)
        {
            var spec = new OrdersWithItemsAndOrderingSpecification(buyerEmail);
            return await _unitOfWork.Repository<Order>().ListAsync(spec);
        }
    }
}