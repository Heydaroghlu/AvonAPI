using Avon.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.DTOs.OrderDTOs
{
    public class CheckOutDto
    {
        public List<CheckOutItemDto> CheckOutItemDtos { get; set; }
        public Order Order { get; set; }
    }
}
