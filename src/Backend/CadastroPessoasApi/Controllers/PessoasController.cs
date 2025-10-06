using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CadastroPessoasApi.Data;
using CadastroPessoasApi.Models;

namespace CadastroPessoasApi.Controllers
{
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

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPessoa(int id, Pessoa pessoa)
        {
            if (id != pessoa.Id)
            {
                return BadRequest(new { message = "O ID da URL não corresponde ao ID do objeto enviado." });
            }

            pessoa.DataAtualizacao = DateTime.UtcNow;

            bool cpfDuplicado = await _context.Pessoas
                .AnyAsync(p => p.CPF == pessoa.CPF && p.Id != id);

            if (cpfDuplicado)
            {
                return BadRequest(new { message = "O CPF informado já está cadastrado para outro usuário." });
            }

            _context.Entry(pessoa).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Pessoas.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePessoa(int id)
        {
            var pessoa = await _context.Pessoas.FindAsync(id);

            if (pessoa == null)
            {
                return NotFound();
            }

            _context.Pessoas.Remove(pessoa);
            await _context.SaveChangesAsync();

            return NoContent();
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
}