#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace NoMercy.Database.Migrations.Queue;

/// <inheritdoc />
public partial class Test : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            "FailedJobs",
            table => new
            {
                Id = table.Column<long>("INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Uuid = table.Column<Guid>("TEXT", nullable: false),
                Connection = table.Column<string>("TEXT", maxLength: 256, nullable: false),
                Queue = table.Column<string>("TEXT", maxLength: 256, nullable: false),
                Payload = table.Column<string>("TEXT", maxLength: 256, nullable: false),
                Exception = table.Column<string>("TEXT", maxLength: 256, nullable: false),
                FailedAt = table.Column<DateTime>("TEXT", nullable: false)
            },
            constraints: table => { table.PrimaryKey("PK_FailedJobs", x => x.Id); });

        migrationBuilder.CreateTable(
            "QueueJobs",
            table => new
            {
                Id = table.Column<long>("INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Priority = table.Column<int>("INTEGER", nullable: true),
                Queue = table.Column<string>("TEXT", maxLength: 256, nullable: false),
                Payload = table.Column<string>("TEXT", maxLength: 256, nullable: false),
                Attempts = table.Column<byte>("INTEGER", nullable: false),
                ReservedAt = table.Column<DateTime>("TEXT", nullable: true),
                AvailableAt = table.Column<DateTime>("TEXT", nullable: false),
                CreatedAt = table.Column<DateTime>("TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
            },
            constraints: table => { table.PrimaryKey("PK_QueueJobs", x => x.Id); });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            "FailedJobs");

        migrationBuilder.DropTable(
            "QueueJobs");
    }
}