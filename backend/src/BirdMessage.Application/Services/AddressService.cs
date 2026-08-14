using BirdMessage.Application.Dto;
using BirdMessage.Application.Externals.Interfaces;
using BirdMessage.Application.Services.Interface;
using BirdMessage.Domain.Common;
using BirdMessage.Domain.Entities;
using BirdMessage.Domain.Interfaces;

namespace BirdMessage.Application.Services
{
    public class AddressService(IAddressRepository addressRepository, ICepService cepService) : IAddressService
    {
        public Task<Address?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => addressRepository.GetByIdAsync(id, cancellationToken);

        public Task<PaginatedResult<Address>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
            => addressRepository.GetPagedAsync(page, pageSize, cancellationToken);

        public async Task<Address> CreateAsync(Address address, CancellationToken cancellationToken = default)
        {
            var cepInfo = await cepService.GetCepInfosAsync(address.Cep);

            address.Cep = cepInfo.Cep;
            address.Street = cepInfo.Logradouro;
            address.Neighborhood = cepInfo.Bairro;
            address.Local = cepInfo.Localidade;
            address.Uf = cepInfo.Uf;
            address.State = cepInfo.Estado;
            address.Region = cepInfo.Regiao;
            address.DDD = cepInfo.Ddd;

            await addressRepository.AddAsync(address, cancellationToken);
            await addressRepository.SaveChangesAsync(cancellationToken);
            return address;
        }

        public async Task UpdateAsync(Address address, CancellationToken cancellationToken = default)
        {
            if (!string.IsNullOrWhiteSpace(address.Cep))
            {
                var cepInfo = await cepService.GetCepInfosAsync(address.Cep);

                address.Cep = cepInfo.Cep;
                address.Street = cepInfo.Logradouro;
                address.Neighborhood = cepInfo.Bairro;
                address.Local = cepInfo.Localidade;
                address.Uf = cepInfo.Uf;
                address.State = cepInfo.Estado;
                address.Region = cepInfo.Regiao;
                address.DDD = cepInfo.Ddd;
            }

            addressRepository.Update(address);
            await addressRepository.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var address = await addressRepository.GetByIdAsync(id, cancellationToken);
            if (address is null) return;

            addressRepository.Delete(address);
            await addressRepository.SaveChangesAsync(cancellationToken);
        }
    }
}