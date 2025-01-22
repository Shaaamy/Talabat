using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.OrderAggregation;

namespace Talabat.Core.Specifications.OrderSpecifications
{
    public class OrderSpecifications : BaseSpecifications<Order>
    {
        public OrderSpecifications(string email) : base(O=>O.BuyerEmail == email)
        {
            Includes.Add(O=>O.DeliveryMethod);
            Includes.Add(O=>O.Items);
            AddOrderByDesc(O => O.OrderDate);
        }
        public OrderSpecifications(string email , int orderId) : base(O => O.BuyerEmail == email && O.Id==orderId)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
        }
        public OrderSpecifications(string paymentIntentId) : base(O=>O.PaymentIntentId == paymentIntentId)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
        }
    }
}
