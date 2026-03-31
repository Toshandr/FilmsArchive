public class CreateMovieDto
{
    public string Title { get; set; } = string.Empty;
    public int Year { get; set; }
    public string Genre { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<CreateMovieActorDto> Actors { get; set; } = new();
}

public class CreateMovieActorDto
{
    public int ActorId { get; set; }
    public string Role { get; set; } = string.Empty;
}
