
using BirdMessage.Application.Dto;

namespace BirdMessage.Application.Externals.Interfaces
{
    public interface ICepService
    {
        Task <CepServiceDto> GetCepInfosAsync (string cep);

    }
}