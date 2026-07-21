using GavGavich.Domain.Common;

namespace GavGavich.Domain.Entities;

/// <summary>
/// Владелец собаки (отдельная сущность — не смешивается с анкетой и собакой).
/// </summary>
public class Client : EntityBase
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public ICollection<Dog> Dogs { get; set; } = new List<Dog>();
    public ICollection<BookingApplication> Applications { get; set; } = new List<BookingApplication>();
}
