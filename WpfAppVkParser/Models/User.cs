namespace WpfAppVkParser.Models
{
    public struct User // описує користувача
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Sex { get; set; }

        public string BDate { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string MobilePhone { get; set; }

        public string Skype { get; set; }

        public string Instagram { get; set; }

        public string Relation { get; set; }

        public string University { get; set; }

        public User(string id, string firstName, string lastName, string sex, string bDate, string country, string city, string mobilePhone, string skype, string instagram, string relation, string university)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Sex = sex;
            BDate = bDate;
            Country = country;
            City = city;
            MobilePhone = mobilePhone;
            Skype = skype;
            Instagram = instagram;
            Relation = relation;
            University = university;
        }
    }
}
