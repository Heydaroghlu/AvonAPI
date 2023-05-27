using Avon.Domain.Entities;
using Avon.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Domain.Entitity 
{ 
    public class BasketItem : BaseEntity
    {
        public AppUser AppUser { get; set; }
        public string AppUserId { get;set; }
        public int ProductId { get; set; }
        public Product Product  { get; set; }

        public int ProductCount { get; set; } = 1;
    }
}
