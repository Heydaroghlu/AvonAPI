using Avon.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Domain.Entities
{
    public class ReferalTable:BaseEntity
    {
        public string Name { get; set; }
        public int FIrstAmount { get; set; }
        public int UserCount { get; set; }
        public int Percent { get; set; }
        public int SecondPercent { get; set; }
        public int ThirdPercent { get; set; }
        public int MustSale { get; set; }
    }
}
