using Film.Domain.Entities;

namespace Film.Application.Dtos;

public class CommentDto
{
    public int MovieId { get; set; }
    public string FullName { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
