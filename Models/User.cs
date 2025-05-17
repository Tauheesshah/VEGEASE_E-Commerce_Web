namespace VegEaseBackend.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } // Can be "Admin" or "User"
    }
    public class UserLoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
