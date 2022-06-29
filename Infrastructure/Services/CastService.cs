using ApplicationCore.Contracts.Repository;
using ApplicationCore.Contracts.Services;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class CastService : ICastService
    {
        private readonly ICastRepository _castRepository;

        public CastService(ICastRepository castRepository)
        {
            _castRepository = castRepository;
        }
        public async Task<CastDetailsModel> GetCastDetails(int id)
        {
            var castDetails = await _castRepository.GetById(id);

            var cast = new CastDetailsModel
            {
                Id = castDetails.Id,
                Name = castDetails.Name,
                Gender = castDetails.Gender,
                TmdbUrl = castDetails.TmdbUrl,
                ProfilePath = castDetails.ProfilePath,
            };

            foreach (var movie in castDetails.MoviesOfCast)
            {
                cast.Movies.Add(new MovieDetailsModel { Id = movie.CastId, Title = movie.Movie.Title, PosterUrl = movie.Movie.PosterUrl});
            }

            foreach (var movie in castDetails.MoviesOfCast)
            {
                cast.Casts.Add(new CastModel { Id = movie.CastId, Name = movie.Cast.Name, Character = cast.Character});
            }

            return cast;
        }
    }
}
