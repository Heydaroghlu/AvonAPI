using Avon.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.DTOs.SubCategoryDTOs
{
    public class SubCategoryReturnDto
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string? Icon { get; set; }
        public string? Image { get; set; }
        public string? ColorName { get; set; }
        public string? ColorCode { get; set; }


        public int CategoryId { get; set; }
        public List<ProductSubCategory> ProductSubCategories { get; set; }

    }
}
