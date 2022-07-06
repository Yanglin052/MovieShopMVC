using ApplicationCore.Entities;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Contracts.Repository
{
    public interface IMovieRepository: IRepository<Movie>
    {
        Task<IEnumerable<Movie>> Get30HighestGrossingMovies();
        Task<IEnumerable<Review>> Get30HighestRatedMovies();
        Task<PagedResultSetModel<Movie>> GetMoviesByGenre(int genreId, int pageSize = 30, int pageNumber = 1);
        Task<PagedResultSetModel<Movie>> GetMovies(int movieId, int pageSize = 30, int pageNumber = 1);
        Task<PagedResultSetModel<Review>> GetReviewsByMovie(int movieId, int pageSize = 30, int pageNumber = 1);
    }
}
