using GavGavich.Application.Abstractions;
using GavGavich.Application.DTOs;
using GavGavich.Domain.Entities;
using GavGavich.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace GavGavich.Infrastructure.Repositories;

public sealed class CatalogService(IUnitOfWork unitOfWork) : ICatalogService
{
    public async Task<IReadOnlyList<ProgramDto>> GetProgramsAsync(CancellationToken cancellationToken = default)
    {
        var programs = await unitOfWork.Programs.Query()
            .AsNoTracking()
            .Where(p => p.IsActive)
            .Include(p => p.Features)
            .Include(p => p.Packages)
            .OrderBy(p => p.SortOrder)
            .ToListAsync(cancellationToken);

        return programs.Select(MapProgram).ToList();
    }

    public async Task<IReadOnlyList<PainPointDto>> GetPainPointsAsync(CancellationToken cancellationToken = default)
    {
        return await unitOfWork.PainPoints.Query()
            .AsNoTracking()
            .OrderBy(p => p.SortOrder)
            .Select(p => new PainPointDto(p.Title, p.Description))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<VideoDto>> GetVideosAsync(VideoSection? section = null, CancellationToken cancellationToken = default)
    {
        var query = unitOfWork.Videos.Query().AsNoTracking().Where(v => v.IsPublished);
        if (section.HasValue)
        {
            query = query.Where(v => v.Section == section.Value);
        }

        return await query
            .OrderBy(v => v.SortOrder)
            .Select(v => new VideoDto(
                v.Id,
                v.Title,
                v.Description,
                v.FilePath,
                v.PosterPath,
                v.Section.ToString()))
            .ToListAsync(cancellationToken);
    }

    public async Task<OrganizationDto> GetOrganizationAsync(CancellationToken cancellationToken = default)
    {
        var org = await unitOfWork.Organizations.Query()
            .AsNoTracking()
            .OrderBy(o => o.Id)
            .FirstAsync(cancellationToken);

        return new OrganizationDto(
            org.BrandName,
            org.Tagline,
            org.LegalName,
            org.Inn,
            org.Ogrnip,
            org.Email,
            org.TrainerName,
            org.AboutText,
            org.ServiceArea);
    }

    private static ProgramDto MapProgram(TrainingProgram program) =>
        new(
            program.Id,
            program.Name,
            program.Slug,
            program.ShortTitle,
            program.Description,
            program.Features.OrderBy(f => f.SortOrder).Select(f => f.Text).ToList(),
            program.Packages.OrderBy(p => p.SortOrder)
                .Select(p => new PricePackageDto(p.Id, p.LessonCount, p.PriceRub))
                .ToList());
}
