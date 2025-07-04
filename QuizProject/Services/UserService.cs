

using QuizProject.JsonDataProviderr.Interfacee;
using QuizProject.Models;
using QuizProject.Services.Interface;

namespace QuizProject.Services;

public class UserService : IUserService
{
    private IDataProvider<List<User>> _dataProvider;
    private List<User> _users;

    public UserService(IDataProvider<List<User>> dataProvider)
    {
        _dataProvider = dataProvider;
        _users = _dataProvider.Load() ?? new List<User>(); 
    }

    public User Register(string username, string password, DateTime birthDate)
    {
        if (_users.Any(u => u.Username == username)) return null;

        int newId = _users.Count > 0 ? _users.Max(u => u.Id) + 1 : 1;

        var user = new User
        {
            Id = newId,
            Username = username,
            Password = password,
            BirthDate = birthDate
        };
        _users.Add(user);
        _dataProvider.Save(_users);
        return user;
    }

    public User Login(string username, string password)
    {
        return _users.FirstOrDefault(u => u.Username == username && u.Password == password);
    }

    public void UpdateUser(User updatedUser)
    {
        var existingUser = _users.FirstOrDefault(u => u.Username == updatedUser.Username);
        if (existingUser != null)
        {
            existingUser.Password = updatedUser.Password;
            existingUser.BirthDate = updatedUser.BirthDate;
            SaveUsers();
        }
    }

    private void SaveUsers()
    {
        _dataProvider.Save(_users);
    }

}
