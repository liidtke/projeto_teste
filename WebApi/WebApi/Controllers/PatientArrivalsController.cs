using Microsoft.AspNetCore.Mvc;
using WebApi.Domain;
using WebApi.Domain.Commands;
using WebApi.Domain.Queries;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PatientArrivalsController : ControllerBase
{

    [HttpGet("next")]
    public async Task<IActionResult> Get([FromQuery] PatientArrivalsQueryParams input, [FromServices] PatientArrivalsQuery query, IIncrementalIdCacheService cacheService)
    {
        var number = cacheService.GetCurrentValue("Arrival");
        if (number == null)
        {
            return NoContent();
        }
        
        var currentNumber = cacheService.NextNumber("Queue");
        
        var result = await query.GetPatientArrival(new PatientArrivalsQueryParams()
        {
            SequentialNumber = currentNumber,
        });
        
        return Output.FromResult(result);
    }
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] PatientArrivalsQueryParams input, [FromServices] PatientArrivalsQuery query)
    {
        var result = await query.GetPatientArrival(input);
        return Output.FromResult(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id, [FromServices] PatientArrivalsQuery query)
    {
        var result = await query.GetPatientArrival(new PatientArrivalsQueryParams()
        {
            Id = id
        });
        return Output.FromResult(result, x => x.FirstOrDefault());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] PatientArrival p, [FromServices] UpdateArrivalCommand command)
    {
        var result = await command.Complete(id, ArrivalStatus.Completed);
        return Output.FromResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> Put([FromBody] PatientArrival p, [FromServices] CreateArrivalCommand command)
    {
        var result = await command.InsertArrival(p);
        return Output.FromResult(result);
    }


}