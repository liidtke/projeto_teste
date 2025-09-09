using WebApi.Domain.SharedKernel;

namespace WebApi.Domain.Commands;

public class CreateArrivalCommand
{
    private readonly IRepository<PatientArrival> _arrivalRepository;
    private readonly IIncrementalIdCacheService cache;

    public CreateArrivalCommand(IRepository<PatientArrival> arrivalRepository, IIncrementalIdCacheService cache)
    {
        _arrivalRepository = arrivalRepository;
        this.cache = cache;
    }

    public async Task<Result<PatientArrival>> InsertArrival(PatientArrivalInputModel input)
    {
        var number = cache.NextNumber("Arrival");
        if (input == null)
            return Result.Validation<PatientArrival>("Paciente não pode ser nulo");

        var arrival = new PatientArrival()
        {
            PatientId = input.PatientId,
            Arrival = DateTime.Now,
            Status = input.Status,
            SequentialNumber = number,
        };

        // Validar todos os campos
        var validationResult = ValidateArrival.Validate(arrival);
        if (!validationResult.IsSuccess)
            return validationResult;


        // Adicionar ao repositório
        _arrivalRepository.Add(arrival);

        // Salvar mudanças
        var saveResult = await _arrivalRepository.SaveChangesAsync();
        if (!saveResult.IsSuccess)
            return Result.Failure<PatientArrival>("Erro ao salvar a chegada do patient no banco de dados");

        return Result.Success(arrival);
    }


}

public static class ValidateArrival
{
    public static Result<PatientArrival> Validate(PatientArrival arrival)
    {
        var result = new Result<PatientArrival>();

        if (arrival.SequentialNumber <= 0)
            result.AddMessage("Número sequencial deve ser maior que zero");

        if (arrival.PatientId <= 0)
            result.AddMessage("PatientId é obrigatório e deve ser maior que zero");

        if (arrival.Arrival == DateTime.MinValue || arrival.Arrival == default(DateTime))
            result.AddMessage("Data de chegada é obrigatória");

        if (arrival.Arrival > DateTime.Now)
            result.AddMessage("Data de chegada não pode ser no futuro");

        if (!Enum.IsDefined(typeof(ArrivalStatus), arrival.Status))
            result.AddMessage("Status deve ser válido (Pending ou Completed)");

        // Se não há mensagens de erro, retorna sucesso
        if (result.Messages.Count == 0)
            return Result.Success(arrival);

        return result; // Retorna com as validações
    }
}

public class PatientArrivalInputModel
{
    public int Id { get; set; }

    public int PatientId { get; set; }
    public DateTime? Arrival { get; set; }
    public ArrivalStatus Status { get; set; }
}