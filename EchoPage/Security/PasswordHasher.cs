namespace EchoPage.Security
{
    public class PasswordHasher
    {
        public static string Hashpassword(string password)
        {
            var salt = BCrypt.Net.BCrypt.GenerateSalt();
            return BCrypt.Net.BCrypt.HashPassword(password, salt);
        }

        public static bool VerifyPassword(string password, string hashed)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashed);
        }
    }
}
