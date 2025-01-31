namespace Authentication_Server.Utils
{
    public class PasswordHasher
    {
        public string HashPassword(string enteredPassword)
        {
            return BCrypt.Net.BCrypt.HashPassword(enteredPassword);
        }

        public bool VerifyPassword(string enteredPassword, string storedPasswordHash)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, storedPasswordHash);
        }
    }
}
