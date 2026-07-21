using GavGavich.Domain.Entities;
using GavGavich.Domain.Enums;

namespace GavGavich.Application.Abstractions;

public interface IUnitOfWork
{
    IRepository<Client> Clients { get; }
    IRepository<Dog> Dogs { get; }
    IRepository<TrainingProgram> Programs { get; }
    IRepository<BookingApplication> Applications { get; }
    IRepository<LegalDocument> LegalDocuments { get; }
    IRepository<ConsentType> ConsentTypes { get; }
    IRepository<ConsentAcceptance> ConsentAcceptances { get; }
    IRepository<VideoAsset> Videos { get; }
    IRepository<PainPoint> PainPoints { get; }
    IRepository<OrganizationProfile> Organizations { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<T>> ListAsync(CancellationToken cancellationToken = default);
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    IQueryable<T> Query();
}
