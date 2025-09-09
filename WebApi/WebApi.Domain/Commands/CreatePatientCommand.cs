using WebApi.Domain.SharedKernel;

namespace WebApi.Domain.Commands;

public class CreatePatientCommand
{
    private readonly IRepository<Patient> _patientRepository;

    public CreatePatientCommand(IRepository<Patient> patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public async Task<Result<Patient>> InsertPatient(Patient patient)
    {
        if (patient == null)
            return Result.Validation<Patient>("Patient não pode ser nulo");

        var validationResult = ValidatePatient.Validate(patient);
        if (!validationResult.IsSuccess)
            return validationResult;

        _patientRepository.Add(patient);
        
        var saveResult = await _patientRepository.SaveChangesAsync();
        if (!saveResult.IsSuccess)
            return Result.Failure<Patient>("Erro ao salvar");

        return Result.Success(patient);
    }

}