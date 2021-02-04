using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class AddSshKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateAt",
                table: "Cluster",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 2, 4, 18, 23, 52, 117, DateTimeKind.Utc).AddTicks(8551),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 2, 4, 10, 32, 48, 368, DateTimeKind.Utc).AddTicks(354));

            migrationBuilder.CreateTable(
                name: "SshKey",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Public = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Private = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fingerprint = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeleteAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SshKey", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Template",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BaseTemplate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CpuCount = table.Column<int>(type: "int", nullable: false),
                    MemoryCount = table.Column<int>(type: "int", nullable: false),
                    DiskSpace = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Template", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SshKey");

            migrationBuilder.DropTable(
                name: "Template");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateAt",
                table: "Cluster",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 2, 4, 10, 32, 48, 368, DateTimeKind.Utc).AddTicks(354),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 2, 4, 18, 23, 52, 117, DateTimeKind.Utc).AddTicks(8551));
        }
    }
}
