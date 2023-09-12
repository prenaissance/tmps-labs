namespace Journal.Domain.Models.Abstractions;

public interface IRepository<T> where T : Entity
{
    Task<T?> GetById(Guid id);
    Task<IList<T>> GetAll();
    Task<T> Add(T entity);
    Task<T> Update(T entity);
    Task<T> Delete(Guid id);
}