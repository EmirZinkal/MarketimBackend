using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePasswordResetTokensTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PasswordResetTokens_TokenHash",
                table: "PasswordResetTokens");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "PasswordResetTokens");

            migrationBuilder.DropColumn(
                name: "IpAddress",
                table: "PasswordResetTokens");

            migrationBuilder.DropColumn(
                name: "TokenHash",
                table: "PasswordResetTokens");

            migrationBuilder.DropColumn(
                name: "UsedAt",
                table: "PasswordResetTokens");

            migrationBuilder.RenameColumn(
                name: "ExpireAt",
                table: "PasswordResetTokens",
                newName: "ExpireDate");

            migrationBuilder.AlterColumn<bool>(
                name: "IsUsed",
                table: "PasswordResetTokens",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "PasswordResetTokens",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Token",
                table: "PasswordResetTokens");

            migrationBuilder.RenameColumn(
                name: "ExpireDate",
                table: "PasswordResetTokens",
                newName: "ExpireAt");

            migrationBuilder.AlterColumn<bool>(
                name: "IsUsed",
                table: "PasswordResetTokens",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "PasswordResetTokens",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "IpAddress",
                table: "PasswordResetTokens",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TokenHash",
                table: "PasswordResetTokens",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UsedAt",
                table: "PasswordResetTokens",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetTokens_TokenHash",
                table: "PasswordResetTokens",
                column: "TokenHash",
                unique: true);
        }
    }
}
