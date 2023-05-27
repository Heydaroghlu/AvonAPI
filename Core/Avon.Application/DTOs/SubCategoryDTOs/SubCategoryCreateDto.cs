using Avon.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.DTOs.SubCategoryDTOs
{
    public class SubCategoryCreateDto
    {
        public string Name { get; set; }
        public string? Icon { get; set; }
        public string? ColorName { get; set; }
        public string? ColorCode { get; set; }


        public int CategoryId { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
