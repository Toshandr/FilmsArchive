using StackExchange.Redis;
using DotNetEnv;

public class ConnectBasicExample
{
    public void run()
    {
        Env.Load();
        
        var connectionString = Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING");
        
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Redis connection string is not configured");
        }
        
        var muxer = ConnectionMultiplexer.Connect(connectionString);
        var db = muxer.GetDatabase();
        
        db.StringSet("foo", "bar");
        var result = db.StringGet("foo");
        Console.WriteLine(result);
    }
}