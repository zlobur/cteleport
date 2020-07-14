using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace App.Services
{
    public class GetInfoService: IGetInfoService
    {
        private readonly HttpClient _httpClient;
        private readonly string _url;
        public GetInfoService(string url)
        {
            this._url = url;
            this._httpClient = new HttpClient() { Timeout = TimeSpan.FromHours(1) };
        }

        public async Task<HttpResponseMessage> GetAsync(string code)
        {
            var msg = new HttpRequestMessage(HttpMethod.Get, $"{_url}{code}");

            var response = await this._httpClient.SendAsync(msg);
            return response;
        }
    }
}