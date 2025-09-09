using WebApi.Domain.SharedKernel;

namespace WebApi.Domain.Commands;

public class UpdatePatientCommand
{
    private readonly IRepository<Patient> _patientRepository;

    public UpdatePatientCommand(IRepository<Patient> patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public async Task<Result<Patient>> UpdatePatient(int id, Patient patient)
    {
        if (patient == null)
            return Result.Validation<Patient>("Patient não pode ser nulo");

        var existingPatient = await _patientRepository.FindAsync(id);
        if (existingPatient == null)
            return Result.NotFound<Patient>();

        var validationResult = ValidatePatient.Validate(patient);
        if (!validationResult.IsSuccess)
            return validationResult;

        existingPatient.Name = patient.Name;
        existingPatient.Phone = patient.Phone;
        existingPatient.Email = patient.Email;
        existingPatient.Gender = patient.Gender;

        var saveResult = await _patientRepository.SaveChangesAsync();
        if (!saveResult.IsSuccess)
            return Result.Failure<Patient>("Erro ao atualizar o patient no banco de dados");

        return Result.Success(existingPatient);
    }


}