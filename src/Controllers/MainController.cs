using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using src.Infra;
using src.Model;

namespace src.Controllers;

[ApiController]
public class MainController : ControllerBase
{
    private readonly RedisDb _db;

    public MainController(RedisDb db) => _db = db;

    [HttpGet("generate-result/{key}")]
    public async Task<ObjectResult> Get([FromRoute] string key)
    {
        var cacheKey = _db.GetCacheKey(key.Trim());

        var output = await _db.GetStringAsync<Result>(cacheKey);

        if (output != null)
        {
            return Ok(output);
        }

        var result = new Result("Data saved");

        await _db.SetStringAsync(cacheKey, result);

        return Ok(result);
    }
}
