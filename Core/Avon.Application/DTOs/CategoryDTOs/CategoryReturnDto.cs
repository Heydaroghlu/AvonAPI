using Avon.Application.DTOs.SubCategoryDTOs;
using Avon.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.DTOs.CategoryDTOs
{
    public class CategoryReturnDto
    {
        public int Id { get; set; } 

        public string Name { get; set; }
        public string Icon { get; set; }
        public string Image { get; set; }
        public string ColorName { get; set; }
        public string ColorCode { get; set; }
    }
}
