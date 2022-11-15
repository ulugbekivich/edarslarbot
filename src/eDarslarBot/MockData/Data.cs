using eDarslarBot.Constants;
using eDarslarBot.Models;
using Npgsql;

namespace eDarslarBot.MockData
{
    public class Data
    {
        private readonly NpgsqlConnection _connection = new NpgsqlConnection(DbConstants.CONNECTION_STRING);

        public List<User> users = new List<User>();

        public List<string> main_menus = new List<string>();
        public List<string> internal_menus = new List<string>();
        public List<string> internal_menus_all = new List<string>();
        public List<string> last_menus = new List<string>();
        public List<string> last_menus_all = new List<string>();
        public List<int> posts = new List<int>();
        public List<int> posts_all = new List<int>();
        public string admin_status = "";
        public string user_status = "";

        public long UsersCount()
        {
            long UsersCount = 0;

            _connection.Open();

            NpgsqlCommand npgsqlCommand = new NpgsqlCommand("select count(*) from users;", _connection);

            var reader = npgsqlCommand.ExecuteReader();

            while (reader.Read())
            {
                UsersCount = reader.GetInt64(0);
            }

            _connection.Close();

            return UsersCount;
        }

        public List<string> Menu()
        {
            main_menus = new List<string>();
            _connection.Open();

            NpgsqlCommand npgsqlCommand = new NpgsqlCommand("select * from main_menus;", _connection);

            var reader = npgsqlCommand.ExecuteReader();

            while (reader.Read())
            {
                main_menus.Add(reader.GetString(1));
            }

            main_menus.Add("💬 Savol va takliflar yozib qoldirish");

            _connection.Close();

            return main_menus;
        }

        public List<string> InternalMenu(string main_menu_name)
        {
            internal_menus = new List<string>();
            _connection.Open();

            NpgsqlCommand npgsqlCommand = new NpgsqlCommand("select * from internal_menus " +
                $"WHERE main_menu_name = '{main_menu_name}';", _connection);

            var reader = npgsqlCommand.ExecuteReader();

            while (reader.Read())
            {
                internal_menus.Add(reader.GetString(1));
            }

            _connection.Close();

            return internal_menus;
        }

        public List<string> InternalMenuAll()
        {
            internal_menus_all = new List<string>();
            _connection.Open();

            NpgsqlCommand npgsqlCommand = new NpgsqlCommand("select * from internal_menus;", _connection);

            var reader = npgsqlCommand.ExecuteReader();

            while (reader.Read())
            {
                internal_menus_all.Add(reader.GetString(1));
            }

            _connection.Close();

            return internal_menus_all;
        }

        public List<string> LastMenu(string internal_menu_name)
        {
            last_menus = new List<string>();
            _connection.Open();

            NpgsqlCommand npgsqlCommand = new NpgsqlCommand("select * from last_menus " +
                $"where internal_menu_name = '{internal_menu_name}';", _connection);

            var reader = npgsqlCommand.ExecuteReader();

            while (reader.Read())
            {
                last_menus.Add(reader.GetString(1));
            }

            _connection.Close();

            return last_menus;
        }

        public List<string> LastMenuAll()
        {
            last_menus_all = new List<string>();
            _connection.Open();

            NpgsqlCommand npgsqlCommand = new NpgsqlCommand("select * from last_menus;", _connection);

            var reader = npgsqlCommand.ExecuteReader();

            while (reader.Read())
            {
                last_menus_all.Add(reader.GetString(1));
            }

            _connection.Close();

            return last_menus_all;
        }

        public List<int> PostsAll()
        {
            posts = new List<int>();
            _connection.Open();

            NpgsqlCommand npgsqlCommand = new NpgsqlCommand("select * from posts;", _connection);

            var reader = npgsqlCommand.ExecuteReader();

            while (reader.Read())
            {
                posts_all.Add(reader.GetInt32(1));
            }

            _connection.Close();

            return posts_all;
        }

        public List<int> PostsPrint(string last_menu_name)
        {
            posts = new List<int>();
            _connection.Open();

            NpgsqlCommand npgsqlCommand = new NpgsqlCommand("select * from posts " +
                $"WHERE last_menu_name = '{last_menu_name}';", _connection);

            var reader = npgsqlCommand.ExecuteReader();

            while (reader.Read())
            {
                posts.Add(reader.GetInt32(1));
            }

            _connection.Close();

            return posts;
        }

        public string GetAdminStatus(string admin_id)
        {
            _connection.Open();

            NpgsqlCommand npgsqlCommand = new NpgsqlCommand("select * from admins " +
                $"WHERE admin_id = '{admin_id}';", _connection);

            var reader = npgsqlCommand.ExecuteReader();

            while (reader.Read())
            {
                admin_status = reader.GetString(2);
            }

            _connection.Close();

            return admin_status;
        }

        public string GetUserStatus(string user_id)
        {
            _connection.Open();

            NpgsqlCommand npgsqlCommand = new NpgsqlCommand("select * from users " +
                $"WHERE user_id = '{user_id}';", _connection);

            var reader = npgsqlCommand.ExecuteReader();

            while (reader.Read())
            {
                user_status = reader.GetString(5);
            }

            _connection.Close();

            return user_status;
        }

        public string GetQuestionsPath()
        {
            _connection.Open();

            NpgsqlCommand npgsqlCommand = new NpgsqlCommand("select * from questions_suggestions", _connection);

            var reader = npgsqlCommand.ExecuteReader();

            while (reader.Read())
            {
                user_status = reader.GetString(2);
            }

            _connection.Close();

            return user_status;
        }

        public List<User> UserSearch(string searchText)
        {
            _connection.Open();

            string query = $"SELECT user_id, full_name, username, created_at FROM users " +
                $"WHERE user_id like '%{searchText}%' union " +
                $"SELECT user_id, full_name, username, created_at FROM users " +
                $"WHERE full_name like '%{searchText}%' union " +
                $"SELECT user_id, full_name, username, created_at FROM users " +
                $"WHERE username like '%{searchText}%'";
            NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, _connection);

            var reader = npgsqlCommand.ExecuteReader();

            while (reader.Read())
            {
                var user = new User()
                {
                    UserId = reader.GetString(0),
                    FullName = reader.GetString(1),
                    Username = reader.GetString(2),
                    CreatedAt = reader.GetDateTime(3),
                };

                users.Add(user);
            }

            _connection.Close();

            return users;
        }
    }
}
