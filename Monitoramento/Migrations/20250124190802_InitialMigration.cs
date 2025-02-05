using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Monitoramento.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConfiguracaoAplicacao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IntervaloMinutos = table.Column<int>(type: "INTEGER", nullable: false),
                    FalhasParaNotificacao = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracaoAplicacao", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Monitoramento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    Tipo = table.Column<int>(type: "INTEGER", nullable: false),
                    Versao = table.Column<int>(type: "INTEGER", nullable: false),
                    Endereco = table.Column<string>(type: "TEXT", nullable: false),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false),
                    UltimaVerificacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    StatusOnline = table.Column<bool>(type: "INTEGER", nullable: false),
                    ConfiguracaoAplicacaoId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Monitoramento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Monitoramento_ConfiguracaoAplicacao_ConfiguracaoAplicacaoId",
                        column: x => x.ConfiguracaoAplicacaoId,
                        principalTable: "ConfiguracaoAplicacao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LogsMonitoramento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DataHora = table.Column<DateTime>(type: "TEXT", nullable: false),
                    MensagemErro = table.Column<string>(type: "TEXT", nullable: false),
                    MonitoramentoId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogsMonitoramento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LogsMonitoramento_Monitoramento_MonitoramentoId",
                        column: x => x.MonitoramentoId,
                        principalTable: "Monitoramento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Webhooks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    NomeResponsavel = table.Column<string>(type: "TEXT", nullable: false),
                    Url = table.Column<string>(type: "TEXT", nullable: false),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false),
                    MonitoramentoId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Webhooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Webhooks_Monitoramento_MonitoramentoId",
                        column: x => x.MonitoramentoId,
                        principalTable: "Monitoramento",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "ConfiguracaoAplicacao",
                columns: new[] { "Id", "FalhasParaNotificacao", "IntervaloMinutos" },
                values: new object[] { 1, 3, 5 });

            migrationBuilder.CreateIndex(
                name: "IX_LogsMonitoramento_MonitoramentoId",
                table: "LogsMonitoramento",
                column: "MonitoramentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Monitoramento_ConfiguracaoAplicacaoId",
                table: "Monitoramento",
                column: "ConfiguracaoAplicacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Webhooks_MonitoramentoId",
                table: "Webhooks",
                column: "MonitoramentoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogsMonitoramento");

            migrationBuilder.DropTable(
                name: "Webhooks");

            migrationBuilder.DropTable(
                name: "Monitoramento");

            migrationBuilder.DropTable(
                name: "ConfiguracaoAplicacao");
        }
    }
}
