using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProdutosApi.Migrations
{
    /// <inheritdoc />
    public partial class FixSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("UPDATE Produtos SET Price = 49.00 WHERE Name = 'O Poder dos Quietos'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("UPDATE Produtos SET Price = 4899.00 WHERE Name = 'O Poder dos Quietos'");
        }
    }
}
