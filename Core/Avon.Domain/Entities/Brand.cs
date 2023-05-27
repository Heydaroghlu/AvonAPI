using Avon.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Domain.Entities
{
    public class Brand:BaseEntity
    {
        public string? Image { get; set; }
        public string Name { get; set; }    

        public List<Product> Products { get; set; }
    }
}
