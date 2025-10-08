using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace CadastroPessoasApi.Tests;

public class PessoasV2ControllerTests : IClassFixture<TestingWebAppFactory<Program>>
{
    private readonly HttpClient _client;

    public PessoasV2ControllerTests(TestingWebAppFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Post_WithoutEndereco_ReturnsBadRequest()
    {
        var dto = new
        {
            Nome = "Teste V2",
            CPF = "529.982.247-25",
            DataNascimento = new DateTime(1995,1,1)
            // Endereco is missing
        };

        var response = await _client.PostAsJsonAsync("/api/v2/pessoas", dto);

        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Full_CRUD_Workflow_V2()
    {
        var create = new
        {
            Nome = "João V2",
            CPF = "111.444.777-35",
            DataNascimento = new DateTime(1988, 3, 10),
            Endereco = "Rua Teste, 123"
        };

        var postResp = await _client.PostAsJsonAsync("/api/v2/pessoas", create);
        Assert.Equal(System.Net.HttpStatusCode.Created, postResp.StatusCode);

    using var doc = await System.Text.Json.JsonDocument.ParseAsync(await postResp.Content.ReadAsStreamAsync());
    int id = doc.RootElement.GetProperty("id").GetInt32();

        // GET by id
        var getResp = await _client.GetAsync($"/api/v2/pessoas/{id}");
        Assert.Equal(System.Net.HttpStatusCode.OK, getResp.StatusCode);

        // PUT update
        var update = new
        {
            Id = id,
            Nome = "João V2 Atualizado",
            CPF = "111.444.777-35",
            DataNascimento = new DateTime(1988, 3, 10),
            Endereco = "Rua Nova, 45"
        };

        var putResp = await _client.PutAsJsonAsync($"/api/v2/pessoas/{id}", update);
        Assert.Equal(System.Net.HttpStatusCode.NoContent, putResp.StatusCode);

        // DELETE
        var delResp = await _client.DeleteAsync($"/api/v2/pessoas/{id}");
        Assert.Equal(System.Net.HttpStatusCode.NoContent, delResp.StatusCode);
    }
}
