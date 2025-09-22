using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProdutosApi.Migrations
{
    /// <inheritdoc />
    public partial class SeedDatabase : Migration
    {
        //Executado quando você aplica a migration com Update-Database. Aqui entram os comandos para criar tabelas, adicionar colunas, inserir dados, etc
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("Insert into Categorias(Name, ImageUrl) Values('Notebook', 'notebook.jpg')");
            mb.Sql("Insert into Categorias(Name, ImageUrl) Values('Book','book.jpg')");
            mb.Sql("Insert into Categorias(Name, ImageUrl) Values('Skincare','skincare.jpg')");

            mb.Sql("Insert into Produtos(Name, Description, Price, ImageUrl, Stock, RegistrationDate, CategoryId) " +
            "Values('Notebook Acer Nitro V15 • 8GB, Intel Core i7 13620H, RTX 3050', 'Descricao', 4899, 'notebook-acer-nitro-v15.jpg', 21, now(), 1)");
            mb.Sql("Insert into Produtos(Name, Description, Price, ImageUrl, Stock, RegistrationDate, CategoryId) " +
            "Values('O Poder dos Quietos', 'Descricao', 4899, 'o-poder-dos-quietos.jpg', 1617, now(), 2)");
            mb.Sql("Insert into Produtos(Name, Description, Price, ImageUrl, Stock, RegistrationDate, CategoryId) " +
            "Values('Sérum de Vitamina C Principia', 'Descricao', 64, 'serum-vitamina-c-principia.jpg', 111, now(), 3)");
        }

        //Executado quando você digita Update-Database NomeDaMigrationAnterior para reverter mudanças do banco de dados
        protected override void Down(MigrationBuilder mb)
        {
            //Deleta todos os registros das tabelas Categorias e Produtos
            mb.Sql("Delete from Categorias");
            mb.Sql("Delete from Produtos");
        }
    }
}
