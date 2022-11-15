using eDarslarBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eDarslarBot.Interfaces.Repositories
{
    public interface IAdminRepository
    {
        public Task<int> CreateMessageAsync(Message message);

        public Task<int> DeleteUserAsync(string id);
        public Task<int> DeleteMenuAsync(string menu_name, string name);
        public Task<int> DeletePostAsync(string path);
    }
}
