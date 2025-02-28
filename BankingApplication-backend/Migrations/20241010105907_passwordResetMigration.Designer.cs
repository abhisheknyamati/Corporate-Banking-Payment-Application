﻿// <auto-generated />
using System;
using BankingApplication_backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BankingApplication_backend.Migrations
{
    [DbContext(typeof(BankingAppDbContext))]
    [Migration("20241010105907_passwordResetMigration")]
    partial class passwordResetMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BankingApplication_backend.Models.Account", b =>
                {
                    b.Property<int>("AccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AccountId"));

                    b.Property<int>("AccountBalance")
                        .HasColumnType("int");

                    b.Property<int>("AccountNumber")
                        .HasColumnType("int");

                    b.Property<string>("IFSC")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AccountId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("BankingApplication_backend.Models.Admin", b =>
                {
                    b.Property<int>("AdminId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AdminId"));

                    b.Property<string>("AdminEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AdminName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AdminPassword")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("AdminId");

                    b.HasIndex("UserId");

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("BankingApplication_backend.Models.Bank", b =>
                {
                    b.Property<int>("BankId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BankId"));

                    b.Property<string>("BankEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BankName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BankPassword")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("IsApproved")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("BankId");

                    b.HasIndex("UserId");

                    b.ToTable("Banks");
                });

            modelBuilder.Entity("BankingApplication_backend.Models.BeneficiaryTransaction", b =>
                {
                    b.Property<int>("TransactionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TransactionId"));

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<DateTime>("BeneficiaryTransactionDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("InboundId")
                        .HasColumnType("int");

                    b.Property<int>("InitiatorOrgId")
                        .HasColumnType("int");

                    b.Property<string>("IsApproved")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("OutboundId")
                        .HasColumnType("int");

                    b.HasKey("TransactionId");

                    b.HasIndex("InboundId")
                        .IsUnique()
                        .HasFilter("[InboundId] IS NOT NULL");

                    b.HasIndex("OutboundId")
                        .IsUnique()
                        .HasFilter("[OutboundId] IS NOT NULL");

                    b.ToTable("BeneficiaryTransactions");
                });

            modelBuilder.Entity("BankingApplication_backend.Models.Creds", b =>
                {
                    b.Property<int>("CredId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CredId"));

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CredId");

                    b.HasIndex("UserId");

                    b.ToTable("Credentials");
                });

            modelBuilder.Entity("BankingApplication_backend.Models.Document", b =>
                {
                    b.Property<int>("DocumentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DocumentId"));

                    b.Property<int?>("BankId")
                        .HasColumnType("int");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("OrganisationId")
                        .HasColumnType("int");

                    b.HasKey("DocumentId");

                    b.HasIndex("BankId")
                        .IsUnique()
                        .HasFilter("[BankId] IS NOT NULL");

                    b.HasIndex("OrganisationId")
                        .IsUnique()
                        .HasFilter("[OrganisationId] IS NOT NULL");

                    b.ToTable("Documents");
                });

            modelBuilder.Entity("BankingApplication_backend.Models.Download", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<byte[]>("Content")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Downloads");
                });

            modelBuilder.Entity("BankingApplication_backend.Models.EmpTransaction", b =>
                {
                    b.Property<int>("EmpTransactionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EmpTransactionId"));

                    b.Property<int>("AccountNumber")
                        .HasColumnType("int");

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<string>("EmployeeEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<string>("EmployeeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EmployeeTransactionDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("IFSC")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IsApproved")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OrgID")
                        .HasColumnType("int");

                    b.HasKey("EmpTransactionId");

                    b.HasIndex("OrgID");

                    b.ToTable("EmpTransactions");
                });

            modelBuilder.Entity("BankingApplication_backend.Models.Employee", b =>
                {
                    b.Property<int>("EmployeeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EmployeeId"));

                    b.Property<int>("AccountNumber")
                        .HasColumnType("int");

                    b.Property<string>("EmployeeEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EmployeeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EmployeeSalary")
                        .HasColumnType("int");

                    b.Property<string>("IFSC")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int>("OrganisationId")
                        .HasColumnType("int");

                    b.HasKey("EmployeeId");

                    b.HasIndex("OrganisationId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("BankingApplication_backend.Models.Inbound", b =>
                {
                    b.Property<int>("InboundId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("InboundId"));

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<int?>("InboundBankId")
                        .HasColumnType("int");

                    b.Property<string>("InboundEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InboundFounderName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InboundName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IsApproved")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("InboundId");

                    b.HasIndex("AccountId");

                    b.HasIndex("InboundBankId");

                    b.ToTable("InboundOrgs");
                });

            modelBuilder.Entity("BankingApplication_backend.Models.Organisation", b =>
                {
                    b.Property<int>("OrganisationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrganisationId"));

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<int?>("BankId")
                        .HasColumnType("int");

                    b.Property<string>("BankName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FounderName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("IsApproved")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OrganisationEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OrganisationName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OrganisationPassword")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OrganisationRegNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("OrganisationId");

                    b.HasIndex("AccountId");

                    b.HasIndex("BankId");

                    b.HasIndex("UserId");

                    b.ToTable("Organisations");
                });

            modelBuilder.Entity("BankingApplication_backend.Models.Outbound", b =>
                {
                    b.Property<int>("OrganisationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrganisationId"));

                    b.Property<int>("AccountNumber")
                        .HasColumnType("int");

                    b.Property<int>("AddedBy")
                        .HasColumnType("int");

                    b.Property<string>("FounderName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IFSC")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IsApproved")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OrganisationEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OrganisationName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("OrganisationId");

                    b.ToTable("OutboundOrgs");
                });

            modelBuilder.Entity("BankingApplication_backend.Models.PasswordResetToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("ExpiryDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("PasswordResetTokens");
                });

            modelBuilder.Entity("BankingApplication_backend.Models.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoleId"));

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RoleId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("BankingApplication_backend.Models.SalaryRequest", b =>
                {
                    b.Property<int>("SalaryRequestId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SalaryRequestId"));

                    b.Property<int>("EmployeeIds")
                        .HasColumnType("int");

                    b.Property<int>("OrgID")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SalaryRequestId");

                    b.HasIndex("OrgID");

                    b.ToTable("salaryRequests");
                });

            modelBuilder.Entity("BankingApplication_backend.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BankingApplication_backend.Models.Admin", b =>
                {
                    b.HasOne("BankingApplication_backend.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("BankingApplication_backend.Models.Bank", b =>
                {
                    b.HasOne("BankingApplication_backend.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("BankingApplication_backend.Models.BeneficiaryTransaction", b =>
                {
                    b.HasOne("BankingApplication_backend.Models.Inbound", "Inbound")
                        .WithOne("BeneficiaryTransaction")
                        .HasForeignKey("BankingApplication_backend.Models.BeneficiaryTransaction", "InboundId");

                    b.HasOne("BankingApplication_backend.Models.Outbound", "Outbound")
                        .WithOne("BeneficiaryTransaction")
                        .HasForeignKey("BankingApplication_backend.Models.BeneficiaryTransaction", "OutboundId");

                    b.Navigation("Inbound");

                    b.Navigation("Outbound");
                });

            modelBuilder.Entity("BankingApplication_backend.Models.Creds", b =>
                {
                    b.HasOne("BankingApplication_backend.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("BankingApplication_backend.Models.Document", b =>
                {
                    b.HasOne("BankingApplication_backend.Models.Bank", "Bank")
                        .WithOne("Document")
                        .HasForeignKey("BankingApplication_backend.Models.Document", "BankId");

                    b.HasOne("BankingApplication_backend.Models.Organisation", "Organisation")
                        .WithOne("Document")
                        .HasForeignKey("BankingApplication_backend.Models.Document", "OrganisationId");

                    b.Navigation("Bank");

                    b.Navigation("Organisation");
                });

            modelBuilder.Entity("BankingApplication_backend.Models.EmpTransaction", b =>
                {
                    b.HasOne("BankingApplication_backend.Models.Organisation", "Organisation")
                        .WithMany()
                        .HasForeignKey("OrgID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Organisation");
                });

            modelBuilder.Entity("BankingApplication_backend.Models.Employee", b =>
                {
                    b.HasOne("BankingApplication_backend.Models.Organisation", "Organisation")
                        .WithMany()
                        .HasForeignKey("OrganisationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Organisation");
                });

            modelBuilder.Entity("BankingApplication_backend.Models.Inbound", b =>
                {
                    b.HasOne("BankingApplication_backend.Models.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BankingApplication_backend.Models.Bank", "Bank")
                        .WithMany()
                        .HasForeignKey("InboundBankId");

                    b.Navigation("Account");

                    b.Navigation("Bank");
                });

            modelBuilder.Entity("BankingApplication_backend.Models.Organisation", b =>
                {
                    b.HasOne("BankingApplication_backend.Models.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BankingApplication_backend.Models.Bank", "Bank")
                        .WithMany()
                        .HasForeignKey("BankId");

                    b.HasOne("BankingApplication_backend.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Bank");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BankingApplication_backend.Models.PasswordResetToken", b =>
                {
                    b.HasOne("BankingApplication_backend.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("BankingApplication_backend.Models.SalaryRequest", b =>
                {
                    b.HasOne("BankingApplication_backend.Models.Organisation", "Organisation")
                        .WithMany()
                        .HasForeignKey("OrgID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Organisation");
                });

            modelBuilder.Entity("BankingApplication_backend.Models.User", b =>
                {
                    b.HasOne("BankingApplication_backend.Models.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("BankingApplication_backend.Models.Bank", b =>
                {
                    b.Navigation("Document")
                        .IsRequired();
                });

            modelBuilder.Entity("BankingApplication_backend.Models.Inbound", b =>
                {
                    b.Navigation("BeneficiaryTransaction")
                        .IsRequired();
                });

            modelBuilder.Entity("BankingApplication_backend.Models.Organisation", b =>
                {
                    b.Navigation("Document")
                        .IsRequired();
                });

            modelBuilder.Entity("BankingApplication_backend.Models.Outbound", b =>
                {
                    b.Navigation("BeneficiaryTransaction")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
