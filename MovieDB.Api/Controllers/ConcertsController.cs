using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieDB.Api.Helpers;
using MovieDB.Api.Models.Concerts;
using MovieDB.Api.Services;
using CreateRequest = MovieDB.Api.Models.Concerts.CreateRequest;
using UpdateRequest = MovieDB.Api.Models.Concerts.UpdateRequest;

namespace MovieDB.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ConcertsController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IConcertService _concertService;

        public ConcertsController(IMapper mapper, IConcertService concertService)
        {
            _mapper = mapper;
            _concertService = concertService;
        }

        [HttpGet]
        public ActionResult<List<ConcertResponse>> GetAll()
        {
            var concerts = _concertService.GetAllAsync(Account!);
            return _mapper.Map<List<ConcertResponse>>(concerts);
        }

        [HttpGet("{id:int}", Name = "GetConcert")]
        public async Task<ActionResult<ConcertResponse>> GetById(int id)
        {
            var concert = await _concertService.GetByIdAsync(id, Account!);
            return _mapper.Map<ConcertResponse>(concert);
        }

        [HttpPost]
        public async Task<ActionResult<ConcertResponse>> Create(CreateRequest model)
        {
            var concert = await _concertService.CreateAsync(model, Account!);
            var response = _mapper.Map<ConcertResponse>(concert);
            return CreatedAtAction(nameof(GetById), new { id = concert.Id }, response);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ConcertResponse>> Update(int id, UpdateRequest model)
        {
            var concert = await _concertService.UpdateAsync(id, model, Account!);
            var response = _mapper.Map<ConcertResponse>(concert);
            return CreatedAtAction(nameof(GetById), new { id = concert.Id }, response);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _concertService.DeleteByIdAsync(id, Account!);
            return NoContent();
        }
    }
}
