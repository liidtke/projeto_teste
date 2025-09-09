using WebApi.Domain.SharedKernel;

namespace WebApi.Domain.Commands;

public class TriageInputModel
{
    public int Id { get; set; }
    
    public int PatientId { get; set; }
    
    public int PatientArrivalId { get; set; }
    
    public string Symptoms { get; set; }
    public string ArterialPressure { get; set; }
    public decimal Weight { get; set; }
    public decimal Height { get; set; }

    public int KindId { get; set; } = 1;
}

public class CreateTriageCommand
{
    private readonly IRepository<Triage> _triageRepository;

    public CreateTriageCommand(IRepository<Triage> triageRepository)
    {
        _triageRepository = triageRepository;
    }

    public async Task<Result<Triage>> InsertTriage(TriageInputModel input)
    {
        if (input == null)
            return Result.Validation<Triage>("Triage não pode ser nulo");

        var triage = new Triage()
        {
            PatientId = input.PatientId,
            PatientArrivalId = input.PatientArrivalId,
            Symptoms = input.Symptoms,
            ArterialPressure = input.ArterialPressure,
            Weight = input.Weight,
            Height = input.Height,
        };

        // Validar todos os campos
        var validationResult = ValidateTriage.Validate(triage);
        if (!validationResult.IsSuccess)
            return validationResult;

        // Adicionar ao repositório
        _triageRepository.Add(triage);

        // Salvar mudanças
        var saveResult = await _triageRepository.SaveChangesAsync();
        if (!saveResult.IsSuccess)
            return Result.Failure<Triage>("Erro ao salvar a triagem no banco de dados");

        return Result.Success(triage);
    }
}

public static class ValidateTriage
{
    public static Result<Triage> Validate(Triage triage)
    {
        var result = new Result<Triage>();

        if (triage.PatientArrivalId <= 0)
            result.AddMessage("PatientArrivalId é obrigatório e deve ser maior que zero");

        if (string.IsNullOrWhiteSpace(triage.Symptoms))
            result.AddMessage("Sintomas são obrigatórios e não podem estar vazios");

        if (result.Messages.Count == 0)
            return Result.Success(triage);

        return result; 
    }
}