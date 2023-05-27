using Avon.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.DTOs.CommentDTOs
{
    public class CommentCreateDto
    {
        public string Name { get; set; }
        public string Message { get; set; }

        public int Star { get; set; }

        public int ProductId { get; set; }
        public string AppUserId { get; set; }
    }
}
