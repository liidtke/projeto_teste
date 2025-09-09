using Microsoft.EntityFrameworkCore;
using WebApi.Domain.SharedKernel;

namespace WebApi.Domain.Queries;

public class PatientQueryParams
{
    public int? Id { get; set; }
    public string Name { get; set; }
    public int Count { get; set; } = 50;
}

public class PatientQuery
{
    private readonly IRepository<Patient> _patientRepository;

    public PatientQuery(IRepository<Patient> patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public async Task<Result<List<Patient>>> GetPatients(PatientQueryParams p)
    {
        var query = _patientRepository.Query();
        if (!string.IsNullOrEmpty(p.Name))
        {
            query = query.Where(x => x.Name.Contains(p.Name));
        }

        if (p.Id.HasValue)
        {
            query = query.Where(x => x.Id == p.Id);       
        }
        
        var items = await query.Take(p.Count).ToListAsync();
        return Result.Success(items);
    }
}