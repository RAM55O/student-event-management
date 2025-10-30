using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentEventManagement.Data
{
    public class Feedback
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int EventId { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5 stars.")]
        public int Rating { get; set; }

        [Required]
        public string ImproveSuggestion { get; set; } = string.Empty;

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        [ForeignKey("UserId")]
        public User? User { get; set; }

        [ForeignKey("EventId")]
        public Event? Event { get; set; }
    }
}
