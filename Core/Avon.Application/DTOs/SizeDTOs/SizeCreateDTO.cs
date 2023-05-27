using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.DTOs.SizeDTOs
{
    public class SizeCreateDTO
    {
        public string Name { get; set; }
        public string SizeName { get; set; }
        public decimal Discount { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }
        public string Weight { get; set; }
        public string Lenght { get; set; }
    }
}
