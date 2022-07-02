using MensaBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MensaBot
{
    public class TextAnalyzer
    {
        public Tuple<int, DateTime> Run(string input)
        {
            int canteenId = GetCanteenFromString(input);
            DateTime dateTime = GetDateFromString(input);
            return new Tuple<int, DateTime>(canteenId, dateTime);
        }

        private int GetCanteenFromString(string input)
        {
            OpenMensa openMensa = new OpenMensa();
            List<Canteen> canteens = openMensa.GetAllCanteens();
            Canteen match = canteens.Where(x => input.Contains(x.Name, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (match != null)
                return match.Id;

            List<Tuple<int, string>> levenStheinResults = canteens.Select(x => ComputeLevensthein(x.Name, input)).ToList();
            string stringMatch = levenStheinResults.OrderByDescending(x => x.Item1).FirstOrDefault()?.Item2;
            if (string.IsNullOrWhiteSpace(stringMatch))
                return -1;

            match = canteens.FirstOrDefault(x => x.Name.Equals(stringMatch, StringComparison.Ordinal));
            return (match == null) ? -1 : match.Id;

        }

        private Tuple<int, string> ComputeLevensthein(string mensaName, string inputText)
        {
            int n = mensaName.Length;
            int m = inputText.Length;
            int[,] d = new int[n + 1, m + 1];

            if (n == 0)
            {
                return new Tuple<int, string>(m, mensaName);
            }

            if (m == 0)
            {
                return new Tuple<int, string>(n, mensaName);
            }

            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; d[0, j] = j++)
            {
            }

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = (inputText[j - 1] == mensaName[i - 1]) ? 0 : 1;

                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }

            return new Tuple<int, string>(d[n, m], mensaName);
        }

        private DateTime GetDateFromString(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return DateTime.MinValue;

            switch (input)
            {
                // today
                case "heute":
                    return DateTime.UtcNow;

                // tomorrow
                case "morgen":
                    return DateTime.UtcNow.AddDays(1);
            }

            string[] textParts = input.Split(' ');
            foreach(string part in textParts)
            {
                if (DateTime.TryParse(part, out DateTime date) == true)
                    return date;
            }

            return DateTime.UtcNow;
        }
    }
}
