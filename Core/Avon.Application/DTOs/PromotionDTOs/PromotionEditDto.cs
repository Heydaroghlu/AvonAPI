using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.DTOs.PromotionDTOs
{
    public class PromotionEditDto
    {
        public int Id { get; set; }
        public IFormFile? FormFile { get; set; }
        public string Key { get; set; }
    }
}
