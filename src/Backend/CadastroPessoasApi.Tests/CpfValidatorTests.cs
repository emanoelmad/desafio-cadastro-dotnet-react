using CadastroPessoasApi.Validators;
using Xunit;

namespace CadastroPessoasApi.Tests;

public class CpfValidatorTests
{
    [Theory]
    [InlineData("529.982.247-25")]
    [InlineData("52998224725")]
    [InlineData("111.444.777-35")]
    public void IsValid_ValidCpfs_ReturnsTrue(string cpf)
    {
        var result = CpfValidator.IsValid(cpf);
        Assert.True(result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("111.111.111-11")]
    [InlineData("12345678900")]
    [InlineData("529.982.247-24")] // invalid check digits
    public void IsValid_InvalidCpfs_ReturnsFalse(string? cpf)
    {
        var result = CpfValidator.IsValid(cpf ?? string.Empty);
        Assert.False(result);
    }
}
