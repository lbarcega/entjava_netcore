using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Models;

namespace SampleWebApiAspNetCore.Repositories
{
    public interface IRockRepository
    {
        RockEntity GetSingle(int id);
        void Add(RockEntity item);
        void Delete(int id);
        RockEntity Update(int id, RockEntity item);
        IQueryable<RockEntity> GetAll(QueryParameters queryParameters);
        ICollection<RockEntity> GetRandomMineral();
        int Count();
        bool Save();
    }
}
