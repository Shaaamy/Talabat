using Microsoft.Extensions.Configuration;
using Stripe;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.OrderAggregation;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Product = Talabat.Core.Entities.Product;

namespace Talabat.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IConfiguration configuration , IBasketRepository basketRepository , IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string BasketId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeKeys:Secretkey"];
            var Basket = await _basketRepository.GetBasketAsync(BasketId);
            if (Basket is null) return null;
            var ShippingPrice = 0M;
            if (Basket.DeliveryMethodId.HasValue)
            {
                var DeliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(Basket.DeliveryMethodId.Value);
                ShippingPrice = DeliveryMethod.Cost;
            }
            // Total = SubTotal + DM.Cost
            if(Basket.Items.Count > 0)
            {
                foreach(var item in Basket.Items)
                {
                    var Product =await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    if (item.Price != Product.Price)
                        item.Price = Product.Price;
                }
            }
            var SubTotal = Basket.Items.Sum(item => item.Price * item.Quantity);

            // Create Payment Intent
            var Service = new PaymentIntentService();
            PaymentIntent paymentIntent;
            if(string.IsNullOrEmpty(Basket.PaymentIntentId)) //Create if null
            {
                var Options = new PaymentIntentCreateOptions()
                {
                    Amount=(long) ((SubTotal + ShippingPrice)*100),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>()
                    {
                        "card"
                    }
                };
                paymentIntent = await Service.CreateAsync(Options);
                Basket.PaymentIntentId = paymentIntent.Id;
                Basket.ClientSecret = paymentIntent.ClientSecret; 
            }
            else //Update if not null
            {
                var Options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)((SubTotal + ShippingPrice) * 100)
                };
                paymentIntent = await Service.UpdateAsync(Basket.PaymentIntentId, Options);
                Basket.PaymentIntentId = paymentIntent.Id;
                Basket.ClientSecret = paymentIntent.ClientSecret;
            }
            await _basketRepository.UpdateBasketAsync(Basket);
            return Basket;

        }
    }
}
