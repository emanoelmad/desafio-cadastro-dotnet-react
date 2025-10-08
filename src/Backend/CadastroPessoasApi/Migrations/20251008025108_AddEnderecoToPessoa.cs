using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CadastroPessoasApi.Migrations
{
    /// <inheritdoc />
    public partial class AddEnderecoToPessoa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Endereco",
                table: "Pessoas",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Endereco",
                table: "Pessoas");
        }
    }
}
