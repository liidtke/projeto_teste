using Microsoft.AspNetCore.Mvc;
using WebApi.Domain;
using WebApi.Domain.SharedKernel;
namespace WebApi.Controllers;

public static class Output
{
    public static IActionResult FromResult<T,TReturn>(Result<T> result, Func<T, TReturn> func)
    {
        switch (result.ResultType)
        {
            case ResultType.Success:
                return new OkObjectResult(func.Invoke(result.Object));
                break;
            case ResultType.Validation:
                return new BadRequestObjectResult(result.Messages);
            case ResultType.Failure:
                return new ConflictObjectResult(result.Messages);
            case ResultType.NotFound:
                return new NotFoundObjectResult(result.Messages);
            default:
                return new BadRequestObjectResult(result.Messages);
        }
    }
    
      public static IActionResult FromResult<T>(Result<T> result)
        {
            switch (result.ResultType)
            {
                case ResultType.Success:
                    return new OkObjectResult(result.Object);
                    break;
                case ResultType.Validation:
                    return new BadRequestObjectResult(result.Messages);
                case ResultType.Failure:
                    return new ConflictObjectResult(result.Messages);
                case ResultType.NotFound:
                    return new NotFoundObjectResult(result.Messages);
                default:
                    return new BadRequestObjectResult(result.Messages);
            }
        }
}