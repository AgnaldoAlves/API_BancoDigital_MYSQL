using BancoDigital.Models;
using Microsoft.EntityFrameworkCore;

namespace BancoDigital.Data
{
    public class APIDbContext : DbContext
    {
        public APIDbContext(DbContextOptions<APIDbContext> options): base (options) 
        {

        }

        public DbSet<Conta> Conta { get; set; }
        public DbSet<Divida> Dividas { get; set; }
        public  DbSet<Transacao> Transacaos { get; set; }

    }
}
