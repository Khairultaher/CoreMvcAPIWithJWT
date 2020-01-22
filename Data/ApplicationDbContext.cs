using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;

namespace CoreMvcAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>,IDataContext
    {
        public DbSet<Customer> Customers { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        private IDbContextTransaction _transaction;
        public void BeginTransaction()
        {
            _transaction = Database.BeginTransaction();
        }
        public void Commit()
        {
            try
            {
                SaveChanges();

                _transaction.Commit();
            }
            finally
            {
                _transaction.Dispose();
            }
        }
        public void Rollback()
        {
            _transaction.Rollback();
            _transaction.Dispose();
        }
        public async Task<string> RunSqlQuery(string sqlQuery)
        {
            var table = new DataTable();
            var conn = Database.GetDbConnection();
            if (conn.State.ToString() != "Open") 
            {
                await conn.OpenAsync();
            }
            
            var command = conn.CreateCommand();
            command.CommandText = sqlQuery;

            using (var reader = await command.ExecuteReaderAsync())
            {
                table.Load(reader);               
            }
            if (conn.State.ToString() == "Open")
            {
                await conn.CloseAsync();               
            }
            return  table.Rows[0][0].ToString();     
        }
    }

    public interface IDataContext
    {
        DbSet<Customer> Customers { get; set; }
        Task<string> RunSqlQuery(string sqlQuery);
        void BeginTransaction();
        void Commit();
        void Rollback();
    }
}
