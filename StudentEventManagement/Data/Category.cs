
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentEventManagement.Data
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public virtual ICollection<EventCategory> EventCategories { get; set; }
    }
}
