using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Monitoramento.Migrations
{
    /// <inheritdoc />
    public partial class AdicionandoCampoEmConfigSistema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MensagemErro",
                table: "LogsMonitoramento",
                newName: "Mensagem");

            migrationBuilder.AddColumn<bool>(
                name: "ExecutarEmSegundoPlano",
                table: "ConfiguracaoSistema",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "ConfiguracaoSistema",
                keyColumn: "Id",
                keyValue: 1,
                column: "ExecutarEmSegundoPlano",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExecutarEmSegundoPlano",
                table: "ConfiguracaoSistema");

            migrationBuilder.RenameColumn(
                name: "Mensagem",
                table: "LogsMonitoramento",
                newName: "MensagemErro");
        }
    }
}
