using MensaBot.Telegram;
namespace MensaBot
{
    class Program
    {
        private static ManualResetEvent _Wait = new ManualResetEvent(false);

        static void Main(string[] args)
        {
            Console.WriteLine("Initializing bot...");
            TelegramBot bot = new TelegramBot();
            Console.WriteLine("Initialized bot");
            bot.RunAsync();
            Console.WriteLine("Bot is running");
            _Wait.WaitOne();
        }
    }
}