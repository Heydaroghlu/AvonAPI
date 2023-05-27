using Avon.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Domain.Entities
{
    public class ProductTeg : BaseEntity
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }


        public int TegId { get; set; }
        public Teg Teg { get; set; }
    }
}
