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
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(MovieShopDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> CheckIfMoviePurchasedByUser(int movieId, int userId)
        {
            var purchase = await _dbContext.Purchases
                .Where(r => r.MovieId == movieId && r.UserId == userId)
                .FirstOrDefaultAsync();

            return purchase != null;

        }

        public async Task<Favorite> GetFavoriteById(int userId, int movieId)
        {
            var favorite = await _dbContext.Favorites
                .FirstOrDefaultAsync(r => r.MovieId == movieId && r.UserId == userId);

            return favorite;
        }


        public async Task<IEnumerable<Favorite>> GetAllFavoriteMoviesByUserId(int userId)
        {
            var movies = await _dbContext.Favorites
                .Where(x => x.UserId == userId)
                .Include(x => x.Movie)
                .OrderByDescending(x => x.Movie.Revenue)
                .ToListAsync();

            return movies;
        }

        public async Task<IEnumerable<Purchase>> GetPurchaseById(int Id)
        {
            var purchase = await _dbContext.Purchases
                .Where(r => r.UserId == Id)
                .Include(r => r.Movie)
                .OrderByDescending(r => r.Movie.Revenue)
                .ToListAsync();

            return purchase;

        }

        public async Task<Review> GetReviewById(int userId, int movieId)
        {
            var review = await _dbContext.Reviews
                .Where(r => r.MovieId == movieId && r.UserId == userId)
                .FirstOrDefaultAsync();

            return review;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

            return user;
        }
    }
}
