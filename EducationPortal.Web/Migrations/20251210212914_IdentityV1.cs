using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EducationPortal.Web.Migrations
{
    /// <inheritdoc />
    public partial class IdentityV1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "67681f32-907c-4e44-8237-a1d31fc17d2c", "AQAAAAIAAYagAAAAEMtOtHXX5j50Ui01XgbiprMGfoXmsXQev9D0WYU8B3gL4TExy267StYvHjvNPQb8xQ==", "bbcc9ad9-349f-4600-ad57-832d2fde5998" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6ba9a1d7-36c0-4698-9944-cb829c44ecfe", "AQAAAAIAAYagAAAAEImVfMi8/c3YJyYtmtLPMCpngs7xQg2bIAQnpVsXjgQlzo4pImA1nScgkt5uwQDu+w==", "1c9c4afe-9eed-46cd-9ff4-061ac493455a" });
        }
    }
}
