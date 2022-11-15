using eDarslarBot.Constants;
using eDarslarBot.Pages;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace eDarslarBot
{
    public class Program
    {
        private readonly static TelegramBotClient Bot = new TelegramBotClient
            (BotConstants.BOT_TOKEN);

        public static ReceiverOptions receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = new UpdateType[]
                {
                    UpdateType.Message,
                    UpdateType.EditedMessage,
                    UpdateType.ChannelPost,
                }
        };

        public static async Task Main(string[] args)
        {
            Console.WriteLine("Bot started -> @eDarslarBot\n");

            Bot.StartReceiving(ForUser.UpdateHandler, ForUser.ErrorHandler, receiverOptions);

            Console.ReadKey();
        }
    }
}