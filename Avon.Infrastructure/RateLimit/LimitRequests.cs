using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Infrastructure.RateLimit
{
    [AttributeUsage(AttributeTargets.Method)]
    public class LimitRequests : Attribute
    {
        public int TimeWindow { get; set; }
        public int MaxRequests { get; set; }
    }
}
