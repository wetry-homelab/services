using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class Template
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Type { get; set; } = "k3s";

        [Required]
        public string BaseTemplate { get; set; }

        [Required]
        public int CpuCount { get; set; } = 1;

        [Required]
        public int MemoryCount { get; set; } = 1024;

        [Required]
        public int DiskSpace { get; set; } = 20;

        [Required]
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;

        public DateTime? DeleteAt { get; set; }
    }
}
