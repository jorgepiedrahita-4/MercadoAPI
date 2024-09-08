using MercadoAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MercadoAPI.Context
{
    public class ApplicationDbContext:DbContext
    {
        public DbSet<Producto> Producto { get; set; }

    }
}
