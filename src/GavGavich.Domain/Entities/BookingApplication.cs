using GavGavich.Domain.Common;
using GavGavich.Domain.Enums;

namespace GavGavich.Domain.Entities;

/// <summary>
/// Анкета на занятие ссылается на Client, Dog, Program и Package — без денормализации ФИО/телефона.
/// </summary>
public class BookingApplication : EntityBase
{
    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;

    public int DogId { get; set; }
    public Dog Dog { get; set; } = null!;

    public int? TrainingProgramId { get; set; }
    public TrainingProgram? TrainingProgram { get; set; }

    public int? PricePackageId { get; set; }
    public PricePackage? PricePackage { get; set; }

    public string? Message { get; set; }
    public ApplicationStatus Status { get; set; } = ApplicationStatus.New;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public ICollection<ConsentAcceptance> Consents { get; set; } = new List<ConsentAcceptance>();
}
