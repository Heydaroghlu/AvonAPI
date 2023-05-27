using Avon.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Domain.Entities
{
	public class VariantFeature:BaseEntity
	{
		public VFeature VFeature { get; set; }
		public int VFeatureId { get; set; }

		public Variant Variant { get; set; }
		public int VariantId { get; set; }
	}
}