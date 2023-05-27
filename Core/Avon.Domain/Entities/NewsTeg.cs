using Avon.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Domain.Entities
{
    public class NewsTeg:BaseEntity
    {
        public int TegId { get; set; }
        public Teg Teg { get; set; }    
        public int NewsId { get; set; }
        public News News { get; set; }
    }
}
