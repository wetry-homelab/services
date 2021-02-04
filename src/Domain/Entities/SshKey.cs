using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class SshKey
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Public { get; set; }

        [Required]
        public string Private { get; set; }

        [Required]
        public string Fingerprint { get; set; }

        [Required]
        public string Pem { get; set; }

        [Required]
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;

        public DateTime? DeleteAt { get; set; }
    }
}
