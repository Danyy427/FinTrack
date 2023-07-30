using FinTrack.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace FinTrack.Data
{
    public class ApplicationDbContext : IdentityDbContext<FinTrackUser>
    {
        public DbSet<Income> Incomes { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<RecurringIncome> RecurringIncomes { get; set; }
        public DbSet<RecurringExpense> RecurringExpenses { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Income>().UseTptMappingStrategy();
            modelBuilder.Entity<Expense>().UseTptMappingStrategy();

            base.OnModelCreating(modelBuilder);
        }
    }
}