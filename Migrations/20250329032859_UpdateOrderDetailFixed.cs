using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TranThienTrung2122110179.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderDetailFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPrice",
                table: "OrderDetails",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldComputedColumnSql: "[Quantity] * [UnitPrice]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPrice",
                table: "OrderDetails",
                type: "decimal(18,2)",
                nullable: false,
                computedColumnSql: "[Quantity] * [UnitPrice]",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }
    }
}
