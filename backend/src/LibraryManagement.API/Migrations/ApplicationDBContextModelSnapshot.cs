﻿// <auto-generated />
using System;
using LibraryManagement.API.Infrastructure.Persistence.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LibraryManagement.API.Migrations
{
    [DbContext(typeof(ApplicationDBContext))]
    partial class ApplicationDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("LibraryManagement.Core.Domains.Entities.Book", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("AvailableAmount")
                        .HasColumnType("int");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Books");

                    b.HasData(
                        new
                        {
                            Id = new Guid("33333333-3333-3333-3333-333333333333"),
                            Amount = 10,
                            Author = "Thomas H. Cormen",
                            AvailableAmount = 10,
                            CategoryId = new Guid("11111111-1111-1111-1111-111111111111"),
                            Name = "Introduction to Algorithms"
                        },
                        new
                        {
                            Id = new Guid("44444444-4444-4444-4444-444444444444"),
                            Amount = 5,
                            Author = "William Shakespeare",
                            AvailableAmount = 5,
                            CategoryId = new Guid("22222222-2222-2222-2222-222222222222"),
                            Name = "Hamlet"
                        },
                        new
                        {
                            Id = new Guid("55555555-5555-5555-5555-555555555555"),
                            Amount = 7,
                            Author = "Robert C. Martin",
                            AvailableAmount = 7,
                            CategoryId = new Guid("11111111-1111-1111-1111-111111111111"),
                            Name = "Clean Code"
                        },
                        new
                        {
                            Id = new Guid("66666666-6666-6666-6666-666666666666"),
                            Amount = 8,
                            Author = "George Orwell",
                            AvailableAmount = 8,
                            CategoryId = new Guid("22222222-2222-2222-2222-222222222222"),
                            Name = "1984"
                        },
                        new
                        {
                            Id = new Guid("77777777-7777-7777-7777-777777777777"),
                            Amount = 6,
                            Author = "Andrew Hunt",
                            AvailableAmount = 6,
                            CategoryId = new Guid("11111111-1111-1111-1111-111111111111"),
                            Name = "The Pragmatic Programmer"
                        },
                        new
                        {
                            Id = new Guid("88888888-8888-8888-8888-888888888888"),
                            Amount = 5,
                            Author = "Harper Lee",
                            AvailableAmount = 5,
                            CategoryId = new Guid("22222222-2222-2222-2222-222222222222"),
                            Name = "To Kill a Mockingbird"
                        },
                        new
                        {
                            Id = new Guid("99999999-9999-9999-9999-999999999999"),
                            Amount = 4,
                            Author = "Erich Gamma",
                            AvailableAmount = 4,
                            CategoryId = new Guid("11111111-1111-1111-1111-111111111111"),
                            Name = "Design Patterns"
                        },
                        new
                        {
                            Id = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                            Amount = 6,
                            Author = "Jane Austen",
                            AvailableAmount = 6,
                            CategoryId = new Guid("22222222-2222-2222-2222-222222222222"),
                            Name = "Pride and Prejudice"
                        },
                        new
                        {
                            Id = new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                            Amount = 5,
                            Author = "Abraham Silberschatz",
                            AvailableAmount = 5,
                            CategoryId = new Guid("11111111-1111-1111-1111-111111111111"),
                            Name = "Operating System Concepts"
                        },
                        new
                        {
                            Id = new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                            Amount = 3,
                            Author = "William Shakespeare",
                            AvailableAmount = 3,
                            CategoryId = new Guid("22222222-2222-2222-2222-222222222222"),
                            Name = "Macbeth"
                        });
                });

            modelBuilder.Entity("LibraryManagement.Core.Domains.Entities.BookBorrowingRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ApproverId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateRequested")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("RequestorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ApproverId");

                    b.HasIndex("RequestorId");

                    b.ToTable("BookBorrowingRequests");

                    b.HasData(
                        new
                        {
                            Id = new Guid("77777777-7777-7777-7777-777777777777"),
                            ApproverId = new Guid("66666666-6666-6666-6666-666666666666"),
                            DateRequested = new DateTime(2025, 4, 28, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            RequestorId = new Guid("55555555-5555-5555-5555-555555555555"),
                            Status = 0
                        },
                        new
                        {
                            Id = new Guid("88888888-8888-8888-8888-888888888888"),
                            DateRequested = new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            RequestorId = new Guid("99999999-9999-9999-9999-999999999999"),
                            Status = 2
                        },
                        new
                        {
                            Id = new Guid("99999999-9999-9999-9999-999999999999"),
                            ApproverId = new Guid("66666666-6666-6666-6666-666666666666"),
                            DateRequested = new DateTime(2025, 4, 25, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            RequestorId = new Guid("88888888-8888-8888-8888-888888888888"),
                            Status = 1
                        },
                        new
                        {
                            Id = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                            ApproverId = new Guid("77777777-7777-7777-7777-777777777777"),
                            DateRequested = new DateTime(2025, 4, 26, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            RequestorId = new Guid("55555555-5555-5555-5555-555555555555"),
                            Status = 0
                        },
                        new
                        {
                            Id = new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                            DateRequested = new DateTime(2025, 4, 29, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            RequestorId = new Guid("99999999-9999-9999-9999-999999999999"),
                            Status = 2
                        });
                });

            modelBuilder.Entity("LibraryManagement.Core.Domains.Entities.BookBorrowingRequestDetails", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BookId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RequestId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BookId");

                    b.HasIndex("RequestId");

                    b.ToTable("BooksBorrowingRequestDetails");

                    b.HasData(
                        new
                        {
                            Id = new Guid("88888888-8888-8888-8888-888888888888"),
                            BookId = new Guid("33333333-3333-3333-3333-333333333333"),
                            RequestId = new Guid("77777777-7777-7777-7777-777777777777"),
                            Status = 1
                        },
                        new
                        {
                            Id = new Guid("99999999-9999-9999-9999-999999999999"),
                            BookId = new Guid("55555555-5555-5555-5555-555555555555"),
                            RequestId = new Guid("77777777-7777-7777-7777-777777777777"),
                            Status = 1
                        },
                        new
                        {
                            Id = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                            BookId = new Guid("77777777-7777-7777-7777-777777777777"),
                            RequestId = new Guid("77777777-7777-7777-7777-777777777777"),
                            Status = 3
                        },
                        new
                        {
                            Id = new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                            BookId = new Guid("44444444-4444-4444-4444-444444444444"),
                            RequestId = new Guid("88888888-8888-8888-8888-888888888888"),
                            Status = 0
                        },
                        new
                        {
                            Id = new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                            BookId = new Guid("66666666-6666-6666-6666-666666666666"),
                            RequestId = new Guid("88888888-8888-8888-8888-888888888888"),
                            Status = 0
                        });
                });

            modelBuilder.Entity("LibraryManagement.Core.Domains.Entities.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<Guid>("RequestId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = new Guid("11111111-1111-1111-1111-111111111111"),
                            Name = "Computer Science",
                            RequestId = new Guid("00000000-0000-0000-0000-000000000000")
                        },
                        new
                        {
                            Id = new Guid("22222222-2222-2222-2222-222222222222"),
                            Name = "Literature",
                            RequestId = new Guid("00000000-0000-0000-0000-000000000000")
                        },
                        new
                        {
                            Id = new Guid("33333333-3333-3333-3333-333333333333"),
                            Name = "History",
                            RequestId = new Guid("00000000-0000-0000-0000-000000000000")
                        },
                        new
                        {
                            Id = new Guid("44444444-4444-4444-4444-444444444444"),
                            Name = "Mathematics",
                            RequestId = new Guid("00000000-0000-0000-0000-000000000000")
                        },
                        new
                        {
                            Id = new Guid("55555555-5555-5555-5555-555555555555"),
                            Name = "Science",
                            RequestId = new Guid("00000000-0000-0000-0000-000000000000")
                        },
                        new
                        {
                            Id = new Guid("66666666-6666-6666-6666-666666666666"),
                            Name = "Art",
                            RequestId = new Guid("00000000-0000-0000-0000-000000000000")
                        },
                        new
                        {
                            Id = new Guid("77777777-7777-7777-7777-777777777777"),
                            Name = "Philosophy",
                            RequestId = new Guid("00000000-0000-0000-0000-000000000000")
                        },
                        new
                        {
                            Id = new Guid("88888888-8888-8888-8888-888888888888"),
                            Name = "Engineering",
                            RequestId = new Guid("00000000-0000-0000-0000-000000000000")
                        },
                        new
                        {
                            Id = new Guid("99999999-9999-9999-9999-999999999999"),
                            Name = "Medicine",
                            RequestId = new Guid("00000000-0000-0000-0000-000000000000")
                        },
                        new
                        {
                            Id = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                            Name = "Business",
                            RequestId = new Guid("00000000-0000-0000-0000-000000000000")
                        });
                });

            modelBuilder.Entity("LibraryManagement.Core.Domains.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("HashedPassword")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("66666666-6666-6666-6666-666666666666"),
                            HashedPassword = "AQAAAAIAAYagAAAAEDz3ec3OjOMKLtYlR92qaODZ+/CdM8N+Gw5XGDS7Gmx5RRXK159MYZDJxhOWD4LCXA==",
                            Name = "admin",
                            Type = 0
                        },
                        new
                        {
                            Id = new Guid("55555555-5555-5555-5555-555555555555"),
                            HashedPassword = "AQAAAAIAAYagAAAAEJmlkalhsfhRYjDhXw2VjZo4cbcCabe3gtBP4CkGHfrzxq7k4vpZBkI6K+ZL41t9Uw==",
                            Name = "user1",
                            Type = 1
                        },
                        new
                        {
                            Id = new Guid("77777777-7777-7777-7777-777777777777"),
                            HashedPassword = "AQAAAAIAAYagAAAAEMueJ9ZyYHM8B7yTUgHXZG5QfePLHWfrqbftJBxaPrx+q1POstM5JdGrAa9Ep3nIPQ==",
                            Name = "supervisor",
                            Type = 0
                        },
                        new
                        {
                            Id = new Guid("88888888-8888-8888-8888-888888888888"),
                            HashedPassword = "AQAAAAIAAYagAAAAEGMvGaroT+yywY8Ys6U9ie/lzeeLnKWXbc/WG+P0J1WDS6c3K6ZID6hTpkqcck1Djg==",
                            Name = "user2",
                            Type = 1
                        },
                        new
                        {
                            Id = new Guid("99999999-9999-9999-9999-999999999999"),
                            HashedPassword = "AQAAAAIAAYagAAAAEHJQvDm+ISY/LYdr0IM/2mjUbKpCROhvH+zOb8RqbEvaThBZstTP+JzY0i1kc9Cpew==",
                            Name = "user3",
                            Type = 1
                        },
                        new
                        {
                            Id = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                            HashedPassword = "AQAAAAIAAYagAAAAENwC+2FP/hI/4iGAuEwkZdgAw76FMo3DKN6zjfRa2RDAGJBQAGeFpisHOoBLTjKnLw==",
                            Name = "manager",
                            Type = 0
                        });
                });

            modelBuilder.Entity("LibraryManagement.Core.Domains.Entities.Book", b =>
                {
                    b.HasOne("LibraryManagement.Core.Domains.Entities.Category", "BookCategory")
                        .WithMany("Books")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BookCategory");
                });

            modelBuilder.Entity("LibraryManagement.Core.Domains.Entities.BookBorrowingRequest", b =>
                {
                    b.HasOne("LibraryManagement.Core.Domains.Entities.User", "Approver")
                        .WithMany("RequestsToApprove")
                        .HasForeignKey("ApproverId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("LibraryManagement.Core.Domains.Entities.User", "Requestor")
                        .WithMany("BorrowingRequests")
                        .HasForeignKey("RequestorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Approver");

                    b.Navigation("Requestor");
                });

            modelBuilder.Entity("LibraryManagement.Core.Domains.Entities.BookBorrowingRequestDetails", b =>
                {
                    b.HasOne("LibraryManagement.Core.Domains.Entities.Book", "BorrowBook")
                        .WithMany("RequestDetails")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LibraryManagement.Core.Domains.Entities.BookBorrowingRequest", "BookBorrowingRequest")
                        .WithMany("BookBorrowingRequestDetails")
                        .HasForeignKey("RequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BookBorrowingRequest");

                    b.Navigation("BorrowBook");
                });

            modelBuilder.Entity("LibraryManagement.Core.Domains.Entities.Book", b =>
                {
                    b.Navigation("RequestDetails");
                });

            modelBuilder.Entity("LibraryManagement.Core.Domains.Entities.BookBorrowingRequest", b =>
                {
                    b.Navigation("BookBorrowingRequestDetails");
                });

            modelBuilder.Entity("LibraryManagement.Core.Domains.Entities.Category", b =>
                {
                    b.Navigation("Books");
                });

            modelBuilder.Entity("LibraryManagement.Core.Domains.Entities.User", b =>
                {
                    b.Navigation("BorrowingRequests");

                    b.Navigation("RequestsToApprove");
                });
#pragma warning restore 612, 618
        }
    }
}
