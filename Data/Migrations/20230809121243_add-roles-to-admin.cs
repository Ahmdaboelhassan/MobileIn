using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class addrolestoadmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO AspNetUserRoles SELECT 'f9741a8c-682d-4c34-bbfd-1157b7e45efa' , Id FROM AspNetRoles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM AspNetUserRoles WHERE UserId = 'f9741a8c-682d-4c34-bbfd-1157b7e45efa'");
        }
    }
}
