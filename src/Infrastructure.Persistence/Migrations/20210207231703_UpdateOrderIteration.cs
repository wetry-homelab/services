using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class UpdateOrderIteration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "ClusterNode",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateAt",
                table: "Cluster",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 2, 7, 23, 17, 2, 667, DateTimeKind.Utc).AddTicks(3578),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 2, 7, 16, 9, 31, 358, DateTimeKind.Utc).AddTicks(1982));

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Cluster",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProxmoxNodeId",
                table: "Cluster",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "ClusterNode");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Cluster");

            migrationBuilder.DropColumn(
                name: "ProxmoxNodeId",
                table: "Cluster");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateAt",
                table: "Cluster",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 2, 7, 16, 9, 31, 358, DateTimeKind.Utc).AddTicks(1982),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 2, 7, 23, 17, 2, 667, DateTimeKind.Utc).AddTicks(3578));
        }
    }
}
