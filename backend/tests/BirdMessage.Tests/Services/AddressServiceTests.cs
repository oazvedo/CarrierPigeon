using BirdMessage.Application.Dto;
using BirdMessage.Application.Externals.Interfaces;
using BirdMessage.Application.Services;
using BirdMessage.Domain.Entities;
using BirdMessage.Domain.Interfaces;
using Moq;
using Xunit;

namespace BirdMessage.Tests.Services;

public class AddressServiceTests
{
    [Fact]
    public async Task CreateAsync_WhenCepIsValid_PopulatesAddressDataBeforeSaving()
    {
        var userId = Guid.NewGuid();
        var cep = "01001000";
        var repository = new Mock<IAddressRepository>();
        var cepService = new Mock<ICepService>();

        cepService
            .Setup(service => service.GetCepInfosAsync(cep))
            .ReturnsAsync(new CepServiceDto(
                "01001-000",
                "Praça da Sé",
                "lado ímpar",
                "",
                "Sé",
                "São Paulo",
                "SP",
                "São Paulo",
                "Sudeste",
                "3550308",
                "1004",
                "11",
                "7107"));

        repository
            .Setup(r => r.AddAsync(It.IsAny<Address>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        repository
            .Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var service = new AddressService(repository.Object, cepService.Object);
        var address = new Address
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Cep = cep
        };

        await service.CreateAsync(address);

        Assert.Equal("Praça da Sé", address.Street);
        Assert.Equal("Sé", address.Neighborhood);
        Assert.Equal("São Paulo", address.Local);
        Assert.Equal("SP", address.Uf);
        Assert.Equal("São Paulo", address.State);
        Assert.Equal("Sudeste", address.Region);
        Assert.Equal("11", address.DDD);

        repository.Verify(r => r.AddAsync(address, It.IsAny<CancellationToken>()), Times.Once);
        repository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
