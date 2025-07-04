
using QuizProject.Models;
using QuizProject.Services;
using QuizProject.Services.Interface;
using QuizProject.JsonDataProviderr;
using QuizProject.ReadPasswordWithStarss;
using System.Text.RegularExpressions;

var userProvider = new JsonDataProvider<User>("users.json");
var quizProvider = new JsonDataProvider<Quiz>("quizzes.json");
var scoreProvider = new JsonDataProvider<UserScore>("scores.json");

var userService = new UserService(userProvider);
var quizService = new QuizPlayerService(quizProvider);
IScoreService scoreService = new ScoreService(scoreProvider);
IChangeUserInfo changeUserInfo = new ChangeUserInfo();
ReadPasswordWithStarsss readPasswordWithStar = new ReadPasswordWithStarsss();


bool isRunning = true;

while (isRunning)
{
    User user = null;


    while (user == null)
    {
        Console.WriteLine(""" 
 █████╗ ██╗  ██╗ ██████╗      ██████╗ ██╗   ██╗██╗███████╗    
██╔══██╗██║ ██╔╝██╔═══██╗    ██╔═══██╗██║   ██║██║╚══███╔╝    
███████║█████╔╝ ██║   ██║    ██║   ██║██║   ██║██║  ███╔╝     
██╔══██║██╔═██╗ ██║   ██║    ██║▄▄ ██║██║   ██║██║ ███╔╝      
██║  ██║██║  ██╗╚██████╔╝    ╚██████╔╝╚██████╔╝██║███████╗    
╚═╝  ╚═╝╚═╝  ╚═╝ ╚═════╝      ╚══▀▀═╝  ╚═════╝ ╚═╝╚══════╝    
""");
        Console.WriteLine("\n1. Qeydiyyat");
        Console.WriteLine("2. Giriş");
        Console.WriteLine("3. Çıxış");
        Console.Write("Seçim: ");
        var option = Console.ReadLine();

        if (option == "1")
        {
            string username;
            do
            {
                Console.Write("İstifadəçi adı ");
                username = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(username) || username.Length < 3 || username.Length > 10)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("İstifadəçi adı 3 ve ya 10 simvol arasında olmalıdır!");
                    Console.ResetColor();
                }
            } while (string.IsNullOrEmpty(username) || username.Length < 3 || username.Length > 10);


            string password;
            Regex regex = new Regex(@"^(?=.*[A-Za-z])(?=.*\d).{8,}$");

            do
            {
                Console.Write("Şifrə Daxil Edin:  ");
                password = ReadPasswordWithStarsss.ReadPasswordWithStars();

                if (!regex.IsMatch(password))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Şifrə ən az 8 simvol olmalı, ən azı 1 hərf və 1 rəqəm daxil edilməlidir!");
                    Console.ResetColor();
                }
            } while (!regex.IsMatch(password));

            DateTime dob;
            while (true)
            {
                Console.Write("Doğum tarixi Il-Ay-Gün : ");
                string dateInput = Console.ReadLine()?.Trim();

                if (DateTime.TryParse(dateInput, out dob))
                    break;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Yanlış format! Tarix İl-Ay-Gün formatında olmalıdır.");
                Console.ResetColor();
            }


            user = userService.Register(username, password, dob);
            if (user == null) Console.WriteLine("İstifadəçi artıq mövcuddur.");
        }
        else if (option == "2")
        {
            Console.Write("İstifadəçi adı: ");
            var username = Console.ReadLine();
            Console.Write("Şifrə: ");
            var password = ReadPasswordWithStarsss.ReadPasswordWithStars();

            Console.Clear();
            user = userService.Login(username, password);
            Console.ForegroundColor = ConsoleColor.Red;
            if (user == null) Console.WriteLine("Yanlış giriş.");
            Console.ResetColor();
            Console.WriteLine();
        }
        else if (option == "3")
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Quizi Test Etdiyiniz Üçün Təşəkkürlər :) ");
            Console.ResetColor();
            return;
        }
        else
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Yanlış Seçim Etdiniz! ");
            Console.ResetColor();
            Console.WriteLine();
        }
    }

    Console.Clear();

    bool inMainMenu = true;

    while (inMainMenu)
    {
        Console.WriteLine($"\nXoş gəlmisiniz, {user.Username}!");

        Console.WriteLine("\n1. Quiz başla");
        Console.WriteLine("2. Məlumat dəyişmək");
        Console.WriteLine("3. Nəticələrim");
        Console.WriteLine("4. Sıralama");
        Console.WriteLine("5. Hesabdan çıxış");
        var choice = Console.ReadLine();

        if (choice == "1")
        {
            var categories = quizService.GetCategories();
            Console.WriteLine("Kateqoriyalar:");
            for (int i = 0; i < categories.Count; i++)
                Console.WriteLine($"{i + 1}. {categories[i]}");
            Console.WriteLine($"{categories.Count + 1}. Qarışıq Quiz");
            Console.WriteLine("0. Geri");


            Console.Write("Seçim: ");
            var input = Console.ReadLine();

            if (input == "0")
            {
                Console.Clear();
                continue;

            }

            if (int.TryParse(input, out int index))
            {
                if (index > 0 && index <= categories.Count)
                {
                    var selectedCategory = categories[index - 1];
                    int score = quizService.PlayQuizAndReturnScore(selectedCategory);
                    scoreService.SaveScore(new UserScore
                    {
                        Username = user.Username,
                        Category = selectedCategory,
                        Score = score,
                        Date = DateTime.Now
                    });
                }
                else if (index == categories.Count + 1)
                {
                    int score = quizService.PlayMixedQuiz(20);
                    scoreService.SaveScore(new UserScore
                    {
                        Username = user.Username,
                        Category = "Qarışıq",
                        Score = score,
                        Date = DateTime.Now
                    });
                }
                else
                {
                    Console.WriteLine("Yanlış seçim!");
                }
            }
            else
            {
                Console.WriteLine("Yanlış seçim!");
            }
        }
        else if (choice == "2")
        {
            changeUserInfo.ChangeUserInfoo(user, userService);
            break;
        }
        else if (choice == "3")
        {
            Console.Clear();
            Console.WriteLine($"\n{user.Username} adlı istifadəçinin nəticələri:");
            var scores = scoreService.GetUserScores(user.Username);
            if (scores.Count == 0)
            {
                Console.WriteLine("Nəticə yoxdur.");
            }
            else
            {
                foreach (var s in scores.OrderByDescending(x => x.Date))
                {
                    int wrong = 20 - s.Score;
                    Console.WriteLine($"{s.Date:yyyy-MM-dd} | {s.Category} | Düzgün: {s.Score} | Səhv: {wrong}");
                }
            }
            Console.WriteLine("Çıxmaq istəyirsinizsə, hər hansı bir düyməyə basın..");
            Console.ReadKey();
            Console.Clear();
        }
        else if (choice == "4")
        {
            Console.Clear();
            var categories = quizService.GetCategories();
            categories.Add("Qarışıq");

            Console.WriteLine("Kateqoriyalar:");
            for (int i = 0; i < categories.Count; i++)
                Console.WriteLine($"{i + 1}. {categories[i]}");

            Console.Write("Seçim: ");
            var index = int.Parse(Console.ReadLine()) - 1;

            if (index < 0 || index >= categories.Count)
            {
                Console.WriteLine("Yanlış seçim!");
                return;
            }

            var selectedCategory = categories[index];

            Console.WriteLine($"\n📊 {selectedCategory} üzrə Sıralama:\n");
            var topScores = scoreService.GetTopScores(selectedCategory, 20);

            if (topScores.Count == 0)
                Console.WriteLine("Heç bir nəticə yoxdur.");
            else
            {
                int rank = 1;
                foreach (var s in topScores)
                    Console.WriteLine($"{rank++}. {s.Username} — {s.Score} xal ({s.Date:yyyy-MM-dd})");
            }
            Console.WriteLine("Çıxmaq istəyirsinizsə, hər hansı bir düyməyə basın..");
            Console.ReadKey();
            Console.Clear();
        }

        else if (choice == "5")
        {
            inMainMenu = false;
            Console.Clear();
            break;
        }

        else 
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Yanlış Seçim Etdiniz! ");
            Console.ResetColor();
            Console.WriteLine();
        }
    }
}
