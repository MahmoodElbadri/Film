namespace Film.Application.Dtos;

public class CreateReviewDto
{
    public int MovieId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }
}
