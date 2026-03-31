public class CreateActorDto //Data Transfer Object при запросе инфы при создании актёра
{
    public string Name { get; set; } = string.Empty;
    public int BirthYear { get; set; }
}