using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieDB.Api.Entities;
using MovieDB.Api.Helpers;
using MovieDB.Api.Models.Movies;
using MovieDB.Api.Services;

namespace MovieDB.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : BaseController
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMapper mapper, IMovieService movieService)
        {
            _movieService = movieService;
        }

        [Authorize]
        [HttpGet("{id:int}", Name = "GetMovie")]
        public ActionResult<Movie> Get(int id)
        {
            return new Movie();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateRequest model)
        {
            var movie = await _movieService.UpdateAsync(model, Account!);
            return CreatedAtRoute("GetMovie", new { id = movie.Id }, movie);
        }
    }
}