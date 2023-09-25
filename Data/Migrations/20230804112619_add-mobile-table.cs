using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class addmobiletable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "mobiles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    yearRelease = table.Column<int>(type: "int", nullable: false),
                    photoURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RAM = table.Column<byte>(type: "tinyint", nullable: false),
                    size = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    companyId = table.Column<byte>(type: "tinyint", nullable: false),
                    processorId = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mobiles", x => x.id);
                    table.ForeignKey(
                        name: "FK_mobiles_companies_companyId",
                        column: x => x.companyId,
                        principalTable: "companies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_mobiles_processors_processorId",
                        column: x => x.processorId,
                        principalTable: "processors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_mobiles_companyId",
                table: "mobiles",
                column: "companyId");

            migrationBuilder.CreateIndex(
                name: "IX_mobiles_processorId",
                table: "mobiles",
                column: "processorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "mobiles");
        }
    }
}
