using Journal.Domain.Models.Abstractions;

namespace Journal.Domain.Factory.Abstractions;

public interface IRepositoryFactory<BaseRepository, ModelT> where ModelT : Entity where BaseRepository : IRepository<ModelT>
{
    BaseRepository CreateRepository(string name);
}