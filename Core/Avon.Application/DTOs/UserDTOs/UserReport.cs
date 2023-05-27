using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.DTOs.UserDTOs
{
    public class UserReport
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string otherAddress { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsAdmin { get; set; }
        [DataType("decimal(18,2)")]
        public double Gain { get; set; }
        [DataType("decimal(18,2)")]
        public double FirstAmount { get; set; }
        [DataType("decimal(18,2)")]
        public double TotalAmount { get; set; }
        [NotMapped]
        public List<UserReport> ReferalUsers { get; set; }
        public int OrderCountForMounth { get; set; }
        public int ReferalCount { get; set; }
        public string Position { get; set; }
        public bool IsFirst { get; set; }
        public string? ProfileImage { get; set; }

    }
}
