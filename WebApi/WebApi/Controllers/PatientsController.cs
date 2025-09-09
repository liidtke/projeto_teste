using Microsoft.AspNetCore.Mvc;
using WebApi.Domain;
using WebApi.Domain.Commands;
using WebApi.Domain.Queries;
namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PatientsController : ControllerBase
{
 
    
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] PatientQueryParams input, [FromServices] PatientQuery query)
    {
        var result = await query.GetPatients(input);
        return Output.FromResult(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id, [FromServices] PatientQuery query)
    {
        var result = await query.GetPatients(new PatientQueryParams
        {
            Id = id
        });
        return Output.FromResult(result, x => x.FirstOrDefault());
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] Patient p, [FromServices] UpdatePatientCommand command)
    {
        var result = await command.UpdatePatient(id, p);
        return Output.FromResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> Put([FromBody] Patient p, [FromServices] CreatePatientCommand command)
    {
        var result = await command.InsertPatient(p);
        return Output.FromResult(result);
    }


}