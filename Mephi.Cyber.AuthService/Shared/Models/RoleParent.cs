namespace Mephi.Cyber.AuthService.Core.Models
{
    public class RoleParent
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public int ParentId { get; set; }
        public Role Parent { get; set; }
    }
}