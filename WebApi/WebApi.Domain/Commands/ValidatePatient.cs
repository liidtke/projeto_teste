using WebApi.Domain.SharedKernel;
namespace WebApi.Domain.Commands;

public static class ValidatePatient
{
    public static Result<Patient> Validate(Patient patient)
    {
        var result = new Result<Patient>();

        if (string.IsNullOrWhiteSpace(patient.Name))
            result.AddMessage("Nome é obrigatório e não pode estar vazio");

        if (string.IsNullOrWhiteSpace(patient.Phone))
            result.AddMessage("Telefone é obrigatório e não pode estar vazio");

        if (string.IsNullOrWhiteSpace(patient.Email))
            result.AddMessage("Email é obrigatório e não pode estar vazio");

        if (!Enum.IsDefined(typeof(Gender), patient.Gender))
            result.AddMessage("Gênero deve ser válido (Male ou Female)");

        if (!string.IsNullOrWhiteSpace(patient.Email) && !IsValidEmail(patient.Email))
            result.AddMessage("Formato de email inválido");

        if (!string.IsNullOrWhiteSpace(patient.Name) && patient.Name.Length > 200)
            result.AddMessage("Nome não pode ter mais de 200 caracteres");

        if (!string.IsNullOrWhiteSpace(patient.Phone) && patient.Phone.Length > 50)
            result.AddMessage("Telefone não pode ter mais de 50 caracteres");

        if (!string.IsNullOrWhiteSpace(patient.Email) && patient.Email.Length > 100)
            result.AddMessage("Email não pode ter mais de 100 caracteres");

        if (result.Messages.Count == 0)
            return Result.Success(patient);

        return result; 
    }
    
    
    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}