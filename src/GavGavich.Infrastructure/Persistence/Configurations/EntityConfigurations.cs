using GavGavich.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GavGavich.Infrastructure.Persistence.Configurations;

public sealed class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("clients");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.FirstName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.LastName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.MiddleName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Phone).HasMaxLength(32).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(256).IsRequired();
        builder.HasIndex(x => x.Email).IsUnique();
    }
}

public sealed class DogConfiguration : IEntityTypeConfiguration<Dog>
{
    public void Configure(EntityTypeBuilder<Dog> builder)
    {
        builder.ToTable("dogs");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Breed).HasMaxLength(120);
        builder.HasOne(x => x.Client).WithMany(c => c.Dogs).HasForeignKey(x => x.ClientId).OnDelete(DeleteBehavior.Cascade);
    }
}

public sealed class TrainingProgramConfiguration : IEntityTypeConfiguration<TrainingProgram>
{
    public void Configure(EntityTypeBuilder<TrainingProgram> builder)
    {
        builder.ToTable("training_programs");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(150).IsRequired();
        builder.Property(x => x.Slug).HasMaxLength(120).IsRequired();
        builder.HasIndex(x => x.Slug).IsUnique();
        builder.Property(x => x.ShortTitle).HasMaxLength(150).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(2000).IsRequired();
    }
}

public sealed class ProgramFeatureConfiguration : IEntityTypeConfiguration<ProgramFeature>
{
    public void Configure(EntityTypeBuilder<ProgramFeature> builder)
    {
        builder.ToTable("program_features");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Text).HasMaxLength(300).IsRequired();
        builder.HasOne(x => x.TrainingProgram).WithMany(p => p.Features).HasForeignKey(x => x.TrainingProgramId).OnDelete(DeleteBehavior.Cascade);
    }
}

public sealed class PricePackageConfiguration : IEntityTypeConfiguration<PricePackage>
{
    public void Configure(EntityTypeBuilder<PricePackage> builder)
    {
        builder.ToTable("price_packages");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.PriceRub).HasPrecision(10, 2);
        builder.HasIndex(x => new { x.TrainingProgramId, x.LessonCount }).IsUnique();
        builder.HasOne(x => x.TrainingProgram).WithMany(p => p.Packages).HasForeignKey(x => x.TrainingProgramId).OnDelete(DeleteBehavior.Cascade);
    }
}

public sealed class BookingApplicationConfiguration : IEntityTypeConfiguration<BookingApplication>
{
    public void Configure(EntityTypeBuilder<BookingApplication> builder)
    {
        builder.ToTable("booking_applications");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Message).HasMaxLength(2000);
        builder.HasOne(x => x.Client).WithMany(c => c.Applications).HasForeignKey(x => x.ClientId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Dog).WithMany(d => d.Applications).HasForeignKey(x => x.DogId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.TrainingProgram).WithMany().HasForeignKey(x => x.TrainingProgramId).OnDelete(DeleteBehavior.SetNull);
        builder.HasOne(x => x.PricePackage).WithMany().HasForeignKey(x => x.PricePackageId).OnDelete(DeleteBehavior.SetNull);
    }
}

public sealed class LegalDocumentConfiguration : IEntityTypeConfiguration<LegalDocument>
{
    public void Configure(EntityTypeBuilder<LegalDocument> builder)
    {
        builder.ToTable("legal_documents");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Title).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Slug).HasMaxLength(120).IsRequired();
        builder.Property(x => x.Version).HasMaxLength(20).IsRequired();
        builder.HasIndex(x => new { x.Slug, x.Version }).IsUnique();
    }
}

public sealed class ConsentTypeConfiguration : IEntityTypeConfiguration<ConsentType>
{
    public void Configure(EntityTypeBuilder<ConsentType> builder)
    {
        builder.ToTable("consent_types");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Code).HasMaxLength(64).IsRequired();
        builder.HasIndex(x => x.Code).IsUnique();
        builder.Property(x => x.Title).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(1000).IsRequired();
        builder.HasOne(x => x.LegalDocument).WithMany().HasForeignKey(x => x.LegalDocumentId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class ConsentAcceptanceConfiguration : IEntityTypeConfiguration<ConsentAcceptance>
{
    public void Configure(EntityTypeBuilder<ConsentAcceptance> builder)
    {
        builder.ToTable("consent_acceptances");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.DocumentVersion).HasMaxLength(20).IsRequired();
        builder.Property(x => x.IpAddress).HasMaxLength(64);
        builder.Property(x => x.UserAgent).HasMaxLength(512);
        builder.HasIndex(x => new { x.BookingApplicationId, x.ConsentTypeId }).IsUnique();
        builder.HasOne(x => x.BookingApplication).WithMany(a => a.Consents).HasForeignKey(x => x.BookingApplicationId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.ConsentType).WithMany().HasForeignKey(x => x.ConsentTypeId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.LegalDocument).WithMany().HasForeignKey(x => x.LegalDocumentId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class VideoAssetConfiguration : IEntityTypeConfiguration<VideoAsset>
{
    public void Configure(EntityTypeBuilder<VideoAsset> builder)
    {
        builder.ToTable("video_assets");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Title).HasMaxLength(200).IsRequired();
        builder.Property(x => x.FilePath).HasMaxLength(500).IsRequired();
        builder.Property(x => x.PosterPath).HasMaxLength(500);
        builder.Property(x => x.Description).HasMaxLength(1000);
    }
}

public sealed class PainPointConfiguration : IEntityTypeConfiguration<PainPoint>
{
    public void Configure(EntityTypeBuilder<PainPoint> builder)
    {
        builder.ToTable("pain_points");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Title).HasMaxLength(120).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(500).IsRequired();
    }
}

public sealed class OrganizationProfileConfiguration : IEntityTypeConfiguration<OrganizationProfile>
{
    public void Configure(EntityTypeBuilder<OrganizationProfile> builder)
    {
        builder.ToTable("organization_profiles");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.BrandName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Tagline).HasMaxLength(200).IsRequired();
        builder.Property(x => x.LegalName).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Inn).HasMaxLength(20).IsRequired();
        builder.Property(x => x.Ogrnip).HasMaxLength(20).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(256).IsRequired();
        builder.Property(x => x.TrainerName).HasMaxLength(120).IsRequired();
        builder.Property(x => x.AboutText).HasMaxLength(4000).IsRequired();
        builder.Property(x => x.ServiceArea).HasMaxLength(300).IsRequired();
    }
}
