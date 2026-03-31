public class MovieDto//Data Trancfer Object для выдачи инфы о фильме
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int Year { get; set; }
    public string Genre { get; set; } = string.Empty;
    public List<ActorInMovieDto> Actors { get; set; } = new();
}

public class ActorInMovieDto //Data Transfer Object для списка актёров в фильме
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}