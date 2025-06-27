using AdminPanel.CRUD;
using AdminPanel.ReadPasswordWithStar;
using AdminPanel.Models;
using AdminPanel.Services;
namespace AdminPanel;


public static class AdminPanell
{
    public static QuizService quizService;
    public static List<Quiz> quizzes;
    static string jsonPath = "quizzes.json";
    static AuthService authService = new AuthService();
    public static void Run()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        if (!Login())
            return;

        quizService = new QuizService(jsonPath);
        quizzes = quizService.GetAllQuizzes();

        while (true)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("""
 █████  ██████  ███    ███ ██ ███    ██     ██████   █████  ███    ██ ███████ ██      
██   ██ ██   ██ ████  ████ ██ ████   ██     ██   ██ ██   ██ ████   ██ ██      ██      
███████ ██   ██ ██ ████ ██ ██ ██ ██  ██     ██████  ███████ ██ ██  ██ █████   ██      
██   ██ ██   ██ ██  ██  ██ ██ ██  ██ ██     ██      ██   ██ ██  ██ ██ ██      ██      
██   ██ ██████  ██      ██ ██ ██   ████     ██      ██   ██ ██   ████ ███████ ███████ 

                                                                                      
""");
            Console.ResetColor();
            Console.WriteLine("1. Quizləri göstər");
            Console.WriteLine("2. Yeni quiz əlavə et");
            Console.WriteLine("3. Quiz redaktə et");
            Console.WriteLine("4. Quiz sil");
            Console.WriteLine("5. Çıxış");
            Console.Write("Seçim edin: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ShowQuizzess.ShowQuizzes();
                    break;
                case "2":
                    AddQuizz.AddQuiz();
                    break;
                case "3":
                    EditQuizz.EditQuiz();
                    break;
                case "4":
                    DeleteQuizz.DeleteQuiz();
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Yanlış seçim. Davam etmək üçün Enter basın...");
                    Console.ReadLine();
                    break;
            }
        }
    }

    static bool Login()
    {
        Console.WriteLine("Admin Panel Login");
        Console.Write("İstifadəçi adı: ");
        var username = Console.ReadLine();
        Console.Write("Şifrə: ");
        var password = ReadPasswordd.ReadPassword();

        if (authService.Login(username, password))
        {
            Console.WriteLine("\nGiriş uğurludur!");
            Console.WriteLine("Davam etmək üçün Enter basın...");
            Console.ReadLine();
            return true;
        }
        else
        {
            Console.WriteLine("\nİstifadəçi adı və ya şifrə yalnışdır.");
            Console.WriteLine("Çıxmaq üçün Enter basın...");
            Console.ReadLine();
            return false;
        }
    }  

   
   
}

