using Film.Application.Configurations;
using Film.Application.Dtos;
using Film.Application.Interfaces;
using Film.Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;

namespace Film.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly ContactsApi _contactsApi;
    private readonly EmailCampaignsApi _campaignsApi;
    private readonly TransactionalEmailsApi _emailApi;
    private readonly BrevoSettings _settings;
    private readonly ILogger<EmailService> logger;

    public EmailService(IOptions<BrevoSettings> options, ILogger<EmailService> logger)
    {
        _settings = options.Value;

        var configuration = new Configuration();
        configuration.AddApiKey("api-key", _settings.ApiKey);

        _contactsApi = new ContactsApi(configuration);
        _campaignsApi = new EmailCampaignsApi(configuration);
        _emailApi = new TransactionalEmailsApi(configuration);
        this.logger = logger;
    }


    public async System.Threading.Tasks.Task SendNewMovieEmailAsync(MovieDto movie)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "email-templates", "new-movie.html");
        var htmlContent =  File.ReadAllText(path);

        htmlContent = htmlContent
        .Replace("{{Title}}", movie.Title)
        .Replace("{{PosterUrl}}", movie.PosterUrl)
        .Replace("{{Genre}}", movie.Genre)
        .Replace("{{ReleaseDate}}", movie.ReleaseDate.ToShortDateString())
        .Replace("{{Description}}", movie.Description)
        .Replace("{{MovieLink}}", $"http://localhost:4200/movies/details/{movie.Id}");


        var sendSMTPEmail = new SendSmtpEmail(
            sender: new SendSmtpEmailSender(_settings.SenderName, _settings.SenderEmail),
            to: new List<SendSmtpEmailTo>() { new SendSmtpEmailTo("mahmoud.elbadry357@gmail.com") },
            subject: "عمك لقي اثار ومحتاجين حد امين يصرفها",
            htmlContent: htmlContent
            );

        try
        {
            logger.LogInformation("Sending email...");
            await _emailApi.SendTransacEmailAsync(sendSMTPEmail);
        }
        catch (Exception ex)
        {
         logger.LogError($"Error while sending email: {ex.Message}");
        }
    }    
}
