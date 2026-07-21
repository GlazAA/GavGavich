using GavGavich.Domain.Common;

namespace GavGavich.Domain.Entities;

public class TrainingProgram : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string ShortTitle { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<ProgramFeature> Features { get; set; } = new List<ProgramFeature>();
    public ICollection<PricePackage> Packages { get; set; } = new List<PricePackage>();
}
