using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

public class AppDBContext(DbContextOptions<AppDBContext> options) : DbContext(options)
{
    public DbSet<SchoolEntity> School { get; set; }
}
