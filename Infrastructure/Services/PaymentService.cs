using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace Infrastructure.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        public PaymentService(IBasketRepository basketRepository, IUnitOfWork unitOfWork,        
            IConfiguration config)
        {
            _config = config;
            _unitOfWork = unitOfWork;
            _basketRepository = basketRepository;
        }

        public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = _config["StripeSettings:SecretKey"];

            var basket = await _basketRepository.GetBasketAsync(basketId);
            if (basket == null) return null;

            long amount = await UpdateBasketItemPricesAndGetToal(basket);

            var intent = string.IsNullOrEmpty(basket.PaymentIntentId)
                ? await CreateNewPaymentIntent(amount, _config["StripeSettings:Currency"])
                : await UpdatePaymentIntent(amount, basket.PaymentIntentId);

            basket.PaymentIntentId = intent.Id;
            basket.ClientSecret = intent.ClientSecret;

            basket = await _basketRepository.CreateOrUpdateBasketAsync(basket);
            return basket;
        }
        
        private async Task<PaymentIntent> CreateNewPaymentIntent(long amount, string currency)
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = amount,
                Currency = currency,
                PaymentMethodTypes = GetPaymentMethodTypes()
            };

            var service = new PaymentIntentService();
            return await service.CreateAsync(options);
        }

        private async Task<PaymentIntent> UpdatePaymentIntent(long amount, string paymentIntentId)
        {
            var options = new PaymentIntentUpdateOptions
            {
                Amount = amount,
                PaymentMethodTypes = GetPaymentMethodTypes()
            };

            var service = new PaymentIntentService();
            return await service.UpdateAsync(paymentIntentId, options);
        }

        private List<string> GetPaymentMethodTypes()
        {
            return new List<string>{"card"};
        }
        private async Task<long> UpdateBasketItemPricesAndGetToal(CustomerBasket basket)
        {
            long amount = 0;
            if (basket.DeliveryMethodId.HasValue) 
            {
                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>()
                    .GetByIdAsync(basket.DeliveryMethodId.Value);
                    amount += (long) (deliveryMethod.Price * 100);
            }

            foreach (var item in basket.Items) 
            {
                var product = await _unitOfWork.Repository<Core.Entities.Product>()
                    .GetByIdAsync(item.Id);
                item.Price = product.Price;
                amount += (long) (item.Quantity * (item.Price * 100));
            }

            return amount;
        }

        public async Task<Core.Entities.OrderAggregate.Order> UpdateOrderStatus(string paymentIntentId, OrderStatus orderStatus)
        {
            var spec = new OrderWithPaymentIntentIdSpecification(paymentIntentId);
            var order = await _unitOfWork.Repository<Core.Entities.OrderAggregate.Order>().GetEntitiesWithSpecAsync(spec);
            if (order == null) return null;

            order.Status = orderStatus;
            _unitOfWork.Repository<Core.Entities.OrderAggregate.Order>().Update(order);
            await _unitOfWork.Complete();

            return order;
        }
    }
}