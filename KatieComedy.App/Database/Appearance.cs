using System.ComponentModel.DataAnnotations.Schema;

namespace KatieComedy.App.Database;

[Table(nameof(Appearance))]
public class Appearance
{
    public int Id { get; set; }

    public DateTimeOffset DateTime { get; set; }

    public string EventName { get; set; }

    public string EventUrl { get; set; }

    public string LocationName { get; set; }

    public string LocationUrl { get; set; }
}
