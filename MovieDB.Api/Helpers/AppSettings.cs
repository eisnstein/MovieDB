namespace MovieDB.Api.Helpers
{
    public interface IAppSettings
    {
        string Secret { get; set; }
        int RefreshTokenTtl { get; set; }
        string EmailFrom { get; set; }
        string SmtpHost { get; set; }
        int SmtpPort { get; set; }
        string SmtpUser { get; set; }
        string SmtpPass { get; set; }
    }

    public class AppSettings : IAppSettings
    {
        public string Secret { get; set; }
        public int RefreshTokenTtl { get; set; }
        public string EmailFrom { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPass { get; set; }
    }
}