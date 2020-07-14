using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace App.Services
{
    public interface IGetInfoService
    {
        Task<HttpResponseMessage> GetAsync(string code);
    }
}
