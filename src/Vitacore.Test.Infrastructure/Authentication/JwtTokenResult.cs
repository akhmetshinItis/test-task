namespace Vitacore.Test.Infrastructure.Authentication
{
    public class JwtTokenResult
    {
        public string AccessToken { get; set; } = string.Empty;
        public DateTime ExpiresAtUtc { get; set; }
    }
}
