using Avon.Domain.Entities.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Domain.Entities
{
    public class Product:BaseEntity
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
		public string PosterImage { get; set; }
		public int AttributeId { get; set; }
        public int BrandId { get; set; }
        public Brand Brand { get; set; }


        public List<Comment> Comments { get; set; }
        public List<ProductSubCategory> ProductSubCategories { get; set; }
        public List<ProductImages> ProductImages { get; set; }
        public List<ProductTeg> ProductTegs { get; set; }


        [NotMapped]
        public List<IFormFile> ImageFiles { get; set; }= new List<IFormFile>();
        [NotMapped]
        public List<int> ProductImageIds { get; set; } = new List<int>();

        [NotMapped]
        public List<int> ProductSubCategoryIds { get; set; } = new List<int>();


    }
}
