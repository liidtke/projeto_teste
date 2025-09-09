using Microsoft.EntityFrameworkCore;
using WebApi.Domain.SharedKernel;

namespace WebApi.Domain.Queries;

public class PatientQueryParams
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public int Count { get; set; } = 50;
    public bool WithArrival { get; set; } = false;
}

public class PatientQuery
{
    private readonly IRepository<Patient> patientRepository;
    private readonly IRepository<PatientArrival> arrivalRepository;

    public PatientQuery(IRepository<Patient> patientRepository, IRepository<PatientArrival> arrivalRepository)
    {
        this.patientRepository = patientRepository;
        this.arrivalRepository = arrivalRepository;
    }

    public async Task<Result<List<Patient>>> GetPatients(PatientQueryParams p)
    {
        var query = patientRepository.Query();
        if (!string.IsNullOrEmpty(p.Name))
        {
            query = query.Where(x => x.Name.Contains(p.Name));
        }

        if (p.Id.HasValue)
        {
            query = query.Where(x => x.Id == p.Id);
        }

        if (p.WithArrival)
        {
            var joinedData = arrivalRepository.Query().Join(
                query,
                arrival => arrival.PatientId,
                patient => patient.Id,
                (arrival, patient) => new Patient()
                {
                   Id = patient.Id,
                   Name = patient.Name,
                   PatientArrivalId = arrival.Id,
                   Email = patient.Email,
                   Phone = patient.Phone,
                   Gender = patient.Gender,
                }
            );

            var result = await joinedData.AsNoTracking().Take(p.Count).ToListAsync();
            return Result.Success(result.DistinctBy(x=>x.Id).ToList());

        }

        var items = await query.Take(p.Count).ToListAsync();
        return Result.Success(items);
    }
}