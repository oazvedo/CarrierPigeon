using BirdMessage.Infrastructure.Security;
using Xunit;

namespace BirdMessage.Tests.Security;

public class BCryptPasswordHasherTests
{
    private readonly BCryptPasswordHasher _hasher = new();

    [Fact]
    public void Hash_DoesNotReturnPlainPassword()
    {
        var hash = _hasher.Hash("plain-password");

        Assert.NotEqual("plain-password", hash);
    }

    [Fact]
    public void Verify_CorrectPassword_ReturnsTrue()
    {
        var hash = _hasher.Hash("plain-password");

        Assert.True(_hasher.Verify("plain-password", hash));
    }

    [Fact]
    public void Verify_WrongPassword_ReturnsFalse()
    {
        var hash = _hasher.Hash("plain-password");

        Assert.False(_hasher.Verify("wrong-password", hash));
    }
}
