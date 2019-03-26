using System;
using Guide.Core.AuthModels;
using Guide.Core.DomainModels;
using Guide.Core.DomainModels.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace Guide.Application.Data
{
    public class GuideContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        private readonly IConfiguration _config;
       
        public GuideContext(DbContextOptions options, IConfiguration config)
            : base(options)
        {
            _config = config;
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Dealer> Dealers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<StockTransaction> StockTransactions { get; set; }

        #region Sql Views

        public DbQuery<ProductListDto> ProductListDtos { get; set; }

        #endregion



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region Auth
            builder.Entity<ApplicationUser>(entity => { entity.ToTable("SystemUser"); });
            builder.Entity<ApplicationRole>(entity => { entity.ToTable("Role"); });

            builder.Entity<IdentityUserToken<int>>(entity => { entity.ToTable("UserToken"); });
            builder.Entity<IdentityUserLogin<int>>(entity => { entity.ToTable("UserLogin"); });
            builder.Entity<IdentityRoleClaim<int>>(entity => { entity.ToTable("RoleClaim"); });
            builder.Entity<IdentityUserRole<int>>(entity => { entity.ToTable("UserRole"); });
            builder.Entity<IdentityUserClaim<int>>(entity => { entity.ToTable("UserClaim"); });
            #endregion

            
            #region Product
            builder.Query<ProductListDto>().ToView("ProductListView");

            #endregion
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(_config["connectionStrings:ConnectionString"]);

        }
    }
}
