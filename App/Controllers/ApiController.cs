using System.Threading.Tasks;
using App.Services;
using Microsoft.AspNetCore.Mvc;
using App.Models;
using Microsoft.Extensions.Caching.Memory;

namespace App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        public ApiController(IApiService apiService, IMemoryCache cache)
        {
            _apiService = apiService;
            _cache = cache;
        }
        IApiService _apiService { get; set; }
        IMemoryCache _cache { get; set; }

        [HttpPost]
        public async Task<ActionResult<Response>> Post([FromBody]Request request)
        {
            if(request == null || request.FirstAirportCode == null || request.SecondAirportCode == null)
            {
                return BadRequest("bad request parameter");
            }
            Response response = null;
            if (!_cache.TryGetValue($"{request.FirstAirportCode}{request.SecondAirportCode}", out response))
            {
                response = await _apiService.MeasureDistance(request);
                _cache.Set($"{request.FirstAirportCode}{request.SecondAirportCode}", response);
            }
            return Ok(response);
        }
    }
}