using GavGavich.Application.Abstractions;
using GavGavich.Application.DTOs;
using GavGavich.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GavGavich.Infrastructure.Repositories;

public sealed class BookingService(IUnitOfWork unitOfWork) : IBookingService
{
    public async Task<SubmitBookingResult> SubmitAsync(SubmitBookingRequest request, CancellationToken cancellationToken = default)
    {
        if (!request.AcceptedPersonalDataConsent)
        {
            return new SubmitBookingResult(false, "Необходимо согласие на обработку персональных данных.");
        }

        if (string.IsNullOrWhiteSpace(request.FirstName) ||
            string.IsNullOrWhiteSpace(request.LastName) ||
            string.IsNullOrWhiteSpace(request.Phone) ||
            string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.DogName))
        {
            return new SubmitBookingResult(false, "Заполните обязательные поля: фамилия, имя, телефон, email и кличку собаки.");
        }

        var consentType = await unitOfWork.ConsentTypes.Query()
            .Include(c => c.LegalDocument)
            .FirstOrDefaultAsync(c => c.Code == "personal_data", cancellationToken);

        if (consentType is null || !consentType.LegalDocument.IsCurrent)
        {
            return new SubmitBookingResult(false, "Тип согласия не настроен. Обратитесь к администратору.");
        }

        if (request.PricePackageId is int packageId)
        {
            var packageExists = await unitOfWork.Programs.Query()
                .SelectMany(p => p.Packages)
                .AnyAsync(p => p.Id == packageId, cancellationToken);

            if (!packageExists)
            {
                return new SubmitBookingResult(false, "Выбранный пакет не найден.");
            }
        }

        var client = await unitOfWork.Clients.Query()
            .FirstOrDefaultAsync(c => c.Email == request.Email.Trim(), cancellationToken);

        if (client is null)
        {
            client = new Client
            {
                FirstName = request.FirstName.Trim(),
                LastName = request.LastName.Trim(),
                MiddleName = request.MiddleName?.Trim() ?? string.Empty,
                Phone = request.Phone.Trim(),
                Email = request.Email.Trim()
            };
            await unitOfWork.Clients.AddAsync(client, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        else
        {
            client.FirstName = request.FirstName.Trim();
            client.LastName = request.LastName.Trim();
            client.MiddleName = request.MiddleName?.Trim() ?? string.Empty;
            client.Phone = request.Phone.Trim();
        }

        var dog = new Dog
        {
            ClientId = client.Id,
            Name = request.DogName.Trim(),
            Breed = string.IsNullOrWhiteSpace(request.DogBreed) ? null : request.DogBreed.Trim(),
            AgeMonths = request.DogAgeMonths,
            Notes = request.Message
        };
        await unitOfWork.Dogs.AddAsync(dog, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var application = new BookingApplication
        {
            ClientId = client.Id,
            DogId = dog.Id,
            TrainingProgramId = request.TrainingProgramId,
            PricePackageId = request.PricePackageId,
            Message = request.Message
        };
        await unitOfWork.Applications.AddAsync(application, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var acceptance = new ConsentAcceptance
        {
            BookingApplicationId = application.Id,
            ConsentTypeId = consentType.Id,
            LegalDocumentId = consentType.LegalDocumentId,
            DocumentVersion = consentType.LegalDocument.Version,
            AcceptedAt = DateTimeOffset.UtcNow,
            IpAddress = request.IpAddress,
            UserAgent = request.UserAgent
        };
        await unitOfWork.ConsentAcceptances.AddAsync(acceptance, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new SubmitBookingResult(true, "Анкета отправлена. Мы свяжемся с вами в ближайшее время.", application.Id);
    }
}
