using Microsoft.AspNetCore.Mvc;

namespace Hackathon1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SampleController : ControllerBase
{
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
    public IActionResult Post([FromBody] object data)
    {
        return CreatedAtAction(nameof(GetById), new { id = 1 }, new { message = "Item created successfully", data });
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] object data)
    {
        return Ok(new { message = $"Item {id} updated successfully", data });
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        return Ok(new { message = $"Item {id} deleted successfully" });
    }
}
