using GavGavich.Domain.Common;

namespace GavGavich.Domain.Entities;

/// <summary>
/// Факт согласия: кто, когда, какой тип, какая версия документа, с какого IP.
/// </summary>
public class ConsentAcceptance : EntityBase
{
    public int BookingApplicationId { get; set; }
    public BookingApplication BookingApplication { get; set; } = null!;

    public int ConsentTypeId { get; set; }
    public ConsentType ConsentType { get; set; } = null!;

    public int LegalDocumentId { get; set; }
    public LegalDocument LegalDocument { get; set; } = null!;

    public string DocumentVersion { get; set; } = string.Empty;
    public DateTimeOffset AcceptedAt { get; set; } = DateTimeOffset.UtcNow;
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
}
