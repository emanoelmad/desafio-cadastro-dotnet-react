using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CadastroPessoasApi.Data;
using CadastroPessoasApi.Models;
using CadastroPessoasApi.Validators;
using CadastroPessoasApi.Dtos.v2;
using Microsoft.AspNetCore.Authorization;

namespace CadastroPessoasApi.Controllers.v2
{
    [ApiController]
    [Route("api/v2/[controller]")]
    [Authorize]
    public class PessoasV2Controller : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PessoasV2Controller(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PessoaReadV2Dto>>> GetPessoas()
        {
            var list = await _context.Pessoas.ToListAsync();
            return list.Select(p => new PessoaReadV2Dto
            {
                Id = p.Id,
                Nome = p.Nome,
                CPF = p.CPF,
                DataNascimento = p.DataNascimento,
                Sexo = p.Sexo,
                Email = p.Email,
                Naturalidade = p.Naturalidade,
                Nacionalidade = p.Nacionalidade,
                Endereco = p.Endereco ?? string.Empty,
                DataCadastro = p.DataCadastro,
                DataAtualizacao = p.DataAtualizacao
            }).ToList();
        }

        [HttpPost]
        public async Task<ActionResult<PessoaReadV2Dto>> PostPessoa([FromBody] PessoaCreateV2Dto dto)
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
                Endereco = dto.Endereco,
                DataCadastro = DateTime.UtcNow,
                DataAtualizacao = DateTime.UtcNow
            };

            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync();

            var read = new PessoaReadV2Dto
            {
                Id = pessoa.Id,
                Nome = pessoa.Nome,
                CPF = pessoa.CPF,
                DataNascimento = pessoa.DataNascimento,
                Sexo = pessoa.Sexo,
                Email = pessoa.Email,
                Naturalidade = pessoa.Naturalidade,
                Nacionalidade = pessoa.Nacionalidade,
                Endereco = pessoa.Endereco ?? string.Empty,
                DataCadastro = pessoa.DataCadastro,
                DataAtualizacao = pessoa.DataAtualizacao
            };

            return CreatedAtAction(nameof(GetPessoa), new { id = pessoa.Id }, read);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPessoa(int id, PessoaUpdateV2Dto dto)
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
            pessoa.Endereco = dto.Endereco;
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
        public async Task<ActionResult<PessoaReadV2Dto>> GetPessoa(int id)
        {
            var pessoa = await _context.Pessoas.FindAsync(id);

            if (pessoa == null)
            {
                return NotFound();
            }

            var read = new PessoaReadV2Dto
            {
                Id = pessoa.Id,
                Nome = pessoa.Nome,
                CPF = pessoa.CPF,
                DataNascimento = pessoa.DataNascimento,
                Sexo = pessoa.Sexo,
                Email = pessoa.Email,
                Naturalidade = pessoa.Naturalidade,
                Nacionalidade = pessoa.Nacionalidade,
                Endereco = pessoa.Endereco ?? string.Empty,
                DataCadastro = pessoa.DataCadastro,
                DataAtualizacao = pessoa.DataAtualizacao
            };

            return read;
        }
    }
}
