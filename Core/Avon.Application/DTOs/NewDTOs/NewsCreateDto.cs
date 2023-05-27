using Avon.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.DTOs.NewDTOs
{
    public class NewsCreateDto
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public int NewsCategoryId { get; set; }
        public IFormFile PosterFile { get; set; }

        public List<int> TagIds { get; set; }
    }
}
