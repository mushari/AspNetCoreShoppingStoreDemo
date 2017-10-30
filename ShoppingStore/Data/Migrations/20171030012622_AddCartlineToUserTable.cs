using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ShoppingStore.Data.Migrations
{
    public partial class AddCartlineToUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CartLineID",
                table: "CartLines",
                newName: "CartLineId");

            migrationBuilder.AddColumn<int>(
                name: "CartLineId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CartLineId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "CartLineId",
                table: "CartLines",
                newName: "CartLineID");
        }
    }
}
