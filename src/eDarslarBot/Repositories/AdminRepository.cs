using eDarslarBot.Constants;
using eDarslarBot.Interfaces.Repositories;
using eDarslarBot.Models;
using Npgsql;

namespace eDarslarBot.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly NpgsqlConnection _connection = new NpgsqlConnection(DbConstants.CONNECTION_STRING);

        public async Task<int> CreateMessageAsync(Message message)
        {
            try
            {
                await _connection.OpenAsync();
                string query = "INSERT INTO messages(send_type, sent, not_sent, message_path, start_time, end_time)" +
                $" VALUES('{message.SendType}', '{message.Sent}', '{message.NotSent}', '{message.MessagePath}', " +
                $"'{message.StartTime}', 'now()'); ";
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

        public async Task<int> DeleteUserAsync(string id)
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"delete from users where user_id = {id}";
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

        public async Task<int> DeleteMenuAsync(string menu_name, string name)
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"delete from {menu_name} where menu_name = '{name}'";
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

        public async Task<int> DeletePostAsync(string path)
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"delete from posts where post_path = '{path}'";
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
