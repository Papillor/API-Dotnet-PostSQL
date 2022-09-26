using API_Dotnet_PostSQL.Models;
using Microsoft.EntityFrameworkCore;

namespace API_Dotnet_PostSQL.Data
{
    public class Aerowdb : DbContext
    {
        public Aerowdb(DbContextOptions<Aerowdb> options) : base(options)
        {
        
        }
        public DbSet<Employee> Employees => Set<Employee>();
    }
}
