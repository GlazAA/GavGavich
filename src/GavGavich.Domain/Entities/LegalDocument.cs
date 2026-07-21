using GavGavich.Domain.Common;
using GavGavich.Domain.Enums;

namespace GavGavich.Domain.Entities;

/// <summary>
/// Версионируемый юридический документ. Согласия ссылаются на конкретную версию.
/// </summary>
public class LegalDocument : EntityBase
{
    public LegalDocumentType DocumentType { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string ContentHtml { get; set; } = string.Empty;
    public string Version { get; set; } = "1.0";
    public DateTimeOffset EffectiveFrom { get; set; }
    public bool IsCurrent { get; set; } = true;
}
