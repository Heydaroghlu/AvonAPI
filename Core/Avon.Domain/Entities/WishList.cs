using Avon.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Domain.Entities
{
    public class WishList:BaseEntity
    {
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public Product Product { get; set; }
        public int ProductId { get;set; }
    }
}
