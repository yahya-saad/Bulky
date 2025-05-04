namespace BulkyBookWeb.Configuration;

public static class FluentEmailConfiguration
{
    public static IServiceCollection AddFluentEmailConfiguration(this IServiceCollection services, ConfigurationManager configuration)
    {
        var emailSettings = configuration.GetSection("FluentEmail").Get<SendingEmailSettings>();
        services.AddFluentEmail(emailSettings.Email)
            .AddSmtpSender(emailSettings.Host, emailSettings.Port, emailSettings.Email, emailSettings.Password);

        return services;
    }
}

