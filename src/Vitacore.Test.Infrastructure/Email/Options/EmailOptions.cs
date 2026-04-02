namespace Vitacore.Test.Infrastructure.Email.Options
{
    public class EmailOptions
    {
        public const string SectionName = "Email";

        public string Host { get; set; } = string.Empty;
        public int Port { get; set; } = 25;
        public bool EnableSsl { get; set; }
        public bool UseDefaultCredentials { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string FromAddress { get; set; } = string.Empty;
        public string? FromName { get; set; }
    }
}
