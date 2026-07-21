using GavGavich.Application.DTOs;
using GavGavich.Domain.Enums;

namespace GavGavich.Application.Abstractions;

public interface ICatalogService
{
    Task<IReadOnlyList<ProgramDto>> GetProgramsAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PainPointDto>> GetPainPointsAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<VideoDto>> GetVideosAsync(VideoSection? section = null, CancellationToken cancellationToken = default);
    Task<OrganizationDto> GetOrganizationAsync(CancellationToken cancellationToken = default);
}

public interface ILegalService
{
    Task<LegalDocumentDto?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<LegalDocumentDto>> GetCurrentDocumentsAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ConsentTypeDto>> GetRequiredConsentsAsync(CancellationToken cancellationToken = default);
}

public interface IBookingService
{
    Task<SubmitBookingResult> SubmitAsync(SubmitBookingRequest request, CancellationToken cancellationToken = default);
}
