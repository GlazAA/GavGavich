using GavGavich.Domain.Common;

namespace GavGavich.Domain.Entities;

/// <summary>
/// Пункты программы вынесены в отдельную таблицу (вместо CSV в одной колонке).
/// </summary>
public class ProgramFeature : EntityBase
{
    public int TrainingProgramId { get; set; }
    public TrainingProgram TrainingProgram { get; set; } = null!;

    public string Text { get; set; } = string.Empty;
    public int SortOrder { get; set; }
}
