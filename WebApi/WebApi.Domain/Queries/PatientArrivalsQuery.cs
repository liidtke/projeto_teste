using Microsoft.EntityFrameworkCore;
using WebApi.Domain.SharedKernel;
namespace WebApi.Domain.Queries;

public class PatientArrivalsQueryParams
{
    public int? Id { get; set; }
    public int? PatientId { get; set; }
    public int? SequentialNumber { get; set; }
    public ArrivalStatus? Status { get; set; }
    public int Count { get; set; } = 50;
}

public class PatientArrivalsQuery
{
    private readonly IRepository<PatientArrival> _patientRepository;

    public PatientArrivalsQuery(IRepository<PatientArrival> patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public async Task<Result<List<PatientArrival>>> GetPatientArrival(PatientArrivalsQueryParams p)
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

        if (p.SequentialNumber.HasValue)
        {
            query = query.Where(x => x.SequentialNumber == p.SequentialNumber && x.Status == ArrivalStatus.Pending); 
        }

        if (p.Status.HasValue)
        {
            query = query.Where(x => x.Status == p.Status);
        }

        var items = await query.Include(x=>x.Patient).Take(p.Count).ToListAsync();
        return Result.Success(items);
    }
}
