using Microsoft.EntityFrameworkCore;
using WebApi.Domain;
namespace WebApi.Infrastructure;

public class WebApiContext : DbContext
{
    public WebApiContext(DbContextOptions<WebApiContext> options):base(options)
    {
        
    }

    public DbSet<Patient> Patients { get; set; }
    public DbSet<Triage> Triages { get; set; }
    public DbSet<PatientArrival> PatientArrivals { get; set; }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Triage>().HasOne(x => x.Patient).WithMany().OnDelete(DeleteBehavior.Restrict);
    }

}