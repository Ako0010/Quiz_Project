


using Newtonsoft.Json;

namespace AdminPanel.Models;

public class Question
{
    [JsonProperty("QuestionId")]
    public int QuestionId { get; set; }

    [JsonProperty("Sual")]
    public string Sual { get; set; }

    [JsonProperty("Variantlar")]
    public List<string> Variantlar { get; set; }

    [JsonProperty("DuzgunCavablar")]
    public List<string> DuzgunCavablar { get; set; }
}
