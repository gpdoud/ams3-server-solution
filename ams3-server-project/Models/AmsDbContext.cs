using Ams3.Models;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ams3.Models {
    public class AmsDbContext : DbContext {

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<Logger> Loggers { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<SystemConfig> SystemConfig { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }

        public AmsDbContext(DbContextOptions<AmsDbContext> options) : base(options) { }

        public AmsDbContext() {
        }

        protected override void OnModelCreating(ModelBuilder builder) {
            builder.Entity<User>(e => {
                e.HasIndex(p => p.Username).IsUnique();
            });
            builder.Entity<Department>(e => {
                e.HasIndex(p => p.Code).IsUnique();
            });
            builder.Entity<Equipment>(e => {
                e.HasIndex(p => p.SerialNumber).IsUnique();
            });
        }
    }
}
