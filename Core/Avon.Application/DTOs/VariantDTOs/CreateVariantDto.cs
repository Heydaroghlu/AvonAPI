using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.DTOs.VariantDTOs
{
	public class CreateVariantDto
	{
		public string Name { get; set; }
		public string Type { get; set; }
		public int[] selectFeatureIds { get; set; }
	}
}
