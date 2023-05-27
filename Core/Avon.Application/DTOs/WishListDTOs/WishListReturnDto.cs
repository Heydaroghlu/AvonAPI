using Avon.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.DTOs.WishListDTOs
{
    public class WishListReturnDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }

        public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }
        
        public int ProductId { get; set; }
        public Product Product { get; set; }

    }
}
