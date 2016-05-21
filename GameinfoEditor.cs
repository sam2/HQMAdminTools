﻿using System;
using HQMEditorDedicated;

namespace HQMAdminTools
{
    class GameInfoEditor : CommandProcessor
    {
        public void ProcessCommand(Command newCommand)
        {
            if (!newCommand.Sender.IsAdmin || newCommand.Args.Length < 2)
                return;

            switch(newCommand.Args[0])
            {
                case "redscore":
                    int redscore = 0;
                    if (Int32.TryParse(newCommand.Args[1], out redscore))
                    {
                        GameInfo.RedScore = redscore;
                    }
                    break;
                case "bluescore":                    
                    int bluescore = 0;
                    if (Int32.TryParse(newCommand.Args[1], out bluescore))
                    {
                        GameInfo.BlueScore = bluescore;
                    }
                    break;
                case "clock":                    
                    TimeSpan clock;
                    if (TimeSpan.TryParse(newCommand.Args[1], out clock))
                    {
                        GameInfo.GameTime = clock;
                    }
                    break;
                case "period":
                    int period = 0;
                    if (Int32.TryParse(newCommand.Args[1], out period))
                    {
                        GameInfo.Period = period;
                    }
                    break;                    
            }
            
                      
        }
    }
}
