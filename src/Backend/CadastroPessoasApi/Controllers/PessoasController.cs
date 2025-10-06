using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CadastroPessoasApi.Data;
using CadastroPessoasApi.Models;
using CadastroPessoasApi.Dtos;
using CadastroPessoasApi.Validators;
using Microsoft.AspNetCore.Authorization;

namespace CadastroPessoasApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PessoasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PessoasController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PessoaReadDto>>> GetPessoas()
        {
            var list = await _context.Pessoas.ToListAsync();
            return list.Select(p => new PessoaReadDto
            {
                Id = p.Id,
                Nome = p.Nome,
                CPF = p.CPF,
                DataNascimento = p.DataNascimento,
                Sexo = p.Sexo,
                Email = p.Email,
                Naturalidade = p.Naturalidade,
                Nacionalidade = p.Nacionalidade,
                DataCadastro = p.DataCadastro,
                DataAtualizacao = p.DataAtualizacao
            }).ToList();
        }

        [HttpPost]
        public async Task<ActionResult<PessoaReadDto>> PostPessoa([FromBody] PessoaCreateDto dto)
        {
            if (!CpfValidator.IsValid(dto.CPF))
                return BadRequest(new { message = "CPF inválido." });

            var cpfNormalized = System.Text.RegularExpressions.Regex.Replace(dto.CPF, "[^0-9]", string.Empty);

            bool cpfExiste = await _context.Pessoas.AnyAsync(p => p.CPF == cpfNormalized);
            if (cpfExiste)
            {
                return BadRequest(new { message = "O CPF informado já está cadastrado." });
            }

            var pessoa = new Pessoa
            {
                Nome = dto.Nome,
                CPF = cpfNormalized,
                DataNascimento = dto.DataNascimento,
                Sexo = dto.Sexo,
                Email = dto.Email,
                Naturalidade = dto.Naturalidade,
                Nacionalidade = dto.Nacionalidade,
                DataCadastro = DateTime.UtcNow,
                DataAtualizacao = DateTime.UtcNow
            };

            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync();

            var read = new PessoaReadDto
            {
                Id = pessoa.Id,
                Nome = pessoa.Nome,
                CPF = pessoa.CPF,
                DataNascimento = pessoa.DataNascimento,
                Sexo = pessoa.Sexo,
                Email = pessoa.Email,
                Naturalidade = pessoa.Naturalidade,
                Nacionalidade = pessoa.Nacionalidade,
                DataCadastro = pessoa.DataCadastro,
                DataAtualizacao = pessoa.DataAtualizacao
            };

            return CreatedAtAction(nameof(GetPessoa), new { id = pessoa.Id }, read);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPessoa(int id, PessoaUpdateDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest(new { message = "O ID da URL não corresponde ao ID do objeto enviado." });
            }

            if (!CpfValidator.IsValid(dto.CPF))
                return BadRequest(new { message = "CPF inválido." });

            var cpfNormalized = System.Text.RegularExpressions.Regex.Replace(dto.CPF, "[^0-9]", string.Empty);

            bool cpfDuplicado = await _context.Pessoas
                .AnyAsync(p => p.CPF == cpfNormalized && p.Id != id);

            if (cpfDuplicado)
            {
                return BadRequest(new { message = "O CPF informado já está cadastrado para outro usuário." });
            }

            var pessoa = await _context.Pessoas.FindAsync(id);
            if (pessoa == null) return NotFound();

            pessoa.Nome = dto.Nome;
            pessoa.CPF = cpfNormalized;
            pessoa.DataNascimento = dto.DataNascimento;
            pessoa.Sexo = dto.Sexo;
            pessoa.Email = dto.Email;
            pessoa.Naturalidade = dto.Naturalidade;
            pessoa.Nacionalidade = dto.Nacionalidade;
            pessoa.DataAtualizacao = DateTime.UtcNow;

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
        public async Task<ActionResult<PessoaReadDto>> GetPessoa(int id)
        {
            var pessoa = await _context.Pessoas.FindAsync(id);

            if (pessoa == null)
            {
                return NotFound();
            }

            var read = new PessoaReadDto
            {
                Id = pessoa.Id,
                Nome = pessoa.Nome,
                CPF = pessoa.CPF,
                DataNascimento = pessoa.DataNascimento,
                Sexo = pessoa.Sexo,
                Email = pessoa.Email,
                Naturalidade = pessoa.Naturalidade,
                Nacionalidade = pessoa.Nacionalidade,
                DataCadastro = pessoa.DataCadastro,
                DataAtualizacao = pessoa.DataAtualizacao
            };

            return read;
        }
    }
}