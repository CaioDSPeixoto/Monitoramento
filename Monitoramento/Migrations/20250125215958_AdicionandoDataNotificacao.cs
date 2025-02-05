using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Monitoramento.Migrations
{
    /// <inheritdoc />
    public partial class AdicionandoDataNotificacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UltimaNotificacao",
                table: "Monitoramento",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UltimaNotificacao",
                table: "Monitoramento");
        }
    }
}
