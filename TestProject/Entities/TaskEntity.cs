using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestProject.Entities {
    public class TaskEntity {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(5000)]
        public string? Description { get; set; }

        [ForeignKey("FromProjectId")]
        public Project? FromProject { get; set; }
        public int FromProjectId { get; set; }

        public TaskEntity(string name) {
            Name = name;
        }
    }
}
