using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace WpfAppVkParser.ViewModels.Converters
{
    class EnUaRelationConverter : IValueConverter //перекладає елементи списку «Сімейний стан»
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string tempvalue)
            {
                switch (tempvalue)
                {
                    case "Unknown": return " ";
                    case "NotMarried": return "Не одружений ";
                    case "HasFriend": return "Зустрічається";
                    case "Engaged": return "Заручений";
                    case "Married": return "Одружений";
                    case "ItsComplex": return "Уcе складно";
                    case "InActiveSearch": return "В активному пошуку";
                    case "Amorous": return "Закоханий";
                    case "CivilMarriage": return "У цивільному шлюбі";
                    default: return "...";
                }
            }
            if (value is IEnumerable)
            {
                List<string> list = value as List<string>;
                var templist = new List<string>();
                foreach (var item in list)
                {
                    switch (item)
                    {
                        case "Unknown": templist.Add(" "); break;
                        case "NotMarried": templist.Add("Не одружений"); break;
                        case "HasFriend": templist.Add("Зустрічається"); break;
                        case "Engaged":  templist.Add("Заручений"); break;
                        case "Married": templist.Add("Одружений"); break;
                        case "ItsComplex": templist.Add("Уcе складно"); break;
                        case "InActiveSearch": templist.Add("В активному пошуку"); break;
                        case "Amorous":  templist.Add("Закоханий"); break;
                        case "CivilMarriage": templist.Add("У цивільному шлюбі"); break;
                        default: templist.Add("..."); break;
                    }
                }
                return templist;
            }
            return "...";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string tempvalue = value as string;
            switch (tempvalue)
            {
                case "Невідомо": return "Unknown"; 
                case "Не одружений": return "NotMarried"; 
                case "Зустрічається": return "HasFriend"; 
                case "Заручений": return "Engaged"; 
                case "Одружений": return "Married"; 
                case "Уcе складно": return "ItsComplex"; 
                case "В активному пошуку": return "InActiveSearch"; 
                case "Закоханий": return "Amorous"; 
                case "У цивільному шлюбі": return "CivilMarriage"; 
                default: return "";
            }
        }
    }
}
