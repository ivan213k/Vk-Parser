using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;
using VkNet.Exception;
using WpfAppVkParser.Models;
using WpfAppVkParser.Models.ExelExport;

namespace WpfAppVkParser.ViewModels
{
    class MainViewModel : BaseViewModel
    {
        private readonly VkParser vkParser;
        private readonly ExcelExporterXML excelExporter;
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        long? sid;

        public MainViewModel()
        {
            vkParser = new VkParser();
            excelExporter = new ExcelExporterXML();
            vkParser.OnNewData += VkParser_OnNewData;
            vkParser.NewMessage += VkParser_NewMessage;
            excelExporter.OneNewMessage += VkParser_NewMessage;
            vkParser.OnCompleted += VkParser_OnCompleted;
            vkParser.ProgressChange += VkParser_ProgressChange;
            excelExporter.ProgressChaneged += VkParser_ProgressChange;
            AuthorizeCommand = new Command(Authorize);
            StartParseCommand = new Command(StartParse);
            ExportToExelCommand = new Command(ExportToExel);
            ClearTableCommand = new Command(CleareTable);
            SelectCountryCommand = new Command(SelectCoutry);
            SelectUniversityCommand = new Command(SelectUniversity);
            StopParseCommand = new Command(StopParse);
            ListRelation = ParametrLists.GetRelationList();
            BindingOperations.EnableCollectionSynchronization(list, new object());

        }
        //changes the progress of processes
        private void VkParser_ProgressChange(int curruntprogress, int maxvalue)
        {
            MaxValueProgress = maxvalue;
            CurrentProgress = curruntprogress;
        }

        private void VkParser_OnCompleted()//handles shutdown parse
        {
            LogText = $"[{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}] Парсинг завершено";
            
        }

        private void VkParser_NewMessage(string message) //adds a message to the log
        {
            LogText = message;
        }

        #region PropertyUi
        bool isauthorize;
        public bool ISAutorize
        {
            get
            {
                return isauthorize;
            }
            private set
            {
                isauthorize = value; OnePropertyChanged(nameof(ISAutorize));
            }
        }

        public bool IsFilterDate { get; set; }

        bool isDeterminate;
        public bool IsDeterminate
        {
            get { return isDeterminate; }
            set { isDeterminate = value; OnePropertyChanged(nameof(IsDeterminate)); }
        }

        string login;
        public string Login { private get { return login; } set { login = value; } }

        #region
        string password;
        public string Password { private get { return password; } set { password = value; } }
        #endregion

        bool isCaptchaNeded;
        public bool IsCaptchaNeded
        {
            get { return isCaptchaNeded; }
            set { isCaptchaNeded = value; OnePropertyChanged(nameof(IsCaptchaNeded)); }
        }

        string captchaImageUrl;
        public string CaptchaimageUrl
        {
            get { return captchaImageUrl; }
            set { captchaImageUrl = value; OnePropertyChanged(nameof(CaptchaimageUrl)); }
        }

        string groupid;
        public string GroupId
        {
            private get { return groupid; }
            set { groupid = value; OnePropertyChanged(nameof(GroupId)); }
        }

        string captcha;
        public string Captcha
        {
            get { return captcha; }
            set { captcha = value; OnePropertyChanged(nameof(Captcha)); }
        }

        List<string> listuniversity = new List<string>();
        public List<string> ListUniversity
        {
            get { return listuniversity; }
            set { listuniversity = value; OnePropertyChanged(nameof(ListUniversity)); }
        }

        ObservableCollection<User> list = new ObservableCollection<User>();
        public ObservableCollection<User> List { get { return list; } set { list = value; } }

        Dictionary<long?, string> listcountry = new Dictionary<long?, string>();
        public Dictionary<long?, string> ListCountry
        {
            get { return listcountry; }
            set { listcountry = value; OnePropertyChanged(nameof(ListCountry)); }
        }
        long? selectedCountryId;
        public string SelectedCountry
        {
            get { return selectedCountryId.ToString(); }
            set
            {
                foreach (var el in listcountry)
                {
                    if (value.Contains(el.Value))
                    {
                        selectedCountryId = el.Key;
                    }
                }
                SelectCity(null);

            }
        }

        Dictionary<long?, string> listcity = new Dictionary<long?, string>();
        public Dictionary<long?, string> ListCity
        {
            get { return listcity; }
            set
            {
                listcity = value;
                OnePropertyChanged(nameof(ListCity));
            }
        }
        long? selectedCityId;
        public string SelectedCity
        {
            set
            {
                foreach (var el in listcity)
                {
                    if (value == null) continue;
                    if (value.Contains(el.Value))
                    {
                        selectedCityId = el.Key;
                    }
                }
                SelectUniversity(null);
            }
        }

        string logtext;
        public string LogText { get { return logtext; } set { logtext += value + "\n"; OnePropertyChanged(nameof(LogText)); } }

        public DateTime BeginDate { get; set; }


        public DateTime EndDate { get; set; }


        public string SelectSex { get; set; }

        public string SelectRelation { get; set; }

        public int DeleyBetwenQery { get; set; }

        int maxvalueprogress = 1;
        public int MaxValueProgress { get { return maxvalueprogress; } set { maxvalueprogress = value; OnePropertyChanged(nameof(MaxValueProgress)); } }

        int currentprogress;
        public int CurrentProgress { get { return currentprogress; } set { currentprogress = value; OnePropertyChanged(nameof(CurrentProgress)); } }

        List<string> listrelation;
        public List<string> ListRelation { get { return listrelation; } set { listrelation = value; OnePropertyChanged(nameof(ListRelation)); } }


        public string SelectEducation { get; set; }
        #endregion

        #region Команди
        public Command StartParseCommand { get; }
        public Command AuthorizeCommand { get; }
        public Command ExportToExelCommand { get; }
        public Command ClearTableCommand { get; }
        public Command SelectCountryCommand { get; }
        public Command SelectCityCommand { get; }
        public Command SelectUniversityCommand { get; }
        public Command StopParseCommand { get; }

        //Authorize
        async void Authorize(object parametr)
        {
            var Passwordbox = parametr as PasswordBox;
            Password = Passwordbox.Password;
            IsDeterminate = true;
            try
            {
                ISAutorize = await vkParser.AuthorizeAsync(Login, Password, Captcha, sid);
            }
            catch (CaptchaNeededException ex)
            {
                IsCaptchaNeded = true;
                CaptchaimageUrl = ex.Img.AbsoluteUri;
                sid = ex.Sid;


            }
            catch (VkApiException ex)
            {
                LogText = string.Format("[{0}:{1}:{2}] Логін або пароль введено невірно", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            }
            finally
            {
                IsDeterminate = false;
            }
        }
        //start parse
        void StartParse(object parametr)
        {

            try
            {
                vkParser.StartParseAsync(GroupId, new FilterParametrs(SelectedCountry, selectedCityId.ToString(), BeginDate, EndDate, SelectSex, SelectRelation, SelectEducation, DeleyBetwenQery, IsFilterDate), cancellationTokenSource);
            }
            catch
            {
                LogText = string.Format("[{0}:{1}:{2}] ID групи введено невірно", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            }
        }

        void ExportToExel(object parametr)//export to MS Excel
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = @"Exel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.ShowDialog();
            if (!string.IsNullOrWhiteSpace(saveFileDialog.FileName))
            {
                excelExporter.ExportToExcelAsync(list, saveFileDialog.FileName);
            }
        }

        void CleareTable(object parametr) // clear table
        {
            List.Clear();
        }
        async void SelectCoutry(object parametr) // loading country list
        {

            ListCountry = await ParametrLists.GetListCountriesAsync();

        }
        async void SelectCity(object parametr) // loading city list
        {
            ListCity = await ParametrLists.GetListCityAsync((int)selectedCountryId);
        }
        async void SelectUniversity(object parametr) //loading university list
        {
            ListUniversity = await ParametrLists.GetUniversityListAsync((int)selectedCountryId, (int)selectedCityId);
        }

        void StopParse(object parametr) // stops parse
        {
            cancellationTokenSource.Cancel();
            CurrentProgress = 0;
        }

        #endregion

        private void VkParser_OnNewData(User newdata) //adds the data to the table
        {
            List.Add(newdata);
        }
       
    }
}
