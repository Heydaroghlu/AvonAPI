using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.DTOs.SliderDTOs
{
    public class SliderReturnDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }

        public string Title1 { get; set; }
        public string Title2 { get; set; }
        public string Image { get; set; }
    }
}
