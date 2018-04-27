using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppVkParser.Models
{
    struct FilterParams
    {
        public string Country { get; set; }
        public string City { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Sex { get; set; }
        public string Relation { get; set; }
        public string University { get; set; }

        public FilterParams(string Country, string City, DateTime BeginDate, DateTime EndDate, string Sex, string Relation, string University)
        {
            this.Country = Country;
            this.City = City;
            this.BeginDate = BeginDate;
            this.EndDate = EndDate;
            this.Sex = Sex;
            this.Relation = Relation;
            this.University = University;
        }
    }
}
