using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HQMEditorDedicated;

namespace HQMAdminTools
{
    public class BanHelper : ICommandProcessor
    {
        public void ProcessCommand(Command newCommand)
        {
            if (!newCommand.Sender.IsAdmin)
                return;

            if(newCommand.Args.Length > 0)
            {
                int playerIndex = -1;
                if(int.TryParse(newCommand.Args[0], out playerIndex))
                {
                    playerIndex -= 1; //1 based instead of 0 based
                    if(playerIndex >= 0 && playerIndex < PlayerManager.PlayersInServer.Length)
                    {
                        PlayerManager.BanIP(PlayerManager.PlayersInServer[playerIndex].IPAddress);
                    }
                }
            }
        }
    }
}
