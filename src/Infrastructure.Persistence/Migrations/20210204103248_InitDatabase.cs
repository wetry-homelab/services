using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistence.Migrations
{
    public partial class InitDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cluster",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Node = table.Column<int>(type: "int", nullable: false),
                    Cpu = table.Column<int>(type: "int", nullable: false),
                    Memory = table.Column<int>(type: "int", nullable: false),
                    Storage = table.Column<int>(type: "int", nullable: false),
                    Ip = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SshKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    User = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "Provisionning"),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2021, 2, 4, 10, 32, 48, 368, DateTimeKind.Utc).AddTicks(354)),
                    DeleteAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cluster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DatacenterNode",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ip = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Online = table.Column<bool>(type: "bit", nullable: false),
                    PveVersion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KernelVersion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Uptime = table.Column<int>(type: "int", nullable: false),
                    Mhz = table.Column<double>(type: "float", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Hvm = table.Column<bool>(type: "bit", nullable: false),
                    Core = table.Column<int>(type: "int", nullable: false),
                    UserHz = table.Column<int>(type: "int", nullable: false),
                    Socket = table.Column<int>(type: "int", nullable: false),
                    Flag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Thread = table.Column<int>(type: "int", nullable: false),
                    RootFsUsed = table.Column<long>(type: "bigint", nullable: false),
                    RootFsTotal = table.Column<long>(type: "bigint", nullable: false),
                    RootFsFree = table.Column<long>(type: "bigint", nullable: false),
                    RootFsAvailable = table.Column<long>(type: "bigint", nullable: false),
                    RamTotal = table.Column<long>(type: "bigint", nullable: false),
                    RamFree = table.Column<long>(type: "bigint", nullable: false),
                    SwapUsed = table.Column<long>(type: "bigint", nullable: false),
                    SwapTotal = table.Column<long>(type: "bigint", nullable: false),
                    SwapFree = table.Column<long>(type: "bigint", nullable: false),
                    RamUsed = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatacenterNode", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cluster");

            migrationBuilder.DropTable(
                name: "DatacenterNode");
        }
    }
}
