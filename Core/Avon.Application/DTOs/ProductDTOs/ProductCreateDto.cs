using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.DTOs.ProductDTOs
{
    public class ProductCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal SalePrice { get; set; }
        public decimal CostPrice { get; set; }
        public decimal DiscountPrice { get; set; }

        public bool inStock { get; set; }
        public int StockCount { get; set; }
        public bool PreOrder { get; set; }
        public bool IsNew { get; set; }
        public int AttributeId { get; set; }
        public int BrandId { get; set; }
        public int TegId { get; set; }

        public IFormFile PosterFile { get; set; }
        public List<IFormFile>? ImageFiles { get; set; }
        public List<int> ProductSubCategoryIds { get; set; }
    }
}
