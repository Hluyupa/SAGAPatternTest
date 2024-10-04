using AnotherService.Models;
using Microsoft.EntityFrameworkCore;

namespace AnotherService;

public class Context : DbContext
{
    public Context(DbContextOptions<Context> options) : base(options)
    {
        
    }
    
    public DbSet<Account> Accounts { get; set; }
}