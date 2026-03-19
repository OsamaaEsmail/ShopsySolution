using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Order.Infrastructure.Migrations;

/// <inheritdoc />
public partial class AddCrossModuleForeignKeys : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(@"
            ALTER TABLE [order].[Orders]
            ADD CONSTRAINT FK_Orders_Users
            FOREIGN KEY (UserId) REFERENCES [users].[AspNetUsers](Id)
        ");

        migrationBuilder.Sql(@"
            ALTER TABLE [order].[OrderItems]
            ADD CONSTRAINT FK_OrderItems_Products
            FOREIGN KEY (ProductId) REFERENCES [catalog].[Products](Id)
        ");

        migrationBuilder.Sql(@"
            ALTER TABLE [order].[Payments]
            ADD CONSTRAINT FK_Payments_Users
            FOREIGN KEY (UserId) REFERENCES [users].[AspNetUsers](Id)
        ");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("ALTER TABLE [order].[Orders] DROP CONSTRAINT FK_Orders_Users");
        migrationBuilder.Sql("ALTER TABLE [order].[OrderItems] DROP CONSTRAINT FK_OrderItems_Products");
        migrationBuilder.Sql("ALTER TABLE [order].[Payments] DROP CONSTRAINT FK_Payments_Users");
    }
}