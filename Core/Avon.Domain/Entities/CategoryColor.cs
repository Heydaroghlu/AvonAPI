using Avon.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Domain.Entities
{
    public class CategoryColor : BaseEntity
    {
        public Color Color { get; set; }
        public int ColorId { get; set; }

        public Category Category { get; set; }
        public int CategoryId { get; set; }
    }
}
