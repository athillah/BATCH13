using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FilmAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialMerge : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cef63438-a129-4aac-b5aa-3040af930629");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fb6723ef-0c66-489a-a1ca-ae69b338e9cf");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "7e8ac59d-65b9-4b7c-96e3-4ac1a457d286", "b06a70f0-70e9-40b8-bf59-1d355eace5e9" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7e8ac59d-65b9-4b7c-96e3-4ac1a457d286");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b06a70f0-70e9-40b8-bf59-1d355eace5e9");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "22d0ecbc-aa58-4140-a927-eabcc78e3703", null, "Admin", "ADMIN" },
                    { "403893d0-1f61-4b5f-8437-6a30850eb3cb", null, "User", "USER" },
                    { "ec03891e-5bf8-4345-af96-93cffa52adc8", null, "Manager", "MANAGER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "408f7dd7-6d13-4809-91de-c329ba5b8f15", 0, "b4dff035-8f3c-4a93-a3d0-084862585e4c", new DateTime(2025, 6, 16, 4, 34, 40, 6, DateTimeKind.Utc).AddTicks(6603), "admin@jwtauth.com", true, "System", "Administrator", false, null, "ADMIN@JWTAUTH.COM", "ADMIN@JWTAUTH.COM", "AQAAAAIAAYagAAAAEPFGgGRBdI8rw04mQ3es1mjAiisOX8kaRhoOnuab8nXqbAFRB2GUEUPa9Swrbz5cyg==", null, false, "e2d7f1e7-8a16-4954-bfea-e36ffe1e2c73", false, "admin@jwtauth.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "22d0ecbc-aa58-4140-a927-eabcc78e3703", "408f7dd7-6d13-4809-91de-c329ba5b8f15" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "403893d0-1f61-4b5f-8437-6a30850eb3cb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ec03891e-5bf8-4345-af96-93cffa52adc8");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "22d0ecbc-aa58-4140-a927-eabcc78e3703", "408f7dd7-6d13-4809-91de-c329ba5b8f15" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "22d0ecbc-aa58-4140-a927-eabcc78e3703");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "408f7dd7-6d13-4809-91de-c329ba5b8f15");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7e8ac59d-65b9-4b7c-96e3-4ac1a457d286", null, "Admin", "ADMIN" },
                    { "cef63438-a129-4aac-b5aa-3040af930629", null, "Manager", "MANAGER" },
                    { "fb6723ef-0c66-489a-a1ca-ae69b338e9cf", null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "b06a70f0-70e9-40b8-bf59-1d355eace5e9", 0, "69ca169c-abef-4e36-b87e-6411d8317fb8", new DateTime(2025, 6, 16, 3, 56, 11, 888, DateTimeKind.Utc).AddTicks(2898), "admin@jwtauth.com", true, "System", "Administrator", false, null, "ADMIN@JWTAUTH.COM", "ADMIN@JWTAUTH.COM", "AQAAAAIAAYagAAAAEA5CtJ0z6IeVaGVZ0E7ylP3Ajbi+CdzxqMBQttWgkV4/VNL74PoDjzyg35lmmAEKsw==", null, false, "114b1eb5-18f4-4c85-a551-4193ab1e519d", false, "admin@jwtauth.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "7e8ac59d-65b9-4b7c-96e3-4ac1a457d286", "b06a70f0-70e9-40b8-bf59-1d355eace5e9" });
        }
    }
}
