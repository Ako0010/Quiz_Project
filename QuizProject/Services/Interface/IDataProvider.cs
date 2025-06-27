

namespace QuizProject.Services.Interface;


public interface IDataProvider<T>
{
    T Load();
    void Save(T data);
}
