using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChainDegree.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateForeignKeyCSDT_NTD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "CoSoDaoTaoApprovedEvents",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2026, 4, 21, 8, 9, 16, 209, DateTimeKind.Utc).AddTicks(5032),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2026, 4, 20, 16, 41, 55, 559, DateTimeKind.Utc).AddTicks(2626));

            migrationBuilder.CreateIndex(
                name: "IX_NhaTuyenDung_YeuCauDangKyId",
                table: "NhaTuyenDung",
                column: "YeuCauDangKyId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_NhaTuyenDung_YeuCauDangKy_YeuCauDangKyId",
                table: "NhaTuyenDung",
                column: "YeuCauDangKyId",
                principalTable: "YeuCauDangKy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NhaTuyenDung_YeuCauDangKy_YeuCauDangKyId",
                table: "NhaTuyenDung");

            migrationBuilder.DropIndex(
                name: "IX_NhaTuyenDung_YeuCauDangKyId",
                table: "NhaTuyenDung");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "CoSoDaoTaoApprovedEvents",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2026, 4, 20, 16, 41, 55, 559, DateTimeKind.Utc).AddTicks(2626),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2026, 4, 21, 8, 9, 16, 209, DateTimeKind.Utc).AddTicks(5032));
        }
    }
}
