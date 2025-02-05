using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Monitoramento.Migrations
{
    /// <inheritdoc />
    public partial class CriandoRelacionamentos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Monitoramento_ConfiguracaoAplicacao_ConfiguracaoAplicacaoId",
                table: "Monitoramento");

            migrationBuilder.DropForeignKey(
                name: "FK_Webhooks_Monitoramento_MonitoramentoId",
                table: "Webhooks");

            migrationBuilder.DropTable(
                name: "ConfiguracaoAplicacao");

            migrationBuilder.DropIndex(
                name: "IX_Webhooks_MonitoramentoId",
                table: "Webhooks");

            migrationBuilder.DropIndex(
                name: "IX_Monitoramento_ConfiguracaoAplicacaoId",
                table: "Monitoramento");

            migrationBuilder.DropColumn(
                name: "MonitoramentoId",
                table: "Webhooks");

            migrationBuilder.RenameColumn(
                name: "ConfiguracaoAplicacaoId",
                table: "Monitoramento",
                newName: "ContadorDeFalha");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UltimaVerificacao",
                table: "Monitoramento",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.CreateTable(
                name: "ConfiguracaoSistema",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IntervaloMinutos = table.Column<int>(type: "INTEGER", nullable: false),
                    FalhasParaNotificacao = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracaoSistema", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MonitoramentoWebhook",
                columns: table => new
                {
                    MonitoramentoId = table.Column<int>(type: "INTEGER", nullable: false),
                    WebhookId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonitoramentoWebhook", x => new { x.MonitoramentoId, x.WebhookId });
                    table.ForeignKey(
                        name: "FK_MonitoramentoWebhook_Monitoramento_MonitoramentoId",
                        column: x => x.MonitoramentoId,
                        principalTable: "Monitoramento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonitoramentoWebhook_Webhooks_WebhookId",
                        column: x => x.WebhookId,
                        principalTable: "Webhooks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ConfiguracaoSistema",
                columns: new[] { "Id", "FalhasParaNotificacao", "IntervaloMinutos" },
                values: new object[] { 1, 3, 5 });

            migrationBuilder.CreateIndex(
                name: "IX_MonitoramentoWebhook_WebhookId",
                table: "MonitoramentoWebhook",
                column: "WebhookId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfiguracaoSistema");

            migrationBuilder.DropTable(
                name: "MonitoramentoWebhook");

            migrationBuilder.RenameColumn(
                name: "ContadorDeFalha",
                table: "Monitoramento",
                newName: "ConfiguracaoAplicacaoId");

            migrationBuilder.AddColumn<int>(
                name: "MonitoramentoId",
                table: "Webhooks",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UltimaVerificacao",
                table: "Monitoramento",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ConfiguracaoAplicacao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FalhasParaNotificacao = table.Column<int>(type: "INTEGER", nullable: false),
                    IntervaloMinutos = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracaoAplicacao", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ConfiguracaoAplicacao",
                columns: new[] { "Id", "FalhasParaNotificacao", "IntervaloMinutos" },
                values: new object[] { 1, 3, 5 });

            migrationBuilder.CreateIndex(
                name: "IX_Webhooks_MonitoramentoId",
                table: "Webhooks",
                column: "MonitoramentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Monitoramento_ConfiguracaoAplicacaoId",
                table: "Monitoramento",
                column: "ConfiguracaoAplicacaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Monitoramento_ConfiguracaoAplicacao_ConfiguracaoAplicacaoId",
                table: "Monitoramento",
                column: "ConfiguracaoAplicacaoId",
                principalTable: "ConfiguracaoAplicacao",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Webhooks_Monitoramento_MonitoramentoId",
                table: "Webhooks",
                column: "MonitoramentoId",
                principalTable: "Monitoramento",
                principalColumn: "Id");
        }
    }
}
