using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CadastroPessoasApi.Data;
using CadastroPessoasApi.Models;

[ApiController]
[Route("api/[controller]")]
public class PessoasController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public PessoasController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Pessoa>>> GetPessoas()
    {
        return await _context.Pessoas.ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Pessoa>> PostPessoa(Pessoa pessoa)
    {
        pessoa.DataCadastro = DateTime.UtcNow;
        pessoa.DataAtualizacao = DateTime.UtcNow;

        bool cpfExiste = await _context.Pessoas.AnyAsync(p => p.CPF == pessoa.CPF);
        if (cpfExiste)
        {
            return BadRequest(new { message = "O CPF informado já está cadastrado." });
        }

        _context.Pessoas.Add(pessoa);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPessoa), new { id = pessoa.Id }, pessoa);
    }

    [HttpGet("{id}")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<ActionResult<Pessoa>> GetPessoa(int id)
    {
        var pessoa = await _context.Pessoas.FindAsync(id);

        if (pessoa == null)
        {
            return NotFound();
        }

        return pessoa;
    }
}