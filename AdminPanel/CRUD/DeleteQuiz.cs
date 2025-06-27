
namespace AdminPanel.CRUD;

public class DeleteQuizz
{
    public static void DeleteQuiz()
    {
        Console.Clear();
        Console.Write("Silmək istədiyiniz Quiz ID-ni daxil edin: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Yanlış ID. Davam etmək üçün Enter basın...");
            Console.ReadLine();
            return;
        }

        var quiz = AdminPanell.quizzes.Find(q => q.Id == id);
        if (quiz == null)
        {
            Console.WriteLine("Belə ID-li quiz tapılmadı. Davam etmək üçün Enter basın...");
            Console.ReadLine();
            return;
        }

        AdminPanell.quizzes.Remove(quiz);
        AdminPanell.quizService.SaveQuizzes(AdminPanell.quizzes);

        Console.WriteLine("Quiz silindi. Davam etmək üçün Enter basın...");
        Console.ReadLine();
    }
}
