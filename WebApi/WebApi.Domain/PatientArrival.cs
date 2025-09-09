namespace WebApi.Domain;

public class PatientArrival
{
    public int Id { get; set; }
    public int SequentialNumber { get; set; }

    public int PatientId { get; set; }
    public Patient Patient { get; set; }

    public DateTime Arrival { get; set; }
    public ArrivalStatus Status { get; set; }
}

public enum ArrivalStatus
{
    Pending,
    Completed,
}