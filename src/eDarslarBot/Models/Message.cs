using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eDarslarBot.Models
{
    public class Message
    {
        public string SendType { get; set; } = string.Empty;
        public int Sent { get; set; }
        public int NotSent { get; set; }
        public string MessagePath { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
