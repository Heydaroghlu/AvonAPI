using Avon.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.DTOs.CategoryDTOs
{
    public class CategoryCreateDto
    {
        public string Name { get; set; }
        public string? Icon { get; set; }
        public string? ColorName { get; set; }
        public string? ColorCode { get; set; }

        public IFormFile? ImageFile { get; set; }
        //public double DisCount { get; set; }
    }
}
