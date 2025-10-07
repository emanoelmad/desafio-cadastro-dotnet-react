using System.ComponentModel.DataAnnotations;

namespace CadastroPessoasApi.Dtos.v2
{
    public class PessoaCreateV2Dto
    {
        [Required]
        public string Nome { get; set; } = string.Empty;

        [Required]
        public string CPF { get; set; } = string.Empty;

        [Required]
        public DateTime DataNascimento { get; set; }

        public string? Sexo { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public string? Naturalidade { get; set; }
        public string? Nacionalidade { get; set; }

        [Required]
        public string Endereco { get; set; } = string.Empty;
    }
}
