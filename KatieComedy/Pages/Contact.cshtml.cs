using KatieComedy.App.Email;
using KatieComedy.Web.Cloudflare;

namespace KatieComedy.Web.Pages;

public class ContactModel(
    EmailSender emailSender,
    CloudflareClient cloudflareClient,
    IOptions<EmailOptions> options) : BasePageModel
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

    public async Task<IActionResult> OnPostAsync([FromForm(Name = "cf-turnstile-response")] string turnstileToken, CancellationToken cancel)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var turnstileResponse = await cloudflareClient.ValidateToken(turnstileToken, cancel);

        if (!turnstileResponse.Success)
        {
            Toast(ToastLevel.Error, "Cloudflare validation failed.");
            return Page();
        }

        var request = new EmailRequest
        {
            ToAddress = options.Value.ContactFormAddress,
            ToName = "Contact",
            Subject = $"Message from {Name}",
            ReplyAddress = Email,
            ReplyName = Name,
            HtmlText = Message
        };

        await emailSender.SendEmailAsync(request, cancel);

        Toast(ToastLevel.Error, "Message sent.");
        return RedirectToPage();
    }
}
