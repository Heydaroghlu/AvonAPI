using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.DTOs.OrderDTOs
{
    public class OrderCreateDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string FatherName { get; set; }
        public string Apartment { get; set; }
        public string StreetAddres { get; set; }
        public string Address { get; set; }

        public string City { get; set; }
        public string Message { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }
        public string ZipCode { get; set; }
        public decimal TotalAmount { get; set; }
        public int DeliveryAdressId { get; set; }

    }
}
