using MensaBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;

namespace MensaBot
{
    public class TelegramBot
    {
        private readonly TelegramBotClient _BotClient;
        private readonly CancellationTokenSource _Cts;

        public TelegramBot()
        {
            _BotClient = new TelegramBotClient("");
            _Cts = new CancellationTokenSource();

            ReceiverOptions receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>() // receive all update types
            };

            _BotClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: _Cts.Token
            );
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message is not { } message)
                return;

            if (message.Text is not { } messageText)
                return;

            long chatId = message.Chat.Id;

            Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

            // Echo received message text
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "You said:\n" + messageText,
                cancellationToken: cancellationToken);
        }

        private Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
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

        private string GetCanteensText()
        {
            OpenMensa openMensa = new OpenMensa();
            List<Canteen> canteens = openMensa.GetAllCanteens();
            if (canteens == null || canteens.Count == 0)
                return string.Empty;

            StringBuilder stringBuilder = new StringBuilder();
            canteens.ForEach(x => stringBuilder.AppendLine($"{x.Id} - {x.Name} - {x.Address}"));
            return stringBuilder.ToString();
        }

        private string GetMealsText(Canteen canteen)
        {
            return GetMealsText(canteen, DateTime.UtcNow);
        }

        private string GetMealsText(Canteen canteen, DateTime dateTime)
        {
            if (canteen == null || canteen == default)
                return string.Empty;

            OpenMensa openMensa = new OpenMensa();
            Day dayInformation = openMensa.GetDayInformation(canteen.Id, dateTime);
            if (dayInformation == null || dayInformation == default)
                return string.Empty;

            if (dayInformation.IsClosed == true)
                return $"{canteen.Name} ist am  {dateTime.ToString("dd.MM.yyyy")} geschlossen.";

            List<Meal> meals = openMensa.GetMeals(canteen.Id, dateTime);
            return "";
        }
    }
}
