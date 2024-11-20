namespace Core.Utilities.Helpers
{
    public static class TokenGenerator
    {
        public static string GenerateToken()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}