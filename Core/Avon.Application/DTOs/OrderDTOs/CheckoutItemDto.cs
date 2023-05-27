using Avon.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.DTOs.OrderDTOs
{
    public class CheckOutItemDto
    {
        public Product Product { get; set; }
        public int Count { get; set; }
    }
}
