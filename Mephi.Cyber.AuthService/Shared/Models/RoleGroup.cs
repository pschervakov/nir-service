namespace Mephi.Cyber.AuthService.Core.Models
{
    public class RoleGroup
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}