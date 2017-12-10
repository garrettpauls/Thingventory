using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Thingventory.Core.Migrations
{
    public partial class AddBasicItemInformation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "AcquiredDate",
                table: "Item",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AcquiredFrom",
                table: "Item",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "Item",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<uint>(
                name: "Quantity",
                table: "Item",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<decimal>(
                name: "Value",
                table: "Item",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcquiredDate",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "AcquiredFrom",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Comments",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "Item");
        }
    }
}
