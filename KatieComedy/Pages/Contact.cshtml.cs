using KatieComedy.App.Email;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace KatieComedy.Web.Pages;

public class ContactModel(EmailSender emailSender, IOptions<EmailOptions> options) : PageModel
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

    public async Task<IActionResult> OnPostAsync(CancellationToken cancel)
    {
        if (!ModelState.IsValid)
        {
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

        return RedirectToPage();
    }
}
