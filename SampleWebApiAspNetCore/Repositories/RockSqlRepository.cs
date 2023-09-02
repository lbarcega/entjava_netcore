using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Helpers;
using SampleWebApiAspNetCore.Models;
using System.Linq.Dynamic.Core;

namespace SampleWebApiAspNetCore.Repositories
{
    public class RockSqlRepository : IRockRepository
    {
        private readonly RockDbContext _rockDbContext;

        public RockSqlRepository(RockDbContext rockDbContext)
        {
            _rockDbContext = rockDbContext;
        }

        public RockEntity GetSingle(int id)
        {
            return _rockDbContext.RockItems.FirstOrDefault(x => x.Id == id);
        }

        public void Add(RockEntity item)
        {
            _rockDbContext.RockItems.Add(item);
        }

        public void Delete(int id)
        {
            RockEntity rockItem = GetSingle(id);
            _rockDbContext.RockItems.Remove(rockItem);
        }

        public RockEntity Update(int id, RockEntity item)
        {
            _rockDbContext.RockItems.Update(item);
            return item;
        }

        public IQueryable<RockEntity> GetAll(QueryParameters queryParameters)
        {
            IQueryable<RockEntity> _allItems = _rockDbContext.RockItems.OrderBy(queryParameters.OrderBy,
              queryParameters.IsDescending());

            if (queryParameters.HasQuery())
            {
                _allItems = _allItems
                    .Where(x => x.Hardness.ToString().Contains(queryParameters.Query.ToLowerInvariant())
                    || x.Name.ToLowerInvariant().Contains(queryParameters.Query.ToLowerInvariant()));
            }

            return _allItems
                .Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);
        }

        public int Count()
        {
            return _rockDbContext.RockItems.Count();
        }

        public bool Save()
        {
            return (_rockDbContext.SaveChanges() >= 0);
        }

        public ICollection<RockEntity> GetRandomMineral()
        {
            List<RockEntity> toReturn = new List<RockEntity>();

            toReturn.Add(GetRandomItem("Metallic"));
            toReturn.Add(GetRandomItem("Adamantine"));
            toReturn.Add(GetRandomItem("Vitreous"));
            toReturn.Add(GetRandomItem("Dull"));
            toReturn.Add(GetRandomItem("Greasy"));
            toReturn.Add(GetRandomItem("Resinous"));
            toReturn.Add(GetRandomItem("Waxy"));
            toReturn.Add(GetRandomItem("Silky"));
            toReturn.Add(GetRandomItem("Pearly"));

            return toReturn;
        }

        private RockEntity GetRandomItem(string luster)
        {
            return _rockDbContext.RockItems
                .Where(x => x.Luster == luster)
                .OrderBy(o => Guid.NewGuid())
                .FirstOrDefault();
        }
    }
}
