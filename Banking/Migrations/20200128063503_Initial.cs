using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Banking.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    CustomerID = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Address = table.Column<string>(maxLength: 50, nullable: true),
                    City = table.Column<string>(maxLength: 40, nullable: true),
                    State = table.Column<string>(maxLength: 3, nullable: true),
                    PostCode = table.Column<string>(maxLength: 4, nullable: true),
                    Phone = table.Column<string>(maxLength: 9, nullable: false),
                    TFN = table.Column<string>(maxLength: 11, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.CustomerID);
                });

            migrationBuilder.CreateTable(
                name: "Payee",
                columns: table => new
                {
                    PayeeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Address = table.Column<string>(maxLength: 50, nullable: true),
                    City = table.Column<string>(maxLength: 40, nullable: true),
                    State = table.Column<string>(maxLength: 3, nullable: true),
                    PostCode = table.Column<string>(maxLength: 4, nullable: true),
                    Phone = table.Column<string>(maxLength: 9, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payee", x => x.PayeeID);
                });

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    AccountNumber = table.Column<int>(nullable: false),
                    ModifyDate = table.Column<DateTime>(nullable: false),
                    AccountType = table.Column<int>(nullable: false),
                    CustomerID = table.Column<int>(nullable: false),
                    Balance = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.AccountNumber);
                    table.ForeignKey(
                        name: "FK_Account_Customer_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Login",
                columns: table => new
                {
                    UserID = table.Column<string>(maxLength: 8, nullable: false),
                    ModifyDate = table.Column<DateTime>(nullable: false),
                    PasswordHash = table.Column<string>(maxLength: 64, nullable: false),
                    CustomerID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Login", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_Login_Customer_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BillPay",
                columns: table => new
                {
                    BillPayID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifyDate = table.Column<DateTime>(nullable: false),
                    AccountNumber = table.Column<int>(nullable: false),
                    PayeeID = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    ScheduleDate = table.Column<DateTime>(nullable: false),
                    Period = table.Column<int>(maxLength: 1, nullable: false),
                    Comment = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillPay", x => x.BillPayID);
                    table.ForeignKey(
                        name: "FK_BillPay_Account_AccountNumber",
                        column: x => x.AccountNumber,
                        principalTable: "Account",
                        principalColumn: "AccountNumber",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BillPay_Payee_PayeeID",
                        column: x => x.PayeeID,
                        principalTable: "Payee",
                        principalColumn: "PayeeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    TransactionID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifyDate = table.Column<DateTime>(nullable: false),
                    TransactionType = table.Column<int>(nullable: false),
                    AccountNumber = table.Column<int>(nullable: false),
                    DestAccountNumber = table.Column<int>(nullable: true),
                    Amount = table.Column<decimal>(nullable: false),
                    Comment = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.TransactionID);
                    table.ForeignKey(
                        name: "FK_Transaction_Account_AccountNumber",
                        column: x => x.AccountNumber,
                        principalTable: "Account",
                        principalColumn: "AccountNumber",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transaction_Account_DestAccountNumber",
                        column: x => x.DestAccountNumber,
                        principalTable: "Account",
                        principalColumn: "AccountNumber",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_CustomerID",
                table: "Account",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_BillPay_AccountNumber",
                table: "BillPay",
                column: "AccountNumber");

            migrationBuilder.CreateIndex(
                name: "IX_BillPay_PayeeID",
                table: "BillPay",
                column: "PayeeID");

            migrationBuilder.CreateIndex(
                name: "IX_Login_CustomerID",
                table: "Login",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_AccountNumber",
                table: "Transaction",
                column: "AccountNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_DestAccountNumber",
                table: "Transaction",
                column: "DestAccountNumber");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BillPay");

            migrationBuilder.DropTable(
                name: "Login");

            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropTable(
                name: "Payee");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "Customer");
        }
    }
}
