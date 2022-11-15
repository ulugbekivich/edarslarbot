using eDarslarBot.Constants;
using eDarslarBot.Interfaces.Repositories;
using eDarslarBot.Models;
using Npgsql;

namespace eDarslarBot.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly NpgsqlConnection _connection = new NpgsqlConnection(DbConstants.CONNECTION_STRING);

        public async Task<int> CreateAsync(User user)
        {
            try
            {
                await _connection.OpenAsync();
                string query = "INSERT INTO users(user_id, full_name, username, created_at) " +
                    "VALUES(@userid, @fullname, @user, now());";
                NpgsqlCommand command = new NpgsqlCommand(query, _connection)
                {
                    Parameters =
                    {
                        new("userid", user.UserId),
                        new("fullname", user.FullName),
                        new("user", user.Username)
                    }
                };
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
