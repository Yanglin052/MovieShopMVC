using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MovieShopMVC.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        // all these actionmethods should only be executed when user is logged in

        [HttpGet]
        public async Task<IActionResult> Purchases()
        {
            // go to database and get all movies purchased by user, user id
            // var cookie = this.HttpContext.Request.Cookies["MovieShopAuthCookie"];

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Favorites()
        {
            return View();
        }

        [HttpPost]
        
        public async Task<IActionResult> AddReview()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddFavorite()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BuyMovie()
        {
            return View();
        }
    }
}
