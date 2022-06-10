using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Operativka.Migrations.OperativkaIdentity
{
    public partial class AddedProfilePicture : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ProfilePicture",
                schema: "Identity",
                table: "User",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<int>(
                name: "UsernameChangeLimit",
                schema: "Identity",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePicture",
                schema: "Identity",
                table: "User");

            migrationBuilder.DropColumn(
                name: "UsernameChangeLimit",
                schema: "Identity",
                table: "User");
        }
    }
}
