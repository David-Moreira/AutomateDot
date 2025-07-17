using AutomateDot.Configurations;

using System.Net;
using System.Net.Mail;

namespace AutomateDot.Actions;

public sealed class SmtpEmailAction : IActionHandler<SmtpEmailConfiguration>
{
    public async Task ExecuteAsync(SmtpEmailConfiguration configuration, object? triggerPayload = null)
    {
        var host = ActionHelper.ReplacePlaceholders(configuration.SmtpHost, triggerPayload);
        var user = ActionHelper.ReplacePlaceholders(configuration.SmtpUser, triggerPayload);
        var pass = ActionHelper.ReplacePlaceholders(configuration.SmtpPass, triggerPayload);
        var from = ActionHelper.ReplacePlaceholders(configuration.From, triggerPayload);
        var to = ActionHelper.ReplacePlaceholders(configuration.To, triggerPayload);
        var subject = ActionHelper.ReplacePlaceholders(configuration.Subject, triggerPayload);
        var body = ActionHelper.ReplacePlaceholders(configuration.Body, triggerPayload);

        using var client = new SmtpClient(host, configuration.SmtpPort)
        {
            Credentials = new NetworkCredential(user, pass),
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network
        };

        using var message = new MailMessage(from, to, subject, body)
        {
            IsBodyHtml = true
        };

        await client.SendMailAsync(message);
    }
}