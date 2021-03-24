using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieDB.Api.Helpers;
using MovieDB.Api.Models.Movies;
using MovieDB.Api.Services;

namespace MovieDB.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IMovieService _movieService;

        public MoviesController(IMapper mapper, IMovieService movieService)
        {
            _mapper = mapper;
            _movieService = movieService;
        }

        [Authorize]
        [HttpGet("{id:int}", Name = "GetMovie")]
        public async Task<ActionResult<MovieResponse>> GetById(int id)
        {
            var movie = await _movieService.GetByIdAsync(id);
            return _mapper.Map<MovieResponse>(movie);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<MovieResponse>> Create(CreateRequest model)
        {
            var movie = await _movieService.CreateAsync(model, Account!);
            return CreatedAtAction(nameof(GetById), new { id = movie.Id }, movie);
        }

        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<MovieResponse>> Update(int id, UpdateRequest model)
        {
            var movie = await _movieService.UpdateAsync(id, model);
            return CreatedAtAction(nameof(GetById), new { id = movie.Id }, movie);
        }
    }
}