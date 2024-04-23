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

        [Required]
        public Employ Manager { get; set; }

        public ICollection<Task> Tasks { get; set; } = new List<Task>();


        public ICollection<Employ> AssignedTo { get; set; } = new List<Employ>();

        public Project(string name, Employ manager) {
            Name = name;
            Manager = manager;
        }
    }
}
