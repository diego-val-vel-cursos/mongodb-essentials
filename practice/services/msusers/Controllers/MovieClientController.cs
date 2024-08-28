using Microsoft.AspNetCore.Mvc;
using Practice.Services.msusers.Controllers;
using Practice.Services.msusers.Services;

namespace Practice.Services.msusers.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieClientController : ControllerBase
    {
        private readonly MovieClientService _movieClientService;

        public MovieClientController(MovieClientService movieClientService)
        {
            _movieClientService = movieClientService;
        }

        [HttpGet("movie-details/{movieId}")]
        public async Task<IActionResult> GetMovieDetails(string movieId)
        {
            var movieDetails = await _movieClientService.GetMovieDetailsAsync(movieId);
            return Ok(movieDetails);
        }

    }
}
