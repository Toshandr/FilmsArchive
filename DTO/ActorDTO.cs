public class ActorDto //Data Trancfer Object для выдачи инфы об актёрах
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int BirthYear { get; set; }
    public int MoviesCount { get; set; }
}