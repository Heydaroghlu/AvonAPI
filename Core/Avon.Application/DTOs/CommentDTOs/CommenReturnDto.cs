using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.DTOs.CommentDTOs
{
    public class CommenReturnDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public int ProductId { get; set; }
        
        public string IsAccepted { get; set; }
    }
}
