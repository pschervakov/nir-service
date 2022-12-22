namespace Mephi.Cyber.AuthService.Core.Models
{
    public class Permission
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Read { get; set; }
        public bool Write { get; set; }
        // public bool Edit { get; set; }
        // public bool Delete{ get; set; }
        public int EntityId { get; set; }
        public Entity Entity { get; set; }
    }
}
 