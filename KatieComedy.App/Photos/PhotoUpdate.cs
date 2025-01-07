namespace KatieComedy.App.Photos;

public record PhotoUpdate
{
    public int Id { get; init; }

    public DateOnly? Date { get; set; }

    public string? Caption { get; set; }

    public string? Credit { get; set; }
}
