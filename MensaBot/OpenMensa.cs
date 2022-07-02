using MensaBot.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MensaBot
{
    public class OpenMensa
    {
        private readonly HttpClient _HttpClient;

        public OpenMensa()
        {
            _HttpClient = new HttpClient() { BaseAddress = new Uri("https://openmensa.org/api/v2/") };
        }

        private T ConvertHttpResponse<T>(HttpResponseMessage httpResponse) where T : new()
        {
            if (httpResponse.IsSuccessStatusCode == false)
                return default(T);

            string json = httpResponse.Content.ReadAsStringAsync().Result;
            T item = new T();
            try
            {
                item = JsonConvert.DeserializeObject<T>(json);
            }
            catch(Exception ex)
            {
                string test = ex.Message;
            }

            return item;
        }

        public List<Canteen> GetAllCanteens()
        {
            HttpResponseMessage httpResponse = _HttpClient.GetAsync("canteens").Result;
            return ConvertHttpResponse<List<Canteen>>(httpResponse);
        }

        public Day GetDayInformation(int canteenId, DateTime dateTime)
        {
            HttpResponseMessage httpResponse = _HttpClient.GetAsync($"canteens/{canteenId}/days/{dateTime.ToString("dd-MM-yyyy")}").Result;
            return ConvertHttpResponse<Day>(httpResponse);
        }

        public List<Meal> GetMeals(int canteenId, DateTime dateTime)
        {
            HttpResponseMessage httpResponse = _HttpClient.GetAsync($"canteens/{canteenId}/days/{dateTime.ToString("dd-MM-yyyy")}/meals").Result;
            return ConvertHttpResponse<List<Meal>>(httpResponse);
        }
    }
}
