using Microsoft.AspNetCore.Mvc;
using Hackathon1.Models;

namespace Hackathon1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SampleController : ControllerBase
{
    private static int _nextId = 0;

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { message = "Welcome to Hackathon1 Web API" });
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        return Ok(new { id, message = $"Retrieved item with ID: {id}" });
    }

    [HttpPost]
    public IActionResult Post([FromBody] ItemDto data)
    {
        var newId = System.Threading.Interlocked.Increment(ref _nextId);
        return CreatedAtAction(nameof(GetById), new { id = newId }, new { message = "Item created successfully", id = newId, data });
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] ItemDto data)
    {
        return Ok(new { message = $"Item {id} updated successfully", data });
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        return Ok(new { message = $"Item {id} deleted successfully" });
    }
}
