using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Metric
    {
        [Key]
        public long Id { get; set; }

        public float Value { get; set; }

        public string Type { get; set; }

        public Guid ItemId { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
