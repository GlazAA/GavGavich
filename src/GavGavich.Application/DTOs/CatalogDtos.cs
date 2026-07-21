namespace GavGavich.Application.DTOs;

public sealed record ProgramDto(
    int Id,
    string Name,
    string Slug,
    string ShortTitle,
    string Description,
    IReadOnlyList<string> Features,
    IReadOnlyList<PricePackageDto> Packages);

public sealed record PricePackageDto(int Id, int LessonCount, decimal PriceRub);

public sealed record PainPointDto(string Title, string Description);

public sealed record VideoDto(
    int Id,
    string Title,
    string? Description,
    string FilePath,
    string? PosterPath,
    string Section);

public sealed record OrganizationDto(
    string BrandName,
    string Tagline,
    string LegalName,
    string Inn,
    string Ogrnip,
    string Email,
    string TrainerName,
    string AboutText,
    string ServiceArea);

public sealed record LegalDocumentDto(
    int Id,
    string Title,
    string Slug,
    string ContentHtml,
    string Version,
    string DocumentType);

public sealed record ConsentTypeDto(
    int Id,
    string Code,
    string Title,
    string Description,
    string LegalDocumentSlug,
    bool IsRequired);

public sealed class SubmitBookingRequest
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string DogName { get; set; } = string.Empty;
    public string? DogBreed { get; set; }
    public int? DogAgeMonths { get; set; }
    public string? Message { get; set; }
    public int? TrainingProgramId { get; set; }
    public int? PricePackageId { get; set; }
    public bool AcceptedPersonalDataConsent { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
}

public sealed record SubmitBookingResult(bool Success, string Message, int? ApplicationId = null);
