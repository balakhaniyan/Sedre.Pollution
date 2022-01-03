﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sedre.Pollution.Infrastructure.Migrations
{
    public partial class Init7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Indicators");

            migrationBuilder.CreateTable(
                name: "HourIndicators",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<int>(type: "int", nullable: false),
                    ALatitude = table.Column<double>(type: "float", nullable: false),
                    ALongitude = table.Column<double>(type: "float", nullable: false),
                    BLatitude = table.Column<double>(type: "float", nullable: false),
                    BLongitude = table.Column<double>(type: "float", nullable: false),
                    CLatitude = table.Column<double>(type: "float", nullable: false),
                    CLongitude = table.Column<double>(type: "float", nullable: false),
                    DLatitude = table.Column<double>(type: "float", nullable: false),
                    DLongitude = table.Column<double>(type: "float", nullable: false),
                    O3 = table.Column<double>(type: "float", nullable: false),
                    Co = table.Column<double>(type: "float", nullable: false),
                    No2 = table.Column<double>(type: "float", nullable: false),
                    So2 = table.Column<double>(type: "float", nullable: false),
                    Pm10 = table.Column<double>(type: "float", nullable: false),
                    Pm25 = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HourIndicators", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HourIndicators");

            migrationBuilder.CreateTable(
                name: "Indicators",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ALatitude = table.Column<double>(type: "float", nullable: false),
                    ALongitude = table.Column<double>(type: "float", nullable: false),
                    BLatitude = table.Column<double>(type: "float", nullable: false),
                    BLongitude = table.Column<double>(type: "float", nullable: false),
                    CLatitude = table.Column<double>(type: "float", nullable: false),
                    CLongitude = table.Column<double>(type: "float", nullable: false),
                    Co = table.Column<double>(type: "float", nullable: false),
                    DLatitude = table.Column<double>(type: "float", nullable: false),
                    DLongitude = table.Column<double>(type: "float", nullable: false),
                    Date = table.Column<int>(type: "int", nullable: false),
                    No2 = table.Column<double>(type: "float", nullable: false),
                    O3 = table.Column<double>(type: "float", nullable: false),
                    Pm10 = table.Column<double>(type: "float", nullable: false),
                    Pm25 = table.Column<double>(type: "float", nullable: false),
                    So2 = table.Column<double>(type: "float", nullable: false),
                    Time = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Indicators", x => x.Id);
                });
        }
    }
}
