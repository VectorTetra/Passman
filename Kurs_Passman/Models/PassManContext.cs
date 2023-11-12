using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurs_Passman.Models
{
    // Цей клас виконує роль контекста даних для БД SQLite
    public class PassManContext: DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=SQLiteDB/PassMan.db");
        }
    }
}
