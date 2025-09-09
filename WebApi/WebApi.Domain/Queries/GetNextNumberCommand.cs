using WebApi.Domain.SharedKernel;
namespace WebApi.Domain.Queries;

public class GetNextNumberQuery(IIncrementalIdCacheService cacheService, PatientArrivalsQuery query)
{
    public async Task<Result<List<PatientArrival>>> Handle()
    {
        var number = cacheService.GetCurrentValue("Arrival");

        var currentNumber = cacheService.NextNumber("Queue");

        if (number != null && number != currentNumber)
        {
            currentNumber = number.Value;
            cacheService.Update("Queue", currentNumber);
        }

        var result = await query.GetPatientArrival(new PatientArrivalsQueryParams()
        {
            SequentialNumber = currentNumber,
        });

        return result;
    }
}