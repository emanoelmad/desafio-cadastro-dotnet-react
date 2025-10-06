using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace CadastroPessoasApi.Tests;

public class PessoasControllerTests : IClassFixture<TestingWebAppFactory<Program>>
{
    private readonly HttpClient _client;

    public PessoasControllerTests(TestingWebAppFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Post_PessoaValida_Retorna201Created()
    {
        var novaPessoa = new
        {
            Nome = "Maria da Silva",
            CPF = "529.982.247-25",
            DataNascimento = new DateTime(1990, 5, 15)
        };

        var jsonContent = new StringContent(
            System.Text.Json.JsonSerializer.Serialize(novaPessoa),
            System.Text.Encoding.UTF8,
            "application/json");

        var response = await _client.PostAsync("/api/pessoas", jsonContent);

        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);

        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Contains("Maria da Silva", responseString);
    }
}