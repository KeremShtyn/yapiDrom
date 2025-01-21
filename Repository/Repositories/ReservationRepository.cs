using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Models;
using Persistence.Abstracts;

namespace Persistence.Repositories
{
    public class ReservationRepository : RepositoryBase<Reservation, YapidromDbContext>, IReservationRepository
    {
        public ReservationRepository(YapidromDbContext context) : base(context)
        {
        }
    }
}
