using GavGavich.Application.Abstractions;
using GavGavich.Domain.Entities;
using GavGavich.Infrastructure.Persistence;

namespace GavGavich.Infrastructure.Repositories;

public sealed class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    private readonly Lazy<IRepository<Client>> _clients = new(() => new EfRepository<Client>(context));
    private readonly Lazy<IRepository<Dog>> _dogs = new(() => new EfRepository<Dog>(context));
    private readonly Lazy<IRepository<TrainingProgram>> _programs = new(() => new EfRepository<TrainingProgram>(context));
    private readonly Lazy<IRepository<BookingApplication>> _applications = new(() => new EfRepository<BookingApplication>(context));
    private readonly Lazy<IRepository<LegalDocument>> _legalDocuments = new(() => new EfRepository<LegalDocument>(context));
    private readonly Lazy<IRepository<ConsentType>> _consentTypes = new(() => new EfRepository<ConsentType>(context));
    private readonly Lazy<IRepository<ConsentAcceptance>> _consentAcceptances = new(() => new EfRepository<ConsentAcceptance>(context));
    private readonly Lazy<IRepository<VideoAsset>> _videos = new(() => new EfRepository<VideoAsset>(context));
    private readonly Lazy<IRepository<PainPoint>> _painPoints = new(() => new EfRepository<PainPoint>(context));
    private readonly Lazy<IRepository<OrganizationProfile>> _organizations = new(() => new EfRepository<OrganizationProfile>(context));

    public IRepository<Client> Clients => _clients.Value;
    public IRepository<Dog> Dogs => _dogs.Value;
    public IRepository<TrainingProgram> Programs => _programs.Value;
    public IRepository<BookingApplication> Applications => _applications.Value;
    public IRepository<LegalDocument> LegalDocuments => _legalDocuments.Value;
    public IRepository<ConsentType> ConsentTypes => _consentTypes.Value;
    public IRepository<ConsentAcceptance> ConsentAcceptances => _consentAcceptances.Value;
    public IRepository<VideoAsset> Videos => _videos.Value;
    public IRepository<PainPoint> PainPoints => _painPoints.Value;
    public IRepository<OrganizationProfile> Organizations => _organizations.Value;

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        context.SaveChangesAsync(cancellationToken);
}
