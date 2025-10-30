namespace StudentEventManagement.Data
{
    public class Event
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime Date { get; set; }
        public int CreatedBy { get; set; }  // Foreign key to User.Id
        public User? Creator { get; set; }  // Navigation property
        public virtual ICollection<EventCategory> EventCategories { get; set; }
    }
}
