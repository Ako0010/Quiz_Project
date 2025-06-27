using QuizProject.Models;
using QuizProject.Services.Interface;


namespace QuizProject.Services;

public static class Extensions
{
    private static Random rng = new Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}

public class QuizPlayerService : IQuizPlayerService
{
    private IDataProvider<List<Quiz>> _quizProvider;
    private List<Quiz> _quizzes;

    public QuizPlayerService(IDataProvider<List<Quiz>> quizProvider)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        _quizProvider = quizProvider;
        _quizzes = _quizProvider.Load() ?? new List<Quiz>();
    }

    public List<string> GetCategories()
    {
        return (_quizzes ?? new List<Quiz>()).Select(q => q.Category).Distinct().ToList();
    }

    public int PlayQuizAndReturnScore(string category)
    {
        var quizzesInCategory = _quizzes.Where(q => q.Category.ToLower() == category.ToLower()).ToList();
        if (quizzesInCategory.Count == 0)
        {
            Console.WriteLine($"\n'{category}' kateqoriyasında quiz tapılmadı.");
            return 0;
        }

        var selectedQuiz = quizzesInCategory.First();
        var questions = selectedQuiz.Questions.ToList();
        questions.Shuffle();

        int totalQuestions = questions.Count >= 20 ? 20 : questions.Count;

        return PlayQuizFromQuestions(questions, totalQuestions, category);
    }

    public int PlayMixedQuiz(int totalQuestions)
    {
        var allQuestions = _quizzes.SelectMany(q => q.Questions).ToList();
        if (allQuestions.Count == 0)
        {
            Console.WriteLine("Quiz sualları tapılmadı.");
            return 0;
        }

        allQuestions.Shuffle();

        int maxQuestions = allQuestions.Count >= totalQuestions ? totalQuestions : allQuestions.Count;

        return PlayQuizFromQuestions(allQuestions, maxQuestions, "Qarışıq");
    }

    private int PlayQuizFromQuestions(List<Question> questions, int totalQuestions, string categoryName)
    {
        Console.WriteLine($"\n--- {categoryName} üzrə {totalQuestions} sual ---");

        int correctAnswers = 0;

        for (int questionIndex = 0; questionIndex < totalQuestions; questionIndex++)
        {
            var q = questions[questionIndex];

            var variants = q.Variantlar.ToList();
            variants.Shuffle();

            Console.WriteLine($"\nSual {questionIndex + 1}: {q.Sual}");

            for (int i = 0; i < variants.Count; i++)
                Console.WriteLine($"   {i + 1}) {variants[i]}");

            Console.Write("Cavabların nömrəsini ver vergüllə ayır: ");
            var input = Console.ReadLine();

            var selectedIndexes = input == null ? new List<int>() :
                input.Split(',')
                     .Where(s => s != null && s.Trim().Length > 0)
                     .Select(s => int.TryParse(s.Trim(), out int idx) ? idx - 1 : -1)
                     .Where(idx => idx >= 0 && idx < variants.Count)
                     .ToList();

            var correctAnswerIndexes = (q.DuzgunCavablar ?? new List<string>())
                .Select(ans => variants.IndexOf(ans))
                .Where(idx => idx >= 0)
                .ToList();

            bool isCorrect = selectedIndexes.Count == correctAnswerIndexes.Count &&
                             selectedIndexes.All(idx => correctAnswerIndexes.Contains(idx));

            if (isCorrect)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("✔️  Düzgün cavab!");
                correctAnswers++;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("❌  Səhv cavab.");
                Console.Write("Düzgün cavab(lar): ");
                Console.WriteLine(string.Join(", ", correctAnswerIndexes.Select(i => variants[i])));
            }

            Console.ResetColor();
        }

        Console.WriteLine($"\n✅ Nəticə: {correctAnswers} / {totalQuestions} düzgün cavab.");
        return correctAnswers;
    }



}
