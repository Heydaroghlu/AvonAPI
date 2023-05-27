using Avon.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.DTOs.OrderDTOs
{
    public class TrackOrderDto
    {
        public string Code { get; set; }

        public Order Order { get; set; }
    }
}
