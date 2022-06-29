using ApplicationCore.Contracts.Repository;
using ApplicationCore.Contracts.Services;
using ApplicationCore.Entities;
using ApplicationCore.Exceptions;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRepository<Favorite> _favoriteRepository;
        private readonly IRepository<Purchase> _purchaseRepository;
        private readonly IRepository<Review> _reviewRepository;
        private readonly IMovieRepository _movieRepository;
        public UserService(IUserRepository userRepository, IRepository<Favorite> favoriteRepository, 
            IRepository<Purchase> purchaseRepository, IRepository<Review> reviewRepository, IMovieRepository movieRepository)
        {
            _userRepository = userRepository;
            _favoriteRepository = favoriteRepository;
            _purchaseRepository = purchaseRepository;
            _reviewRepository = reviewRepository;
            _movieRepository = movieRepository;
        }


        public async Task<bool> AddFavorite(FavoriteRequestModel favoriteRequest)
        {

            var newFavorite = new Favorite
            {
                Id = favoriteRequest.Id,
                UserId = favoriteRequest.UserId,
                MovieId = favoriteRequest.MovieId
            };

            var addFavorite = await _favoriteRepository.Add(newFavorite);
            if (addFavorite.Id > 1)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> AddMovieReview(ReviewRequestModel reviewRequest)
        {

            var newReview = new Review
            {
                UserId = reviewRequest.UserId,
                MovieId = reviewRequest.MovieId,
                Rating = reviewRequest.Rating,
                ReviewText = reviewRequest.ReviewText

            };

            var addReview = await _reviewRepository.Add(newReview);
            if (addReview.UserId > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteMovieReview(int userId, int movieId)
        {

            var deleteReview = await _reviewRepository.Delete(new Review { UserId = userId, MovieId = movieId});
            if (deleteReview.UserId > 0)
            {
                return false;
            }
            return true;

        }

        public async Task<bool> FavoriteExists(int id, int movieId)
        {
            var favoriteExists = await _userRepository.GetFavoriteById(id, movieId);

            if (favoriteExists != null)
            {
                throw new ConflictException("Favorite already exists.");
            }

            return true;
        }

        public async Task<IEnumerable<MovieCardModel>> GetAllFavoritesForUser(int id)
        {
            List<MovieCardModel> movieModels = new List<MovieCardModel>();

            var movies = await _userRepository.GetAllFavoriteMoviesByUserId(id);

            foreach (var movie in movies)
            {
                movieModels.Add(new MovieCardModel
                {
                    Id = movie.MovieId,
                    Title = movie.Movie.Title,
                    PosterUrl = movie.Movie.PosterUrl
                });
            }

            return movieModels;

        }

        public async Task<IEnumerable<PurchaseRequestModel>> GetAllPurchasesForUser(int id)
        {
            var purchaseRequests = new List<PurchaseRequestModel>();

            var purchases = await _userRepository.GetPurchaseById(id);

            foreach (var purchase in purchases)
            {
                purchaseRequests.Add(new PurchaseRequestModel
                {
                    MovieId = purchase.MovieId,
                    Title = purchase.Movie.Title,
                    PosterUrl = purchase.Movie.PosterUrl,
                    PurchaseDate = purchase.PurchaseDateTime,
                    Price = purchase.TotalPrice,
                    PurchaseNumber = purchase.PurchaseNumber
                });
            }

            return purchaseRequests;
        }

        public async Task<Favorite> GetFavoriteById (int useId, int movieId)
        {
            var favorite = await _userRepository.GetFavoriteById(useId, movieId);
            return favorite;

        }

        public async Task<ReviewRequestModel> GetAllReviewsByUser(int userId, int movieId)
        {
            var review = await _userRepository.GetReviewById(userId, movieId);

            if (review == null)
                return new ReviewRequestModel { Rating = 0, ReviewText = "" };

            var model = new ReviewRequestModel
            {
                MovieId = review.MovieId,
                UserId = review.UserId,
                Rating = review.Rating,
                ReviewText = review.ReviewText,
            };

            return model;
        }

        public async Task<PurchaseModel> GetPurchasesDetails(int id)
        {
            var purchaseDetails = await _purchaseRepository.GetById(id);


            var purchase = new PurchaseModel
            {
                Id = purchaseDetails.Id,
                UserId = purchaseDetails.UserId,
                PurchaseNumber = purchaseDetails.PurchaseNumber,
                PurchaseDateTime = purchaseDetails.PurchaseDateTime,
                TotalPrice = purchaseDetails.TotalPrice,
                MovieId = purchaseDetails.MovieId
            };

            return purchase;
        }

        public async Task<bool> IsMoviePurchased(PurchaseRequestModel purchaseRequest, int userId)
        {
            var isMoviePurchased = await _userRepository.CheckIfMoviePurchasedByUser(purchaseRequest.MovieId, userId);

            if (isMoviePurchased != null)
            {
                throw new ConflictException("Movie is already purchased.");
            }

            return true;
        }

        public async Task<bool> PurchaseMovie(PurchaseRequestModel purchaseRequest, int userId)
        {
            //check user exists
            var user = await _userRepository.GetById(userId);

            if (user == null)
                return false;

            var movie = await _movieRepository.GetById(purchaseRequest.MovieId);

            //create purchase object
            var newPurchase = new Purchase
            {
                MovieId = purchaseRequest.MovieId,
                UserId = userId,
                TotalPrice = (decimal)movie.Price,
                PurchaseDateTime = purchaseRequest.PurchaseDate,
                PurchaseNumber = Guid.NewGuid()
            };

            //save object to purchase repo
            var saved = await _purchaseRepository.Add(newPurchase);

            //returned if saved
            if (saved.Id > 1)
                return true;

            return false;
        }

        public async Task<bool> RemoveFavorite(FavoriteRequestModel favoriteRequest)
        {
            var remFavorite = new Favorite
            {
                Id = favoriteRequest.Id,
                MovieId = favoriteRequest.MovieId,
                UserId = favoriteRequest.UserId
            };

            var removed = await _favoriteRepository.Delete(remFavorite);


            if (removed.Id > 0)
                return false;

            return true;
        }

        public async Task<bool> UpdateMovieReview(ReviewRequestModel reviewRequest)
        {
            var updReview = new Review
            {
                MovieId = reviewRequest.MovieId,
                UserId = reviewRequest.UserId,
                Rating = reviewRequest.Rating,
                ReviewText = reviewRequest.ReviewText
            };

            var saved = await _reviewRepository.Updated(updReview);

            if (saved.User != null)
                return true;

            return false;
        }
    }

}