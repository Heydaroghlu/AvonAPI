using Avon.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Domain.Entities
{
    public class News:BaseEntity
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public string PosterImage { get; set; }

        public int NewsCategoryId { get; set; }
        public NewsCategory NewsCategory { get; set; }
        
        public List<NewsTeg> NewsTegs { get; set; }  
    }
}
