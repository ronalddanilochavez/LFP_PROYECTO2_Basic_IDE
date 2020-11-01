using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFP_PROYECTO2_Basic_IDE
{
    class CommandList
    {
        public List<CommandList> commandList;
        public List<Token> commandTokens;
        public string commandType;

        public CommandList ()
        {
            this.commandList = new List<CommandList>();
            this.commandTokens = new List<Token>();
            this.commandType = "";
        }
    }
}
