using ApplicationCore.Contracts.Repository;
using ApplicationCore.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class CastRepository : Repository<Cast>, ICastRepository
    {
        public CastRepository(MovieShopDbContext dbContext) : base(dbContext)
        {

        }

        public override Cast GeyById(int id)
        {
            var castDetails = _dbContext.Casts
                .Include(m => m.MoviesOfCast).ThenInclude(m => m.Movie)
                .FirstOrDefault(m => m.Id == id);

            return castDetails;
        }
    }
}
