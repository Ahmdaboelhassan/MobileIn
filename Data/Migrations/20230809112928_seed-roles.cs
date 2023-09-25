using Microsoft.EntityFrameworkCore.Migrations;
using Business.SD;
#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class seedroles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // seed role = super admin
            migrationBuilder.InsertData(
             table: "AspNetRoles",
             columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
             values: new object[] {Guid.NewGuid().ToString(), SD.SuperAdmin,SD.SuperAdmin.ToUpper(),Guid.NewGuid().ToString()}
             ) ;

            // seed role = admin
            migrationBuilder.InsertData(
             table: "AspNetRoles",
             columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
             values: new object[] { Guid.NewGuid().ToString(), SD.Admin, SD.Admin.ToUpper(), Guid.NewGuid().ToString() }
             );

            // seed role = user
            migrationBuilder.InsertData(
             table: "AspNetRoles",
             columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
             values: new object[] { Guid.NewGuid().ToString(), SD.User, SD.User.ToUpper(), Guid.NewGuid().ToString() }
             );


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM AspNetRoles");

        }
    }
}
