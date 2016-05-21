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
        static CommandListener commandListener = new CommandListener(Chat.MessageCount);
        
        static PauseManager pauseManager = new PauseManager();
        static GameInfoEditor gameInfo = new GameInfoEditor();
        static PositionHelper positionHelper = new PositionHelper();

        static Dictionary<string, CommandProcessor> processor = new Dictionary<string, CommandProcessor>();

        static void Main(string[] args)
        {
            Console.WriteLine("AdminTools for Hockey?");
            Console.WriteLine("Created by Omaha");
            Console.WriteLine("Contribute -> github.com/sam2/HQMAdminTools\n");
            Console.Write("Initializing...");                              
            Init();
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
                    Init();
                    Console.WriteLine("connected.");
                }              
            }
        }        

        static void Init()
        {
            while (!MemoryEditor.Init()) { }

            commandListener = new CommandListener(Chat.MessageCount);

            pauseManager = new PauseManager();
            gameInfo = new GameInfoEditor();
            positionHelper = new PositionHelper();

            processor = new Dictionary<string, CommandProcessor>();
            processor["set"] = gameInfo;
            processor["pause"] = pauseManager;
            processor["resume"] = pauseManager;
            processor["faceoff"] = pauseManager;
            processor["sp"] = positionHelper;
        }
    }
}
