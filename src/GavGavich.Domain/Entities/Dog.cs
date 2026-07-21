using GavGavich.Domain.Common;

namespace GavGavich.Domain.Entities;

/// <summary>
/// Собака принадлежит клиенту (1:N). Данные питомца не дублируются в анкете.
/// </summary>
public class Dog : EntityBase
{
    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;

    public string Name { get; set; } = string.Empty;
    public string? Breed { get; set; }
    public int? AgeMonths { get; set; }
    public string? Notes { get; set; }

    public ICollection<BookingApplication> Applications { get; set; } = new List<BookingApplication>();
}
