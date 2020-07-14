using App.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace App.Services
{
    public class ApiService : IApiService
    {
        const double PIx = 3.141592653589793;
        const double RADIUS = 3949.90;

        private IGetInfoService _getInfoService;

        public ApiService(IGetInfoService getInfoService)
        {
            _getInfoService = getInfoService;
        }
        public async Task<Response> MeasureDistance(Request request)
        {

            var responseFirstAirport = await _getInfoService.GetAsync(request.FirstAirportCode);
            var responseSecondAirport = await _getInfoService.GetAsync(request.SecondAirportCode);

            if (responseFirstAirport == null || responseSecondAirport == null)
            {
                throw new HttpRequestException();
            }
            if (!responseFirstAirport.IsSuccessStatusCode)
            {
                throw new HttpRequestException(responseFirstAirport.StatusCode.ToString());
            }

            if (!responseSecondAirport.IsSuccessStatusCode)
            {
                throw new HttpRequestException(responseSecondAirport.StatusCode.ToString());
            }

            var firstStrResponse = await responseFirstAirport?.Content?.ReadAsStringAsync();
            AirportDetail firstAirportDetail = JsonConvert.DeserializeObject<AirportDetail>(firstStrResponse);

            var secondStrResponse = await responseSecondAirport?.Content?.ReadAsStringAsync();
            AirportDetail secondAirportDetail = JsonConvert.DeserializeObject<AirportDetail>(secondStrResponse);

            if (firstAirportDetail == null || secondAirportDetail == null)
            {
                throw new HttpRequestException();
            }

            return Measure(firstAirportDetail, secondAirportDetail);
        }
        private double Radians(double x)
        {
            return x * PIx / 180;
        }

        private Response Measure(AirportDetail first, AirportDetail second)
        {
            double dlon = Radians(second.location.lon - first.location.lon);
            double dlat = Radians(second.location.lat - first.location.lat);

            double a = (Math.Sin(dlat / 2) * Math.Sin(dlat / 2)) + Math.Cos(Radians(first.location.lat)) * Math.Cos(Radians(second.location.lat)) * (Math.Sin(dlon / 2) * Math.Sin(dlon / 2));
            double angle = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var result = angle * RADIUS;

            return new Response { Miles = result.ToString() };
        }
    }
}