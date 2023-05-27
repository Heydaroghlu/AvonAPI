using Avon.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Domain.Entities
{
    public class DeliveryAdress:BaseEntity
    {
        public string Name { get; set; }


        public List<Order> Orders { get; set; }
    }
}
