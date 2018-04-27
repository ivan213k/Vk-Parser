using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppVkParser.Models.ExelExport
{
    class ExcelExporterXML // відповідає за експорт даних в MS  Excel.
    {
        public event ProgressChaneged ProgressChaneged;
        public event NewMessage OneNewMessage;

        Cell ConstructCell(string value, CellValues dataType) // Повертає екземпляр клітинки Excel 
        {
            return new Cell()
            {
                CellValue = new CellValue(value),
                DataType = new EnumValue<CellValues>(dataType)
            };
        }
        // Запис даних в файл MS Excel
        public async void ExportToExcelAsync(ObservableCollection<User> list, string fileName)
        {
            await Task.Run(() =>
            {
                using (SpreadsheetDocument document = SpreadsheetDocument.Create(fileName, SpreadsheetDocumentType.Workbook))
                {
                    WorkbookPart workbookPart = document.AddWorkbookPart();
                    workbookPart.Workbook = new Workbook();

                    WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                    worksheetPart.Worksheet = new Worksheet();

                    Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());

                    Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Vk" };

                    sheets.Append(sheet);

                    workbookPart.Workbook.Save();
                    SheetData sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

                    Row row = new Row();
                    OneNewMessage?.Invoke($"[{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}] Експорт розпочато");
                    row.Append(
                        ConstructCell("Id", CellValues.String),
                        ConstructCell("Ім'я", CellValues.String),
                        ConstructCell("Прізвище", CellValues.String),
                        ConstructCell("Стать", CellValues.String),
                        ConstructCell("День народження", CellValues.String),
                        ConstructCell("Країна", CellValues.String),
                        ConstructCell("Місто", CellValues.String),
                        ConstructCell("Мобільний телефон", CellValues.String),
                        ConstructCell("Skype", CellValues.String),
                        ConstructCell("Instagram", CellValues.String));

                    sheetData.AppendChild(row);
                    int i = 0;
                    foreach (var user in list)
                    {
                        try
                        {
                            row = new Row();
                            row.Append(
                                                ConstructCell(user.Id, CellValues.String),
                                                ConstructCell(user.FirstName, CellValues.String),
                                                ConstructCell(user.LastName, CellValues.String),
                                                ConstructCell(user.Sex, CellValues.String),
                                                ConstructCell(user.BDate, CellValues.String),
                                                ConstructCell(user.Country, CellValues.String),
                                                ConstructCell(user.City, CellValues.String),
                                                ConstructCell(user.MobilePhone, CellValues.String),
                                                ConstructCell(user.Skype, CellValues.String),
                                                ConstructCell(user.Instagram, CellValues.String));

                            sheetData.AppendChild(row);
                            i++;

                        }
                        catch (Exception)
                        {

                            // 
                        }
                        finally
                        {
                            ProgressChaneged?.Invoke(i, list.Count);
                        }

                    }
                    worksheetPart.Worksheet.Save();
                    OneNewMessage?.Invoke($"[{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}] Експорт завершено");
                }
            });
        }
    }
}
