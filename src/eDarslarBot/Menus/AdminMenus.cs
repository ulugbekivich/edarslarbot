using Telegram.Bot.Types.ReplyMarkups;

namespace eDarslarBot.Menus
{
    public static class AdminMenus
    {
        public static ReplyKeyboardMarkup admin_menu = new(new[]
        {
            new KeyboardButton[] { "📊 Statistika" },
            new KeyboardButton[] { "➕ Ma'lumot qo'shish", "➕ Kategoriya qo'shish" },
            new KeyboardButton[] { "🔍 Qidirish" },
            new KeyboardButton[] { "📨 Xabar yuborish", "📩 Javob yuborish" },
            new KeyboardButton[] { "🗑 O'chirish" },
        })
        {
            ResizeKeyboard = true
        };

        public static ReplyKeyboardMarkup back_to_admin_panel = new(new[]
        {
            new KeyboardButton[] { "🔙 Admin panelga qaytish" },
        })
        {
            ResizeKeyboard = true
        };

        public static ReplyKeyboardMarkup send_type_choose = new(new[]
        {
            new KeyboardButton[] { "📑 Copy Message", "↪️ Forward Message" },
        })
        {
            ResizeKeyboard = true
        };
    }
}
