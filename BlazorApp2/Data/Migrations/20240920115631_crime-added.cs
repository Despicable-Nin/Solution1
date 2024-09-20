using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorApp2.Migrations
{
    /// <inheritdoc />
    public partial class crimeadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Crimes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaseID = table.Column<int>(type: "int", nullable: false),
                    CrimeType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Time = table.Column<TimeSpan>(type: "time", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Severity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WeaponUsed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VictimCount = table.Column<int>(type: "int", nullable: false),
                    SuspectDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ArrestMade = table.Column<bool>(type: "bit", nullable: false),
                    ArrestDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResponseTimeInMinutes = table.Column<int>(type: "int", nullable: false),
                    PoliceDistrict = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WeatherCondition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CrimeMotive = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NearbyLandmarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecurringIncident = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_Crimes", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Crimes");
        }
    }
}
