using Avon.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Domain.Entities
{
    public class OrderItem:BaseEntity
    {
        public int OrderID { get; set; }
        public Order Order { get; set; }    
        public int ProductID { get; set; }
        public Product Product { get; set; }
        public decimal CostPrice { get; set; }
        public decimal DiscountPrice { get; set; }
        public int Count { get; set; }
        public decimal SalePrice { get; set; }
    }
}
