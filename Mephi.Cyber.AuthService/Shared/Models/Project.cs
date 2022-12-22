namespace Mephi.Cyber.AuthService.Core.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SharedSecret { get; set; }
        public string AuthSecret { get; set; }
    }
}