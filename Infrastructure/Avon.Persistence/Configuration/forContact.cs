using Avon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Persistence.Configuration
{
    public class forContact : IEntityTypeConfiguration<ContactUs>
    {
        public void Configure(EntityTypeBuilder<ContactUs> builder)
        {
            builder.Property(x=>x.Name).IsRequired().HasMaxLength(25);
            builder.Property(x=>x.Email).IsRequired().HasMaxLength(30);
            builder.Property(x=>x.Phone).IsRequired().HasMaxLength(30);
            builder.Property(x => x.Message).IsRequired().HasMaxLength(600);
            builder.Property(x => x.Status).IsRequired();

        }
    }
}
