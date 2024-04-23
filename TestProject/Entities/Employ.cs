﻿using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestProject.Entities {
    public class Employ {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        //TODO consider oop version
        [Required]
        public bool IsManager { get; set; }

        public Employ(string name) {
            Name = name;
        }

    }
}
