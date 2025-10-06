namespace CadastroPessoasApi.Dtos
{
    public class PessoaReadDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; }
        public string? Sexo { get; set; }
        public string? Email { get; set; }
        public string? Naturalidade { get; set; }
        public string? Nacionalidade { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAtualizacao { get; set; }
    }
}
