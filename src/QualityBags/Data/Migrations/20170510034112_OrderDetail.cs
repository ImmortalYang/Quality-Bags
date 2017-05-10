using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QualityBags.Data.Migrations
{
    public partial class OrderDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Order",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Order",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Order",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Order",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "Order",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Order",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Order");
        }
    }
}
