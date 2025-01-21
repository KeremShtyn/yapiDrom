using Entity.Models;
using Persistence.Abstracts;

namespace Persistence.Repositories
{
    public class ScooterRepository: RepositoryBase<Scooter, YapidromDbContext>, IScooterRepository
    {
        public ScooterRepository(YapidromDbContext context) : base(context)
        {
        }
    }
}
