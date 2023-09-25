using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class addadminuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [BD], [adress], [firstName], [lastName], [profilePicture]) VALUES (N'f9741a8c-682d-4c34-bbfd-1157b7e45efa', N'superadmin', N'SUPERADMIN', N'superadmin@mobilein.com', N'SUPERADMIN@MOBILEIN.COM', 0, N'AQAAAAIAAYagAAAAEHqf5PcN8tSygubTig6rKCx6hd2DxoRJuZDgLiIysiQXvoAU9xBnI89WMwLHyPDxwg==', N'PPWYZXGBI3QSYA6CLR6PP4QWBQMC2N7O', N'934e4c40-b49e-424a-a866-6b6feb981c90', NULL, 0, 0, NULL, 1, 0, N'2002-02-15 20:37:00', N'egypt', N'Super', N'admin', NULL ) ");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [dbo].[AspNetUsers] WHERE Id = 'f9741a8c-682d-4c34-bbfd-1157b7e45efa'");
        }
    }
}
