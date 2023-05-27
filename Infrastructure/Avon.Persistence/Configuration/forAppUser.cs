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
    public class forAppUser : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(x => x.Id).HasColumnName("AppUserId");
            builder.Property(x => x.Name).IsRequired().HasMaxLength(20);
            builder.Property(x => x.otherAddress).IsRequired().HasMaxLength(20);
            builder.Property(x => x.Surname).IsRequired().HasMaxLength(20);
        }
    }
}
