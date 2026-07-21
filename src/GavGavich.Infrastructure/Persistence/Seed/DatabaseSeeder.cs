using GavGavich.Domain.Entities;
using GavGavich.Domain.Enums;
using GavGavich.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GavGavich.Infrastructure.Persistence.Seed;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await db.Database.EnsureCreatedAsync();
        await EnsureClientMiddleNameColumnAsync(db);
        await UpsertLegalDocumentsAsync(db);
        await UpsertProgramCopyAsync(db);

        if (await db.OrganizationProfiles.AnyAsync())
        {
            return;
        }

        db.OrganizationProfiles.Add(new OrganizationProfile
        {
            BrandName = "ГавГавыч",
            Tagline = "школа кинологии",
            LegalName = "Индивидуальный предприниматель Великая Анастасия Сергеевна",
            Inn = "772748033423",
            Ogrnip = "325774600053967",
            Email = "info@gavgavich.ru",
            TrainerName = "Анастасия",
            ServiceArea = "Индивидуальные занятия с кинологом у вас дома в Москве и области.",
            AboutText =
                "С одной стороны, это интуитивно довольно просто: мы все понимаем, когда собака выпрашивает кусочек колбасы или хочет поиграть с соседским Бобиком. Однако, иногда людям кажется, что пёс «делает это мне назло» или «точно всё понимает, но выполнять не хочет». Или вовсе нам непонятно, почему «я для него всё, а он мне!…»\n\n" +
                "И тогда на помощь приходят кинологи. Чтобы собака слушала команды, не тянула поводок, не орала на собак и людей, ходила по своим делам на улицу — одним словом, чтобы с собакой было комфортно жить.\n\n" +
                "Я не буду вас обманывать, что это быстро и легко. Нет, многое зависит от вас: есть ли у вас время и желание налаживать отношения с этим непонятным существом. Если есть — я очень постараюсь превратить вашу собаку в вашего друга!\n\n" +
                "Меня зовут Анастасия, и я создала свою школу, глядя на то, как часто люди и собаки не понимают друг друга."
        });

        db.PainPoints.AddRange(
            new PainPoint { Title = "Тянет как трактор", Description = "Прогулка превращается в соревнование: кто кого перетянет.", SortOrder = 1 },
            new PainPoint { Title = "Лает на всё", Description = "Собаки, люди, звонок, лифт, пакет — всё повод поднять голос.", SortOrder = 2 },
            new PainPoint { Title = "Грызёт и скачет", Description = "Руки, тапки, провода — всё идёт в работу.", SortOrder = 3 },
            new PainPoint { Title = "Не слышит команды", Description = "Дома гений, на улице внезапно «я вас не знаю, вы кто такие?».", SortOrder = 4 });

        var puppy = CreateProgram(1, "Щенячий старт", "puppy-start", "Щенячий старт",
            "Социализация, туалет, кусание рук, первые команды и база для взрослой собаки.",
            ["первые правила дома", "подготовка к улице", "социализация"],
            [(1, 3500), (5, 12000), (10, 21000), (15, 27000)]);

        var basics = CreateProgram(2, "Воспитание взрослой собаки", "base", "База",
            "Чтобы собака слушала команды, не тянула поводок, не орала на собак и людей, ходила по своим делам на улицу.",
            ["поводок", "подзыв и выдержка", "базовые команды"],
            [(1, 4000), (5, 14000), (10, 25000), (15, 33000)]);

        var correction = CreateProgram(3, "Коррекция поведения", "behavior-correction", "Коррекция",
            "Уберём агрессию на собак, реактивность, лишний лай, сепарационную тревогу. Адаптация собаки из приюта.",
            ["агрессия на собак", "страхи и фобии", "реактивность"],
            [(1, 4500), (5, 16000), (10, 29000), (15, 38000)]);

        var aggression = CreateProgram(4, "Коррекция агрессии на людей", "human-aggression", "Коррекция",
            "Работа с бросками на людей, территориальной и пищевой агрессией. Безопасность семьи и спокойствие в быту.",
            ["бросается на людей", "территориальная агрессия", "пищевая агрессия"],
            [(1, 5000), (5, 18000), (10, 33000), (15, 43000)]);

        db.TrainingPrograms.AddRange(puppy, basics, correction, aggression);

        var consentDoc = await db.LegalDocuments.FirstAsync(d => d.Slug == "consent" && d.IsCurrent);

        if (!await db.ConsentTypes.AnyAsync())
        {
            db.ConsentTypes.Add(new ConsentType
            {
                Code = "personal_data",
                Title = "Согласие на обработку персональных данных",
                Description = "Для продолжения необходимо дать согласие на обработку персональных данных.",
                LegalDocumentId = consentDoc.Id,
                IsRequired = true
            });
        }

        db.VideoAssets.AddRange(
            new VideoAsset
            {
                Title = "Как проходят занятия",
                Description = "Короткий обзор формата индивидуальных занятий на дому.",
                FilePath = "/videos/how-lessons-work.mp4",
                PosterPath = null,
                Section = VideoSection.HowLessonsWork,
                SortOrder = 1,
                IsPublished = true
            },
            new VideoAsset
            {
                Title = "Знакомство с кинологом",
                Description = "Анастасия рассказывает о подходе школы ГавГавыч.",
                FilePath = "/videos/about-trainer.mp4",
                PosterPath = null,
                Section = VideoSection.About,
                SortOrder = 2,
                IsPublished = true
            });

        await db.SaveChangesAsync();
    }

    private static TrainingProgram CreateProgram(
        int sort,
        string name,
        string slug,
        string shortTitle,
        string description,
        string[] features,
        (int lessons, decimal price)[] packages)
    {
        var program = new TrainingProgram
        {
            Name = name,
            Slug = slug,
            ShortTitle = shortTitle,
            Description = description,
            SortOrder = sort,
            IsActive = true
        };

        for (var i = 0; i < features.Length; i++)
        {
            program.Features.Add(new ProgramFeature { Text = features[i], SortOrder = i + 1 });
        }

        for (var i = 0; i < packages.Length; i++)
        {
            program.Packages.Add(new PricePackage
            {
                LessonCount = packages[i].lessons,
                PriceRub = packages[i].price,
                SortOrder = i + 1
            });
        }

        return program;
    }

    private static async Task EnsureClientMiddleNameColumnAsync(AppDbContext db)
    {
        try
        {
            await db.Database.ExecuteSqlRawAsync(
                "ALTER TABLE clients ADD COLUMN MiddleName TEXT NOT NULL DEFAULT ''");
        }
        catch
        {
            // колонка уже есть
        }
    }

    private static async Task UpsertLegalDocumentsAsync(AppDbContext db)
    {
        await UpsertDocAsync(db, LegalDocumentType.PersonalDataConsent, "consent",
            "Согласие на обработку персональных данных", LegalContent.ConsentHtml);
        await UpsertDocAsync(db, LegalDocumentType.PersonalDataPolicy, "privacy",
            "Политика обработки персональных данных", LegalContent.PrivacyHtml);
        await UpsertDocAsync(db, LegalDocumentType.TermsOfUse, "terms",
            "Пользовательское соглашение", LegalContent.TermsHtml);
        await db.SaveChangesAsync();
    }

    private static async Task UpsertProgramCopyAsync(AppDbContext db)
    {
        var updates = new (string Slug, string Name, string ShortTitle, string Description)[]
        {
            ("puppy-start", "Щенячий старт", "Щенячий старт",
                "Социализация, туалет, кусание рук, первые команды и база для взрослой собаки."),
            ("base", "Воспитание взрослой собаки", "База",
                "Чтобы собака слушала команды, не тянула поводок, не орала на собак и людей, ходила по своим делам на улицу."),
            ("behavior-correction", "Коррекция поведения", "Коррекция",
                "Уберём агрессию на собак, реактивность, лишний лай, сепарационную тревогу. Адаптация собаки из приюта."),
            ("human-aggression", "Коррекция агрессии на людей", "Коррекция",
                "Работа с бросками на людей, территориальной и пищевой агрессией. Безопасность семьи и спокойствие в быту.")
        };

        foreach (var item in updates)
        {
            var program = await db.TrainingPrograms.FirstOrDefaultAsync(p => p.Slug == item.Slug);
            if (program is null)
            {
                continue;
            }

            program.Name = item.Name;
            program.ShortTitle = item.ShortTitle;
            program.Description = item.Description;
        }

        await db.SaveChangesAsync();
    }

    private static async Task UpsertDocAsync(
        AppDbContext db,
        LegalDocumentType type,
        string slug,
        string title,
        string html)
    {
        var existing = await db.LegalDocuments.FirstOrDefaultAsync(d => d.Slug == slug && d.IsCurrent);
        if (existing is null)
        {
            db.LegalDocuments.Add(new LegalDocument
            {
                DocumentType = type,
                Title = title,
                Slug = slug,
                Version = "2.0",
                EffectiveFrom = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero),
                IsCurrent = true,
                ContentHtml = html
            });
            return;
        }

        existing.Title = title;
        existing.ContentHtml = html;
        existing.Version = "2.0";
        existing.DocumentType = type;
    }
}
