using ApplicationCore.Contracts.Repository;
using ApplicationCore.Contracts.Services;
using ApplicationCore.Entities;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class AdminService : IAdminService
    {
        private readonly IMovieRepository _movieRepository;

        public AdminService(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public async Task<bool> CreateMovie(MovieCreateRequest createRequest)
        {
            var newMovie = new Movie
            {
                Title = createRequest.Title,
                Overview = createRequest.Overview,
                Tagline = createRequest.Tagline,
                Revenue = createRequest.Revenue,
                Budget = createRequest.Budget,
                ImdbUrl = createRequest.ImdbUrl,
                TmdbUrl = createRequest.TmdbUrl,
                PosterUrl = createRequest.PosterUrl,
                BackdropUrl = createRequest.BackdropUrl,
                OriginalLanguage = createRequest.OriginalLanguage,
                ReleaseDate = createRequest.ReleaseDate,
                RunTime = createRequest.RunTime,
                Price = createRequest.Price
            };


            var createMovie = await _movieRepository.Add(newMovie);

            if (createMovie.Id > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateMovie(MovieCreateRequest createRequest)
        {
            var newMovie = new Movie
            {
                Title = createRequest.Title,
                Overview = createRequest.Overview,
                Tagline = createRequest.Tagline,
                Revenue = createRequest.Revenue,
                Budget = createRequest.Budget,
                ImdbUrl = createRequest.ImdbUrl,
                TmdbUrl = createRequest.TmdbUrl,
                PosterUrl = createRequest.PosterUrl,
                BackdropUrl = createRequest.BackdropUrl,
                OriginalLanguage = createRequest.OriginalLanguage,
                ReleaseDate = createRequest.ReleaseDate,
                RunTime = createRequest.RunTime,
                Price = createRequest.Price
            };


            var updateMovie = await _movieRepository.Updated(newMovie);

            if (updateMovie.Id > 0)
            {
                return true;
            }
            return false;
        }
    }
}
