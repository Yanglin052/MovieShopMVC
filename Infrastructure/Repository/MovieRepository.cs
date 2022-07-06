using ApplicationCore.Contracts.Repository;
using ApplicationCore.Entities;
using ApplicationCore.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class MovieRepository : Repository<Movie>, IMovieRepository
    {
        public MovieRepository(MovieShopDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<IEnumerable<Movie>> Get30HighestGrossingMovies()
        {
            var movies = await _dbContext.Movies.OrderByDescending(m => m.Revenue).Take(30).ToListAsync();
            return movies;
        }

        public async Task<IEnumerable<Review>> Get30HighestRatedMovies()
        {
            var movies = await _dbContext.Reviews
                .OrderByDescending(m => m.Rating)
                .Take(30)
                .ToListAsync();     

            return movies;
        }

        public async Task<PagedResultSetModel<Movie>> GetMoviesByGenre(int genreId, int pageSize = 30, int pageNumber = 1)
        {
            // get total count movies for the genre
            var totalMoviesForGenre = await _dbContext.MovieGenres.Where(g => g.GenreId == genreId).CountAsync();

            var movies = await _dbContext.MovieGenres
                .Where(g => g.GenreId == genreId)
                .Include(g => g.Movie)
                .OrderByDescending(m => m.Movie.Revenue)
                .Select(m => new Movie { Id = m.MovieId, PosterUrl = m.Movie.PosterUrl, Title = m.Movie.Title })
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).ToListAsync();

            var pagedMovies = new PagedResultSetModel<Movie>(pageNumber, totalMoviesForGenre, pageSize, movies);
            return pagedMovies;
        }

        public async Task<PagedResultSetModel<Movie>> GetMovies(int movieId, int pageSize = 30, int pageNumber = 1)
        {
            // get total count movies 
            var totalMovies = await _dbContext.Movies.Where(g => g.Id == movieId).CountAsync();

            var movies = await _dbContext.Movies
                .Select(m => new Movie { Id = m.Id, PosterUrl = m.PosterUrl, Title = m.Title })
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).ToListAsync();

            var pagedMovies = new PagedResultSetModel<Movie>(pageNumber, totalMovies, pageSize, movies);
            return pagedMovies;
        }

        public async Task<PagedResultSetModel<Review>> GetReviewsByMovie(int movieId, int pageSize = 30, int pageNumber = 1)
        {
            // get total count reviews 
            var totalReviewsForMovie = await _dbContext.Reviews.Where(g => g.MovieId == movieId).CountAsync();

            var reviews = await _dbContext.Reviews
                .Where(g => g.MovieId == movieId)
                .Include(g => g.Movie)
                .Select(m => new Review { MovieId = m.MovieId, UserId = m.UserId, Rating = m.Rating, ReviewText = m.ReviewText})
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).ToListAsync();

            var pagedReviews = new PagedResultSetModel<Review>(pageNumber, totalReviewsForMovie, pageSize, reviews);
            return pagedReviews;
        }

        public async override Task<Movie> GetById(int id)
        {
            var movieDetails = await _dbContext.Movies
                .Include(m => m.GenresOfMovie).ThenInclude(m => m.Genre)
                .Include(m => m.Trailers)
                .Include(m => m.CastsOfMovie).ThenInclude(m => m.Cast)
                .Include(m => m.ReviewsOfMovie)
                .FirstOrDefaultAsync(m => m.Id == id);

            return movieDetails;
        }
    }
}
