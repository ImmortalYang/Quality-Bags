using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace QualityBags.Data.Migrations
{
    public partial class ShoppingCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_AspNetUsers_AppUserId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_AppUserId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "TotalCost",
                table: "Order Detail");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Order");

            migrationBuilder.CreateTable(
                name: "CartItem",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CartID = table.Column<string>(nullable: true),
                    Count = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    ProductID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItem", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CartItem_Product_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Product",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.AddColumn<DateTime>(
                name: "OrderDate",
                table: "Order",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Order",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_ApplicationUserId",
                table: "Order",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_ProductID",
                table: "CartItem",
                column: "ProductID");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_AspNetUsers_ApplicationUserId",
                table: "Order",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_AspNetUsers_ApplicationUserId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_ApplicationUserId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "OrderDate",
                table: "Order");

            migrationBuilder.DropTable(
                name: "CartItem");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalCost",
                table: "Order Detail",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Order",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ApplicationUserId",
                table: "Order",
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_Order_AppUserId",
                table: "Order",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_AspNetUsers_AppUserId",
                table: "Order",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
