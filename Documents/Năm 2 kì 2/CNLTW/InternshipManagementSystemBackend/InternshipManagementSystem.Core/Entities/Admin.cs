namespace InternshipManagementSystem.Core.Entities
{
    public class Admin
    {
        public Guid AdminId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string MatKhau { get; set; } = string.Empty;
    }
}