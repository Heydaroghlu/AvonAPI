using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.DTOs.SliderDTOs
{
    public class SliderCreateDto
    {
        public string Title1 { get; set; }
        public string Title2 { get; set; }
        public IFormFile FormFile{ get; set; }
    }
}
