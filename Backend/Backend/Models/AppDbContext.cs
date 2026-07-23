using Microsoft.EntityFrameworkCore;

namespace Backend.Models;

public class AppDbContext : DbContext //dbcontext is the way to connect to the database
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    public DbSet<Person> People { get; set; }
    



}