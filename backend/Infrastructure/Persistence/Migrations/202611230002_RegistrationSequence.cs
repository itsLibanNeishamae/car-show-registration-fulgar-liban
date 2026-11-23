using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations;

[DbContext(typeof(AppDbContext))]
[Migration("202611230002_RegistrationSequence")]
public partial class AddRegistrationSequence : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "RegistrationSequences",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false),
                LastNumber = table.Column<long>(type: "INTEGER", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_RegistrationSequences", x => x.Id));

        migrationBuilder.InsertData(
            table: "RegistrationSequences",
            columns: new[] { "Id", "LastNumber" },
            values: new object[] { 1, 0L });
    }

    protected override void Down(MigrationBuilder migrationBuilder) =>
        migrationBuilder.DropTable(name: "RegistrationSequences");
}
