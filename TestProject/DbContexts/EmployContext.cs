using Microsoft.EntityFrameworkCore;
using TestProject.Entities;

namespace TestProject.DbContexts {

    public class EmployContext : DbContext {
        public DbSet<Employ> Employs { get; set; }

        public EmployContext(DbContextOptions options) : base(options) { 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            modelBuilder.Entity<Employ>().HasData(
                new Employ("franco") { Id = 1, Description = " " },
                new Employ("carlo") { Id = 2, Description = " " }
                );

            base.OnModelCreating(modelBuilder);
        }
    }
}
