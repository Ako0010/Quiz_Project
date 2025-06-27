
using QuizProject.Models;
using QuizProject.Services.Interface;

namespace QuizProject.Services;

public class ChangeUserInfo : IChangeUserInfo
{
    public void ChangeUserInfoo(User user, UserService userService)
    {
        Console.Clear();
        Console.WriteLine("Melumat Deyismek");
        Console.WriteLine("1. Şifrəni dəyiş");
        Console.WriteLine("2. Doğum tarixini dəyiş");
        Console.WriteLine("3. Geri Qayit");
        Console.Write("Seçim: ");
        var secim = Console.ReadLine();

        switch (secim)
        {
            case "1":
                Console.Write("Yeni şifrə daxil edin: ");
                var newPassword = Console.ReadLine();
                if (newPassword == null || newPassword.Trim() == "")
                {
                    Console.WriteLine("Şifrə boş ola bilməz!");
                }
                else
                {
                    user.Password = newPassword;
                    userService.UpdateUser(user);
                    Console.WriteLine("Şifrə uğurla dəyişdirildi.");
                }
                break;

            case "2":
                Console.Write("Yeni doğum tarixini daxil edin (yyyy-MM-dd): ");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime newDob))
                {
                    user.BirthDate = newDob;
                    userService.UpdateUser(user);
                    Console.WriteLine("Doğum tarixi uğurla dəyişdirildi.");
                }
                else
                {
                    Console.WriteLine("Yanlış tarix formatı.");
                }
                break;

            case "3":
                return;

            default:
                Console.WriteLine("Yanlış seçim!");
                break;
        }

        Console.WriteLine("Davam etmək üçün Enter basın...");
        Console.ReadLine();
    }
}

