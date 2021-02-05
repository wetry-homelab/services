using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class SshKeyRemoveRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Private",
                table: "SshKey",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Pem",
                table: "SshKey",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateAt",
                table: "Cluster",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 2, 5, 10, 3, 36, 965, DateTimeKind.Utc).AddTicks(8921),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 2, 4, 18, 23, 52, 117, DateTimeKind.Utc).AddTicks(8551));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Private",
                table: "SshKey",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Pem",
                table: "SshKey",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateAt",
                table: "Cluster",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2021, 2, 4, 18, 23, 52, 117, DateTimeKind.Utc).AddTicks(8551),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2021, 2, 5, 10, 3, 36, 965, DateTimeKind.Utc).AddTicks(8921));
        }
    }
}
