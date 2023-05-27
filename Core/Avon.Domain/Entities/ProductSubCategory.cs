using Avon.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Domain.Entities
{
    public class ProductSubCategory : BaseEntity
    {
		public int ProductId { get; set; }
        public Product? Product { get; set; }
        public SubCategory? SubCategory { get; set; }    
        public int SubCategoryId { get; set; }
    }
}
