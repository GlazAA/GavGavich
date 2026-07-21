using GavGavich.Domain.Common;

namespace GavGavich.Domain.Entities;

/// <summary>
/// Тарифный пакет (кол-во занятий + цена) — отдельная таблица, без дублирования цен в тексте.
/// </summary>
public class PricePackage : EntityBase
{
    public int TrainingProgramId { get; set; }
    public TrainingProgram TrainingProgram { get; set; } = null!;

    public int LessonCount { get; set; }
    public decimal PriceRub { get; set; }
    public int SortOrder { get; set; }
}
