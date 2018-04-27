using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkNet;
using VkNet.Enums;
using VkNet.Model.RequestParams.Database;

namespace WpfAppVkParser.Models
{
    static class ParametrLists //responsible for receiving lists with parameters
    {
        private static readonly VkApi VkApi;
        static ParametrLists()
        {
            VkApi = new VkApi();
        }

        public static async Task<Dictionary<long?,string>> GetListCountriesAsync()
        {
            var list = new Dictionary<long?,string>();
            var templist = await VkApi.Database.GetCountriesAsync(true,null,1000);
            foreach (var item in templist)
            {
                list.Add(item.Id, item.Title);
            }
            return list;
        }

        public static async Task<Dictionary<long?,string>> GetListCityAsync(int idCountry) 
        {
            var list = new Dictionary<long?,string>();
            GetCitiesParams getCitiesParams = new GetCitiesParams {CountryId = idCountry};
            var templist =  await VkApi.Database.GetCitiesAsync(getCitiesParams);
            foreach (var item in templist)
            {
                list.Add(item.Id, item.Title);
            }
            return list; 
        }
        public static async Task<List<string>> GetUniversityListAsync(int countryId, int cityId) 
        {
            var list = new List<string>();
            var templist = await VkApi.Database.GetUniversitiesAsync(countryId,cityId);
            foreach (var item in templist)
            {
                list.Add(item.Name);
            }
            return list;
        }

        public static List<string> GetRelationList() 
        {
            var type = typeof(RelationType);
            var enumnames = type.GetEnumNames();
            return enumnames.ToList();   
        }

    }
}
