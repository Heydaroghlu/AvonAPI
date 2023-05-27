using Avon.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Domain.Entities
{
    public class Category:BaseEntity
    {
        public string Name { get; set; }
        public string? Icon { get; set; }
        public string? Image { get; set; }
        public string? ColorName { get; set; }
        public string? ColorCode { get; set; }
        //public double DisCount { get; set; }

        public List<SubCategory> SubCategories { get; set; }
	}
}
