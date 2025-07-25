using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkingLot.Migrations
{
    /// <inheritdoc />
    public partial class FixPricePropertyType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Logs_Subscriptions_SubscriptionId",
                table: "Logs");

            migrationBuilder.DropIndex(
                name: "IX_Logs_Code",
                table: "Logs");

            migrationBuilder.AddColumn<string>(
                name: "PlateNumber",
                table: "Subscriptions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "SubscriptionId",
                table: "Logs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Logs",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "PlateNumber",
                table: "Logs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Logs_Code",
                table: "Logs",
                column: "Code",
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_Subscriptions_SubscriptionId",
                table: "Logs",
                column: "SubscriptionId",
                principalTable: "Subscriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Logs_Subscriptions_SubscriptionId",
                table: "Logs");

            migrationBuilder.DropIndex(
                name: "IX_Logs_Code",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "PlateNumber",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "PlateNumber",
                table: "Logs");

            migrationBuilder.AlterColumn<int>(
                name: "SubscriptionId",
                table: "Logs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Logs",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Logs_Code",
                table: "Logs",
                column: "Code",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_Subscriptions_SubscriptionId",
                table: "Logs",
                column: "SubscriptionId",
                principalTable: "Subscriptions",
                principalColumn: "Id");
        }
    }
}
