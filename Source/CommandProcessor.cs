using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HQMAdminTools
{
    interface ICommandProcessor
    {
        void ProcessCommand(Command cmd);
    }
}
