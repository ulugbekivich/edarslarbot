using eDarslarBot.Constants;
using eDarslarBot.Interfaces.Repositories;
using eDarslarBot.Menus;
using eDarslarBot.MockData;
using eDarslarBot.Repositories;
using eDarslarBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace eDarslarBot.Pages
{
    public static class ForAdmin
    {
        private readonly static TelegramBotClient Bot = new TelegramBotClient
            (BotConstants.BOT_TOKEN);
        public static string name = "";
        public static string send_type = "";
        public static string main_menu_name = "";
        public static string internal_menu_name = "";
        public static string last_menu_name = "";
        public static string message_path = "";
        public static string user_id = "";
        public static int page = 0;

        public static async Task UpdateHandler(ITelegramBotClient bot, Update update, CancellationToken arg3)
        {

            // Update Handler

            if (update.Type == UpdateType.Message)
            {
                // commonly used variables
                IUserRepository repository = new UserRepository();
                MainService mainService = new MainService();
                Data data = new Data();

                var text = update.Message.Text;
                var userId = update.Message.Chat.Id.ToString();
                string? username = update.Message.Chat.Username;
                string? fullName = update.Message.Chat.FirstName + update.Message.Chat.LastName;

                if (BotConstants.ADMIN_ID != userId)
                {
                    await ForUser.UpdateHandler(bot, update, arg3);
                }
                else
                {
                    ReplyKeyboardMarkup menu =
                    new ReplyKeyboardMarkup(mainService.SortingPrint(data.Menu(), 2, 0));

                    if (text == "/user")
                    {
                        await ForUser.UpdateHandler(bot, update, arg3);
                    }

                    // Response for /panel command
                    if (text == "/panel" || text == "/Panel")
                    {
                        await Bot.SendTextMessageAsync(chatId: userId,
                        text: $"<b>Salom {fullName}!\nAdmin panlega hush kelibsiz</b>",
                        parseMode: ParseMode.Html,
                        replyMarkup: AdminMenus.admin_menu);
                        await mainService.ChangeAdminStatus("sleep", BotConstants.ADMIN_ID);

                        await bot.ReceiveAsync(updateHandler, ErrorHandler, Program.receiverOptions);
                    }

                }
            }
        }

        private static async Task updateHandler(ITelegramBotClient bot, Update update, CancellationToken arg3)
        {
            // commonly used variables
            IAdminRepository adminRepository = new AdminRepository();
            IUserRepository repository = new UserRepository();
            MainService mainService = new MainService();
            Data data = new Data();

            var text = update.Message.Text;
            var userId = update.Message.Chat.Id.ToString();
            string? username = update.Message.Chat.Username;
            string? fullName = update.Message.Chat.FirstName + " " + update.Message.Chat.LastName;

            if (BotConstants.ADMIN_ID != userId)
            {
                await ForUser.UpdateHandler(bot, update, arg3);
            }
            else
            {
                ReplyKeyboardMarkup menu =
                    new ReplyKeyboardMarkup(mainService.SortingPrint(data.Menu(), 2, 0));

                if (text == "/user" || text == "/start" || text == "/Start")
                {
                    await ForUser.UpdateHandler(bot, update, arg3);
                }

                if (text == "🔙 Admin panelga qaytish")
                {
                    await Bot.SendTextMessageAsync(chatId: userId,
                    text: $"<b>Salom {fullName}!\nAdmin panlega hush kelibsiz</b>",
                    parseMode: ParseMode.Html,
                    replyMarkup: AdminMenus.admin_menu);
                    await mainService.ChangeAdminStatus("sleep", BotConstants.ADMIN_ID);
                }

                // Statistika -->
                if (text == "📊 Statistika")
                {
                    await Bot.SendTextMessageAsync(chatId: userId,
                        text: $"⏰ Hozir : {DateTime.Now}\n\n" +
                        $"<b>Bot statistikasi:\n\n" +
                        $"👥 Bot obunachilari - {data.UsersCount()} ta\n" +
                        $"🔢 Asosiy menyular soni - {data.Menu().Count()} ta\n" +
                        $"🔢 Ichki menyular soni - {data.InternalMenuAll().Count()} ta\n" +
                        $"🔢 Ohirgi menyular soni - {data.LastMenuAll().Count()} ta\n" +
                        $"📹 Darsliklari soni - {data.PostsAll().Count()} ta</b>",
                        parseMode: ParseMode.Html,
                        replyMarkup: AdminMenus.admin_menu);
                }
                // Statistika ;
                // Qidirish -->
                else if (text == "🔍 Qidirish")
                {
                    await Bot.SendTextMessageAsync(chatId: userId,
                        text: $"<b>Qidirish uchun foydalanuvchining Telegram idsini, Usernameni yoki Ismini yozib yuboring: </b>",
                        parseMode: ParseMode.Html,
                        replyMarkup: AdminMenus.back_to_admin_panel);
                    await mainService.ChangeAdminStatus("search", BotConstants.ADMIN_ID);
                }
                // Qidirish ;
                // Xabar Yuborish -->
                else if (text == "📨 Xabar yuborish")
                {
                    await Bot.SendTextMessageAsync(chatId: userId,
                        text: $"<b>Qanday usulda xabar yubormoqchisiz?</b>",
                        parseMode: ParseMode.Html,
                        replyMarkup: AdminMenus.send_type_choose);
                    await mainService.ChangeAdminStatus("send", BotConstants.ADMIN_ID);
                }
                // Xabar Yuborish ;
                // Kategoriya qo'shish -->
                else if (text == "➕ Kategoriya qo'shish")
                {
                    await Bot.SendTextMessageAsync(chatId: userId,
                        text: $"Kategoriya qo'shilish uchun buyruqlar:\n" +
                        $"/add name - asosiy menyuga kategoriya qoshish\n" +
                        $"/addin name - ichki menyuga kategoriya qoshish\n" +
                        $"/addlast name - ohirgi menyuga kategoriya qoshish\n" +
                        $"Masalan:\n/add maktab darsliklari",
                        parseMode: ParseMode.Html,
                        replyMarkup: AdminMenus.back_to_admin_panel);

                    await mainService.ChangeAdminStatus("add", BotConstants.ADMIN_ID);
                }
                // Kategoriya qo'shish ;
                // Javob yuborish -- >
                else if (text == "📩 Javob yuborish")
                {
                    await Bot.SendTextMessageAsync(chatId: userId,
                        text: $"Userning xabariga javob yuborish uchun\n" +
                        $"/sms user_id - buyrug'idan foydalaning",
                        parseMode: ParseMode.Html,
                        replyMarkup: AdminMenus.back_to_admin_panel);

                    await mainService.ChangeAdminStatus("senduser", BotConstants.ADMIN_ID);
                }
                // Javob yuborish ;
                else if (text == "🗑 O'chirish")
                {
                    await Bot.SendTextMessageAsync(chatId: userId,
                        text: $"Databasedan o'chirish uchun buyruqlar:\n" +
                        $"/del name - asosiy menyudan kategoriya o'chirish\n" +
                        $"/delin name - ichki menyudan kategoriya o'chirish\n" +
                        $"/dellast name - ohirgi menyudan o'chirish\n" +
                        $"/delpost post linki - ma'lumot o'chirish\n" +
                        $"/deluser user_id - userni o'chirish\n" +
                        $"Masalan:\n/del 👨🏻‍💻 Dasturlash",
                        parseMode: ParseMode.Html,
                        replyMarkup: AdminMenus.back_to_admin_panel);
                    await mainService.ChangeAdminStatus("del", BotConstants.ADMIN_ID);
                }
                // Ma'lumot qo'shish -->
                else if (text == "➕ Ma'lumot qo'shish")
                {
                    await Bot.SendTextMessageAsync(chatId: userId,
                        text: $"Ma'lumot qo'shilishi kerak bo'lgan asosiy menyuni tanlang:",
                        parseMode: ParseMode.Html,
                        replyMarkup: menu);
                    await mainService.ChangeAdminStatus("addData", BotConstants.ADMIN_ID);
                }

                foreach (string str in data.Menu())
                {
                    if (text == str && data.GetAdminStatus(BotConstants.ADMIN_ID) == "addData")
                    {
                        internal_menu_name = str;
                        ReplyKeyboardMarkup internalMenu =
                            new ReplyKeyboardMarkup(mainService.SortingPrint(data.InternalMenu(str), 2, 3));

                        await Bot.SendTextMessageAsync(chatId: userId,
                        text: $"Ma'lumot qo'shilishi kerak bo'lgan ichki menyuni tanlang:",
                        parseMode: ParseMode.Html,
                        replyMarkup: internalMenu);
                        break;
                    }
                }

                foreach (string item in data.InternalMenu(internal_menu_name))
                {
                    if (text == item && data.GetAdminStatus(BotConstants.ADMIN_ID) == "addData")
                    {
                        ReplyKeyboardMarkup lastMenu =
                            new ReplyKeyboardMarkup(mainService.SortingPrint(data.LastMenu(item), 2, 3));

                        await Bot.SendTextMessageAsync(chatId: userId,
                        text: $"Ma'lumot qo'shilishi kerak bo'lgan ohirgi menyuni tanlang:",
                        parseMode: ParseMode.Html,
                        replyMarkup: lastMenu);
                        break;
                    }
                }

                foreach (string item in data.LastMenuAll())
                {
                    if (text == item)
                    {
                        last_menu_name = item;

                        await Bot.SendTextMessageAsync(chatId: userId,
                                    text: $"Ma'lumotning https://t.me/+kr5UTeqKIpAwYWJi kanaldagi post linkini yuboring",
                                    parseMode: ParseMode.Html);
                    }
                }


                if (text.StartsWith("https://t.me/c/1767357869"))
                {
                    int n;

                    if (int.TryParse(text.Substring(text.LastIndexOf('/') + 1), out n))
                    {

                        if (last_menu_name != "")
                        {

                            var result = await mainService.AddPostAsync(n, last_menu_name);

                            if (result)
                            {
                                await Bot.SendTextMessageAsync(chatId: userId,
                                text: $"Ma'lumot {last_menu_name} menyusiga muvaffaqiyatli qo'shildi!",
                                parseMode: ParseMode.Html,
                                replyMarkup: AdminMenus.admin_menu);
                            }
                            else
                            {
                                await Bot.SendTextMessageAsync(chatId: userId,
                                    text: $"Ma'lumot qo'shilmadi!\nQaytadan urinib ko'ring!",
                                    parseMode: ParseMode.Html);
                            }
                        }
                        else
                        {
                            await Bot.SendTextMessageAsync(chatId: userId,
                                    text: $"Ma'lumot qo'shilmadi!\nSiz avval menyuni tanlashingiz kerak!",
                                    parseMode: ParseMode.Html);
                        }
                    }
                    else
                    {
                        await Bot.SendTextMessageAsync(chatId: userId,
                                    text: $"Ma'lumot qo'shilmadi!\nTog'ri post linkini kirityapkaninggizni tekshiring va qaytadan urinib ko'ring!",
                                    parseMode: ParseMode.Html);
                    }
                }

                // Ma'lumot qo'shish ;

                // Kategoriya qo'shish -->
                if (text.StartsWith("/add ") && data.GetAdminStatus(BotConstants.ADMIN_ID) == "add")
                {
                    name = text.Substring(5);
                    while (name.Contains('\''))
                    {
                        name = name.Replace('\'', '`');
                    }
                    var result = await mainService.AddMainCategoryAsync(name);
                    if (result)
                    {
                        await Bot.SendTextMessageAsync(chatId: userId,
                        text: $"Kategoriya muvaffaqiyatli qoshildi!",
                        parseMode: ParseMode.Html,
                        replyMarkup: AdminMenus.admin_menu);
                    }
                    else
                    {
                        await Bot.SendTextMessageAsync(chatId: userId,
                        text: $"Kategoriya qoshilmadi!\n" +
                        $"xatolikni tog'irlab qaytadan urinib ko'ring!",
                        parseMode: ParseMode.Html,
                        replyMarkup: AdminMenus.admin_menu);
                    }
                    await mainService.ChangeAdminStatus("sleep", BotConstants.ADMIN_ID);
                }
                else if (text.StartsWith("/addin ") && data.GetAdminStatus(BotConstants.ADMIN_ID) == "add")
                {
                    name = text.Substring(7);
                    while (name.Contains('\''))
                    {
                        name = name.Replace('\'', '`');
                    }
                    await Bot.SendTextMessageAsync(chatId: userId,
                            text: $"Kategoriya qo'shilishi kerak bo'lgan asosiy menyuni tanlang:",
                            parseMode: ParseMode.Html,
                            replyMarkup: menu);
                }
                else if (text.StartsWith("/addlast ") && data.GetAdminStatus(BotConstants.ADMIN_ID) == "add")
                {
                    await mainService.ChangeAdminStatus("addl", BotConstants.ADMIN_ID);
                    name = text.Substring(9);
                    while (name.Contains('\''))
                    {
                        name = name.Replace('\'', '`');
                    }

                    await Bot.SendTextMessageAsync(chatId: userId,
                            text: $"Kategoriya qo'shilishi qo'shilishi kerak bo'lgan asosiy menyuni tanlang:",
                            parseMode: ParseMode.Html,
                            replyMarkup: menu);
                }

                foreach (string str in data.Menu())
                {
                    if (text == str && data.GetAdminStatus(BotConstants.ADMIN_ID) == "add")
                    {
                        var result = await mainService.AddInternalCategoryAsync(name, str);
                        if (result)
                        {
                            await Bot.SendTextMessageAsync(chatId: userId,
                            text: $"Kategoriya muvaffaqiyatli qoshildi!",
                            parseMode: ParseMode.Html,
                            replyMarkup: AdminMenus.admin_menu);
                            await mainService.ChangeAdminStatus("sleep", BotConstants.ADMIN_ID);
                        }
                        else
                        {
                            await Bot.SendTextMessageAsync(chatId: userId,
                            text: $"Kategoriya qoshilmadi!\n" +
                            $"xatolikni tog'irlab qaytadan urinib ko'ring!",
                            parseMode: ParseMode.Html,
                            replyMarkup: AdminMenus.admin_menu);
                        }
                        await mainService.ChangeAdminStatus("sleep", BotConstants.ADMIN_ID);
                        break;
                    }
                    else if (text == str && data.GetAdminStatus(BotConstants.ADMIN_ID) == "addl")
                    {
                        main_menu_name = str;

                        ReplyKeyboardMarkup InternalMenu =
                                        new ReplyKeyboardMarkup(mainService.SortingPrint(data.InternalMenu(str), 2, 3));

                        await Bot.SendTextMessageAsync(chatId: userId,
                            text: $"Kategoriya qo'shilishi qo'shilishi kerak bo'lgan ichki menyuni tanlang:",
                            parseMode: ParseMode.Html,
                            replyMarkup: InternalMenu);
                        break;
                    }
                }

                foreach (string str in data.InternalMenu(main_menu_name))
                {
                    if (text == str && data.GetAdminStatus(BotConstants.ADMIN_ID) == "addl")
                    {
                        var result = await mainService.AddLastCategoryAsync(name, str);
                        if (result)
                        {
                            await Bot.SendTextMessageAsync(chatId: userId,
                            text: $"Kategoriya muvaffaqiyatli qoshildi!",
                            parseMode: ParseMode.Html,
                            replyMarkup: AdminMenus.admin_menu);
                            await mainService.ChangeAdminStatus("sleep", BotConstants.ADMIN_ID);
                        }
                        else
                        {
                            await Bot.SendTextMessageAsync(chatId: userId,
                            text: $"Kategoriya qoshilmadi!\n" +
                            $"xatolikni tog'irlab qaytadan urinib ko'ring!",
                            parseMode: ParseMode.Html,
                            replyMarkup: AdminMenus.admin_menu);
                        }
                        await mainService.ChangeAdminStatus("sleep", BotConstants.ADMIN_ID);
                        break;
                    }
                }
                // Kategoriy qo'shish ; 

                // Qidirish -->
                if (data.GetAdminStatus(BotConstants.ADMIN_ID) == "search" && text != "🔍 Qidirish")
                {
                    int n;
                    string matn = "";
                    string first = "";
                    var result = text != "▶️" && text != "◀️" && text != "🔴" ? data.UserSearch(text) : data.UserSearch(first);
                    int pages = (result.Count() + 9) / 10;

                    if (text == "🔴")
                    {
                        await Bot.SendTextMessageAsync(chatId: userId,
                            text: $"Page topilmadi",
                            parseMode: ParseMode.Html);
                    }
                    else if (text == "▶️")
                    {
                        page++;
                        matn = mainService.SearchResult(result, page * 10, 10);
                        if (pages == 1)
                        {
                            n = 0;
                        }
                        else if (page == 0)
                        {
                            n = 2;
                        }
                        else if (page == pages - 1)
                        {
                            n = 3;
                        }
                        else
                        {
                            n = 1;
                        }
                        ReplyKeyboardMarkup search_manu =
                        new ReplyKeyboardMarkup(mainService.SearchButtons(n));

                        await Bot.SendTextMessageAsync(chatId: userId,
                            text: $"{matn}",
                            parseMode: ParseMode.Html,
                            replyMarkup: search_manu);
                    }
                    else if (text == "◀️")
                    {
                        page--;
                        matn = mainService.SearchResult(result, page * 10, 10);
                        if (pages == 1)
                        {
                            n = 0;
                        }
                        else if (page == 0)
                        {
                            n = 2;
                        }
                        else if (page == pages - 1)
                        {
                            n = 3;
                        }
                        else
                        {
                            n = 1;
                        }
                        ReplyKeyboardMarkup search_manu =
                        new ReplyKeyboardMarkup(mainService.SearchButtons(n));

                        await Bot.SendTextMessageAsync(chatId: userId,
                            text: $"{matn}",
                            parseMode: ParseMode.Html,
                            replyMarkup: search_manu);
                    }
                    else if (result.Count() > 0)
                    {
                        first = text;
                        matn = mainService.SearchResult(result, page * 10, 10);
                        if (pages == 1)
                        {
                            n = 0;
                        }
                        else if (page == 0)
                        {
                            n = 2;
                        }
                        else if (page == pages - 1)
                        {
                            n = 3;
                        }
                        else
                        {
                            n = 1;
                        }
                        ReplyKeyboardMarkup search_manu =
                        new ReplyKeyboardMarkup(mainService.SearchButtons(n));

                        await Bot.SendTextMessageAsync(chatId: userId,
                            text: $"{matn}",
                            parseMode: ParseMode.Html,
                            replyMarkup: search_manu);
                    }
                    else
                    {
                        await Bot.SendTextMessageAsync(chatId: userId,
                            text: $"<b>Topilmadi!</b>",
                            parseMode: ParseMode.Html,
                            replyMarkup: AdminMenus.back_to_admin_panel);
                    }
                }
                // Qidirish ;

                // send -->
                if (text == "📑 Copy Message" && data.GetAdminStatus(BotConstants.ADMIN_ID) == "send")
                {
                    send_type = "copy";
                    await Bot.SendTextMessageAsync(chatId: userId,
                                    text: $"Xabarning https://t.me/+67z7K0FsnP00ZTAy kanaldagi post linkini yuboring",
                                    parseMode: ParseMode.Html);
                }
                else if (text == "↪️ Forward Message" && data.GetAdminStatus(BotConstants.ADMIN_ID) == "send")
                {
                    send_type = "forward";
                    await Bot.SendTextMessageAsync(chatId: userId,
                                    text: $"Xabarning https://t.me/+67z7K0FsnP00ZTAy kanaldagi post linkini yuboring",
                                    parseMode: ParseMode.Html);
                }

                if (text.StartsWith("https://t.me/c/1553134770"))
                {
                    int n;

                    if (int.TryParse(text.Substring(text.LastIndexOf('/') + 1), out n))
                    {
                        message_path = text;
                        var result = data.UserSearch("1");
                        if (send_type == "copy")
                        {
                            int sent = 0;
                            int not_sent = 0;
                            await Bot.SendTextMessageAsync(chatId: userId,
                                    text: $"Xabarni Copy yuborish boshlandi...",
                                    parseMode: ParseMode.Html);
                            var StartTime = DateTime.Now;

                            foreach (var item in result)
                            {
                                try
                                {
                                    var ok = await Bot.CopyMessageAsync(
                                        chatId: item.UserId,
                                        fromChatId: -1001553134770,
                                        messageId: n
                                    );
                                    sent++;
                                }
                                catch
                                {
                                    not_sent++;
                                }
                            }
                            await Bot.SendTextMessageAsync(chatId: userId,
                                    text: $"✅ Xabar yuborish tugadi!\n\n" +
                                    $"👥 Jami {sent + not_sent} ta\n" +
                                    $"✅ Yuborilganlar {sent} ta\n" +
                                    $"❌ Yuborilmaganlar {not_sent} ta",
                                    parseMode: ParseMode.Html,
                                    replyMarkup: AdminMenus.back_to_admin_panel);

                            Models.Message message = new Models.Message()
                            {
                                SendType = send_type,
                                Sent = sent,
                                NotSent = not_sent,
                                MessagePath = message_path,
                                StartTime = StartTime,
                                EndTime = DateTime.Now
                            };

                            var res = await adminRepository.CreateMessageAsync(message);

                        }
                        else if (send_type == "forward")
                        {
                            message_path = text;
                            int sent = 0;
                            int not_sent = 0;
                            await Bot.SendTextMessageAsync(chatId: userId,
                                    text: $"Xabarni Forward yuborish boshlandi...",
                                    parseMode: ParseMode.Html);
                            var StartTime = DateTime.Now;

                            foreach (var item in result)
                            {
                                try
                                {
                                    await Bot.ForwardMessageAsync(
                                        chatId: item.UserId,
                                        fromChatId: -1001553134770,
                                        messageId: n
                                    );
                                    sent++;
                                }
                                catch
                                {
                                    not_sent++;
                                }
                            }

                            await Bot.SendTextMessageAsync(chatId: userId,
                                    text: $"✅ Xabar yuborish tugadi!\n\n" +
                                    $"👥 Jami {sent + not_sent} ta\n" +
                                    $"✅ Yuborilganlar {sent} ta\n" +
                                    $"❌ Yuborilmaganlar {not_sent} ta",
                                    parseMode: ParseMode.Html,
                                    replyMarkup: AdminMenus.back_to_admin_panel);
                            Models.Message message = new Models.Message()
                            {
                                SendType = send_type,
                                Sent = sent,
                                NotSent = not_sent,
                                MessagePath = message_path,
                                StartTime = StartTime,
                                EndTime = DateTime.Now
                            };

                            var res = await adminRepository.CreateMessageAsync(message);
                        }

                    }
                    else
                    {
                        await Bot.SendTextMessageAsync(chatId: userId,
                                    text: $"Xabar qabul qilinmadi!\nTog'ri post linkini kirityapkaninggizni tekshiring va qaytadan urinib ko'ring!",
                                    parseMode: ParseMode.Html);
                    }

                }
                // send ;

                // send_user -->
                if(text.StartsWith("/sms ") && data.GetAdminStatus(BotConstants.ADMIN_ID) == "senduser")
                {
                    user_id = text.Substring(5);
                    await Bot.SendTextMessageAsync(chatId: userId,
                        text: $"Userga yubormoqchi bo'lgan xabaringizni kiriting:",
                        parseMode: ParseMode.Html,
                        replyMarkup: AdminMenus.back_to_admin_panel);
                }

                if(data.GetAdminStatus(BotConstants.ADMIN_ID) == "senduser")
                {
                    if(text != "📩 Javob yuborish" && text.StartsWith("/sms") == false)
                    {
                        try
                        {
                            await Bot.SendTextMessageAsync(chatId: user_id,
                            text: $"Sizga admindan javob keldi:\n\n" +
                            $"{text}",
                            parseMode: ParseMode.Html);

                            await Bot.SendTextMessageAsync(chatId: userId,
                            text: $"Xabaringiz {user_id} id egasiga yuborildi ✅",
                            parseMode: ParseMode.Html,
                            replyMarkup: AdminMenus.admin_menu);
                            await mainService.ChangeAdminStatus("sleep", BotConstants.ADMIN_ID);
                        }
                        catch
                        {
                            await Bot.SendTextMessageAsync(chatId: userId,
                            text: $"Xabaringiz {user_id} id egasiga yuborilmadi!\n" +
                            $"Xatolikni tog'irab qaytadan urinib ko'ring!",
                            parseMode: ParseMode.Html,
                            replyMarkup: AdminMenus.admin_menu);
                            await mainService.ChangeAdminStatus("sleep", BotConstants.ADMIN_ID);
                        }

                    }
                }
                // send_user ;

                // O'chirish -- >
                if (text.StartsWith("/del ") && data.GetAdminStatus(BotConstants.ADMIN_ID) == "del")
                {
                    name = text.Substring(5);
                    while (name.Contains('\''))
                    {
                        name = name.Replace('\'', '`');
                    }
                    int result = await adminRepository.DeleteMenuAsync("main_menus", name);
                    if (result == 1)
                    {
                        await Bot.SendTextMessageAsync(chatId: userId,
                        text: $"Kategoriya asosiy menyudan muvaffaqiyatli o'chirildi!",
                        parseMode: ParseMode.Html,
                        replyMarkup: AdminMenus.admin_menu);
                    }
                    else
                    {
                        await Bot.SendTextMessageAsync(chatId: userId,
                        text: $"Kategoriya o'chirilmadi!\n" +
                        $"xatolikni tog'irlab qaytadan urinib ko'ring!",
                        parseMode: ParseMode.Html,
                        replyMarkup: AdminMenus.admin_menu);
                    }
                    await mainService.ChangeAdminStatus("sleep", BotConstants.ADMIN_ID);
                }
                if (text.StartsWith("/delin ") && data.GetAdminStatus(BotConstants.ADMIN_ID) == "del")
                {
                    name = text.Substring(7);
                    while (name.Contains('\''))
                    {
                        name = name.Replace('\'', '`');
                    }
                    int result = await adminRepository.DeleteMenuAsync("internal_menus", name);
                    if (result == 1)
                    {
                        await Bot.SendTextMessageAsync(chatId: userId,
                        text: $"Kategoriya ichki menyudan muvaffaqiyatli o'chirildi!",
                        parseMode: ParseMode.Html,
                        replyMarkup: AdminMenus.admin_menu);
                    }
                    else
                    {
                        await Bot.SendTextMessageAsync(chatId: userId,
                        text: $"Kategoriya o'chirilmadi!\n" +
                        $"xatolikni tog'irlab qaytadan urinib ko'ring!",
                        parseMode: ParseMode.Html,
                        replyMarkup: AdminMenus.admin_menu);
                    }
                    await mainService.ChangeAdminStatus("sleep", BotConstants.ADMIN_ID);
                }
                if (text.StartsWith("/dellast ") && data.GetAdminStatus(BotConstants.ADMIN_ID) == "del")
                {
                    name = text.Substring(9);
                    while (name.Contains('\''))
                    {
                        name = name.Replace('\'', '`');
                    }
                    int result = await adminRepository.DeleteMenuAsync("last_menus", name);
                    if (result == 1)
                    {
                        await Bot.SendTextMessageAsync(chatId: userId,
                        text: $"Kategoriya ohirgi menyudan muvaffaqiyatli o'chirildi!",
                        parseMode: ParseMode.Html,
                        replyMarkup: AdminMenus.admin_menu);
                    }
                    else
                    {
                        await Bot.SendTextMessageAsync(chatId: userId,
                        text: $"Kategoriya o'chirilmadi!\n" +
                        $"xatolikni tog'irlab qaytadan urinib ko'ring!",
                        parseMode: ParseMode.Html,
                        replyMarkup: AdminMenus.admin_menu);
                    }
                    await mainService.ChangeAdminStatus("sleep", BotConstants.ADMIN_ID);
                }
                if (text.StartsWith("/delpost ") && data.GetAdminStatus(BotConstants.ADMIN_ID) == "del")
                {
                    name = text.Substring(text.LastIndexOf("/")+1);
                    int result = await adminRepository.DeletePostAsync(name);
                    if (result == 1)
                    {
                        await Bot.SendTextMessageAsync(chatId: userId,
                        text: $"Post muvaffaqiyatli o'chirildi!",
                        parseMode: ParseMode.Html,
                        replyMarkup: AdminMenus.admin_menu);
                    }
                    else
                    {
                        await Bot.SendTextMessageAsync(chatId: userId,
                        text: $"Post o'chirilmadi!\n" +
                        $"xatolikni tog'irlab qaytadan urinib ko'ring!",
                        parseMode: ParseMode.Html,
                        replyMarkup: AdminMenus.admin_menu);
                    }
                    await mainService.ChangeAdminStatus("sleep", BotConstants.ADMIN_ID);
                }
                if (text.StartsWith("/deluser ") && data.GetAdminStatus(BotConstants.ADMIN_ID) == "del")
                {
                    name = text.Substring(9);
                    int result = await adminRepository.DeleteUserAsync(name);
                    if (result == 1)
                    {
                        await Bot.SendTextMessageAsync(chatId: userId,
                        text: $"User muvaffaqiyatli o'chirildi!",
                        parseMode: ParseMode.Html,
                        replyMarkup: AdminMenus.admin_menu);
                    }
                    else
                    {
                        await Bot.SendTextMessageAsync(chatId: userId,
                        text: $"User o'chirilmadi!\n" +
                        $"xatolikni tog'irlab qaytadan urinib ko'ring!",
                        parseMode: ParseMode.Html,
                        replyMarkup: AdminMenus.admin_menu);
                    }
                    await mainService.ChangeAdminStatus("sleep", BotConstants.ADMIN_ID);
                }
                // O'chirish ;
            }
        }

        public static Task ErrorHandler(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
        {
            throw new NotImplementedException();
        }
    }
}
