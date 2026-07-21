# ГавГавыч — школа кинологии

Полноценная замена лендинга [gavgavich.ru](https://gavgavich.ru/) на ASP.NET Core + Blazor с нормализованной БД.

## Структура

```
src/
  GavGavich.Domain/          # сущности и enums (чистый домен)
  GavGavich.Application/     # контракты и DTO
  GavGavich.Infrastructure/  # EF Core, SQLite, сервисы, seed
  GavGavich.Web/             # Blazor UI, CSS/JS/изображения/видео
```

Статика разложена по папкам:

- `wwwroot/css/base|layout|components|pages`
- `wwwroot/js`
- `wwwroot/images`
- `wwwroot/videos` — кладите сюда MP4 (`how-lessons-work.mp4`, `about-trainer.mp4`)

## Нормализация БД

Вместо одной «плоской» анкеты Tilda:

| Таблица | Назначение |
|--------|------------|
| `clients` | владелец |
| `dogs` | собака (FK → client) |
| `training_programs` | программы |
| `program_features` | пункты программы |
| `price_packages` | пакеты занятий и цены |
| `booking_applications` | заявка (FK → client, dog, program, package) |
| `legal_documents` | версионируемые документы |
| `consent_types` | справочник типов согласия |
| `consent_acceptances` | факт согласия + версия документа + IP/UA |
| `video_assets` | видео по секциям |
| `pain_points` | боли на главной |
| `organization_profiles` | реквизиты ИП |

## Запуск

```bash
cd src/GavGavich.Web
dotnet run
```

SQLite-файл `gavgavich.db` создаётся автоматически, данные с сайта подсеиваются при старте.

## Согласия

Перед отправкой анкеты:

1. чекбокс согласия на странице;
2. если не отмечен — модальное окно «Перед заполнением анкеты» (как на Tilda);
3. факт согласия пишется в `consent_acceptances` с версией документа.
