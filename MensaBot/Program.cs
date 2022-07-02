// See https://aka.ms/new-console-template for more information
using MensaBot.Telegram;

Console.WriteLine("Initializing bot...");
TelegramBot bot = new TelegramBot();
Console.WriteLine("Initialized bot");
bot.RunAsync();
Console.WriteLine("Bot is running");
Console.ReadKey();