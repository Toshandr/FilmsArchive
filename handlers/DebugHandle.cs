using Microsoft.AspNetCore.Mvc;
using MovieAPI.Cache;

[ApiController]  //Этjn запрос я делал для проверки подключения к Redis
[Route("api/[controller]")]
public class DebugController : ControllerBase
{
    private readonly RedisCache? _cache;

    public DebugController(RedisCache? cache = null)
    {
        _cache = cache;
    }

    [HttpGet("redis-status")]
    public IActionResult GetRedisStatus()
    {
        if (_cache == null)
        {
            return BadRequest(new { status = "Redis not connected" });
        }
        return Ok(new { status = "Redis connected" });
    }
}
