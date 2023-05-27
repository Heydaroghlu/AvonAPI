using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.DTOs.FeatureDTOs
{
    public class FeatureReturnDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
    }
}
