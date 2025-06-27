


using Newtonsoft.Json;
using QuizProject.Services.Interface;

namespace QuizProject.Services;


public class JsonDataProvider<T> : IDataProvider<List<T>>
{
    private string _filePath;

    public JsonDataProvider(string filePath)
    {
        _filePath = filePath;
    }

    public List<T> Load()
    {
        if (!File.Exists(_filePath))
            return new List<T>();

        var json = File.ReadAllText(_filePath);
        return JsonConvert.DeserializeObject<List<T>>(json) ?? new List<T>();
    }
    public void Save(List<T> data)
    {
        var json = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(_filePath, json);
    }
}


