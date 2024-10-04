using Microsoft.EntityFrameworkCore;
using SomeService.Models;

namespace SomeService;

public class Context : DbContext
{
    public Context(DbContextOptions<Context> options) : base(options)
    {
        
    }
    
    public DbSet<Order> Orders { get; set; }

}