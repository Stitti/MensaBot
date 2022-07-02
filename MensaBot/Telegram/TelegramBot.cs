using MensaBot.Models;
using Newtonsoft.Json;
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

namespace MensaBot.Telegram
{
    public class TelegramBot
    {
        private readonly TelegramBotClient _BotClient;
        private readonly CancellationTokenSource _Cts;

        public TelegramBot()
        {
            string secret;
#if DEBUG
            StreamReader reader = new StreamReader("secret.json");
            string json = reader.ReadToEnd();
            Dictionary<string, string> secrets = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            secret = secrets["telegramSecret"];
#else
            secret = Environment.GetEnvironmentVariable("TELEGRAM_SECRET");
#endif
            _BotClient = new TelegramBotClient(secret);
            _Cts = new CancellationTokenSource();

            ReceiverOptions receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>() // receive all update types
            };

            _BotClient.StartReceiving(
                updateHandler: UpdateHandlers.HandleUpdateAsync,
                pollingErrorHandler: UpdateHandlers.HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: _Cts.Token
            );
        }
    }
}
