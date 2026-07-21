using GavGavich.Application.Abstractions;
using GavGavich.Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace GavGavich.Infrastructure.Repositories;

public sealed class LegalService(IUnitOfWork unitOfWork) : ILegalService
{
    public async Task<LegalDocumentDto?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var doc = await unitOfWork.LegalDocuments.Query()
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Slug == slug && d.IsCurrent, cancellationToken);

        return doc is null ? null : Map(doc);
    }

    public async Task<IReadOnlyList<LegalDocumentDto>> GetCurrentDocumentsAsync(CancellationToken cancellationToken = default)
    {
        return await unitOfWork.LegalDocuments.Query()
            .AsNoTracking()
            .Where(d => d.IsCurrent)
            .OrderBy(d => d.DocumentType)
            .Select(d => new LegalDocumentDto(
                d.Id,
                d.Title,
                d.Slug,
                d.ContentHtml,
                d.Version,
                d.DocumentType.ToString()))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ConsentTypeDto>> GetRequiredConsentsAsync(CancellationToken cancellationToken = default)
    {
        return await unitOfWork.ConsentTypes.Query()
            .AsNoTracking()
            .Include(c => c.LegalDocument)
            .Where(c => c.IsRequired)
            .Select(c => new ConsentTypeDto(
                c.Id,
                c.Code,
                c.Title,
                c.Description,
                c.LegalDocument.Slug,
                c.IsRequired))
            .ToListAsync(cancellationToken);
    }

    private static LegalDocumentDto Map(Domain.Entities.LegalDocument d) =>
        new(d.Id, d.Title, d.Slug, d.ContentHtml, d.Version, d.DocumentType.ToString());
}
