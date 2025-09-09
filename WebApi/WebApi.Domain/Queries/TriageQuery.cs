using Microsoft.EntityFrameworkCore;
using WebApi.Domain.SharedKernel;
namespace WebApi.Domain.Queries;

public class TriageQueryParams
{
    public int? Id { get; set; }
    public int? PatientId { get; set; }
    public int Count { get; set; } = 50;
}

public class TriageQuery
{
    private readonly IRepository<Triage> _patientRepository;

    public TriageQuery(IRepository<Triage> patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public async Task<Result<List<Triage>>> GetTriages(TriageQueryParams p)
    {
        var query = _patientRepository.Query();

        if (p.Id.HasValue)
        {
            query = query.Where(x => x.Id == p.Id);
        }

        if (p.PatientId.HasValue)
        {
            query = query.Where(x => x.PatientId == p.PatientId);
        }

        var items = await query.Take(p.Count).ToListAsync();
        return Result.Success(items);
    }
}