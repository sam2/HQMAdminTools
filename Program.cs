using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HQMEditorDedicated;

namespace HQMAdminTools
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Initializing...");
            MemoryEditor.Init();            
            CommandListener commandListener = new CommandListener(Chat.MessageCount);
            PauseManager pauseManager = new PauseManager();
            Console.Write("done.");            

            while(true)
            {
                Command newCommand = commandListener.NewCommand();
                pauseManager.Update(newCommand);  
                if(newCommand != null)
                { 
                    if(newCommand.Sender.IsAdmin)
                    {
                        if(newCommand.Cmd == "set")
                        {
                            if(newCommand.Args.Length > 1)
                            {
                                if (newCommand.Args[0] == "redscore")
                                {
                                    int score = 0;
                                    if (Int32.TryParse(newCommand.Args[1], out score))
                                    {
                                        GameInfo.RedScore = score;
                                    }
                                }
                                if (newCommand.Args[0] == "bluescore")
                                {
                                    int score = 0;
                                    if (Int32.TryParse(newCommand.Args[1], out score))
                                    {
                                        GameInfo.BlueScore = score;
                                    }
                                }
                                if (newCommand.Args[0] == "clock")
                                {
                                    TimeSpan clock;
                                    if (TimeSpan.TryParse(newCommand.Args[1], out clock))
                                    {
                                        GameInfo.GameTime = clock;
                                    }
                                }
                                if (newCommand.Args[0] == "period")
                                {
                                    int period = 0;
                                    if (Int32.TryParse(newCommand.Args[1], out period))
                                    {
                                        GameInfo.Period = period;
                                    }
                                }
                            }
                        }
                    }
                }
            }

        }        
    }
}
