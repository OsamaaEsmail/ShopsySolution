using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sales.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCrossSalesModuleForeignKeys : Migration
    {
         protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            ALTER TABLE [sales].[SaleItems]
            ADD CONSTRAINT FK_SaleItems_Products
            FOREIGN KEY (ProductId) REFERENCES [catalog].[Products](Id)
        ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE [sales].[SaleItems] DROP CONSTRAINT FK_SaleItems_Products");
        }
    }
}
