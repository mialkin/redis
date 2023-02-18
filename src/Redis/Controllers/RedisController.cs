using Microsoft.AspNetCore.Mvc;

namespace Redis.Controllers;

[Route("redis")]
public class RedisController : ControllerBase
{
    [HttpGet("hello")]
    public string SayHello()
    {
        return "Hello";
    }
}