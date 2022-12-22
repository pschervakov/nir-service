namespace Mephi.Cyber.AuthService.Core.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Email { get; set; }
        public int Salt { get; set; }
        public string SaltedHash { get; set; }

    }
}