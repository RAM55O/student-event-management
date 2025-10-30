namespace StudentEventManagement.Data
{
    public class EventParticipant
    {
        public int EventId { get; set; }
        public int UserId { get; set; }
        public string Status { get; set; } = "Joined"; // Default status
        public Event? Event { get; set; }
        public User? User { get; set; }
    }
}
