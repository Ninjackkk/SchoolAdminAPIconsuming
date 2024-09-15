﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SchoolAdminAPIconsuming.Data;

#nullable disable

namespace SchoolAdminAPIconsuming.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240915125210_Leavecolumnn")]
    partial class Leavecolumnn
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SchoolAdminAPIconsuming.Models.Accountant", b =>
                {
                    b.Property<int>("Accountant_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Accountant_id"));

                    b.Property<string>("Accountant_email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Accountant_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Accountant_id");

                    b.ToTable("Accountants");
                });

            modelBuilder.Entity("SchoolAdminAPIconsuming.Models.Assignment", b =>
                {
                    b.Property<int>("AssignmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AssignmentId"));

                    b.Property<DateTime>("AssignmentDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("AssignmentFile")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AssignmentName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("Deadline")
                        .HasColumnType("datetime2");

                    b.Property<string>("GivenBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StdId")
                        .HasColumnType("int");

                    b.HasKey("AssignmentId");

                    b.HasIndex("StdId");

                    b.ToTable("Assignments");
                });

            modelBuilder.Entity("SchoolAdminAPIconsuming.Models.Attendance", b =>
                {
                    b.Property<int>("AttendanceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AttendanceId"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsPresent")
                        .HasColumnType("bit");

                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.HasKey("AttendanceId");

                    b.HasIndex("StudentId");

                    b.ToTable("Attendances");
                });

            modelBuilder.Entity("SchoolAdminAPIconsuming.Models.LeaveRequest", b =>
                {
                    b.Property<int>("LeaveApplicationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LeaveApplicationId"));

                    b.Property<DateTime>("FromDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TeacherId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ToDate")
                        .HasColumnType("datetime2");

                    b.HasKey("LeaveApplicationId");

                    b.ToTable("LeaveRequests");
                });

            modelBuilder.Entity("SchoolAdminAPIconsuming.Models.Librarian", b =>
                {
                    b.Property<int>("Librarian_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Librarian_id"));

                    b.Property<string>("Librarian_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Librarian_id");

                    b.ToTable("Librarians");
                });

            modelBuilder.Entity("SchoolAdminAPIconsuming.Models.OnlineApplication", b =>
                {
                    b.Property<int>("OnlineApplicationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OnlineApplicationID"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ApplyingForSTD")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Parent_Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Parent_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StudentName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("OnlineApplicationID");

                    b.ToTable("OnlineApplications");
                });

            modelBuilder.Entity("SchoolAdminAPIconsuming.Models.STD", b =>
                {
                    b.Property<int>("StdId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StdId"));

                    b.Property<string>("StdName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("StdId");

                    b.ToTable("STDs");
                });

            modelBuilder.Entity("SchoolAdminAPIconsuming.Models.Student", b =>
                {
                    b.Property<int>("StudentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StudentId"));

                    b.Property<string>("FeesStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Parent_Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("STD")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("StdId")
                        .HasColumnType("int");

                    b.Property<string>("StudentName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("StudentId");

                    b.HasIndex("StdId");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("SchoolAdminAPIconsuming.Models.SystemAdmin", b =>
                {
                    b.Property<int>("SystemAdmin_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SystemAdmin_id"));

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SystemAdmin_email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SystemAdmin_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SystemAdmin_id");

                    b.ToTable("SystemAdmins");
                });

            modelBuilder.Entity("SchoolAdminAPIconsuming.Models.Teacher", b =>
                {
                    b.Property<int>("TeacherId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TeacherId"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("HireDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("MonthlySalary")
                        .HasColumnType("float");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Qualification")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("STD")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("StdId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TeacherId");

                    b.HasIndex("StdId");

                    b.ToTable("Teachers");
                });

            modelBuilder.Entity("SchoolAdminAPIconsuming.Models.Assignment", b =>
                {
                    b.HasOne("SchoolAdminAPIconsuming.Models.STD", "STD")
                        .WithMany("Assignments")
                        .HasForeignKey("StdId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("STD");
                });

            modelBuilder.Entity("SchoolAdminAPIconsuming.Models.Attendance", b =>
                {
                    b.HasOne("SchoolAdminAPIconsuming.Models.Student", "Student")
                        .WithMany()
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Student");
                });

            modelBuilder.Entity("SchoolAdminAPIconsuming.Models.Student", b =>
                {
                    b.HasOne("SchoolAdminAPIconsuming.Models.STD", null)
                        .WithMany("Students")
                        .HasForeignKey("StdId");
                });

            modelBuilder.Entity("SchoolAdminAPIconsuming.Models.Teacher", b =>
                {
                    b.HasOne("SchoolAdminAPIconsuming.Models.STD", null)
                        .WithMany("Teachers")
                        .HasForeignKey("StdId");
                });

            modelBuilder.Entity("SchoolAdminAPIconsuming.Models.STD", b =>
                {
                    b.Navigation("Assignments");

                    b.Navigation("Students");

                    b.Navigation("Teachers");
                });
#pragma warning restore 612, 618
        }
    }
}
