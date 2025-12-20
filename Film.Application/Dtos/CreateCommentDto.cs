using Film.Domain.Entities;

namespace Film.Application.Dtos;

public class CreateCommentDto
{
    //Id, MovieId, UserId, Text, CreatedAt
    public int MovieId { get; set; }
    public string UserId { get; set; }
    public string Text { get; set; }
}
