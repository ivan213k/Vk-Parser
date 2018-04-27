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
        event NewData OnNewData; 
        event OnCompleted OnCompleted; 
        Task<bool> AuthorizeAsync(string login, string password , string captcha = null, long? sid = null); 
        void StartParseAsync(string groupId, FilterParametrs filter, CancellationTokenSource cts);
    }
}
