using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class ClusterNode
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid ClusterId { get; set; }

        public Cluster Cluster { get; set; }

        [Required]
        public string Ip { get; set; }

        [Required]
        public string State { get; set; } = "NotReady";

        [Required]
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;

        public DateTime? DeleteAt { get; set; }
    }
}
