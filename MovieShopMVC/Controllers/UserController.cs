using ApplicationCore.Contracts.Services;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieShopMVC.Services;

namespace MovieShopMVC.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        // all these actionmethods should only be executed when user is logged in

        private readonly ICurrentLogedInUser _currentLogedInUser;
        private readonly IUserService _userService;
        public UserController(ICurrentLogedInUser currentLogedInUser, IUserService userService)
        {
            _currentLogedInUser = currentLogedInUser;
            _userService = userService;
        }


        [HttpGet]
        public async Task<IActionResult> Purchases()
        {
            var userId = _currentLogedInUser.UserId;

            var purchase = await _userService.GetAllPurchasesForUser(userId);
            return View(purchase);
        }

        [HttpGet]
        public async Task<IActionResult> Favorites()
        {
            var userId = _currentLogedInUser.UserId; 

            var favorite = await _userService.GetAllFavoritesForUser(userId);
            return View(favorite);
        }

        [HttpPost]
        
        public async Task<IActionResult> AddReview(ReviewRequestModel model)
        {
            var userId = _currentLogedInUser.UserId;
            var review = await _userService.GetAllReviewsByUser(userId, model.MovieId);

            if (review.UserId == 0)
            {
                var res = await _userService.AddMovieReview(model);
            }
            else
            {
                var res = await _userService.UpdateMovieReview(model);
            }

            return RedirectToAction("Details", "Movies", new { id = model.MovieId });
        }

        [HttpPost]
        public async Task<IActionResult> AddFavorite(int movieId)
        {

            var userId = _currentLogedInUser.UserId;
            var favorited = await _userService.FavoriteExists(userId, movieId);

            var model = new FavoriteRequestModel
            {
                MovieId = movieId,
                UserId = userId
            };

            if (favorited)
            {

                var fav = await _userService.GetFavoriteById(userId, movieId);
                model.Id = fav.Id;
                var res = await _userService.RemoveFavorite(model);
            }
            else
            {
                var res = await _userService.AddFavorite(model);
            }


            return RedirectToAction("Details", "Movies", new { id = movieId });
        }

        [HttpPost]
        public async Task<IActionResult> BuyMovie(PurchaseRequestModel purchaseRequest,int movieId)
        {

            var userId = _currentLogedInUser.UserId;
            var purchased = await _userService.IsMoviePurchased(purchaseRequest,userId);

            if (!purchased)
            {
                var purchase = new PurchaseRequestModel
                {
                    MovieId = movieId,
                    PurchaseDate = DateTime.Now,
                };

                var purchases = await _userService.PurchaseMovie(purchase, userId);
            }

            return RedirectToAction("Details", "Movies", new { id = movieId });
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            return View();
        }
    }
}
