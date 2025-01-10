using System.ComponentModel.DataAnnotations.Schema;

namespace KatieComedy.App.Database;

[Table(nameof(Biography))]
public class Biography
{
    public int Id { get; set; }

    public string Text { get; set; }
}
