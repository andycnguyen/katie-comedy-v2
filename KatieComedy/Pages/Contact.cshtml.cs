using KatieComedy.App.Email;
using KatieComedy.Web.Cloudflare;

namespace KatieComedy.Web.Pages;

public class ContactModel(
    EmailSender emailSender,
    CloudflareClient cloudflareClient,
    IOptions<EmailOptions> options,
    ILogger<ContactModel> logger) : BasePageModel
{
    [BindProperty, EmailAddress]
    public string Email { get; set; }

    [BindProperty, MaxLength(128)]
    public string Name { get; set; }

    [BindProperty, MaxLength(5000)]
    public string Message { get; set; }


    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync(
        [FromForm(Name = "cf-turnstile-response")] string turnstileToken,
        CancellationToken cancel)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (string.IsNullOrEmpty(turnstileToken))
        {
            Toast(ToastLevel.Error, "Cloudflare validation failed. Please try again.");
            return Page();
        }

        var turnstileResult = await cloudflareClient.ValidateToken(turnstileToken, cancel);

        if (!turnstileResult.Success)
        {
            Toast(ToastLevel.Error, "Cloudflare validation failed. Please try again.");
            return Page();
        }

        var htmlText =
        $"""
        Name:<br/> 
        {Name}<br/><br/>
        Email:<br/> 
        {Email}<br/><br/>
        Message:<br/>
        {Message}
        """;

        var request = new EmailRequest
        {
            ToAddress = options.Value.ContactFormAddress,
            ToName = "Contact",
            Subject = $"Message from {Name}",
            ReplyAddress = Email,
            ReplyName = Name,
            HtmlText = htmlText
        };

        try
        {
            await emailSender.SendEmailAsync(request, cancel);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error send email.");
            Toast(ToastLevel.Error, "Error sending message.");
            return Page();
        }

        Toast(ToastLevel.Success, "Message sent.");
        return RedirectToPage();
    }
}
