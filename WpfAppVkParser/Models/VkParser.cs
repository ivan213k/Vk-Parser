using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Exception;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.Utils;

namespace WpfAppVkParser.Models
{
    class VkParser: IParser  //responsible for parse information about community members
    {
        private readonly ulong appid = 6245860;
        private readonly VkApi vkapi;
        ReadOnlyCollection<Group> currentgroup;
        List<string> list;
        readonly GroupsGetMembersParams @params;
        ApiAuthParams apiparams;

        public event NewData OnNewData;
        public event OnCompleted OnCompleted;
        public event NewMessage NewMessage;
        public event ProgressChaneged ProgressChange;

        public VkParser()
        {
            vkapi = new VkApi();
            @params = new GroupsGetMembersParams {Fields = UsersFields.All};
            apiparams = new ApiAuthParams();
        }

        User user;
        protected User User
        {
            get => user;
            set
            {
                user = value;
                OnNewData?.Invoke(value);
            }
        }
        //Authorization
        public async Task<bool> AuthorizeAsync(string login, string password, string captcha = null, long? sid= null) // метод авторизації
        {
            apiparams.ApplicationId = appid;
            apiparams.Login = login;
            apiparams.Password = password;
            apiparams.Settings = Settings.All;
            if (captcha!=null&& sid!=null)
            {
                apiparams.CaptchaKey = captcha;
                apiparams.CaptchaSid = sid;
            }
                      
            await vkapi.AuthorizeAsync(apiparams);
           
            if (vkapi.IsAuthorized) NewMessage?.Invoke(string.Format("[{0}:{1}:{2}] Авторизація пройшла успішно", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second));
            return vkapi.IsAuthorized;
        }
        bool IsDateInRange(DateTime leftdate, DateTime rightdate, string currentdate) // checks whether the date range is in place
        {
            try
            {
                DateTime date = DateTime.Parse(currentdate);
                if (date>=leftdate&&date<=rightdate)
                {
                    return true;
                } return false;
            }
            catch (Exception)
            {

                return false;
            }
        }
        
        public async void StartParseAsync(string groupId, FilterParametrs filter, CancellationTokenSource tokenSource)
        {
            await Task.Run(() =>
             {
                 try
                 {
                     NewMessage?.Invoke(string.Format("[{0}:{1}:{2}] Парсинг почато", DateTime.Now.Hour,
                         DateTime.Now.Minute, DateTime.Now.Second));
                     list = new List<string>();
                     list.Add(groupId);
                     currentgroup = vkapi.Groups.GetById(list, groupId, GroupsFields.All);
                     ShowInformation();
                     @params.GroupId = groupId;
                     @params.Offset = 0;
                     int count = GetIterationsCount();
                     var listuser = new VkCollection<VkNet.Model.User>[count];
                     for (int i = 0; i < count; i++)
                     {
                         if (tokenSource.IsCancellationRequested)
                         {
                             OnCompleted?.Invoke();
                             return;
                         }

                         listuser[i] = vkapi.Groups.GetMembers(@params);
                         foreach (var el in listuser[i])
                         {
                             User tempuser = new User();
                             if (el.Country == null)
                             {
                                 if (!string.IsNullOrWhiteSpace(filter.Country))
                                 {
                                     continue;
                                 }

                                 tempuser.Country = "";
                             }
                             else if (!string.IsNullOrWhiteSpace(filter.Country))
                             {
                                 if (int.Parse(filter.Country) == el.Country.Id)
                                 {
                                     tempuser.Country = el.Country.Title;
                                 }
                                 else continue;
                             }
                             else tempuser.Country = el.Country.Title;

                             #region

                             if (el.BirthDate != null)
                             {
                                 if (filter.IsFilterBDate)
                                 {
                                     if (IsDateInRange(filter.BeginDate, filter.EndDate, el.BirthDate))
                                     {
                                         tempuser.BDate = el.BirthDate;
                                     }
                                     else continue;
                                 }
                                 else tempuser.BDate = el.BirthDate;
                             }
                             else
                             {
                                 tempuser.BDate = el.BirthDate;
                             }

                             #endregion

                             if (el.City == null)
                             {
                                 if (!string.IsNullOrWhiteSpace(filter.City))
                                     continue;
                                 tempuser.City = "";
                             }
                             else if (!string.IsNullOrWhiteSpace(filter.City))
                             {
                                 if (int.Parse(filter.City) == el.City.Id)
                                 {
                                     tempuser.City = el.City.Title;
                                 }
                                 else continue;
                             }
                             else tempuser.City = el.City.Title;

                             if (el.MobilePhone == null) tempuser.MobilePhone = "";
                             else tempuser.MobilePhone = el.MobilePhone;
                             if (el.Connections.Skype == null) tempuser.Skype = "";
                             else tempuser.Skype = el.Connections.Skype;
                             if (el.Connections.Instagram == null) tempuser.Instagram = "";
                             else tempuser.Instagram = el.Connections.Instagram;
                             if (!string.IsNullOrWhiteSpace(filter.Relation))
                             {
                                 if (el.Relation.ToString() == filter.Relation)
                                 {
                                     tempuser.Relation = el.Relation.ToString();
                                 }
                                 else continue;
                             }
                             else tempuser.Relation = el.Relation.ToString();


                             tempuser.Id = el.Id.ToString();
                             tempuser.FirstName = el.FirstName;
                             tempuser.LastName = el.LastName;
                             if (filter.Sex != null)
                             {


                                 if (filter.Sex.Contains("Чоловіча"))
                                 {
                                     if (el.Sex.ToString() == "Male") tempuser.Sex = el.Sex.ToString();
                                     else continue;
                                 }
                                 else if (filter.Sex.Contains("Жіноча"))
                                 {
                                     if (el.Sex.ToString() == "Female") tempuser.Sex = el.Sex.ToString();
                                     else continue;
                                 }
                             }
                             else
                             {
                                 tempuser.Sex = el.Sex.ToString();
                             }

                             if (el.Education == null) tempuser.University = "";
                             else if (string.IsNullOrWhiteSpace(filter.University))
                             {
                                 tempuser.University = el.Education.UniversityName;
                             }
                             else
                             {
                                 if (el.Education.UniversityName == filter.University)
                                 {
                                     tempuser.University = el.Education.UniversityName;
                                 }
                                 else continue;
                             }

                             User = tempuser;
                             if (tokenSource.IsCancellationRequested)
                             {
                                 OnCompleted?.Invoke();
                                 return;
                             }
                         }

                         @params.Offset += 1000;
                         ProgressChange?.Invoke(i + 1, GetIterationsCount());
                         if (tokenSource.IsCancellationRequested)
                         {
                             OnCompleted?.Invoke();
                             return;
                         }

                         Thread.Sleep(filter.DeleyBetwenQery);

                     }

                     OnCompleted?.Invoke(); //  inform subscribers that the work  completed
                 }
                 catch (NullReferenceException)
                 {
                     NewMessage?.Invoke("Id групи невведено!");
                 }
                 catch (InvalidParameterException)
                 {

                     NewMessage?.Invoke("Id групи введено не вірно");
                 }
                 catch (AggregateException)
                 {
                    NewMessage?.Invoke("Інтернет з'єднання відсутнє");
                 }
               
            });
           

        }

        int GetIterationsCount() // get the required number of hits to the server to access all participants
        {
            int count = 0;
            foreach (var el in currentgroup)
            {
                if (el.MembersCount != null)
                {
                    count = (int) el.MembersCount / 1000;
                    if (el.MembersCount % 1000 != 0)
                    {
                        count = (int) el.MembersCount / 1000 + 1;
                    }
                }
            }
            return count;
        }
        void ShowInformation()  // show information about group
        {
            foreach (var el in currentgroup)
            {
                string informationOfgroup = "Ім'я:" + "\t\t" + el.Name + "\n" + "Учасників:" + "\t" + el.MembersCount + "\n";
                MessageBox.Show(informationOfgroup, "Інформація про cпільноту");
            }
        }
    }
}
