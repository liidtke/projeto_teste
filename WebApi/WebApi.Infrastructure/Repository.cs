using WebApi.Domain.SharedKernel;
namespace WebApi.Infrastructure;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly WebApiContext context;

    public Repository(WebApiContext context)
    {
        this.context = context;
    }


    public TEntity Find(int id)
    {
        var obj = context.Set<TEntity>().Find(id);
        return obj;
    }

    public async Task<TEntity> FindAsync(int id)
    {
        return await context.Set<TEntity>().FindAsync(id);
    }

    public IQueryable<TEntity> Query()
    {
        return context.Set<TEntity>();
    }

    public void Remove(TEntity entity)
    {
        context.Remove(entity);
    }

    public void Remove(IEnumerable<TEntity> listEntity)
    {
        foreach (var entity in listEntity)
        {
            context.Remove(entity);
        }
    }

    public void Add(TEntity entity)
    {
        context.Add(entity);
    }

    public async Task<Result<int>> SaveChangesAsync()
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            var ret = await context.SaveChangesAsync();
            await transaction.CommitAsync();

            return new Result<int>()
            {
                Object = ret,
                ResultType = ResultType.Success,
            };
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            // logger.LogWarning(e, "Exception while saving");
            var result = new Result<int>()
            {
                ResultType = ResultType.Failure,
            };
            result.AddMessage("Erro ao salvar");
            return result;
        }
    }

   
}