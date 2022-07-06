using ApplicationCore.Contracts.Services;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MovieShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpGet]
        [Route("details")]
        public async Task<IActionResult> GetUserDetails(int id)
        {
            var user = await _userService.GetUserDetails(id);
            if (user == null)
            {
                return NotFound(new { errorMessage = $"NO USER FOUND FOR {id}" });
            }
            return Ok(user);
        }

        [HttpPost]
        [Route("purchase-movie")]
        public async Task<IActionResult> PurchaseMovie(PurchaseRequestModel model, int userId)
        {
            var purchase = await _userService.PurchaseMovie(model,userId);

            return Ok(purchase);
        }

        [HttpPost]
        [Route("favorite")]
        public async Task<IActionResult> AddFavorite(FavoriteRequestModel model)
        {
            var favorite = await _userService.AddFavorite(model);

            return Ok(favorite);
        }

        [HttpPost]
        [Route("un-favorite")]
        public async Task<IActionResult> RemoveFavorite(FavoriteRequestModel model)
        {
            var unfavorite = await _userService.RemoveFavorite(model);

            return Ok(unfavorite);
        }



        [HttpGet]
        [Route("purchases")]
        public async Task<IActionResult> GetMoviesPurchsaedByUser()
        {
            // we need to get the userId from the token, using HttpContext
            return Ok();
        }
    }
}
