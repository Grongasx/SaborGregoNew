using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaborGregoNew.Migrations
{
    /// <inheritdoc />
    public partial class RemodelaçãoDeTudo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DetalhesPedido",
                table: "DetalhesPedido");

            migrationBuilder.DropIndex(
                name: "IX_DetalhesPedido_PedidoId",
                table: "DetalhesPedido");

            migrationBuilder.DropColumn(
                name: "EnderecoEntrega",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "DetalhesPedido");

            migrationBuilder.DropColumn(
                name: "FormaPagamento",
                table: "Carrinhos");

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Usuarios",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "EnderecoId",
                table: "Pedidos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Imagem",
                table: "DetalhesPedido",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DetalhesPedido",
                table: "DetalhesPedido",
                columns: new[] { "PedidoId", "ProdutoId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DetalhesPedido",
                table: "DetalhesPedido");

            migrationBuilder.DropColumn(
                name: "EnderecoId",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "Imagem",
                table: "DetalhesPedido");

            migrationBuilder.AlterColumn<int>(
                name: "Role",
                table: "Usuarios",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<string>(
                name: "EnderecoEntrega",
                table: "Pedidos",
                type: "TEXT",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "DetalhesPedido",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "FormaPagamento",
                table: "Carrinhos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DetalhesPedido",
                table: "DetalhesPedido",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_DetalhesPedido_PedidoId",
                table: "DetalhesPedido",
                column: "PedidoId");
        }
    }
}
