
using QuizProject.Models;
using QuizProject.Services.Interface;

namespace QuizProject.Services;

public class ScoreService : IScoreService
{
    private JsonDataProvider<UserScore> _provider;
    private List<UserScore> _scores;

    public ScoreService(JsonDataProvider<UserScore> provider)
    {
        _provider = provider;
        _scores = _provider.Load() ?? new List<UserScore>();
    }

    public void SaveScore(UserScore newScore)
    {
        var existingScore = _scores.FirstOrDefault(s => s.Username == newScore.Username && s.Category == newScore.Category);

        if (existingScore != null)
        {
            existingScore.Score += newScore.Score;
            existingScore.Date = DateTime.Now;
        }
        else
        {
            _scores.Add(newScore);
        }

        var top20SameCategory = _scores
        .Where(s => s.Category == newScore.Category)
        .OrderByDescending(s => s.Score)
        .ThenBy(s => s.Date)
        .Take(20);

        var otherCategories = _scores
            .Where(s => s.Category != newScore.Category);

        var result = top20SameCategory
            .ToList(); 

        result.AddRange(otherCategories);


        _provider.Save(_scores);
    }



    public List<UserScore> GetUserScores(string username)
    {
        return _scores
            .Where(x => x.Username == username)
            .OrderByDescending(x => x.Date)
            .ToList();
    }

    public List<UserScore> GetTopScores(string category, int count)
    {
        return _scores
            .Where(x => x.Category == category && x.Score > 0)
            .OrderByDescending(x => x.Score)
            .ThenBy(x => x.Date)
            .Take(count)
            .ToList();
    }
}

