using BankingApplication_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingApplication_backend.Data
{
    public class BankingAppDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<Inbound> InboundOrgs { get; set; }
        public DbSet<Outbound> OutboundOrgs { get; set; }
        public DbSet<Creds> Credentials { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Organisation> Organisations { get; set; }
        public DbSet<Role> Roles { get; set; }//
        public DbSet<User> Users { get; set; }//
        public DbSet<Document> Documents { get; set; }
        public DbSet<BeneficiaryTransaction> BeneficiaryTransactions { get; set; }
        public DbSet<EmpTransaction> EmpTransactions { get; set; }
        public DbSet<SalaryRequest> salaryRequests { get; set; }
        public DbSet<Download> Downloads { get; set; }
        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
        public BankingAppDbContext(DbContextOptions<BankingAppDbContext> options) : base(options){}
    }
}
