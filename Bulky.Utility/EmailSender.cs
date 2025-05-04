using FluentEmail.Core;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace BulkyBook.Utility;
public class EmailSender : IEmailSender
{
    private readonly IFluentEmail fluentEmail;

    public EmailSender(IFluentEmail fluentEmail)
    {
        this.fluentEmail = fluentEmail;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        try
        {
            await fluentEmail.To(email)
                .Subject(subject)
                .Body(htmlMessage, isHtml: true)
                .SendAsync();
        }
        catch (Exception ex)
        {
            throw;

        }
    }
}
