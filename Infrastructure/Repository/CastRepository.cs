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

        public async override Task<Cast> GetById(int id)
        {
            var castDetails = await _dbContext.Casts
                .Include(m => m.MoviesOfCast).ThenInclude(m => m.Movie)
                .FirstOrDefaultAsync(m => m.Id == id);

            return castDetails;
        }
    }
}
