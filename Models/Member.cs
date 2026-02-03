namespace DemoApi.Models
{
    public class Member
    {
        public int Id { get; set; }
        public string? Company { get; set; }
        public string Name { get; set; } = string.Empty; // Contact Name
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string Mobile { get; set; } = string.Empty;
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
