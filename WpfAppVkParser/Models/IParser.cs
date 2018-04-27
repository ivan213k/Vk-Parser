using System.Threading;
using System.Threading.Tasks;

namespace WpfAppVkParser.Models
{
    public delegate void NewData(User newdata);
    public delegate void OnCompleted();
    public delegate void NewMessage(string message);
    public delegate void ProgressChaneged(int curruntprogress, int maxvalue);
    interface IParser
    {
        event NewData OnNewData; //Повідомляє про отримання даних
        event OnCompleted OnCompleted; //Повідомляє про завершення роботи
        Task<bool> AuthorizeAsync(string login, string password , string captcha = null, long? sid = null); //Метод авторизації
        void StartParseAsync(string groupId, FilterParametrs filter, CancellationTokenSource cts);// Метод початку роботи парсера
    }
}
