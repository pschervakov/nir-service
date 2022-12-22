
using System.ComponentModel.DataAnnotations.Schema;

namespace Mephi.Cyber.AuthService.Core.Models
{
    public class Assignment
    {
        public int Id { get; set; }
        
        public int UserId { get; set; }
        public User User { get; set; }
        
        public int RelationId { get; set; }
        public Relation Relation { get; set; }

        public int EntityId { get; set; }
        public Entity Entity { get; set; }
    }
}