using Avon.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Domain.Entities
{
    public class Promotion : BaseEntity
    {
        public string Key { get; set; }
        public string Image { get; set; }
    }
}
