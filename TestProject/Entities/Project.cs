using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestProject.Entities {
    public class Project {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public Manager Manager { get; set; }

        public ICollection<Employee> Employees { get;  }

        public ICollection<TaskEntity> Tasks { get; set; }

    }
}
