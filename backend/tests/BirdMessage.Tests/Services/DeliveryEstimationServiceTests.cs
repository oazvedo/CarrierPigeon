using BirdMessage.Application.Services;
using Xunit;

namespace BirdMessage.Tests.Services;

public class DeliveryEstimationServiceTests
{
    private readonly DeliveryEstimationService _service = new();

    [Fact]
    public void CalculateDistanceKm_SamePoint_ReturnsZero()
    {
        var distance = _service.CalculateDistanceKm(-23.5505m, -46.6333m, -23.5505m, -46.6333m);

        Assert.Equal(0m, Math.Round(distance, 3));
    }

    [Fact]
    public void CalculateDistanceKm_SaoPauloToRioDeJaneiro_ReturnsApproximateDistance()
    {
        var distance = _service.CalculateDistanceKm(-23.5505m, -46.6333m, -22.9068m, -43.1729m);

        Assert.InRange(distance, 350m, 370m);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void CalculateDuration_NonPositiveVelocity_Throws(decimal velocity)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => _service.CalculateDuration(100m, velocity));
    }

    [Fact]
    public void CalculateDuration_ValidInput_ReturnsExpectedTimeSpan()
    {
        var duration = _service.CalculateDuration(100m, 50m);

        Assert.Equal(TimeSpan.FromHours(2), duration);
    }

    [Fact]
    public void Estimate_ReturnsDistanceAndDurationConsistentWithComponents()
    {
        var result = _service.Estimate(-23.5505m, -46.6333m, -22.9068m, -43.1729m, 60m);
        var expectedDistance = _service.CalculateDistanceKm(-23.5505m, -46.6333m, -22.9068m, -43.1729m);
        var expectedDuration = _service.CalculateDuration(expectedDistance, 60m);

        Assert.Equal(expectedDistance, result.DistanceKm);
        Assert.Equal(expectedDuration, result.EstimatedDuration);
    }
}
