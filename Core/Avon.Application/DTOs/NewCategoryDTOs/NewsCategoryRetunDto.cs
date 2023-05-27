using Avon.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.DTOs.NewCategoryDTOs
{
    public class NewsCategoryRetunDto
    {
        public string Name { get; set; }
        public List<News> News { get; set; }
    }
}
