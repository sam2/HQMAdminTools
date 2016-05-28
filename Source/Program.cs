using System;
using System.Collections.Generic;
using HQMEditorDedicated;

namespace HQMAdminTools
{
    class Program
    {
        static CommandListener commandListener = new CommandListener(Chat.MessageCount);

        static PauseManager pauseManager;
        static GameInfoEditor gameInfo;
        static PositionHelper positionHelper;
        static VoteManager voteManager;
        static BanHelper banHelper;

        static Dictionary<string, ICommandProcessor> processor = new Dictionary<string, ICommandProcessor>();

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
                    ICommandProcessor p;
                    if(processor.TryGetValue(newCommand.Cmd, out p))
                    {
                        p.ProcessCommand(newCommand);
                    }
                    Chat.FlushLastCommand();
                }
                if(!MemoryEditor.IsAttached())
                {
                    Console.Write("Lost connection, retrying...");
                    Init(processName);
                    Console.WriteLine("connected.");
                }
                pauseManager.CheckForAutoResume();       
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
            banHelper = new BanHelper();

            processor = new Dictionary<string, ICommandProcessor>();
            processor["set"] = gameInfo;
            processor["pause"] = pauseManager;
            processor["resume"] = pauseManager;
            processor["faceoff"] = pauseManager;
            processor["sp"] = positionHelper;
            processor["vote"] = voteManager;
            processor["kick"] = banHelper; 

            Chat.FlushLastCommand();
        }
    }
}
