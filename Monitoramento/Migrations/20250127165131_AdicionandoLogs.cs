using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Monitoramento.Migrations
{
    /// <inheritdoc />
    public partial class AdicionandoLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "LogsMonitoramento",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "TempoExecucao",
                table: "LogsMonitoramento",
                type: "TEXT",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "LogsMonitoramento");

            migrationBuilder.DropColumn(
                name: "TempoExecucao",
                table: "LogsMonitoramento");
        }
    }
}
