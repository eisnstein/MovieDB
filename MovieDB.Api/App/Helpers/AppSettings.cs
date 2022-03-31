namespace MovieDB.Api.App.Helpers;

public class AppSettings
{
    public string Secret { get; set; } = default!;
    public int RefreshTokenTtl { get; set; }
    public string EmailFrom { get; set; } = default!;
    public string SmtpHost { get; set; } = default!;
    public int SmtpPort { get; set; }
    public string SmtpUser { get; set; } = default!;
    public string SmtpPass { get; set; } = default!;
}
