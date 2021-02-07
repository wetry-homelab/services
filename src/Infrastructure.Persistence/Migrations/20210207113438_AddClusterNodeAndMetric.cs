using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class AddClusterNodeAndMetric : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreateAt",
                table: "Template",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteAt",
                table: "Template",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateAt",
                table: "Cluster",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 2, 7, 11, 34, 38, 177, DateTimeKind.Utc).AddTicks(774),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 2, 5, 10, 3, 36, 965, DateTimeKind.Utc).AddTicks(8921));

            migrationBuilder.AddColumn<string>(
                name: "KubeConfig",
                table: "Cluster",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KubeConfigJson",
                table: "Cluster",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ClusterNode",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClusterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ip = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeleteAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClusterNode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClusterNode_Cluster_ClusterId",
                        column: x => x.ClusterId,
                        principalTable: "Cluster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Metric",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<float>(type: "real", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Metric", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClusterNode_ClusterId",
                table: "ClusterNode",
                column: "ClusterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClusterNode");

            migrationBuilder.DropTable(
                name: "Metric");

            migrationBuilder.DropColumn(
                name: "CreateAt",
                table: "Template");

            migrationBuilder.DropColumn(
                name: "DeleteAt",
                table: "Template");

            migrationBuilder.DropColumn(
                name: "KubeConfig",
                table: "Cluster");

            migrationBuilder.DropColumn(
                name: "KubeConfigJson",
                table: "Cluster");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateAt",
                table: "Cluster",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 2, 5, 10, 3, 36, 965, DateTimeKind.Utc).AddTicks(8921),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 2, 7, 11, 34, 38, 177, DateTimeKind.Utc).AddTicks(774));
        }
    }
}
