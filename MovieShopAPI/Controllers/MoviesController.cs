using ApplicationCore.Contracts.Repository;
using ApplicationCore.Contracts.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MovieShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;
        private readonly IMovieRepository _movieRepository;

        public MoviesController(IMovieService movieService, IMovieRepository movieRepository)
        {
            _movieService = movieService;
            _movieRepository = movieRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetMovies(int id)
        {
            var movie = await _movieService.GetMovies(id);

            return Ok(movie);
        }


        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetMovie(int id)
        {
            var movie = await _movieService.GetMovieDetails(id);
            if (movie == null)
            {
                return NotFound(new { errorMessage = $"NO MOVIES FOUND FOR {id}" });
            }
            return Ok(movie);
        }

        [HttpGet]
        [Route("top-rated")]
        public async Task<IActionResult> GetTopRatedMovies()
        {
            var movies = await _movieRepository.Get30HighestRatedMovies();

            if (movies == null || !movies.Any())
            {
                return NotFound(new { errorMessage = "NO MOVIES FOUND" });
            }
            return Ok(movies);
        }


        [HttpGet]
        [Route("top-grossing")]
        //attribute routing
        public async Task<IActionResult> GetTopGrossingMovies()
        {
            var movies = await _movieService.GetTopGrossingMovies();

            if (movies == null || !movies.Any())
            {
                return NotFound(new { errorMessage = "NO MOVIES FOUND" });
            }
            return Ok(movies);
        }


        [HttpGet]
        [Route("genre/{genreId:int}")]
        public async Task<IActionResult> GetMoviesByGenre(int genreId)
        {
            var movie = await _movieService.GetMoviesByGenre(genreId);

            return Ok(movie);
        }

        [HttpGet]
        [Route("{id:int}/reviews")]
        public async Task<IActionResult> GetMovieReviews(int movieId)
        {
            var review = await _movieRepository.GetReviewsByMovie(movieId);

            return Ok(review);
        }
    }
}
