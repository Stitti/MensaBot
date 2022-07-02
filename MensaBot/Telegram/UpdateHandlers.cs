using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace MensaBot.Telegram
{
    internal class UpdateHandlers
    {
        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message is not { } message)
                return;

            if (message.Text is not { } messageText)
                return;

            long chatId = message.Chat.Id;

            string response = GetResponse(messageText);
            botClient.SendTextMessageAsync(chatId, "").Wait();
        }

        public static Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        private static string GetResponse(string message)
        {
            string[] args = message.Split(' ');
            string response;
            switch (args[0])
            {
                case "/canteens":
                    return Commands.GetCanteens();

                case "/meals":
                    return Commands.GetMeals(args.Skip(0).ToArray());

                default:
                    return "Befehl nicht gefunden";
            }
        }
    }
}
