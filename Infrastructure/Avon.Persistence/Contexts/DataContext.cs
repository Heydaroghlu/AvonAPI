using Avon.Domain.Entities;
using Avon.Domain.Entities.Common;
using FluentValidation.Validators;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Persistence.Contexts
{
    public class DataContext : IdentityDbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<ReferalTable> ReferalTables { get; set; }  
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }    
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<DeliveryAdress> DeliveryAdresses { get; set; }
        public DbSet<ProductSubCategory> ProductSubCategory { get; set; }
		public DbSet<Variant> Variants { get; set; }
		public DbSet<VFeature> VFeatures { get; set; }

		public DbSet<Feature> Features { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<Brand> Brands { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        { 
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
			base.OnModelCreating(builder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entiteis = ChangeTracker.Entries<BaseEntity>();

            foreach (var item in entiteis)
            {
                if(item.State == EntityState.Added)
                {
                    item.Entity.CreatedAt = DateTime.UtcNow.AddHours(4);
                }else if(item.State == EntityState.Modified)
                {
                    item.Entity.UpdatedAt = DateTime.UtcNow.AddHours(4);
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

    }
}
