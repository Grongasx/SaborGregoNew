using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaborGregoNew.Migrations
{
    /// <inheritdoc />
    public partial class statusnovos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Usuarios",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Produtos",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "UsuarioId",
                table: "Pedidos",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Enderecos",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_UsuarioId",
                table: "Pedidos",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Usuarios_UsuarioId",
                table: "Pedidos",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Usuarios_UsuarioId",
                table: "Pedidos");

            migrationBuilder.DropIndex(
                name: "IX_Pedidos_UsuarioId",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Produtos");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Enderecos");
        }
    }
}
