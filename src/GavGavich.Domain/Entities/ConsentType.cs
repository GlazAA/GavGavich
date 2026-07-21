using GavGavich.Domain.Common;

namespace GavGavich.Domain.Entities;

/// <summary>
/// Тип согласия (справочник). Не хранит факт согласия пользователя.
/// </summary>
public class ConsentType : EntityBase
{
    public string Code { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int LegalDocumentId { get; set; }
    public LegalDocument LegalDocument { get; set; } = null!;
    public bool IsRequired { get; set; } = true;
}
