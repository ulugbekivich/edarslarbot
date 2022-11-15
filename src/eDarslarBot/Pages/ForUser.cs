using eDarslarBot.Constants;
using eDarslarBot.Interfaces.Repositories;
using eDarslarBot.MockData;
using eDarslarBot.Repositories;
using eDarslarBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace eDarslarBot.Pages
{
    public class ForUser
    {
        private readonly static TelegramBotClient Bot = new TelegramBotClient
            (BotConstants.BOT_TOKEN);
        public static string main_menu_name = "";

        // Update Handler

        public static async Task UpdateHandler(ITelegramBotClient bot, Update update, CancellationToken arg3)
        {
            if(update.Type == UpdateType.Message)
            {
                // commonly used variables
                IUserRepository repository = new UserRepository();
                MainService mainService = new MainService();
                Data data = new Data();

                var text = update.Message.Text;
                var userId = update.Message.Chat.Id.ToString();
                string? username = update.Message.Chat.Username;
                string? fullName = update.Message.Chat.FirstName + update.Message.Chat.LastName;

                ReplyKeyboardMarkup menu =
                    new ReplyKeyboardMarkup(mainService.SortingPrint(data.Menu(), 2, 0));

                if (text == "/panel" && BotConstants.ADMIN_ID == userId)
                {
                    await ForAdmin.UpdateHandler(bot, update, arg3);
                }

                // Response for /start command
                if (text == "/start" || text == "/Start" || text == "/user")
                {
                    await Bot.SendTextMessageAsync(chatId: userId,
                        text: $"<b>Salom {fullName}!</b>",
                        parseMode: ParseMode.Html,
                        replyMarkup: menu);

                    while (fullName.Contains('\''))
                    {
                        fullName = fullName.Replace('\'', '`');
                    }

                    Models.User user = new Models.User()
                    {
                        UserId = userId,
                        FullName = fullName,
                        Username = username
                    };

                    int result = await repository.CreateAsync(user);
                    await bot.ReceiveAsync(updateHandler, ErrorHandler, Program.receiverOptions);
                }
            }
        }

        private static async Task updateHandler(ITelegramBotClient bot, Update update, CancellationToken arg3)
        {
            // commonly used variables
            IUserRepository repository = new UserRepository();
            MainService mainService = new MainService();
            Data data = new Data();

            var text = update.Message.Text;
            var userId = update.Message.Chat.Id.ToString();
            string? username = update.Message.Chat.Username;
            string? fullName = update.Message.Chat.FirstName + update.Message.Chat.LastName;

            ReplyKeyboardMarkup menu =
                new ReplyKeyboardMarkup(mainService.SortingPrint(data.Menu(), 2, 0));

            ReplyKeyboardMarkup back_to_main_menu = new(new[]
            {
                new KeyboardButton[] { "🔙 Bosh menyuga qaytish" },
            })
            {
                ResizeKeyboard = true
            };


            if (text == "/panel" && BotConstants.ADMIN_ID == userId)
            {
                await ForAdmin.UpdateHandler(bot, update, arg3);
            }

            foreach (string item in data.LastMenuAll())
            {
                if (text == item)
                {
                    foreach (int post_id in data.PostsPrint(item))
                    {
                        await Bot.CopyMessageAsync(
                                chatId: userId,
                                fromChatId: -1001767357869,
                                messageId: post_id
                            );
                    }
                }
            }

            foreach (string str in data.Menu())
            {
                if (text == str && text != "💬 Savol va takliflar yozib qoldirish")
                {
                    main_menu_name = str;
                    ReplyKeyboardMarkup internalMenu =
                        new ReplyKeyboardMarkup(mainService.SortingPrint(data.InternalMenu(str), 2, 1));

                    await Bot.SendTextMessageAsync(chatId: userId,
                    text: $"{str}:\n" +
                    $"Kerakli menyuni tanlang",
                    parseMode: ParseMode.Html,
                    replyMarkup: internalMenu);
                    break;
                }
            }

            foreach (string item in data.InternalMenu(main_menu_name))
            {
                if (text == item)
                {
                    ReplyKeyboardMarkup lastMenu =
                        new ReplyKeyboardMarkup(mainService.SortingPrint(data.LastMenu(item), 2, 2));

                    await Bot.SendTextMessageAsync(chatId: userId,
                    text: $"{item}:\n" +
                    $"Kerakli menyuni tanlang",
                    parseMode: ParseMode.Html,
                    replyMarkup: lastMenu);
                    break;
                }
            }


            if (text == "💬 Savol va takliflar yozib qoldirish")
            {
                await mainService.ChangeUserStatus("send", userId);
                await Bot.SendTextMessageAsync(chatId: userId,
                    text: $"🤓Marhamat, savol va takliflaringiz bo'lsa yozib qoldiring.\n" +
                    $"📸 Savol va Takliflaringizni <b>rasm</b> ko'rinishida ham yuborishingiz mumkin.\n" +
                    $"<b>⚡️Tez orada javob berishga harakat qilamiz.</b>",
                    parseMode: ParseMode.Html,
                    replyMarkup: back_to_main_menu);
            }
            else if (text == "🔙 Bosh menyuga qaytish")
            {
                await mainService.ChangeUserStatus("sleep", userId);
                await Bot.SendTextMessageAsync(chatId: userId,
                        text: $"<b>Bosh menyuga qayttik!</b>",
                        parseMode: ParseMode.Html,
                        replyMarkup: menu);
            }

            if (text == "🔝 Asosiy Menu")
            {
                await Bot.SendTextMessageAsync(chatId: userId,
                    text: $"<b>Asosiy menyuga qayttik!</b>",
                    parseMode: ParseMode.Html,
                    replyMarkup: menu);
            }
            else if (text == "🔙 Orqaga")
            {
                await Bot.SendTextMessageAsync(chatId: userId,
                    text: $"<b>Asosiy menyuga orqaga qayttik!</b>",
                    parseMode: ParseMode.Html,
                    replyMarkup: menu);
            }
            else if (text == "◀️ Orqaga")
            {
                ReplyKeyboardMarkup internalMenu =
                                new ReplyKeyboardMarkup(mainService.SortingPrint(data.InternalMenu(main_menu_name), 2, 1));

                await Bot.SendTextMessageAsync(chatId: userId,
                    text: $"<b>Ichki menyuga orqaga qayttik!</b>",
                    parseMode: ParseMode.Html,
                    replyMarkup: internalMenu);
            }

            // 💬 Savol va takliflar yozib qoldirish -- >
            if (data.GetUserStatus(userId) == "send" && text != "💬 Savol va takliflar yozib qoldirish")
            {
                await Bot.CopyMessageAsync(
                        chatId: -1001870888887,
                        fromChatId: userId,
                        messageId: update.Message.MessageId
                    );
                
                await Bot.SendTextMessageAsync(
                        chatId: -1001870888887,
                        text: $"🆕 Yangi Xabar ⬆️\n\n" +
                        $"👤 Ismi: {fullName}\n" +
                        $"🆔 Idisi: <code>{userId}</code>\n" +
                        $"🌐 Useri: @{username}\n" +
                        $"⏰ Yuborgan vaqti: {DateTime.Now}",
                        parseMode: ParseMode.Html
                    );


                await mainService.ChangeUserStatus("sleep", userId);
                await Bot.SendTextMessageAsync(chatId: userId,
                text: $"<b>✅ Xabaringiz adminga yuborildi!\n" +
                $"⚡️ Tez orada shu bot orqali javob olasiz!</b>",
                parseMode: ParseMode.Html,
                replyMarkup: menu);

                int n;
                int.TryParse(data.GetQuestionsPath().Substring(data.GetQuestionsPath().LastIndexOf('/') + 1), out n);
                await mainService.AddQuestionsAsync(userId, $"https://t.me/c/1870888887/{n+2}");
            }
            // 💬 Savol va takliflar yozib qoldirish ;
        }
        // Error Handler
        public static Task ErrorHandler(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
        {
            throw new NotImplementedException();
        }
    }
}
