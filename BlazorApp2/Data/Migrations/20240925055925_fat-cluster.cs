using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorApp2.Migrations
{
    /// <inheritdoc />
    public partial class fatcluster : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUploaded",
                table: "Crimes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 9, 25, 13, 59, 22, 903, DateTimeKind.Local).AddTicks(2562),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 9, 24, 17, 15, 26, 970, DateTimeKind.Local).AddTicks(7363));

            migrationBuilder.CreateTable(
                name: "FatClusters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseID = table.Column<int>(type: "int", nullable: false),
                    CrimeType = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<int>(type: "int", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Severity = table.Column<int>(type: "int", nullable: false),
                    VictimCount = table.Column<int>(type: "int", nullable: false),
                    ArrestMade = table.Column<int>(type: "int", nullable: false),
                    ArrestDate = table.Column<int>(type: "int", nullable: false),
                    ResponseTimeInMinutes = table.Column<int>(type: "int", nullable: false),
                    PoliceDistrict = table.Column<int>(type: "int", nullable: false),
                    WeatherCondition = table.Column<int>(type: "int", nullable: false),
                    CrimeMotive = table.Column<int>(type: "int", nullable: false),
                    NearbyLandmarkLatitude = table.Column<double>(type: "float", nullable: false),
                    NearbyLandmarkLongitude = table.Column<double>(type: "float", nullable: false),
                    RecurringIncident = table.Column<int>(type: "int", nullable: false),
                    PopulationDensityPerSqKm = table.Column<int>(type: "int", nullable: false),
                    UnemploymentRate = table.Column<double>(type: "float", nullable: false),
                    MedianIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProximityToPoliceStationInKm = table.Column<double>(type: "float", nullable: false),
                    StreetLightPresent = table.Column<bool>(type: "bit", nullable: false),
                    CCTVCoverage = table.Column<bool>(type: "bit", nullable: false),
                    AlcoholOrDrugInvolvement = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FatClusters", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FatClusters");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUploaded",
                table: "Crimes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 9, 24, 17, 15, 26, 970, DateTimeKind.Local).AddTicks(7363),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 9, 25, 13, 59, 22, 903, DateTimeKind.Local).AddTicks(2562));
        }
    }
}
