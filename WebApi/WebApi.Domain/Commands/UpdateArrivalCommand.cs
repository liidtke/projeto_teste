using WebApi.Domain.SharedKernel;

namespace WebApi.Domain.Commands;

public class UpdateArrivalCommand
{
    private readonly IRepository<PatientArrival> _arrivalRepository;

    public UpdateArrivalCommand(IRepository<PatientArrival> arrivalRepository)
    {
        _arrivalRepository = arrivalRepository;
    }

    public async Task<Result<PatientArrival>> Complete(int id, ArrivalStatus status)
    {
        var existing = await _arrivalRepository.FindAsync(id);
        if (existing == null)
        {
            return Result.NotFound<PatientArrival>();
        }

        existing.Status = status;

        var saveResult = await _arrivalRepository.SaveChangesAsync();
        if (!saveResult.IsSuccess)
            return Result.Failure<PatientArrival>("Erro ao salvar a chegada do patient no banco de dados");

        return Result.Success(existing);
    }


}