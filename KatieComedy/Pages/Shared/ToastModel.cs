namespace KatieComedy.Web.Pages.Shared;

public enum ToastLevel
{
    Primary,
    Success,
    Warning,
    Error,
    Info
}

public record ToastModel
{
    public required string Message { get; init; }

    public required ToastLevel Level { get; init; }
}
