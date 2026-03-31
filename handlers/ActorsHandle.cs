using Microsoft.AspNetCore.Mvc;
using MovieAPI.Services;

[ApiController]
[Route("api/[controller]")]
public class ActorsController : ControllerBase
{
    private readonly ActorService _actorService;

    public ActorsController(ActorService actorService)
    {
        _actorService = actorService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ActorDto>>> GetActors()
    {
        var actors = await _actorService.GetAllActors(); 
        return Ok(actors);
    }

    [HttpPost]
    public async Task<ActionResult<ActorDto>> CreateActor(CreateActorDto createDto)
    {
        try
        {
            var actor = await _actorService.CreateActor(createDto);
            return CreatedAtAction(nameof(GetActors), new { id = actor.Id }, actor);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}