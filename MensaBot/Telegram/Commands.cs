using MensaBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MensaBot.Telegram
{
    internal class Commands
    {
        public static string GetCanteens(string[] args)
        {
            if (args == null || args.Length == 0 || TryParseArgument(args, 'c', out string cityParameter) == false)
                return "Bitte überprüfe die Parameter des Befehls";

            OpenMensa openMensa = new OpenMensa();
            List<Canteen> canteens = openMensa.GetAllCanteens();
            if (canteens == null || canteens.Count == 0)
                return "Bitte überprüfe die Parameter des Befehls";

            canteens = canteens.Where(x => x.City.Equals(cityParameter, StringComparison.OrdinalIgnoreCase)).ToList();
            StringBuilder stringBuilder = new StringBuilder();
            canteens.ForEach(x => stringBuilder.AppendLine($"{x.Id} - {x.Name}"));
            return stringBuilder.ToString();
        }

        public static string GetMeals(string[] args)
        {
            if (args == null || args.Length == 0 || TryParseArgument(args, 'c', out string canteenParameter) == false)
                return "Bitte überprüfe die Parameter des Befehls";

            Canteen targetCanteen = GetTargetCanteen(canteenParameter);
            if (targetCanteen == null || targetCanteen == default)
                return "Bitte überprüfe die Parameter des Befehls";

            DateTime dateTime;
            if (TryParseArgument(args, 'd', out string dateTimeParameter) == true)
            {
                dateTime = TextAnalyzer.GetDateFromString(dateTimeParameter);
            }
            else
            {
                dateTime = DateTime.UtcNow;
            }

            OpenMensa openMensa = new OpenMensa();
            Day dayInformation = openMensa.GetDayInformation(targetCanteen.Id, dateTime);
            if (dayInformation == null || dayInformation == default)
                return string.Empty;

            if (dayInformation.IsClosed == true)
                return $"{targetCanteen.Name} ist am  {dateTime.ToString("dd.MM.yyyy")} geschlossen.";

            List<Meal> meals = openMensa.GetMeals(targetCanteen.Id, dateTime);
            StringBuilder stringBuilder = new StringBuilder($"Der Speiseplan für den {dateTime.ToString("dd.MM.yyyy")}");
            meals.ForEach(x => stringBuilder.Append($"{Environment.NewLine}{x.Name}{Environment.NewLine}- Studenten: {x.Prices.Students}{Environment.NewLine}- Angestellte: {x.Prices.Employees}{Environment.NewLine}- Andere: {x.Prices.Others}"));
            return stringBuilder.ToString();
        }

        private static bool TryParseArgument(string[] args, char argumentCode, out string result)
        {
            result = string.Empty;
            int index = (Array.IndexOf(args, $"-{argumentCode}") + 1);
            if (index == 0 || args.Length < index)
                return false;

            string parameter = args[index];
            if (string.IsNullOrWhiteSpace(parameter))
                return false;

            result = parameter;
            return true;
        }

        private static Canteen GetTargetCanteen(string canteenParameter)
        {
            OpenMensa openMensa = new OpenMensa();
            List<Canteen> canteens = openMensa.GetAllCanteens();
            Canteen targetCanteen;
            if (int.TryParse(canteenParameter, out int id) == true)
            {
                targetCanteen = canteens.Where(x => x.Id == id).FirstOrDefault();
            }
            else
            {
                targetCanteen = TextAnalyzer.GetCanteenFromString(canteenParameter);
            }

            return targetCanteen;
        }
    }
}
