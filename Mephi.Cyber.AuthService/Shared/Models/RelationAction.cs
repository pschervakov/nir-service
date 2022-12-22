namespace Mephi.Cyber.AuthService.Core.Models
{
    public class RelationAct
    {
        public int Id { get; set; }
        public int RelationId { get; set; }
        public Relation Relation { get; set; }
        public int ActId { get; set; }
        public Act Act { get; set; }
    }
}