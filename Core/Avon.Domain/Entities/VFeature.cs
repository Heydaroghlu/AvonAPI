using Avon.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Domain.Entities
{
	public class VFeature:BaseEntity
	{
		public string Name { get; set; }
		public string Variable { get; set; }
		public List<VariantFeature> VariantFeatures { get; set; }
	}
}
