using Avon.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Domain.Entities
{
    public class Feature : BaseEntity
    {

        public string Title { get; set; }
        public string Icon { get; set; }
    }
}
