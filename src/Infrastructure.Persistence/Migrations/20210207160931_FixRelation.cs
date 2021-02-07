using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class FixRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClusterNode_Cluster_ClusterId1",
                table: "ClusterNode");

            migrationBuilder.DropIndex(
                name: "IX_ClusterNode_ClusterId1",
                table: "ClusterNode");

            migrationBuilder.DropColumn(
                name: "ClusterId1",
                table: "ClusterNode");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateAt",
                table: "Cluster",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 2, 7, 16, 9, 31, 358, DateTimeKind.Utc).AddTicks(1982),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 2, 7, 12, 36, 55, 450, DateTimeKind.Utc).AddTicks(7122));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                oldDefaultValue: new DateTime(2021, 2, 7, 16, 9, 31, 358, DateTimeKind.Utc).AddTicks(1982));

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
    }
}
