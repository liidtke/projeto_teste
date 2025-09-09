using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using WebApi.Domain.SharedKernel;
namespace WebApi.Domain;

public class Triage
{
    public int Id { get; set; }
    
    public int PatientId { get; set; }
    public Patient Patient { get; set; }
    
    public int PatientArrivalId { get; set; }
    public PatientArrival PatientArrival { get; set; }
    
    public string Symptoms { get; set; }
    public string ArterialPressure { get; set; }
    public decimal Weight { get; set; }
    public decimal Height { get; set; }

    public int KindId { get; set; } = 1;
}

public record Kind : Enumeration
{
    private Kind(int id, string name) : base(id, name)
    {
    }

    public static Kind General = new(1, "Clinico Geral");
    public static Kind Urgency = new(2, "Urgência");
}