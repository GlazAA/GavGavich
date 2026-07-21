using GavGavich.Domain.Common;

namespace GavGavich.Domain.Entities;

/// <summary>
/// Боли на главной — отдельная таблица контента, не захардкожены только в вёрстке.
/// </summary>
public class PainPoint : EntityBase
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int SortOrder { get; set; }
}
