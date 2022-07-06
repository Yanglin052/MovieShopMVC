using ApplicationCore.Entities;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Contracts.Services
{
    public interface IUserService
    {
        Task<UserModel> GetUserDetails(int id);

        Task<bool> IsMoviePurchased(PurchaseRequestModel purchaseRequest, int userId);
        Task<bool> PurchaseMovie(PurchaseRequestModel purchaseRequest, int userId);
        Task<IEnumerable<PurchaseRequestModel>> GetAllPurchasesForUser(int id);
        Task<PurchaseModel> GetPurchasesDetails(int Id);

        Task<bool> FavoriteExists(int id, int movieId);
        Task<bool> AddFavorite(FavoriteRequestModel favoriteRequest);
        Task<bool> RemoveFavorite(FavoriteRequestModel favoriteRequest);
        Task<IEnumerable<MovieCardModel>> GetAllFavoritesForUser(int id);
        Task<Favorite> GetFavoriteById(int useId, int movieId);

        Task<bool> AddMovieReview(ReviewRequestModel reviewRequest);
        Task<bool> UpdateMovieReview(ReviewRequestModel reviewRequest);
        Task<bool> DeleteMovieReview(int userId, int movieId);
        Task<ReviewRequestModel> GetAllReviewsByUser(int userId, int movieId);
    }
}
