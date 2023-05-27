using Avon.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Domain.Entities
{
    public class Size:BaseEntity
    {
        public string Name { get; set; }
        public string SizeName { get; set; }
        [DataType("decimal(18,2)")]
        public decimal Discount { get; set; }
        public string Width { get; set; }
        public string Weight { get; set; }
        public string Lenght { get; set; }
        public string Height { get; set; }

    }
}
