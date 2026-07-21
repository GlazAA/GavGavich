using GavGavich.Domain.Common;

namespace GavGavich.Domain.Entities;

/// <summary>
/// Реквизиты ИП — одна запись профиля организации.
/// </summary>
public class OrganizationProfile : EntityBase
{
    public string BrandName { get; set; } = string.Empty;
    public string Tagline { get; set; } = string.Empty;
    public string LegalName { get; set; } = string.Empty;
    public string Inn { get; set; } = string.Empty;
    public string Ogrnip { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string TrainerName { get; set; } = string.Empty;
    public string AboutText { get; set; } = string.Empty;
    public string ServiceArea { get; set; } = string.Empty;
}
