namespace WebApi.Domain.SharedKernel;

public interface IRepository<TEntity> where TEntity : class
{
    TEntity Find(int id);
    Task<TEntity> FindAsync(int id);
    IQueryable<TEntity> Query();
    void Remove(TEntity entity);
    void Remove(IEnumerable<TEntity> listEntity);
    void Add(TEntity entity);
    Task<Result<int>> SaveChangesAsync();
}