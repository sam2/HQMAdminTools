using HQMEditorDedicated;

namespace HQMAdminTools
{
    class PositionHelper : ICommandProcessor
    {
        public void ProcessCommand(Command newCommand)
        {
            if(newCommand.Cmd == "sp" && newCommand.Args.Length > 0)
            {
                switch(newCommand.Args[0].ToLower())
                {
                    case "c":
                        newCommand.Sender.Role = HQMRole.C;
                        Chat.SendMessage(newCommand.Sender.Name + " position C");
                        break;                                      
                    case "lw":                                      
                        newCommand.Sender.Role = HQMRole.LW;        
                        Chat.SendMessage(newCommand.Sender.Name + " position LW");
                        break;                                      
                    case "rw":                                      
                        newCommand.Sender.Role = HQMRole.RW;        
                        Chat.SendMessage(newCommand.Sender.Name + " position RW");
                        break;                                      
                    case "ld":                                      
                        newCommand.Sender.Role = HQMRole.LD;        
                        Chat.SendMessage(newCommand.Sender.Name + " position LD");
                        break;                                      
                    case "rd":                                      
                        newCommand.Sender.Role = HQMRole.RD;        
                        Chat.SendMessage(newCommand.Sender.Name + " position RD");
                        break;                                      
                    case "g":                                       
                        newCommand.Sender.Role = HQMRole.G;         
                        Chat.SendMessage(newCommand.Sender.Name + " position G");
                        break;
                }
            }
        }
    }
}
