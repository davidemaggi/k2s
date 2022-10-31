using Microsoft.EntityFrameworkCore;
using System;

namespace k2s.Data
{
    public class k2sDbContext:DbContext
    {


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            string dbPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData); 
            optionsBuilder.UseSqlite("Data Source=" + Path.Join(dbPath,"k2s.db"));

            
        }


    }
}
