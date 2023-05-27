using Avon.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Domain.Entities
{
    public class SubCategory:BaseEntity
    {
        public string Name { get; set; }
        public string? Icon { get; set; }
        public string? Image { get; set; }
        public string? ColorName { get; set; }
        public string? ColorCode { get; set; }


        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public List<ProductSubCategory> ProductSubCategories { get; set; }
    }
}
