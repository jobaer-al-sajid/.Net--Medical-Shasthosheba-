namespace Medical.Models;

public class Help
{
    public int Id { get; set; }

    public string? UserId { get; set; }

    public string Title { get; set; } = "";

    public string Message { get; set; } = "";

    public bool IsResolved { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}