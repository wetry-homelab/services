using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class SingleLineMetric : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Metric");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "Metric");

            migrationBuilder.RenameColumn(
                name: "ItemId",
                table: "Metric",
                newName: "EntityId");

            migrationBuilder.AddColumn<long>(
                name: "CpuValue",
                table: "Metric",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "MemoryValue",
                table: "Metric",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<Guid>(
                name: "ClusterId1",
                table: "ClusterNode",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateAt",
                table: "Cluster",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 2, 7, 12, 36, 55, 450, DateTimeKind.Utc).AddTicks(7122),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 2, 7, 11, 34, 38, 177, DateTimeKind.Utc).AddTicks(774));

            migrationBuilder.CreateIndex(
                name: "IX_ClusterNode_ClusterId1",
                table: "ClusterNode",
                column: "ClusterId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ClusterNode_Cluster_ClusterId1",
                table: "ClusterNode",
                column: "ClusterId1",
                principalTable: "Cluster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClusterNode_Cluster_ClusterId1",
                table: "ClusterNode");

            migrationBuilder.DropIndex(
                name: "IX_ClusterNode_ClusterId1",
                table: "ClusterNode");

            migrationBuilder.DropColumn(
                name: "CpuValue",
                table: "Metric");

            migrationBuilder.DropColumn(
                name: "MemoryValue",
                table: "Metric");

            migrationBuilder.DropColumn(
                name: "ClusterId1",
                table: "ClusterNode");

            migrationBuilder.RenameColumn(
                name: "EntityId",
                table: "Metric",
                newName: "ItemId");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Metric",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Value",
                table: "Metric",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateAt",
                table: "Cluster",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 2, 7, 11, 34, 38, 177, DateTimeKind.Utc).AddTicks(774),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 2, 7, 12, 36, 55, 450, DateTimeKind.Utc).AddTicks(7122));
        }
    }
}
