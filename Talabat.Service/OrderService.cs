using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.OrderAggregation;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Core.Specifications.Order_Spec;
using Talabat.Core.Specifications.OrderSpecifications;

namespace Talabat.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        public OrderService(IBasketRepository basketRepository
            ,IUnitOfWork unitOfWork
            ,IPaymentService paymentService)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
        }
        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
        {
            //1.Get Basket From Basket Repo
            var Basket = await _basketRepository.GetBasketAsync(basketId);
            //2.Get Selected Items at Basket From Product Repo
            var OrderItems = new List<OrderItem>();
            if(Basket?.Items.Count > 0)
            {
                foreach(var item in Basket.Items)
                {
                    var Product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    var ProductItemOrdered = new ProductItemOrdered(Product.Id, Product.Name, Product.PictureUrl);
                    var OrderItem = new OrderItem(ProductItemOrdered, item.Quantity , Product.Price);
                    OrderItems.Add(OrderItem);

                }
            }
            //3.Calculate SubTotal //Price Of Product * Quantity



            //decimal SubTotal = 0;
            //foreach(var item in OrderItems)
            //{
            //    SubTotal += (item.Price * item.Quantity);
            //}


            var SubTotal = OrderItems.Sum(item => item.Price * item.Quantity);


            //4.Get Delivery Method From DeliveryMethod Repo

            var DeliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);
            var Spec = new OrderWithPaymentIntentSpec(Basket.PaymentIntentId);
            var ExOrder = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(Spec);
            if(ExOrder is not null)
            {
                _unitOfWork.Repository<Order>().Delete(ExOrder);
                await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            }
            //5.Create Order

            var Order = new Order(buyerEmail, shippingAddress, DeliveryMethod, OrderItems, SubTotal , Basket.PaymentIntentId);
            //6.Add Order Locally
            await _unitOfWork.Repository<Order>().AddAsync(Order);
            //7.Save Order To Database
            var Result = await _unitOfWork.CompleteAsync();
            if (Result <= 0)
                return null;
            return Order;

        }

        public async Task<Order> GetOrderByIdForSpecificUserAsync(string buyerEmail, int orderId)
        {
            var Spec = new OrderSpecifications(buyerEmail, orderId);
            var SpecificOrder = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(Spec);
            return SpecificOrder;
        }

        public async Task<IReadOnlyList<Order>> GetOrderForSpecificUserAsync(string buyerEmail)
        {
            var Spec = new OrderSpecifications(buyerEmail);
            var Orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(Spec);
            return Orders;
        }
    }
}
