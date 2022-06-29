using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Contracts.Repository
{
    public interface IUserRepository: IRepository<User>
    {
        Task<User> GetUserByEmail(string email);
        Task<Favorite> GetFavoriteById(int userId, int movieId);
        Task<Review> GetReviewById(int userId, int movieId);
        Task<IEnumerable<Purchase>> GetPurchaseById(int id);
        Task<bool> CheckIfMoviePurchasedByUser(int movieId, int userId);
        Task<IEnumerable<Favorite>> GetAllFavoriteMoviesByUserId(int userId);
    }
}
