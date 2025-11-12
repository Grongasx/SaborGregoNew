using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SaborGregoNew.Migrations
{
    /// <inheritdoc />
    public partial class OutraMigrationPedido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_DetalhesPedido_ProdutoId",
                table: "DetalhesPedido",
                column: "ProdutoId");

            migrationBuilder.AddForeignKey(
                name: "FK_DetalhesPedido_Produtos_ProdutoId",
                table: "DetalhesPedido",
                column: "ProdutoId",
                principalTable: "Produtos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetalhesPedido_Produtos_ProdutoId",
                table: "DetalhesPedido");

            migrationBuilder.DropIndex(
                name: "IX_DetalhesPedido_ProdutoId",
                table: "DetalhesPedido");
        }
    }
}
