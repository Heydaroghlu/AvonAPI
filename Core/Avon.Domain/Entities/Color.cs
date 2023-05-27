using Avon.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Domain.Entities
{
    public class Color:BaseEntity
    {
        public string Name { get; set; }
        public string ColorName  { get;set; }
        public decimal Discount { get;set; }


        public List<CategoryColor> CategoryColors { get; set; }
    }
}
