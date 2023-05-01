namespace RestApi.Infrastructure.Auth
{
    public static class Users
    {
        public static List<User> GetUsers()
        {
            return new List<User>
            {
                new User { Username = "user1", Password = "password1", Role = "user" },
                new User { Username = "admin1", Password = "password1", Role = "admin" }
            };
        }
    }

    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
