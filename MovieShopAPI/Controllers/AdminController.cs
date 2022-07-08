using ApplicationCore.Contracts.Services;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MovieShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }


        [HttpPost]
        [Route("create-movie")]
        public async Task<IActionResult> CreateMovie([FromBody] MovieCreateRequest createRequest)
        {
            var createMovie = await _adminService.CreateMovie(createRequest);

            return Ok(createMovie);
        }


        [HttpPut]
        [Route("update-movie")]
        public async Task<IActionResult> UpdateMovie([FromBody] MovieCreateRequest createRequest)
        {
            var updateMovie = await _adminService.UpdateMovie(createRequest);

            return Ok(updateMovie);
        }
    }
}
