using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ambev.DeveloperEvaluation.ORM.Migrations
{
    /// <inheritdoc />
    public partial class UpdateItemsMigracao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_sale_items_sales_sale_id1",
                table: "sale_items");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sale_items",
                table: "sale_items");

            migrationBuilder.DropIndex(
                name: "IX_sale_items_sale_id1",
                table: "sale_items");

            migrationBuilder.RenameColumn(
                name: "sale_id1",
                table: "sale_items",
                newName: "id");

            migrationBuilder.AlterColumn<Guid>(
                name: "sale_id",
                table: "sale_items",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sale_items",
                table: "sale_items",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_sale_items_sale_id",
                table: "sale_items",
                column: "sale_id");

            migrationBuilder.AddForeignKey(
                name: "FK_sale_items_sales_sale_id",
                table: "sale_items",
                column: "sale_id",
                principalTable: "sales",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_sale_items_sales_sale_id",
                table: "sale_items");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sale_items",
                table: "sale_items");

            migrationBuilder.DropIndex(
                name: "IX_sale_items_sale_id",
                table: "sale_items");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "sale_items",
                newName: "sale_id1");

            migrationBuilder.AlterColumn<Guid>(
                name: "sale_id",
                table: "sale_items",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_sale_items",
                table: "sale_items",
                columns: new[] { "product_id", "sale_id" });

            migrationBuilder.CreateIndex(
                name: "IX_sale_items_sale_id1",
                table: "sale_items",
                column: "sale_id1");

            migrationBuilder.AddForeignKey(
                name: "FK_sale_items_sales_sale_id1",
                table: "sale_items",
                column: "sale_id1",
                principalTable: "sales",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
