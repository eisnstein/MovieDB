using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using MovieDB.Api.App.Helpers;

namespace MovieDB.Api.App.Services;

public interface IEmailService
{
    public Task SendAsync(string to, string subject, string html, string? from = null);
}

public class EmailService : IEmailService
{
    private readonly AppSettings _appSettings;
    private readonly SmtpClient _smtpClient;

    public EmailService(IOptions<AppSettings> appSettings)
    {
        _appSettings = appSettings.Value;
        _smtpClient = new SmtpClient();
    }

    public async Task SendAsync(string to, string subject, string html, string? from = null)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(from ?? _appSettings.EmailFrom));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;
        email.Body = new TextPart(TextFormat.Html)
        {
            Text = html
        };

        await _smtpClient.ConnectAsync(_appSettings.SmtpHost, _appSettings.SmtpPort, SecureSocketOptions.StartTls);
        await _smtpClient.AuthenticateAsync(_appSettings.SmtpUser, _appSettings.SmtpPass);
        await _smtpClient.SendAsync(email);
        await _smtpClient.DisconnectAsync(true);
    }
}
