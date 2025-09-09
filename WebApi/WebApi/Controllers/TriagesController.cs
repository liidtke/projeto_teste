using Microsoft.AspNetCore.Mvc;
using WebApi.Domain;
using WebApi.Domain.Commands;
using WebApi.Domain.Queries;
namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TriagesController : ControllerBase
{

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] TriageQueryParams input, [FromServices] TriageQuery query)
    {
        var result = await query.GetTriages(input);
        return Output.FromResult(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id, [FromServices] TriageQuery query)
    {
        var result = await query.GetTriages(new TriageQueryParams() { Id = id });
        return Output.FromResult(result, x => x.FirstOrDefault());
    }


    
    [HttpPost]
    public async Task<IActionResult> Put([FromBody] TriageInputModel p, [FromServices] CreateTriageCommand command)
    {
        var result = await command.InsertTriage(p);
        return Output.FromResult(result);
    }
    
    


   

   
}
