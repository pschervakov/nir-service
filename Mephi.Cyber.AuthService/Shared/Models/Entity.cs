namespace Mephi.Cyber.AuthService.Core.Models
{
    public class Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Entity? Parent { get; set; }
        public int? ParentId { get; set; }
        public Project Project { get; set; }
        public int ProjectId { get; set; }
    }
}