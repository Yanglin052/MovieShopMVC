using ApplicationCore.Contracts.Services;
using ApplicationCore.Entities;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MovieShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenreService _genreService;

        public GenresController(IGenreService genreService)
        {
            _genreService = genreService;
        }


        [HttpGet]
        public async Task<IActionResult> GetGenre()
        {
            var genre = await _genreService.GetAllGenres();

            return Ok(genre);
        }


        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddGenre([FromBody] GenreModel genre)
        {
            var addGenre = await _genreService.AddGenre(genre);

            return Ok(addGenre);
        }


        [HttpPost]
        [Route("delete/{id:int}")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            var deleteGenre = await _genreService.DeleteGenre(id);

            return Ok(deleteGenre);
        }
    }
}
