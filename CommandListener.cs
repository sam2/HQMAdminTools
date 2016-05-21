using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HQMEditorDedicated;

namespace HQMAdminTools
{
    public class CommandListener
    {
        int messageCount;

        public CommandListener(int msgCnt)
        {
            messageCount = msgCnt;
        }

        public Command NewCommand()
        {
            if (messageCount == Chat.MessageCount)
                return null;
            messageCount = Chat.MessageCount;
            Chat.ChatMessage lastMessage = Chat.Messages[Chat.MessageCount];
            if (lastMessage.Message.Length > 0 && lastMessage.Message[0] == '.' && lastMessage.Sender != null)
            {
                string[] cmdstring = lastMessage.Message.Substring(1).Split(' ');
                string cmd = cmdstring[0];
                string[] args = cmdstring.Skip(1).ToArray();
                return new Command(lastMessage.Sender, cmd, args);
            }
            return null;
        }
    }

    public class Command
    {
        public Player Sender;
        public string Cmd;
        public string[] Args;

        public Command(Player p, string cmd, string[] args)
        {
            Sender = p;
            Cmd = cmd;
            Args = args;
        }
    }
}
