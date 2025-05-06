using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LibraryManagement.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    HashedPassword = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Author = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    AvailableAmount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Books_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookBorrowingRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateRequested = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RequestorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApproverId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookBorrowingRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookBorrowingRequests_Users_ApproverId",
                        column: x => x.ApproverId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BookBorrowingRequests_Users_RequestorId",
                        column: x => x.RequestorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BooksBorrowingRequestDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BooksBorrowingRequestDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BooksBorrowingRequestDetails_BookBorrowingRequests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "BookBorrowingRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BooksBorrowingRequestDetails_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name", "RequestId" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Computer Science", new Guid("00000000-0000-0000-0000-000000000000") },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "Literature", new Guid("00000000-0000-0000-0000-000000000000") },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "History", new Guid("00000000-0000-0000-0000-000000000000") },
                    { new Guid("44444444-4444-4444-4444-444444444444"), "Mathematics", new Guid("00000000-0000-0000-0000-000000000000") },
                    { new Guid("55555555-5555-5555-5555-555555555555"), "Science", new Guid("00000000-0000-0000-0000-000000000000") },
                    { new Guid("66666666-6666-6666-6666-666666666666"), "Art", new Guid("00000000-0000-0000-0000-000000000000") },
                    { new Guid("77777777-7777-7777-7777-777777777777"), "Philosophy", new Guid("00000000-0000-0000-0000-000000000000") },
                    { new Guid("88888888-8888-8888-8888-888888888888"), "Engineering", new Guid("00000000-0000-0000-0000-000000000000") },
                    { new Guid("99999999-9999-9999-9999-999999999999"), "Medicine", new Guid("00000000-0000-0000-0000-000000000000") },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "Business", new Guid("00000000-0000-0000-0000-000000000000") }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "HashedPassword", "Name", "Type" },
                values: new object[,]
                {
                    { new Guid("55555555-5555-5555-5555-555555555555"), "AQAAAAIAAYagAAAAEJmlkalhsfhRYjDhXw2VjZo4cbcCabe3gtBP4CkGHfrzxq7k4vpZBkI6K+ZL41t9Uw==", "user1", 1 },
                    { new Guid("66666666-6666-6666-6666-666666666666"), "AQAAAAIAAYagAAAAEDz3ec3OjOMKLtYlR92qaODZ+/CdM8N+Gw5XGDS7Gmx5RRXK159MYZDJxhOWD4LCXA==", "admin", 0 },
                    { new Guid("77777777-7777-7777-7777-777777777777"), "AQAAAAIAAYagAAAAEMueJ9ZyYHM8B7yTUgHXZG5QfePLHWfrqbftJBxaPrx+q1POstM5JdGrAa9Ep3nIPQ==", "supervisor", 0 },
                    { new Guid("88888888-8888-8888-8888-888888888888"), "AQAAAAIAAYagAAAAEGMvGaroT+yywY8Ys6U9ie/lzeeLnKWXbc/WG+P0J1WDS6c3K6ZID6hTpkqcck1Djg==", "user2", 1 },
                    { new Guid("99999999-9999-9999-9999-999999999999"), "AQAAAAIAAYagAAAAEHJQvDm+ISY/LYdr0IM/2mjUbKpCROhvH+zOb8RqbEvaThBZstTP+JzY0i1kc9Cpew==", "user3", 1 },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "AQAAAAIAAYagAAAAENwC+2FP/hI/4iGAuEwkZdgAw76FMo3DKN6zjfRa2RDAGJBQAGeFpisHOoBLTjKnLw==", "manager", 0 }
                });

            migrationBuilder.InsertData(
                table: "BookBorrowingRequests",
                columns: new[] { "Id", "ApproverId", "DateRequested", "RequestorId", "Status" },
                values: new object[,]
                {
                    { new Guid("77777777-7777-7777-7777-777777777777"), new Guid("66666666-6666-6666-6666-666666666666"), new DateTime(2025, 4, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("55555555-5555-5555-5555-555555555555"), 0 },
                    { new Guid("88888888-8888-8888-8888-888888888888"), null, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("99999999-9999-9999-9999-999999999999"), 2 },
                    { new Guid("99999999-9999-9999-9999-999999999999"), new Guid("66666666-6666-6666-6666-666666666666"), new DateTime(2025, 4, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("88888888-8888-8888-8888-888888888888"), 1 },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("77777777-7777-7777-7777-777777777777"), new DateTime(2025, 4, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("55555555-5555-5555-5555-555555555555"), 0 },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), null, new DateTime(2025, 4, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("99999999-9999-9999-9999-999999999999"), 2 }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Amount", "Author", "AvailableAmount", "CategoryId", "Name" },
                values: new object[,]
                {
                    { new Guid("33333333-3333-3333-3333-333333333333"), 10, "Thomas H. Cormen", 10, new Guid("11111111-1111-1111-1111-111111111111"), "Introduction to Algorithms" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), 5, "William Shakespeare", 5, new Guid("22222222-2222-2222-2222-222222222222"), "Hamlet" },
                    { new Guid("55555555-5555-5555-5555-555555555555"), 7, "Robert C. Martin", 7, new Guid("11111111-1111-1111-1111-111111111111"), "Clean Code" },
                    { new Guid("66666666-6666-6666-6666-666666666666"), 8, "George Orwell", 8, new Guid("22222222-2222-2222-2222-222222222222"), "1984" },
                    { new Guid("77777777-7777-7777-7777-777777777777"), 6, "Andrew Hunt", 6, new Guid("11111111-1111-1111-1111-111111111111"), "The Pragmatic Programmer" },
                    { new Guid("88888888-8888-8888-8888-888888888888"), 5, "Harper Lee", 5, new Guid("22222222-2222-2222-2222-222222222222"), "To Kill a Mockingbird" },
                    { new Guid("99999999-9999-9999-9999-999999999999"), 4, "Erich Gamma", 4, new Guid("11111111-1111-1111-1111-111111111111"), "Design Patterns" },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), 6, "Jane Austen", 6, new Guid("22222222-2222-2222-2222-222222222222"), "Pride and Prejudice" },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), 5, "Abraham Silberschatz", 5, new Guid("11111111-1111-1111-1111-111111111111"), "Operating System Concepts" },
                    { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), 3, "William Shakespeare", 3, new Guid("22222222-2222-2222-2222-222222222222"), "Macbeth" }
                });

            migrationBuilder.InsertData(
                table: "BooksBorrowingRequestDetails",
                columns: new[] { "Id", "BookId", "RequestId", "Status" },
                values: new object[,]
                {
                    { new Guid("88888888-8888-8888-8888-888888888888"), new Guid("33333333-3333-3333-3333-333333333333"), new Guid("77777777-7777-7777-7777-777777777777"), 1 },
                    { new Guid("99999999-9999-9999-9999-999999999999"), new Guid("55555555-5555-5555-5555-555555555555"), new Guid("77777777-7777-7777-7777-777777777777"), 1 },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new Guid("77777777-7777-7777-7777-777777777777"), new Guid("77777777-7777-7777-7777-777777777777"), 3 },
                    { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), new Guid("44444444-4444-4444-4444-444444444444"), new Guid("88888888-8888-8888-8888-888888888888"), 0 },
                    { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), new Guid("66666666-6666-6666-6666-666666666666"), new Guid("88888888-8888-8888-8888-888888888888"), 0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookBorrowingRequests_ApproverId",
                table: "BookBorrowingRequests",
                column: "ApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_BookBorrowingRequests_RequestorId",
                table: "BookBorrowingRequests",
                column: "RequestorId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_CategoryId",
                table: "Books",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BooksBorrowingRequestDetails_BookId",
                table: "BooksBorrowingRequestDetails",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_BooksBorrowingRequestDetails_RequestId",
                table: "BooksBorrowingRequestDetails",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Name",
                table: "Users",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BooksBorrowingRequestDetails");

            migrationBuilder.DropTable(
                name: "BookBorrowingRequests");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
