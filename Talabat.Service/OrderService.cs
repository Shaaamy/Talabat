using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.OrderAggregation;
using Talabat.Core.Repositories;
using Talabat.Core.Services;

namespace Talabat.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<DeliveryMethod> _deliveryRepository;
        private readonly IGenericRepository<Order> _orderRepository;

        public OrderService(IBasketRepository basketRepository
            , IGenericRepository<Product> productRepository
            ,IGenericRepository<DeliveryMethod> deliveryRepository
            ,IGenericRepository<Order> orderRepository)
        {
            _basketRepository = basketRepository;
            _productRepository = productRepository;
            _deliveryRepository = deliveryRepository;
            _orderRepository = orderRepository;
        }
        public async Task<Order> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
        {
            //1.Get Basket From Basket Repo
            var Basket = await _basketRepository.GetBasketAsync(basketId);
            //2.Get Selected Items at Basket From Product Repo
            var OrderItems = new List<OrderItem>();
            if(Basket?.Items.Count > 0)
            {
                foreach(var item in Basket.Items)
                {
                    var Product = await _productRepository.GetByIdAsync(item.Id);
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

            var DeliveryMethod = await _deliveryRepository.GetByIdAsync(deliveryMethodId);

            //5.Create Order

            var Order = new Order(buyerEmail, shippingAddress, DeliveryMethod, OrderItems, SubTotal);
            //6.Add Order Locally
            await _orderRepository.AddAsync(Order);
            //7.Save Order To Database[ToDo]
            return Order;

        }

        public Task<Order> GetOrderByIdForSpecificUserAsync(string buyerEmail, int orderid)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Order>> GetOrderForSpecificUserAsync(string buyerEmail)
        {
            throw new NotImplementedException();
        }
    }
}
