namespace Film.Application.Dtos;

public class ReviewDto
{
    public int MovieId { get; set; }
    public string FullName { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }
}
