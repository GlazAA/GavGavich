using GavGavich.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GavGavich.Infrastructure.Persistence;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Dog> Dogs => Set<Dog>();
    public DbSet<TrainingProgram> TrainingPrograms => Set<TrainingProgram>();
    public DbSet<ProgramFeature> ProgramFeatures => Set<ProgramFeature>();
    public DbSet<PricePackage> PricePackages => Set<PricePackage>();
    public DbSet<BookingApplication> BookingApplications => Set<BookingApplication>();
    public DbSet<LegalDocument> LegalDocuments => Set<LegalDocument>();
    public DbSet<ConsentType> ConsentTypes => Set<ConsentType>();
    public DbSet<ConsentAcceptance> ConsentAcceptances => Set<ConsentAcceptance>();
    public DbSet<VideoAsset> VideoAssets => Set<VideoAsset>();
    public DbSet<PainPoint> PainPoints => Set<PainPoint>();
    public DbSet<OrganizationProfile> OrganizationProfiles => Set<OrganizationProfile>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
