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
        }

        public async Task RunAsync()
        {
            using CancellationTokenSource cts = new CancellationTokenSource();

            ReceiverOptions receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = new UpdateType[] { UpdateType.Message }
            };

            _BotClient.StartReceiving(
                updateHandler: UpdateHandlers.HandleUpdateAsync,
                pollingErrorHandler: UpdateHandlers.HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );

            User user = await _BotClient.GetMeAsync();
        }
    }
}
