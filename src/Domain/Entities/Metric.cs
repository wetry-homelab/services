using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Metric
    {
        [Key]
        public long Id { get; set; }

        public long CpuValue { get; set; }

        public long MemoryValue { get; set; }

        public Guid EntityId { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
