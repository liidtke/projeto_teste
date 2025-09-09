namespace WebApi.Domain.SharedKernel;

public static class Result
{
    public static Result<T> Success<T>(T value)
    {
        return new Result<T>()
        {
            ResultType = ResultType.Success,
            Object = value,
        };
    }

    public static Result<T> Failure<T>(string message)
    {
        return new Result<T>()
        {
            ResultType = ResultType.Failure,
            Messages = new List<ResultMessage>
            {
                new ResultMessage()
                {
                    Message = message,
                }
            }
        };
    }

    public static Result<T> Validation<T>(string message)
    {
        return new Result<T>()
        {
            ResultType = ResultType.Validation,
            Object = default(T),
            Messages = new List<ResultMessage>
            {
                new ResultMessage()
                {
                    Message = message,
                }
            }
        };
    }
    
    
    public static Result<T> NotFound<T>()
    {
        return new Result<T>()
        {
            ResultType = ResultType.NotFound,
            Object = default(T),
        };
    }

    public static Result<T> Validation<T>(string message, string details)
    {
        return new Result<T>()
        {
            ResultType = ResultType.Validation,
            Object = default(T),
            Messages = new List<ResultMessage>
            {
                new ResultMessage()
                {
                    Message = message,
                    Detail = details,
                }
            }
        };
    }
}

public record Result<T>
{
    public Result()
    {
    }

    public Result(List<ResultMessage> messages, ResultType resultType)
    {
        Messages = messages;
        ResultType = resultType;
    }

    public Result(string message)
    {
        Messages = new List<ResultMessage>() { new ResultMessage() { Message = message } };
        ResultType = ResultType.Validation;
    }

    public Result(T result)
    {
        ResultType = ResultType.Success;
        Object = result;
    }

    public ResultType ResultType { get; set; }
    public T? Object { get; set; }
    public List<ResultMessage> Messages { get; set; } = new List<ResultMessage>();
    public bool IsSuccess => ResultType == ResultType.Success;
    public string Message => string.Join(' ', Messages.Select(p => p.Message));

    public void AddMessage(string message)
    {
        this.ResultType = ResultType.Validation;
        this.Messages.Add(new ResultMessage() { Message = message });
    }

    public void AddMessage(List<ResultMessage> messages)
    {
        this.ResultType = ResultType.Validation;
        this.Messages = messages;
    }

    public override string ToString()
    {
        if (this.Messages == null)
        {
            return string.Empty;
        }
        else
        {
            return string.Join("#", this.Messages.Select(x => x.Message));
        }
    }
}

public record ResultMessage
{
    public string Message { get; set; } = "";
    public string? Detail { get; set; }
}

public enum ResultType
{
    Success = 1,
    Validation,
    Failure,
    NotFound,
}