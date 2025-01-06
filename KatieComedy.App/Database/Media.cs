using KatieComedy.App.Media;
using System.ComponentModel.DataAnnotations.Schema;

namespace KatieComedy.App.Database;

[Table(nameof(Media))]
public class Media
{
    public int Id { get; set; }

    public MediaType Type { get; set; }

    public DateOnly? Date { get; set; }

    public string Url { get; set; }

    public string Title { get; set; }
}
