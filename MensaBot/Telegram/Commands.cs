﻿using MensaBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MensaBot.Telegram
{
    internal class Commands
    {
        public static string GetCanteens()
        {
            OpenMensa openMensa = new OpenMensa();
            List<Canteen> canteens = openMensa.GetAllCanteens();
            if (canteens == null || canteens.Count == 0)
                return string.Empty;

            StringBuilder stringBuilder = new StringBuilder();
            canteens.ForEach(x => stringBuilder.AppendLine($"{x.Id} - {x.Name} - {x.Address}"));
            return stringBuilder.ToString();
        }

        public static string GetMeals(string[] args)
        {
            if (args == null || args.Length == 0)
                return string.Empty;

            if (TryParseArgument(args, 'c', out string canteenParameter) == false)
                return string.Empty;

            Canteen targetCanteen = GetTargetCanteen(canteenParameter);
            if (targetCanteen == null || targetCanteen == default)
                return string.Empty;

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
            return "";
        }

        private static bool TryParseArgument(string[] args, char argumentCode, out string result)
        {
            result = null;
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
