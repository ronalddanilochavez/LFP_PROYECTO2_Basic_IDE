using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFP_PROYECTO2_Basic_IDE
{
    class Parser
    {
        private Token[] arrayOfTokens = new Token[0];

        private Lexer myLexer = new Lexer();

        //**********************
        private void listArrayOfTokens(string tokens)
        {
            // To know the lenth of the array
            int size = 0;
            for (int i = 0; i < tokens.Length; i++)
            {
                if (tokens[i] == '\n')
                {
                    size++;
                }
            }

            arrayOfTokens = new Token[size];
            for (int i = 0; i < arrayOfTokens.Length; i++)
            {
                arrayOfTokens[i] = new Token();
            }

            int position = 0;
            string word = "";
            for (int i = 0; i < size; i++)
            {
                for (int j = position; j < tokens.Length; j++)
                {
                    if (tokens[j] == ',')
                    {
                        arrayOfTokens[i].token = word;
                        word = "";
                    }
                    else if (tokens[j] == '\n')
                    {
                        arrayOfTokens[i].type = word;
                        word = "";
                        position = j + 1;
                        break;
                    }
                    else
                    {
                        word += Convert.ToString(tokens[j]);
                    }
                }
            }
        }

        //**********************
        // The parser main method
        public string parseArrayOfTokens (string lexerString) // Here we use a string of tokens procesed by the lexer
        {
            string log = "";

            log = lexerString;
            log += "\n";
            listArrayOfTokens(lexerString);

            // To test if the count of brakets and its order are right
            if (isBraketRight() == false)
            {
                log += "Los brakets están mal, no están cerrados o su cuenta es incorrecta";
                return log;
            }

            // To verify if the principal structure is right: principal(){}
            if (isPrincipalMethodRight() == false)
            {
                log += "El método principal está mal";
                return log;
            }



            return log;
        }

        //**********************
        // To test if the count of brakets and its order are right
        private bool isBraketRight()
        {
            int counter = 0;
            for (int i = 0; i < arrayOfTokens.Length; i++)
            {
                if (arrayOfTokens[i].token == "{")
                {
                    counter++;
                }
                else if (arrayOfTokens[i].token == "}")
                {
                    counter--;
                }

                if (counter == 0 && i == arrayOfTokens.Length - 1)
                {
                    return true;
                }
            }

            return false;
        }

        //**********************
        // To verify if the principal structure is right: principal(){}
        private bool isPrincipalMethodRight()
        {
            if (arrayOfTokens.Length <= 5)
            {
                if (arrayOfTokens[0].token == "principal" && arrayOfTokens[1].token == "(" && arrayOfTokens[2].token == ")" && arrayOfTokens[3].token == "{" && arrayOfTokens[arrayOfTokens.Length - 1].token == "}")
                {
                    return true;
                }
            }

            return false;
        }
    }
}
