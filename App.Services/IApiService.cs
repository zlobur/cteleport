using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using App.Models;

namespace App.Services
{
    public interface IApiService
    {
        Task<Response> MeasureDistance(Request request);
    }
}
