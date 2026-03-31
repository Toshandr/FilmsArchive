using Microsoft.AspNetCore.Mvc;
using MovieAPI.Services;

[ApiController]
[Route("api/[controller]")]
public class MoviesController : ControllerBase
{
    private readonly MovieService _movieService;

    public MoviesController(MovieService movieService)
    {
        _movieService = movieService;
    }

    [HttpGet]
    public async Task<ActionResult<List<MovieDto>>> GetMovies()
    {
        var movies = await _movieService.GetAllMovies();
        return Ok(movies);
    }

    [HttpPost]
    public async Task<ActionResult<MovieDto>> CreateMovie([FromBody] CreateMovieDto createDto)
    {
        try
        {
            var movie = await _movieService.CreateMovie(createDto);
            return CreatedAtAction(nameof(GetMovies), new { id = movie.Id }, movie);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}