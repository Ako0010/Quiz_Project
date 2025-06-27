
namespace AdminPanel.CRUD;

public static class ShowQuizzess
{
    public static void ShowQuizzes()
    {
        Console.Clear();
        if (AdminPanell.quizzes.Count == 0)
        {
            Console.WriteLine("Heç bir quiz yoxdur.");
        }
        else
        {
            foreach (var quiz in AdminPanell.quizzes)
            {
                Console.WriteLine($"ID: {quiz.Id} | Kateqoriya: {quiz.Category} | Suallar sayı: {quiz.Questions.Count}");
            }
        }
        Console.WriteLine("Davam etmək üçün Enter basın...");
        Console.ReadLine();
    }
}
