using System;

namespace WpfAppVkParser.Models
{
    struct FilterParametrs // параметр для фільтру
    {
        public string Country { get; set; } 
        public string City { get; set; }
        public DateTime BeginDate { get; set; } 
        public DateTime EndDate { get; set; }
        public string Sex { get; set; }
        public string Relation { get; set; }
        public string University { get; set; }
        public int DeleyBetwenQery { get; set; }
        public bool IsFilterBDate { get; set; }

        public FilterParametrs(string country, string city, DateTime beginDate, DateTime endDate, string sex, string relation, string university, int deleyBetwenQery,bool isFilterBDate)
        {
            Country = country;
            City = city;
            BeginDate = beginDate;
            EndDate = endDate;
            Sex = sex;
            Relation = relation;
            University = university;
            DeleyBetwenQery = deleyBetwenQery;
            IsFilterBDate = isFilterBDate;
        }
    }
}
