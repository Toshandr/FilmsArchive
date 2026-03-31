using Microsoft.AspNetCore.Mvc;
using MovieAPI.Cache;

[ApiController]
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
            return Ok(new { status = "Redis not connected" });
        }
        return Ok(new { status = "Redis connected" });
    }

    [HttpPost("test-redis")]
    public async Task<IActionResult> TestRedis()
    {
        if (_cache == null)
        {
            return Ok(new { error = "Redis not available" });
        }

        await _cache.SetAsync("test:key", "test value", TimeSpan.FromMinutes(1));
        var value = await _cache.GetAsync<string>("test:key");
        
        return Ok(new { 
            written = "test value",
            read = value,
            success = value == "test value"
        });
    }
}
