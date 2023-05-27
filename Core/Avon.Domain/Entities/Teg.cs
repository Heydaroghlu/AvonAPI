using Avon.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Domain.Entities
{
    public class Teg:BaseEntity
    {
        public string Name { get; set; }
    

        public List<NewsTeg> NewsTegs { get; set; }
        public List<ProductTeg> ProductTegs { get; set; }
    }
}
