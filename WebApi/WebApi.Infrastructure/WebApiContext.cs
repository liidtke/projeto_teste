using Microsoft.EntityFrameworkCore;
using WebApi.Domain;
namespace WebApi.Infrastructure;

public class WebApiContext : DbContext
{
    public WebApiContext(DbContextOptions<WebApiContext> options):base(options)
    {
        
    }

    public DbSet<Patient> Patients { get; set; }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }

}