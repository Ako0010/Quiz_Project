
using Newtonsoft.Json;


namespace QuizProject.Models;



public class Quiz
{
    [JsonProperty("Id")]
    public int Id { get; set; }
    public string Category { get; set; }

    [JsonProperty("Questions")]
    public List<Question> Questions { get; set; }
}