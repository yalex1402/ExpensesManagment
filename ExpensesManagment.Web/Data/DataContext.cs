using ExpensesManagment.Web.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExpensesManagment.Web.Data
{
    public class DataContext : IdentityDbContext<UserEntity>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<ExpenseEntity> Expenses { get; set; }

        public DbSet<TripEntity> Trips { get; set; }

        public DbSet<ExpenseTypeEntity> ExpenseTypes { get; set; }
    }
}
