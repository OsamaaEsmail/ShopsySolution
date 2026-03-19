using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cart.Infrastructure.Migrations;

public partial class AddCrossModuleForeignKeys : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(@"
            ALTER TABLE [cart].[BasketItems]
            ADD CONSTRAINT FK_BasketItems_Products
            FOREIGN KEY (ProductId) REFERENCES [catalog].[Products](Id)
        ");

        migrationBuilder.Sql(@"
            ALTER TABLE [cart].[Baskets]
            ADD CONSTRAINT FK_Baskets_Users
            FOREIGN KEY (UserId) REFERENCES [users].[AspNetUsers](Id)
        ");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("ALTER TABLE [cart].[BasketItems] DROP CONSTRAINT FK_BasketItems_Products");
        migrationBuilder.Sql("ALTER TABLE [cart].[Baskets] DROP CONSTRAINT FK_Baskets_Users");
    }
}