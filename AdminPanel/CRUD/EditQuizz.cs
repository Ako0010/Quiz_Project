

using AdminPanel.Models;
using AdminPanel.Services;

namespace AdminPanel.CRUD;

public class EditQuizz
{
    public static void EditQuiz()
    {
        Console.Clear();
        Console.Write("Redaktə etmək istədiyiniz Quiz ID-ni daxil edin: ");
        if (!int.TryParse(Console.ReadLine(), out int quizId))
        {
            Console.WriteLine("Yanlış ID. Davam etmək üçün Enter basın...");
            Console.ReadLine();
            return;
        }

        var quiz = AdminPanell.quizzes.Find(q => q.Id == quizId);
        if (quiz == null)
        {
            Console.WriteLine("Belə ID-li quiz tapılmadı. Davam etmək üçün Enter basın...");
            Console.ReadLine();
            return;
        }

        Console.WriteLine($"Kateqoriya: {quiz.Category}");
        Console.Write("Yeni kateqoriya daxil edin (dəyişdirmək istəmirsinizsə boş buraxın): ");
        var newCategory = Console.ReadLine();
        if (newCategory != null && newCategory.Trim().Length > 0)
            quiz.Category = newCategory;

        bool editingQuestions = true;
        while (editingQuestions)
        {
            Console.Clear();
            Console.WriteLine($"Quiz ID: {quiz.Id} | Kateqoriya: {quiz.Category}");
            Console.WriteLine("Suallar:");
            foreach (var q in quiz.Questions)
            {
                Console.WriteLine($"  {q.QuestionId}. {q.Sual}");
            }

            Console.WriteLine("Seçimlər:");
            Console.WriteLine("1. Sual əlavə et");
            Console.WriteLine("2. Sual redaktə et");
            Console.WriteLine("3. Sual sil");
            Console.WriteLine("4. Quiz-dən çıx");
            Console.Write("Seçiminiz: ");
            var sel = Console.ReadLine();

            switch (sel)
            {
                case "1":
                    var newQ = CreateQuestionn.CreateQuestion(quiz.Questions.Count + 1);
                    quiz.Questions.Add(newQ);
                    break;
                case "2":
                    Console.Write("Redaktə etmək istədiyiniz sual nömrəsi: ");
                    if (int.TryParse(Console.ReadLine(), out int qid))
                    {
                        var question = quiz.Questions.Find(x => x.QuestionId == qid);
                        if (question != null)
                        {
                            EditQuestionn.EditQuestion(question);
                        }
                        else
                        {
                            Console.WriteLine("Belə sual yoxdur. Davam etmək üçün Enter basın...");
                            Console.ReadLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Yanlış nömrə. Davam etmək üçün Enter basın...");
                        Console.ReadLine();
                    }
                    break;
                case "3":
                    Console.Write("Silmək istədiyiniz sual nömrəsi: ");
                    if (int.TryParse(Console.ReadLine(), out int sid))
                    {
                        var questionToRemove = quiz.Questions.Find(x => x.QuestionId == sid);
                        if (questionToRemove != null)
                        {
                            quiz.Questions.Remove(questionToRemove);
                            int num = 1;
                            foreach (var q in quiz.Questions)
                                q.QuestionId = num++;
                        }
                        else
                        {
                            Console.WriteLine("Belə sual yoxdur.");
                            Console.ReadLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Yanlış nömrə.");
                        Console.ReadLine();
                    }
                    break;
                case "4":
                    editingQuestions = false;
                    break;
                default:
                    Console.WriteLine("Yanlış seçim.");
                    Console.ReadLine();
                    break;
            }
        }

        AdminPanell.quizService.SaveQuizzes(AdminPanell.quizzes);
    }
}

public class EditQuestionn
{
    public static void EditQuestion(Question question)
    {
        Console.Clear();
        Console.WriteLine($"Sual: {question.Sual}");
        Console.Write("Yeni sual mətni (dəyişdirmək istəmirsinizsə boş buraxın): ");
        var newText = Console.ReadLine();
        if (newText != null && newText.Trim().Length > 0)
            question.Sual = newText;

        Console.WriteLine("Hazırki variantlar:");
        for (int i = 0; i < question.Variantlar.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {question.Variantlar[i]}");
        }

        Console.WriteLine("Yeni variantlar daxil edin (bitirmək üçün boş buraxın):");
        var newVariants = new List<string>();
        while (true)
        {
            Console.Write("Variant: ");
            var v = Console.ReadLine();
            if (v == null || v.Trim().Length == 0)
                break;
            newVariants.Add(v);
        }
        if (newVariants.Count > 0)
            question.Variantlar = newVariants;

        Console.WriteLine("Düzgün cavablar (mövcud: {0})", string.Join(", ", question.DuzgunCavablar));
        Console.WriteLine("Yeni düzgün cavabları daxil edin (bitirmək üçün boş buraxın):");
        var newCorrects = new List<string>();
        while (true)
        {
            Console.Write("Düzgün cavab: ");
            var c = Console.ReadLine();
            if (c == null || c.Trim().Length == 0)
                break;

            if (question.Variantlar.Contains(c))
                newCorrects.Add(c);
            else
                Console.WriteLine("Variantlar arasında yoxdur, yenidən cəhd edin.");
        }
        if (newCorrects.Count > 0)
            question.DuzgunCavablar = newCorrects;
    }

}
