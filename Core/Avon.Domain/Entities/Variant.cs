using Avon.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Domain.Entities
{
	public class Variant:BaseEntity
	{
		public string Name { get; set; }
		public string Type { get; set; }
		public List<VariantFeature> VariantFeatures { get; set; }		

	}
}
