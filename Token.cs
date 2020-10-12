using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFP_PROYECTO2_Basic_IDE
{
    public class Token
    {
        public string token;
        public string type;
        public string value;
        public string identifierType;

        public Token ()
        {
            token = "";
            type = "";
            value = "";
            identifierType = "";
        }
    }
}
