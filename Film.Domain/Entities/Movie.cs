namespace Film.Domain.Entities;

public class Movie
{
    //d, Title, Description, ReleaseDate, Genre, PosterUrl
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string Genre { get; set; }
    public string PosterUrl { get; set; }
}
