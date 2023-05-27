using Avon.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Domain.Entities
{
    public class NewsCategory:BaseEntity
    {
        public string Name { get; set; }
    
        public List<News> News { get; set; }
    }
}
