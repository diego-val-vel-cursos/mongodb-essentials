
using Practice.Services.msusers.Services;
using System.Net.Http;
using System.Threading.Tasks;

namespace Practice.Services.msusers.Services
{
    public class MovieClientService
    {
        private readonly IHttpClientFactory _clientFactory;

        public MovieClientService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<string> GetMovieDetailsAsync(string movieId)
        {
            var client = _clientFactory.CreateClient("msmovies");
            var response = await client.GetAsync($"/api/movie/decrease/{movieId}");

            if (response.IsSuccessStatusCode){
                return await response.Content.ReadAsStringAsync();
            }
            else{
                throw new Exception("Unable to fetch movie details.");
            }
        }
    }
}
