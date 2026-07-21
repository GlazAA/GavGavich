using GavGavich.Domain.Common;
using GavGavich.Domain.Enums;

namespace GavGavich.Domain.Entities;

public class VideoAsset : EntityBase
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string FilePath { get; set; } = string.Empty;
    public string? PosterPath { get; set; }
    public VideoSection Section { get; set; }
    public int SortOrder { get; set; }
    public bool IsPublished { get; set; } = true;
}
