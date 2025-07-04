namespace QuizProject.JsonDataProviderr.Interfacee;


public interface IDataProvider<T>
{
    T Load();
    void Save(T data);
}
