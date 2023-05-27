using Avon.Domain.Entities.Common;
using Avon.Domain.Enums.forOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Domain.Entities
{
    public class Order:BaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set;}
        public string FatherName { get; set; }
        public string StreetAddres { get; set; }
        public string Apartment { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public int CodeNumber { get; set; }
        public string CodePrefix { get; set; }
        public string Email { get; set; }
        public string Message { get; set; } 
        public decimal TotalAmount { get; set; }
        public string AppUserId { get; set; }
        public orderStatus Status { get; set; }
        public deliveryStatus DeliveryStatus { get; set; }
        public int DeliveryAdressId { get; set; }
        public DeliveryAdress DeliveryAdress { get; set; }
        public AppUser AppUser { get; set; }

        public List<OrderItem> OrderItems { get; set; }

    }
}
