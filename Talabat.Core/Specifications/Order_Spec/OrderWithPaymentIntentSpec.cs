﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.OrderAggregation;

namespace Talabat.Core.Specifications.Order_Spec
{
    public class OrderWithPaymentIntentSpec : BaseSpecifications<Order>
    {
        public OrderWithPaymentIntentSpec(string paymentIntentId) : base(O=>O.PaymentIntentId == paymentIntentId)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
        }
    }
}
