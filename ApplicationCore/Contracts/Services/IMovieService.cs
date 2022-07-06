using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Contracts.Services
{
    public interface IMovieService
    {
        Task<List<MovieCardModel>> GetTopGrossingMovies();
        Task<MovieDetailsModel> GetMovieDetails(int id);
        Task<PagedResultSetModel<MovieCardModel>> GetMoviesByGenre (int genreId, int pageSize = 30, int pageNumber = 1);
        Task<PagedResultSetModel<MovieCardModel>> GetMovies(int movieId, int pageSize = 30, int pageNumber = 1);
    }
}
