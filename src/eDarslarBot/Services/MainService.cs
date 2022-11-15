using eDarslarBot.Constants;
using eDarslarBot.Models;
using Npgsql;
using Telegram.Bot.Types.ReplyMarkups;

namespace eDarslarBot.Services
{
    public class MainService
    {
        private readonly NpgsqlConnection _connection = new NpgsqlConnection(DbConstants.CONNECTION_STRING);
        public List<KeyboardButton[]> SortingPrint(List<string> strings, int row, int n)
        {
            List<KeyboardButton[]> keyboardButtons = new List<KeyboardButton[]>();
            List<KeyboardButton> keyboardButtons2 = new List<KeyboardButton>();
            for (int i = 1; i <= strings.Count; i++)
            {
                keyboardButtons2.Add(new KeyboardButton(strings[i - 1]));
                if (i % row == 1) continue;
                else
                {
                    keyboardButtons.Add(keyboardButtons2.ToArray());
                    keyboardButtons2 = new List<KeyboardButton>();
                }
            }
            keyboardButtons.Add(keyboardButtons2.ToArray());
            if (n == 1)
            {
                keyboardButtons.Add(new KeyboardButton[] { "🔙 Orqaga" });
            }
            else if (n == 2)
            {
                keyboardButtons.Add(new KeyboardButton[] { "◀️ Orqaga", "🔝 Asosiy Menu" });
            }
            else if (n == 3)
            {
                keyboardButtons.Add(new KeyboardButton[] { "🔙 Admin panelga qaytish" });
            }

            return keyboardButtons;
        }

        public List<KeyboardButton[]> SearchButtons(int n)
        {
            List<KeyboardButton[]> keyboardButtons = new List<KeyboardButton[]>();
            if (n == 0)
            {
                keyboardButtons.Add(new KeyboardButton[] { "🔙 Admin panelga qaytish" });
            }
            else if (n == 1)
            {
                keyboardButtons.Add(new KeyboardButton[] { "◀️", "▶️" });
                keyboardButtons.Add(new KeyboardButton[] { "🔙 Admin panelga qaytish" });
            }
            else if (n == 2)
            {
                keyboardButtons.Add(new KeyboardButton[] { "🔴", "▶️" });
                keyboardButtons.Add(new KeyboardButton[] { "🔙 Admin panelga qaytish" });
            }
            else if (n == 3)
            {
                keyboardButtons.Add(new KeyboardButton[] { "◀️", "🔴" });
                keyboardButtons.Add(new KeyboardButton[] { "🔙 Admin panelga qaytish" });
            }

            return keyboardButtons;
        }

        public string SearchResult(List<User> user, int page, int pages)
        {
            var userr = user.Skip(page).Take(pages).ToList();
            int count = page == 0 ? 1 : page + 1;
            string matn = "";
            foreach (var item in userr)
            {
                matn = matn + $"<b>{count})\n" +
                        $"🆔 Id - {item.UserId}\n" +
                        $"👤 Foydalanuvchi - {item.FullName}\n" +
                        $"🌐 Username - @{item.Username}\n" +
                        $"🗓 Ro'yhatdan o'tgan sanasi - {item.CreatedAt}</b>\n\n";
                count++;
            }

            return matn;
        }

        public async Task<bool> ChangeAdminStatus(string status, string admin_id)
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"UPDATE admins SET status = '{status}'" +
                $"WHERE admin_id = '{admin_id}';";
                NpgsqlCommand command = new NpgsqlCommand(query, _connection);
                int result = await command.ExecuteNonQueryAsync();
                Console.WriteLine(result);
                if (result == 1)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<bool> ChangeUserStatus(string status, string user_id)
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"UPDATE users SET status = '{status}'" +
                $"WHERE user_id = '{user_id}';";
                NpgsqlCommand command = new NpgsqlCommand(query, _connection);
                int result = await command.ExecuteNonQueryAsync();
                Console.WriteLine(result);
                if (result == 1)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<bool> AddPostAsync(int post_id, string last_menu_name)
        {
            try
            {
                await _connection.OpenAsync();
                string query = "INSERT INTO posts(post_path, last_menu_name)" +
                $"VALUES('{post_id}', '{last_menu_name}');";
                NpgsqlCommand command = new NpgsqlCommand(query, _connection);
                int result = await command.ExecuteNonQueryAsync();
                if (result == 1)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<bool> AddMainCategoryAsync(string category_name)
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"INSERT INTO main_menus(menu_name)" +
                $"VALUES('{category_name}');";
                NpgsqlCommand command = new NpgsqlCommand(query, _connection);
                int result = await command.ExecuteNonQueryAsync();
                if (result == 1)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<bool> AddInternalCategoryAsync(string category_name, string main_menu_name)
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"INSERT INTO internal_menus(menu_name, main_menu_name)" +
                $"VALUES('{category_name}','{main_menu_name}');";
                NpgsqlCommand command = new NpgsqlCommand(query, _connection);
                int result = await command.ExecuteNonQueryAsync();
                if (result == 1)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<bool> AddLastCategoryAsync(string category_name, string internal_menu_name)
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"INSERT INTO last_menus(menu_name, internal_menu_name)" +
                $"VALUES('{category_name}','{internal_menu_name}');";
                NpgsqlCommand command = new NpgsqlCommand(query, _connection);
                int result = await command.ExecuteNonQueryAsync();
                if (result == 1)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<int> AddQuestionsAsync(string user_id, string path)
        {
            try
            {
                await _connection.OpenAsync();
                string query = "INSERT INTO questions_suggestions(user_id, message_path) " +
                $"VALUES('{user_id}', '{path}');";
                NpgsqlCommand command = new NpgsqlCommand(query, _connection);
                int result = await command.ExecuteNonQueryAsync();
                return result;
            }
            catch
            {
                return 0;
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }
    }
}
