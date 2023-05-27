using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.DTOs.PromotionDTOs
{
    public class PromotionCreateDto
    {
        public string Key { get; set; }
        public List<IFormFile> FormFiles { get; set; }
    }
}
