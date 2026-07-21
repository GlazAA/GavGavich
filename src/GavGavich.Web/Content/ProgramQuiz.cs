namespace GavGavich.Web.Content;

/// <summary>
/// Подбор программы: приоритет безопасности (агрессия на людей) выше «просто базы».
/// Не больше 7 вопросов — глубже про лай, триггеры и быт.
/// </summary>
public static class ProgramQuiz
{
    public sealed record Option(string Id, string Label, IReadOnlyDictionary<string, int> Scores);

    public sealed record Question(string Id, string Text, IReadOnlyList<Option> Options);

    public sealed record ResultCopy(
        string Slug,
        string Title,
        string Why,
        IReadOnlyList<string> Focus);

    public static IReadOnlyList<Question> Questions { get; } =
    [
        new("age", "Сколько примерно собаке?",
        [
            new("pup", "До 5–6 месяцев (щенок)", Dict(("puppy-start", 4))),
            new("teen", "От 6 месяцев до года", Dict(("puppy-start", 2), ("base", 2))),
            new("adult", "Старше года", Dict(("base", 2), ("behavior-correction", 1)))
        ]),
        new("bark_how_often", "Как часто собака лает так, что это мешает вам или соседям?",
        [
            new("rare", "Редко — отдельные эпизоды", Dict(("base", 1))),
            new("walk", "В основном на прогулке: на собак, людей, звуки", Dict(("behavior-correction", 3), ("base", 1))),
            new("home", "Дома часто: дверь, лифт, окна, одиночество", Dict(("behavior-correction", 4))),
            new("constant", "Почти каждый день, лай «на всё подряд»", Dict(("behavior-correction", 5)))
        ]),
        new("bark_trigger", "Из‑за чего лай вспыхивает чаще всего?",
        [
            new("play", "Игра, восторг, мало усталости (щенок/молодой)", Dict(("puppy-start", 3), ("base", 1))),
            new("pull", "Натяжение поводка, «хочу туда» и срыв", Dict(("base", 3))),
            new("dogs", "Другие собаки / реактивность на дистанции", Dict(("behavior-correction", 5))),
            new("people", "Люди: гости, курьеры, прохожие — с рычанием или броском", Dict(("human-aggression", 6)))
        ]),
        new("safety", "Были ситуации, когда собака кусала, бросалась или сильно рычала на человека?",
        [
            new("yes", "Да — укус, бросок или жёсткое рычание с выпадом", Dict(("human-aggression", 8))),
            new("tense", "Пока без укуса, но «натягивается», охраняет еду/диван/дверь", Dict(("human-aggression", 4), ("behavior-correction", 2))),
            new("no", "Нет, к людям в целом спокойно", Dict(("base", 1)))
        ]),
        new("alone", "Как собака переносит одиночество дома?",
        [
            new("ok", "Спокойно или почти спокойно", Dict(("base", 1))),
            new("whine", "Скулит / лает у двери, но без разрушений", Dict(("behavior-correction", 3))),
            new("panic", "Сильная тревога: вой, порча вещей, жалобы соседей", Dict(("behavior-correction", 5))),
            new("pup_alone", "Щенок — ещё учимся оставаться одним", Dict(("puppy-start", 3)))
        ]),
        new("main", "Что беспокоит сильнее всего прямо сейчас?",
        [
            new("house", "Туалет дома, кусает руки, скачет, мало социализации", Dict(("puppy-start", 5))),
            new("walk", "Тянет поводок, не слышит команды, хаос на улице", Dict(("base", 5))),
            new("react", "Страхи, реактивность, лишний лай, адаптация после приюта", Dict(("behavior-correction", 5))),
            new("people", "Агрессия / охрана по отношению к людям", Dict(("human-aggression", 6)))
        ]),
        new("goal", "Какой результат хотите в первую очередь за ближайший месяц?",
        [
            new("calm_pup", "Спокойный щенок дома и первые понятные правила", Dict(("puppy-start", 3))),
            new("daily", "Удобные прогулки и команды, которые «работают» на улице", Dict(("base", 3))),
            new("reduce", "Меньше лая и вспышек, больше предсказуемости", Dict(("behavior-correction", 3))),
            new("safe", "Безопасность семьи и спокойствие при гостях / на улице", Dict(("human-aggression", 4)))
        ])
    ];

    private static readonly string[] Priority =
        ["human-aggression", "behavior-correction", "puppy-start", "base"];

    public static string ResolveWinner(IReadOnlyDictionary<string, string> answers)
    {
        var scores = new Dictionary<string, int>(StringComparer.Ordinal)
        {
            ["puppy-start"] = 0,
            ["base"] = 0,
            ["behavior-correction"] = 0,
            ["human-aggression"] = 0
        };

        foreach (var question in Questions)
        {
            if (!answers.TryGetValue(question.Id, out var optionId))
            {
                continue;
            }

            var option = question.Options.FirstOrDefault(o => o.Id == optionId);
            if (option is null)
            {
                continue;
            }

            foreach (var (slug, points) in option.Scores)
            {
                scores[slug] = scores.GetValueOrDefault(slug) + points;
            }
        }

        var max = scores.Values.DefaultIfEmpty(0).Max();
        foreach (var slug in Priority)
        {
            if (scores.GetValueOrDefault(slug) == max)
            {
                return slug;
            }
        }

        return "base";
    }

    public static ResultCopy Describe(string slug) => slug switch
    {
        "puppy-start" => new(
            slug,
            "Щенячий старт",
            "Сейчас важнее заложить привычки, социализацию и первые команды, чем «ломать» поведение взрослой собаки. Чем раньше мягко выстроим режим — тем проще будет жить дальше.",
            ["туалет и правила дома", "кусание рук и скачки", "подготовка к улице и людям", "база для взрослой жизни"]),
        "base" => new(
            slug,
            "Воспитание взрослой собаки (база)",
            "Похоже, вам нужна рабочая база: поводок, подзыв, спокойствие на прогулке и понятные команды в быту — без тяжёлой коррекции агрессии.",
            ["поводок без войны", "подзыв и выдержка", "поведение у лифта/дверей", "домашние правила"]),
        "behavior-correction" => new(
            slug,
            "Коррекция поведения",
            "Здесь уже не «просто команды», а работа с триггерами: лай, страхи, реактивность на собак, тревога в одиночестве. План строим аккуратно и с приоритетом на безопасность.",
            ["разбор триггеров лая", "дистанция и управление", "снижение вспышек", "домашние протоколы"]),
        _ => new(
            slug,
            "Коррекция агрессии на людей",
            "Если есть рычание, броски или укусы в сторону людей — сначала безопасность семьи, гостей и прохожих. Только после этого наращиваем спокойные навыки.",
            ["оценка рисков", "протоколы на прогулке и дома", "территориальная/пищевая охрана", "пошаговая работа без провокаций"])
    };

    private static IReadOnlyDictionary<string, int> Dict(params (string slug, int points)[] items) =>
        items.ToDictionary(i => i.slug, i => i.points, StringComparer.Ordinal);
}
