using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ShoppingStore.Data.Migrations
{
    public partial class AddUserToCartlineTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CartLineId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "CartLines",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartLines_UserId",
                table: "CartLines",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartLines_AspNetUsers_UserId",
                table: "CartLines",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartLines_AspNetUsers_UserId",
                table: "CartLines");

            migrationBuilder.DropIndex(
                name: "IX_CartLines_UserId",
                table: "CartLines");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CartLines");

            migrationBuilder.AddColumn<int>(
                name: "CartLineId",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }
    }
}
