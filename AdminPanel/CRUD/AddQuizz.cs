
using AdminPanel.Models;

namespace AdminPanel.CRUD;

public class AddQuizz
{
    public static void AddQuiz()
    {
        Console.Clear();
        Console.WriteLine("Yeni quiz əlavə et:");

        Quiz newQuiz = new Quiz();
        newQuiz.Id = AdminPanell.quizzes.Count > 0 ? AdminPanell.quizzes[AdminPanell.quizzes.Count - 1].Id + 1 : 1;

        string category;
        do
        {
            Console.Write("Kateqoriya daxil edin: ");
            category = Console.ReadLine()?.Trim();
            if (category == null || category == "" || category.Replace(" ", "") == "")
                Console.WriteLine("Kateqoriya boş ola bilməz. Zəhmət olmasa yenidən daxil edin.");
        } while (category == null || category == "" || category.Replace(" ", "") == "");

        newQuiz.Category = category;
        newQuiz.Questions = new List<Question>();

        bool addMoreQuestions = true;
        while (addMoreQuestions)
        {
            var question = CreateQuestionn.CreateQuestion(newQuiz.Questions.Count + 1);
            if (question != null)
                newQuiz.Questions.Add(question);
            else
            {
                Console.WriteLine("Sual əlavə edilmədi. Yenidən cəhd etmək istəyirsiniz? (h/x): ");
                var retry = Console.ReadLine()?.Trim().ToLower();
                if (retry != "h")
                    break;
                else
                    continue;
            }

            string ans;
            do
            {
                Console.Write("Daha bir sual əlavə etmək istəyirsiniz? (h/x): ");
                ans = Console.ReadLine()?.Trim().ToLower();
                if (ans != "h" && ans != "x")
                    Console.WriteLine("Zəhmət olmasa yalnız 'h' (hə) və ya 'x' (xeyr) daxil edin.");
            } while (ans != "h" && ans != "x");

            addMoreQuestions = ans == "h";
        }

        if (newQuiz.Questions.Count == 0)
        {
            Console.WriteLine("Heç bir sual əlavə edilmədi. Quiz əlavə edilmədi.");
        }
        else
        {
            AdminPanell.quizzes.Add(newQuiz);
            AdminPanell.quizService.SaveQuizzes(AdminPanell.quizzes);
            Console.WriteLine("Quiz əlavə edildi.");
        }

        Console.WriteLine("Davam etmək üçün Enter basın...");
        Console.ReadLine();
    }
}

public class CreateQuestionn
{
    public static Question CreateQuestion(int questionId)
    {
        Console.Clear();
        Console.WriteLine($"Sual #{questionId} əlavə et:");

        Question question = new Question();
        question.QuestionId = questionId;

        Console.Write("Sual mətni: ");
        question.Sual = Console.ReadLine()?.Trim();

        if (question.Sual == null || question.Sual.Trim() == "")
        {
            Console.WriteLine("Sual mətni boş ola bilməz.");
            Console.ReadLine();
            return null;
        }

        question.Variantlar = new List<string>();
        question.DuzgunCavablar = new List<string>();

        Console.WriteLine("Variantları daxil edin (bitirmək üçün boş buraxın):");
        int varIndex = 1;
        while (true)
        {
            if (question.Variantlar.Count >= 6)
            {
                Console.WriteLine("Maksimum 6 variant daxil edilə bilər.");
                break;
            }

            Console.Write($"Variant #{varIndex}: ");
            var variant = Console.ReadLine()?.Trim();
            if ((variant == null || variant.Length == 0))
            {
                if (question.Variantlar.Count < 3)
                {
                    Console.WriteLine("Ən azı 3 variant daxil edilməlidir. Zəhmət olmasa davam edin.");
                    continue;
                }
                else
                {
                    break;
                }
            }

            question.Variantlar.Add(variant);
            varIndex++;
        }

        if (question.Variantlar.Count < 3)
        {
            Console.WriteLine("Ən azı üç variant daxil edilməlidir!");
            Console.ReadLine();
            return null;
        }

        Console.WriteLine("Düzgün cavabların variant nömrələrini daxil edin (bitirmək üçün boş buraxın):");

        while (true)
        {
            Console.Write("Düzgün cavab nömrəsi: ");
            var input = Console.ReadLine()?.Trim();

            if (input == null || input.Trim().Length == 0)
                break;

            if (int.TryParse(input, out int answerIndex))
            {
                if (answerIndex >= 1 && answerIndex <= question.Variantlar.Count)
                {
                    string correctAnswer = question.Variantlar[answerIndex - 1];
                    if (!question.DuzgunCavablar.Contains(correctAnswer))
                    {
                        question.DuzgunCavablar.Add(correctAnswer);
                    }
                    else
                    {
                        Console.WriteLine("Bu düzgün cavab artıq əlavə edilib.");
                    }
                }
                else
                {
                    Console.WriteLine("Daxil etdiyiniz nömrə variantların sayı daxilində deyil. Yenidən cəhd edin.");
                }
            }
            else
            {
                Console.WriteLine("Zəhmət olmasa, yalnız rəqəm daxil edin.");
            }
        }

        if (question.DuzgunCavablar.Count == 0)
        {
            Console.WriteLine("Ən azı bir düzgün cavab daxil edilməlidir!");
            Console.ReadLine();
            return null;
        }

        return question;
    }
}