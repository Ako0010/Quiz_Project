
using QuizProject.JsonDataProviderr;
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

        _scores.Add(newScore);
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
        string normalizedCategory = category.Trim().ToLower();

        var groupedScores = _scores
                       .Where(x => x.Category.Trim().ToLower() == normalizedCategory && x.Score > 0)
                       .GroupBy(x => x.Username.Trim().ToLower())
                       .Select(g => new UserScore
                       {
                           Username = g.First().Username,
                           Category = category,
                           Score = g.Sum(x => x.Score),
                           Date = g.Max(x => x.Date)
                       })
                       .OrderByDescending(x => x.Score)
                       .ThenBy(x => x.Date)
                       .Take(count)
                       .ToList();

        return groupedScores;
    }
}

