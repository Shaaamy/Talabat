using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.OrderAggregation
{
    public class ProductItemOrdered
    {
        public int ProductId { get; set; }
        public string Productname { get; set; }
        public string ProductUrl { get; set; }
    }
}
