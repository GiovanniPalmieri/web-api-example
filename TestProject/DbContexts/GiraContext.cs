using Microsoft.EntityFrameworkCore;
using TestProject.Entities;

namespace TestProject.DbContexts {

    public class GiraContext : DbContext {
        public DbSet<Employee> Employs { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TaskEntity> Tasks { get; set; }
        public DbSet<Manager> Managers { get; set; }

        public GiraContext(DbContextOptions options) : base(options) { 
        }

    }
}
