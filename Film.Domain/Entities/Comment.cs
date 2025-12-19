namespace Film.Domain.Entities;

public class Comment
{
    //Id, MovieId, UserId, Text, CreatedAt
    public int Id { get; set; }
    public int MovieId { get; set; }
    public Movie Movie { get; set; }
    public AppUser AppUser { get; set; }
    public int UserId { get; set; }
    public string Text { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
