using Avon.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Domain.Entities
{
    public class Comment:BaseEntity
    {
        public string Name { get; set; }
        public string Message { get; set; }
        public bool IsAccept { get; set; }
        public int Star { get; set; }

        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
