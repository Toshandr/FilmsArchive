public class CreateMovieDto //Data Transfer Object при запросе инфы при создании фильма
{
    public string Title { get; set; } = string.Empty;
    public int Year { get; set; }
    public string Genre { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<CreateMovieActorDto> Actors { get; set; } = new();
}

public class CreateMovieActorDto //Data Transfer Object при запросе инфы при создании фильма(лист)
{
    public int ActorId { get; set; }
    public string Role { get; set; } = string.Empty;
}
