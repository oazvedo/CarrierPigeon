using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BirdMessage.Application.Dto;
using BirdMessage.Application.Externals.Interfaces;

namespace BirdMessage.Application.Externals
{
    public class GeocodingService : IGeocodingService
    {
        public Task<decimal> CalculateDistanceAsync(CoordinatesDto origin, CoordinatesDto destination, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<CoordinatesDto> GetCoordinatesAsync(string cep, string? street = null, string? city = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}