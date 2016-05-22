using System;
using System.Collections.Generic;
using HQMEditorDedicated;

namespace HQMAdminTools
{
    class Program
    {
        static CommandListener commandListener = new CommandListener(Chat.MessageCount);
        
        static PauseManager pauseManager = new PauseManager();
        static GameInfoEditor gameInfo = new GameInfoEditor();
        static PositionHelper positionHelper = new PositionHelper();
        static VoteManager voteManager = new VoteManager();

        static Dictionary<string, CommandProcessor> processor = new Dictionary<string, CommandProcessor>();

        static void Main(string[] args)
        {
            Console.WriteLine("AdminTools for Hockey?");
            Console.WriteLine("Contribute -> github.com/sam2/HQMAdminTools\n");
            string processName = args.Length > 0 ? args[0] : "hockeydedicated";
            Console.Write("Attaching to "+processName+"...");
                                 
            Init(processName);
            Console.WriteLine("done.");            

            while(true)
            {
                Command newCommand = commandListener.NewCommand();
                 
                if(newCommand != null)
                {
                    CommandProcessor p;
                    if(processor.TryGetValue(newCommand.Cmd, out p))
                    {
                        p.ProcessCommand(newCommand);
                    }                                                            
                }
                if(!MemoryEditor.IsAttached())
                {
                    Console.Write("Lost connection, retrying...");
                    Init(processName);
                    Console.WriteLine("connected.");
                }
            }
        }        

        static void Init(string processName)
        {
            while (!MemoryEditor.Init(processName)) { }

            Chat.RecordCommandSource();

            commandListener = new CommandListener(Chat.MessageCount);

            pauseManager = new PauseManager();
            gameInfo = new GameInfoEditor();
            positionHelper = new PositionHelper();
            voteManager = new VoteManager();


            processor = new Dictionary<string, CommandProcessor>();
            processor["set"] = gameInfo;
            processor["pause"] = pauseManager;
            processor["resume"] = pauseManager;
            processor["faceoff"] = pauseManager;
            processor["sp"] = positionHelper;
            processor["vote"] = voteManager;
        }
    }
}
