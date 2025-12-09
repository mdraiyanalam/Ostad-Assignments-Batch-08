namespace EventManagementSystem.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime Date { get; set; }
        public string Address { get; set; } = null!;
        public int AssignedUserId { get; set; }
        public User AssignedUser { get; set; } = null!;
    }
}