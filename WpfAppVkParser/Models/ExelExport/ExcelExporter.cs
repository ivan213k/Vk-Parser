using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppVkParser.Models.ExelExport
{
    class ExcelExporter
    {
        Application Exel = new Application();
        Workbook Workbook;
        Worksheet Worksheet;
        public event ProgressChaneged progressChaneged;
        public event NewMessage oneNewMessage;

        public ExcelExporter()
        {
            Workbook = Exel.Workbooks.Add(System.Reflection.Missing.Value);
            Worksheet = (Microsoft.Office.Interop.Excel.Worksheet)Workbook.Sheets[1];
        }
        public void WriteToExel(ObservableCollection<User> list, string filepath)
        {
            int i = 1;
            oneNewMessage.Invoke(string.Format("[{0}:{1}:{2}] Експорт розпочато", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second));
            Worksheet.Cells[1, 1] = "Id";
            Worksheet.Cells[1, 2] = "Ім'я";
            Worksheet.Cells[1, 3] = "Прізвище";
            Worksheet.Cells[1, 4] = "Стать";
            Worksheet.Cells[1, 5] = "День народження";
            Worksheet.Cells[1, 6] = "Країна";
            Worksheet.Cells[1, 7] = "Місто";
            Worksheet.Cells[1, 8] = "Мобільний телефон";
            Worksheet.Cells[1, 9] = "Skype";
            Worksheet.Cells[1, 10] = "Instagram";
            foreach (var row in list)
            {
                i++;
                try
                {
                    Worksheet.Cells[i, 1] = row.Id;
                    Worksheet.Cells[i, 2] = row.FirstName;
                    Worksheet.Cells[i, 3] = row.LastName;
                    Worksheet.Cells[i, 4] = row.Sex;
                    Worksheet.Cells[i, 5] = row.BDate;
                    Worksheet.Cells[i, 6] = row.Country;
                    Worksheet.Cells[i, 7] = row.City;
                    Worksheet.Cells[i, 8] = row.MobilePhone;
                    Worksheet.Cells[i, 9] = row.Skype;
                    Worksheet.Cells[i,10] = row.Instagram;
                }
                catch
                {
                    continue;
                }
                finally
                {
                    progressChaneged.Invoke(i, list.Count);
                }
            }
            Exel.Visible = false;
            Exel.UserControl = true;
            
            Workbook.SaveAs(filepath);
            Exel.Quit();
            oneNewMessage.Invoke(string.Format("[{0}:{1}:{2}] Експорт завершено", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second));
        }
    }
}

