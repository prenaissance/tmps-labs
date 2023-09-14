using Journal.Domain.Factory.Abstractions;
using Journal.Domain.Models.Abstractions;

namespace Journal.Domain.Factory;

public class RepositoryFactory<BaseRepository, ModelT> : IRepositoryFactory<BaseRepository, ModelT>
    where ModelT : Entity where BaseRepository : IRepository<ModelT>
{
    private readonly IDictionary<string, Func<BaseRepository>> _repositoryThunks;
    public RepositoryFactory()
    {
        _repositoryThunks = new Dictionary<string, Func<BaseRepository>>();
    }
    public void Register(string name, Func<BaseRepository> repositoryThunk)
    {
        _repositoryThunks.Add(name, repositoryThunk);
    }
    public BaseRepository CreateRepository(string name)
    {
        if (!_repositoryThunks.ContainsKey(name))
        {
            throw new KeyNotFoundException($"Repository '{name}' not registered in {nameof(RepositoryFactory<BaseRepository, ModelT>)}");
        }
        return _repositoryThunks[name]();
    }
}