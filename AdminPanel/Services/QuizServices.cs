
using AdminPanel.Models;
using Newtonsoft.Json;

namespace AdminPanel.Services
{
    public class QuizService
    {
        private string _jsonPath;

        public QuizService(string jsonPath)
        {
            _jsonPath = jsonPath;
        }

        public List<Quiz> GetAllQuizzes()
        {
            if (!File.Exists(_jsonPath))
                return new List<Quiz>();

            try
            {
                using (var fs = new FileStream(_jsonPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var sr = new StreamReader(fs))
                {
                    var json = sr.ReadToEnd();
                    return JsonConvert.DeserializeObject<List<Quiz>>(json);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fayl oxunarkən xəta: {ex.Message}");
                return new List<Quiz>();
            }
        }

        public void SaveQuizzes(List<Quiz> quizzes)
        {
            var json = JsonConvert.SerializeObject(quizzes, Formatting.Indented);

            try
            {
                using (var fs = new FileStream(_jsonPath, FileMode.Create, FileAccess.Write, FileShare.None))
                using (var sw = new StreamWriter(fs))
                {
                    sw.Write(json);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fayl yazılarkən xəta: {ex.Message}");
            }
        }

        public void AddOrUpdateQuiz(Quiz quiz)
        {
            var quizzes = GetAllQuizzes();
            var existing = quizzes.FirstOrDefault(q => q.Id == quiz.Id);
            if (existing != null)
                quizzes.Remove(existing);

            quizzes.Add(quiz);
            SaveQuizzes(quizzes);
        }

        public void DeleteQuiz(int quizId)
        {
            var quizzes = GetAllQuizzes();
            var quizToRemove = quizzes.FirstOrDefault(q => q.Id == quizId);
            if (quizToRemove != null)
            {
                quizzes.Remove(quizToRemove);
                SaveQuizzes(quizzes);
            }
        }
    }
}
