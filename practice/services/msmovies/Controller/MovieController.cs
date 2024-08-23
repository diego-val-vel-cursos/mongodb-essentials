using Microsoft.AspNetCore.Mvc;
using Practice.Services.msmovies.Models;
using Practice.Services.msmovies.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Practice.Services.msmovies.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly MovieService _movieService;

        public MovieController(MovieService movieService)
        {
            _movieService = movieService;
        }

        // GET: api/Movie
        [HttpGet]
        public async Task<ActionResult<List<Movie>>> GetMovies()
        {
            var movies = await _movieService.GetAsync();
            return Ok(movies);
        }

        // GET: api/Movie/:id
        [HttpGet("{id:length(24)}", Name = "GetMovieById")]
        public async Task<ActionResult<Movie>> GetMovieById(string id)
        {
            var movie = await _movieService.GetAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            return Ok(movie);
        }

        // POST: api/Movie
        [HttpPost]
        public async Task<ActionResult<Movie>> CreateMovie(Movie movie)
        {
            await _movieService.CreateAsync(movie);
            return CreatedAtRoute("GetMovieById", new { id = movie.Id.ToString() }, movie);
        }

        // PUT: api/Movie/:id
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> UpdateMovie(string id, Movie updatedMovie)
        {
            var movie = await _movieService.GetAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            updatedMovie.Id = movie.Id;
            await _movieService.UpdateAsync(id, updatedMovie);

            return NoContent();
        }

        // DELETE: api/Movie/:id
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> DeleteMovie(string id)
        {
            var movie = await _movieService.GetAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            await _movieService.RemoveAsync(id);

            return NoContent();
        }
    }
}
