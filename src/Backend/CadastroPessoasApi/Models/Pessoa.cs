using System.ComponentModel.DataAnnotations;

namespace CadastroPessoasApi.Models
{
    public class Pessoa
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; } = string.Empty;

        [Required]
        public string CPF { get; set; } = string.Empty;

        [Required]
        public DateTime DataNascimento { get; set; }

        public string? Sexo { get; set; }
        public string? Email { get; set; }
        public string? Naturalidade { get; set; }
        public string? Nacionalidade { get; set; }

        public DateTime DataCadastro { get; set; }
        public DateTime DataAtualizacao { get; set; }
    }
}