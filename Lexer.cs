using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFP_PROYECTO2_Basic_IDE
{
    class Lexer
    {
        // 1 character
        private string[] tokensPink1 = { "=", ";" }; // Asignation, Semicolon
        private string[] tokensBlue1 = { "+", "-", "*", "/", ">", "<", "!", "(", ")" }; // Arithmetical Operators
        // 2 characters
        private string[] tokensBlue2 = { "++", "--", ">=", "<=", "==", "!=", "||", "&&" }; // Relational Operators, Logical Operators
        private string[] tokensGreen2 = { "SI" }; // Reserved word
        // 4 characters
        private string[] tokensGreen4 = { "SINO", "SEA:", "PARA" }; // Reserved word
        // 5 characters
        private string[] tokensGreen5 = { "CASO:", "HACER", "DESDE", "HASTA" }; // Reserved word
        // 6 characters
        private string[] tokensGreen6 = { "FIN_SI" }; // Reserved word
        private string[] tokensPurple6 = { "entero" }; // Integer Type
        private string[] tokensGray6 = { "cadena" }; // String Type
        // 7 characters
        private string[] tokensGreen7 = { "SINO_SI", "EN_CASO" }; // Reserved word
        private string[] tokensCyan7 = { "decimal" }; // Decimal Type
        // 8 characters
        private string[] tokensGreen8 = { "FIN_CASO", "FIN_PARA", "MIENTRAS" };  // Reserved word
        private string[] tokensOrange8 = { "booleano" }; // Boolean Type
        private string[] tokensBrown8 = { "caracter" }; // Character Type
        // 10 characters
        private string[] tokensGreen10 = { "OTRO CASO:", "INCREMENTO" }; // Reserved word
        // 12 characters
        private string[] tokensGreen12 = { "FIN_MIENTRAS" }; // Reserved word
        // 14 characters
        private string[] tokensGreen14 = { "TERMINAR CICLO" }; // Reserved word
        // 15 characters
        private string[] tokensGreen15 = { "REINICIAR CICLO" }; // Reserved word

        public class Token
        {
            public string token; //{ get; set; }
            public string type; //{ get; set; }
        }

        //************************************************************************
        //************************************************************************
        public string lexer(string text)
        {
            string tokenLog = "********Think Outside the BOX********";
            string line = "";
            string word = "";
            string tempText = "";
            string token = "";
            string type = "";
            int start = 0;
            int wordLength = 0;
            bool isLongComment = false;
            bool isShortComment = false;
            bool identifierExpected = false;
            List<string> identifiers = new List<string>();
            bool identifierNotValid = false;

            List<Token> tokenList = new List<Token>();
            Token myToken = new Token();

            // To process the commentaries we cut off the characters enclosed inside it and we only left the '\n' characters
            for (int i = 0; i < text.Length; i++)
            {
                if (i + 1 < text.Length)
                {
                    if (isLongComment == false && text[i] == '/' && text[i + 1] == '/')
                    {
                        isShortComment = true;
                    }

                    if (isShortComment == false && text[i] == '/' && text[i + 1] == '*')
                    {
                        isLongComment = true;
                    }

                    if (isLongComment == true && text[i] == '*' && text[i + 1] == '/')
                    {
                        isLongComment = false;

                        if (i + 2 < text.Length)
                        {
                            i += 2;
                        }
                        else
                        {
                            // Here the text ends if i + 2 == text.length and we dont need the reading of */
                            break;
                        }
                    }
                }

                if (isShortComment == false && isLongComment == false)
                {
                    tempText += Convert.ToString(text[i]);
                    continue;
                }

                if (text[i] == '\n')
                {
                    isShortComment = false;
                    tempText += Convert.ToString(text[i]);
                    continue;
                }
            }

            // To find the tokens
            for (int i = 0; i < tempText.Length; i++)
            {
                // We find a line to work with
                if (tempText[i] != '\n' && i != tempText.Length - 1)
                {
                    line += Convert.ToString(tempText[i]);
                }
                else
                {
                    // If we are in text.Length - 1 index
                    if (i == tempText.Length - 1)
                    {
                        line += Convert.ToString(tempText[i]);
                    }

                    start = 0;
                    identifierExpected = false;

                    while (start < line.Length)
                    {
                        word = "";
                        token = "";

                        for (int j = start; j < line.Length; j++)
                        {
                            if (identifierExpected == false)
                            {
                                word += Convert.ToString(line[j]);

                                // Defined tokens
                                // Needed, because compareToDefinedTokens erases tokenTemp if is not a defined token
                                string[] temp = compareToDefinedTokens(word);
                                if (temp[0].Length > 0)
                                {
                                    token = temp[0];
                                    type = temp[1];
                                    wordLength = token.Length;
                                }
                                temp = null;
                                if (token == "booleano" || token == "cadena" || token == "caracter" || token == "decimal" || token == "entero")
                                {
                                    identifierExpected = true;
                                    break;
                                }

                                // To test if is a boolean type
                                if (isBoolean(word))
                                {
                                    token = word;
                                    type = "boolean_value";
                                    wordLength = token.Length;
                                }
                                // To test if is a string type
                                if (isString(word))
                                {
                                    token = word;
                                    type = "string_value";
                                    wordLength = token.Length;
                                }
                                // To test if is a character type
                                if (isCharacter(word))
                                {
                                    token = word;
                                    type = "character_value";
                                    wordLength = token.Length;
                                }
                                // To test if is a decimal type, we need to test decimal before integer
                                if (isDecimal(word))
                                {
                                    token = word;
                                    type = "decimal_value";
                                    wordLength = token.Length;
                                }
                                // To test if is an integer type
                                if (isInteger(word))
                                {
                                    token = word;
                                    type = "integer_value";
                                    wordLength = token.Length;
                                }

                                // To test if it is an identifier
                                for (int k = 0; k < identifiers.Count; k++)
                                {
                                    if (word.Length == identifiers[k].Length)
                                    {
                                        if (word == identifiers[k])
                                        {
                                            token = word;
                                            type = "identifier";
                                            wordLength = token.Length;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                word = "";
                                for (int k = start; k < line.Length; k++)
                                {
                                    if (line[k] != '=' && line[k] != ';')
                                    {
                                        word += Convert.ToString(line[k]);
                                        wordLength = word.Length;
                                        // Nedeed if the line ends without a '=' or ';'
                                        if (k == line.Length - 1)
                                        {
                                            word = trimWord(word);
                                            if (isIdentifier(word))
                                            {
                                                identifiers.Add(word);
                                                token = word;
                                                type = "identifier";
                                                identifierNotValid = false;
                                                identifierExpected = false;
                                            }
                                            else
                                            {
                                                token = word;
                                                type = "identifier_NOT_Valid";
                                                identifierNotValid = true;
                                                identifierExpected = false;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        wordLength = word.Length;
                                        // We remove the spaces in the begining and in the end
                                        word = trimWord(word);
                                        if (isIdentifier(word))
                                        {
                                            identifiers.Add(word);
                                            token = word;
                                            type = "identifier";
                                            identifierNotValid = false;
                                            identifierExpected = false;
                                        }
                                        else
                                        {
                                            token = word;
                                            type = "identifier_NOT_Valid";
                                            identifierNotValid = true;
                                            identifierExpected = false;
                                        }

                                        break;
                                    }
                                }

                                //identifierExpected = false;
                                break;
                            }
                        }

                        if (token.Length > 0 || identifierNotValid == true)
                        {
                            start += wordLength;
                            if (identifierNotValid == true)
                            {
                                //tokenLog += "\nToken = \"" + token + "\"" + ", " + type;
                                tokenLog += "\n" + token + "," + type;
                                identifierNotValid = false;
                            }
                            else
                            {
                                //tokenLog += "\nToken = \"" + token + "\"" + ", " + type;
                                tokenLog += "\n" + token + "," + type;
                            }
                        }
                        else
                        {
                            start++;
                        }
                    }

                    line = "";
                }
            }

            return tokenLog;
        }

        public List<Token> lexer2(string text)
        {
            string tokenLog = "********Think Outside the BOX********";
            string line = "";
            string word = "";
            string tempText = "";
            string token = "";
            string type = "";
            int start = 0;
            int wordLength = 0;
            bool isLongComment = false;
            bool isShortComment = false;
            bool identifierExpected = false;
            List<string> identifiers = new List<string>();
            bool identifierNotValid = false;

            List<Token> tokenList = new List<Token>();
            Token myToken = new Token();

            // To process the commentaries we cut off the characters enclosed inside it and we only left the '\n' characters
            for (int i = 0; i < text.Length; i++)
            {
                if (i + 1 < text.Length)
                {
                    if (isLongComment == false && text[i] == '/' && text[i + 1] == '/')
                    {
                        isShortComment = true;
                    }

                    if (isShortComment == false && text[i] == '/' && text[i + 1] == '*')
                    {
                        isLongComment = true;
                    }

                    if (isLongComment == true && text[i] == '*' && text[i + 1] == '/')
                    {
                        isLongComment = false;

                        if (i + 2 < text.Length)
                        {
                            i += 2;
                        }
                        else
                        {
                            // Here the text ends if i + 2 == text.length and we dont need the reading of */
                            break;
                        }
                    }
                }

                if (isShortComment == false && isLongComment == false)
                {
                    tempText += Convert.ToString(text[i]);
                    continue;
                }

                if (text[i] == '\n')
                {
                    isShortComment = false;
                    tempText += Convert.ToString(text[i]);
                    continue;
                }
            }

            // To find the tokens
            for (int i = 0; i < tempText.Length; i++)
            {
                // We find a line to work with
                if (tempText[i] != '\n' && i != tempText.Length - 1)
                {
                    line += Convert.ToString(tempText[i]);
                }
                else
                {
                    // If we are in text.Length - 1 index
                    if (i == tempText.Length - 1)
                    {
                        line += Convert.ToString(tempText[i]);
                    }

                    start = 0;
                    identifierExpected = false;

                    while (start < line.Length)
                    {
                        word = "";
                        myToken.token = "";
                        myToken.type = "";

                        for (int j = start; j < line.Length; j++)
                        {
                            if (identifierExpected == false)
                            {
                                word += Convert.ToString(line[j]);

                                // Defined tokens
                                // Needed, because compareToDefinedTokens erases tokenTemp if is not a defined token
                                string[] temp = compareToDefinedTokens(word);
                                if (temp[0].Length > 0)
                                {
                                    myToken.token = temp[0];
                                    myToken.type = temp[1];
                                    wordLength = token.Length;
                                }
                                temp = null;
                                if (token == "booleano" || token == "cadena" || token == "caracter" || token == "decimal" || token == "entero")
                                {
                                    identifierExpected = true;
                                    break;
                                }

                                // To test if is a boolean type
                                if (isBoolean(word))
                                {
                                    myToken.token = word;
                                    myToken.type = "boolean_value";
                                    wordLength = token.Length;
                                }
                                // To test if is a string type
                                if (isString(word))
                                {
                                    myToken.token = word;
                                    myToken.type = "string_value";
                                    wordLength = token.Length;
                                }
                                // To test if is a character type
                                if (isCharacter(word))
                                {
                                    myToken.token = word;
                                    myToken.type = "character_value";
                                    wordLength = token.Length;
                                }
                                // To test if is a decimal type, we need to test decimal before integer
                                if (isDecimal(word))
                                {
                                    myToken.token = word;
                                    myToken.type = "decimal_value";
                                    wordLength = token.Length;
                                }
                                // To test if is an integer type
                                if (isInteger(word))
                                {
                                    myToken.token = word;
                                    myToken.type = "integer_value";
                                    wordLength = token.Length;
                                }

                                // To test if it is an identifier
                                for (int k = 0; k < identifiers.Count; k++)
                                {
                                    if (word.Length == identifiers[k].Length)
                                    {
                                        if (word == identifiers[k])
                                        {
                                            myToken.token = word;
                                            myToken.type = "identifier";
                                            wordLength = token.Length;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                word = "";
                                for (int k = start; k < line.Length; k++)
                                {
                                    if (line[k] != '=' && line[k] != ';')
                                    {
                                        word += Convert.ToString(line[k]);
                                        wordLength = word.Length;
                                        // Nedeed if the line ends without a '=' or ';'
                                        if (k == line.Length - 1)
                                        {
                                            word = trimWord(word);
                                            if (isIdentifier(word))
                                            {
                                                identifiers.Add(word);
                                                myToken.token = word;
                                                myToken.type = "identifier";
                                                identifierNotValid = false;
                                                identifierExpected = false;
                                            }
                                            else
                                            {
                                                myToken.token = word;
                                                myToken.type = "identifier_NOT_Valid";
                                                identifierNotValid = true;
                                                identifierExpected = false;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        wordLength = word.Length;
                                        // We remove the spaces in the begining and in the end
                                        word = trimWord(word);
                                        if (isIdentifier(word))
                                        {
                                            identifiers.Add(word);
                                            myToken.token = word;
                                            myToken.type = "identifier";
                                            identifierNotValid = false;
                                            identifierExpected = false;
                                        }
                                        else
                                        {
                                            myToken.token = word;
                                            myToken.type = "identifier_NOT_Valid";
                                            identifierNotValid = true;
                                            identifierExpected = false;
                                        }

                                        break;
                                    }
                                }

                                //identifierExpected = false;
                                break;
                            }
                        }

                        if (token.Length > 0 || identifierNotValid == true)
                        {
                            start += wordLength;
                            if (identifierNotValid == true)
                            {
                                tokenList.Add(myToken);
                                //tokenLog += "\nToken = \"" + token + "\"" + ", " + type;
                                identifierNotValid = false;
                            }
                            else
                            {
                                tokenList.Add(myToken);
                                //tokenLog += "\nToken = \"" + token + "\"" + ", " + type;
                            }
                        }
                        else
                        {
                            start++;
                        }
                    }

                    line = "";
                }
            }

            return tokenList;
        }

        private string trimWord(string word)
        {
            string temp1 = "";
            string temp2 = "";

            if (word.Length > 0)
            {
                for (int i = 0; i < word.Length; i++)
                {
                    if (word[i] == ' ')
                    {
                        continue;
                    }
                    else
                    {
                        for (int j = i; j < word.Length; j++)
                        {
                            temp1 += Convert.ToString(word[j]);
                        }
                        break;
                    }
                }

                for (int i = temp1.Length - 1; i >= 0; i--)
                {
                    if (temp1[i] == ' ')
                    {
                        continue;
                    }
                    else
                    {
                        for (int j = 0; j <= i; j++)
                        {
                            temp2 += Convert.ToString(temp1[j]);
                        }
                        break;
                    }
                }
            }

            return temp2;
        }

        private string[] compareToDefinedTokens(string word)
        {
            string[] tokenAndType = new string[2];
            string token = "";
            string type = "";

            // 1 character length
            if (word.Length == 1)
            {
                for (int k = 0; k < tokensPink1.Length; k++)
                {
                    if (word == tokensPink1[k])
                    {
                        token = word;
                        if (token == "=")
                        {
                            type = "asignation";
                        }
                        if (token == ";")
                        {
                            type = "semicolon";
                        }
                    }
                }

                for (int k = 0; k < tokensBlue1.Length; k++)
                {
                    if (word == tokensBlue1[k])
                    {
                        token = word;

                        if (token == "+" || token == "-" || token == "*" || token == "/")
                        {
                            type = "aritmetical_operator";
                        }
                        else if (token == "(")
                        {
                            type = "open_parenthesis";
                        }
                        else if (token == ")")
                        {
                            type = "closed_parenthesis";
                        }
                        else if (token == "<" || token == ">")
                        {
                            type = "relational_operator";
                        }
                        else if (token == "!")
                        {
                            type = "logical_operator";
                        }
                    }
                }
            }

            // 2 characters length
            if (word.Length == 2)
            {
                for (int k = 0; k < tokensBlue2.Length; k++)
                {
                    if (word == tokensBlue2[k])
                    {
                        token = word;

                        if (token == "<=" || token == ">=" || token == "==" || token == "!=")
                        {
                            type = "relational_operator";
                        }
                        else if (token == "||" || token == "&&")
                        {
                            type = "logical_operator";
                        }
                        else if (token == "++" || token == "--")
                        {
                            type = "increment_decrement";
                        }
                    }
                }

                for (int k = 0; k < tokensGreen2.Length; k++)
                {
                    if (word == tokensGreen2[k])
                    {
                        token = word;
                        type = "reserved_word";
                    }
                }
            }

            // 4 characters length
            if (word.Length == 4)
            {
                for (int k = 0; k < tokensGreen4.Length; k++)
                {
                    if (word == tokensGreen4[k])
                    {
                        token = word;
                        type = "reserved_word";
                    }
                }
            }

            // 5 characters length
            if (word.Length == 5)
            {
                for (int k = 0; k < tokensGreen5.Length; k++)
                {
                    if (word == tokensGreen5[k])
                    {
                        token = word;
                        type = "reserved_word";
                    }
                }
            }

            // 6 characters length
            if (word.Length == 6)
            {
                for (int k = 0; k < tokensGreen6.Length; k++)
                {
                    if (word == tokensGreen6[k])
                    {
                        token = word;
                        type = "reserved_word";
                    }
                }

                for (int k = 0; k < tokensPurple6.Length; k++)
                {
                    if (word == tokensPurple6[k])
                    {
                        token = word;
                        type = "integer_type";
                    }
                }

                for (int k = 0; k < tokensGray6.Length; k++)
                {
                    if (word == tokensGray6[k])
                    {
                        token = word;
                        type = "string_type";
                    }
                }
            }

            // 7 characters length
            if (word.Length == 7)
            {
                for (int k = 0; k < tokensGreen7.Length; k++)
                {
                    if (word == tokensGreen7[k])
                    {
                        token = word;
                        type = "reserved_word";
                    }
                }

                for (int k = 0; k < tokensCyan7.Length; k++)
                {
                    if (word == tokensCyan7[k])
                    {
                        token = word;
                        type = "decimal_type";
                    }
                }
            }

            // 8 characters length
            if (word.Length == 8)
            {
                for (int k = 0; k < tokensGreen8.Length; k++)
                {
                    if (word == tokensGreen8[k])
                    {
                        token = word;
                        type = "reserved_word";
                    }
                }

                for (int k = 0; k < tokensOrange8.Length; k++)
                {
                    if (word == tokensOrange8[k])
                    {
                        token = word;
                        type = "boolean_type";
                    }
                }

                for (int k = 0; k < tokensBrown8.Length; k++)
                {
                    if (word == tokensBrown8[k])
                    {
                        token = word;
                        type = "character_type";
                    }
                }
            }

            // 10 characters length
            if (word.Length == 10)
            {
                for (int k = 0; k < tokensGreen10.Length; k++)
                {
                    if (word == tokensGreen10[k])
                    {
                        token = word;
                        type = "reserved_word";
                    }
                }
            }

            // 12 characters length
            if (word.Length == 12)
            {
                for (int k = 0; k < tokensGreen12.Length; k++)
                {
                    if (word == tokensGreen12[k])
                    {
                        token = word;
                        type = "reserved_word";
                    }
                }
            }

            // 14 characters length
            if (word.Length == 14)
            {
                for (int k = 0; k < tokensGreen14.Length; k++)
                {
                    if (word == tokensGreen14[k])
                    {
                        token = word;
                        type = "reserved_word";
                    }
                }
            }

            // 15 characters length
            if (word.Length == 15)
            {
                for (int k = 0; k < tokensGreen15.Length; k++)
                {
                    if (word == tokensGreen15[k])
                    {
                        token = word;
                        type = "reserved_word";
                    }
                }
            }

            tokenAndType[0] = token;
            tokenAndType[1] = type;

            return tokenAndType;
        }

        public bool isInteger(string token)
        {
            bool isInteger = false;

            for (int i = 0; i < token.Length; i++)
            {
                // To test if the first character is '-' or a number and the next character is a number
                if (i == 0 && token.Length > 1)
                {
                    if (token[i].Equals('-') || token[i].Equals('0') || token[i].Equals('1') || token[i].Equals('2') || token[i].Equals('3') || token[i].Equals('4') || token[i].Equals('5') || token[i].Equals('6') || token[i].Equals('7') || token[i].Equals('8') || token[i].Equals('9'))
                    {
                        if (token[i + 1].Equals('0') || token[i + 1].Equals('1') || token[i + 1].Equals('2') || token[i + 1].Equals('3') || token[i + 1].Equals('4') || token[i + 1].Equals('5') || token[i + 1].Equals('6') || token[i + 1].Equals('7') || token[i + 1].Equals('8') || token[i + 1].Equals('9'))
                        {
                            isInteger = true;
                            continue;
                        }
                        else
                        {
                            isInteger = false;
                            break;
                        }
                    }
                }

                // To test if the characters are only numbers
                if (token[i].Equals('0') || token[i].Equals('1') || token[i].Equals('2') || token[i].Equals('3') || token[i].Equals('4') || token[i].Equals('5') || token[i].Equals('6') || token[i].Equals('7') || token[i].Equals('8') || token[i].Equals('9'))
                {
                    isInteger = true;
                }
                else
                {
                    isInteger = false;
                    break;
                }
            }

            return isInteger;
        }

        public bool isDecimal(string token)
        {
            bool isDecimal = false;
            /*String integerPart = "";
            String fractionalPart = "";
            bool fractionalTurn = false;*/
            bool isFirstTime = false;

            if (token.Length > 1)
            {
                for (int i = 0; i < token.Length; i++)
                {
                    // To test if the first character is '-' or a number and the next character is a number
                    if (i == 0 && token.Length > 1)
                    {
                        if (token[i].Equals('-') || token[i].Equals('0') || token[i].Equals('1') || token[i].Equals('2') || token[i].Equals('3') || token[i].Equals('4') || token[i].Equals('5') || token[i].Equals('6') || token[i].Equals('7') || token[i].Equals('8') || token[i].Equals('9'))
                        {
                            if (token[i + 1].Equals('.') || token[i + 1].Equals('0') || token[i + 1].Equals('1') || token[i + 1].Equals('2') || token[i + 1].Equals('3') || token[i + 1].Equals('4') || token[i + 1].Equals('5') || token[i + 1].Equals('6') || token[i + 1].Equals('7') || token[i + 1].Equals('8') || token[i + 1].Equals('9'))
                            {
                                isDecimal = true;
                                continue;
                            }
                            else
                            {
                                isDecimal = false;
                                break;
                            }
                        }
                    }

                    // To test if the character meet a Decimal pattern
                    if (token[i].Equals('.') || token[i].Equals('0') || token[i].Equals('1') || token[i].Equals('2') || token[i].Equals('3') || token[i].Equals('4') || token[i].Equals('5') || token[i].Equals('6') || token[i].Equals('7') || token[i].Equals('8') || token[i].Equals('9'))
                    {
                        if (token[i].Equals('.') && i > 0)
                        {
                            //fractionalTurn = true;
                            isFirstTime = !isFirstTime;  // To ensure we have only one point in expression to be decimal
                            if (isFirstTime == false)
                            {
                                isDecimal = false;
                                break;
                            }
                        }
                        else
                        {
                            isDecimal = true;
                        }
                    }
                    else
                    {
                        isDecimal = false;
                        return isDecimal;
                    }
                }
            }

            return isDecimal;
        }

        public bool isBoolean(string token)
        {
            bool isBoolean = false;

            if (token.Length > 1)
            {
                if (token == "verdadero" || token == "falso")
                {
                    isBoolean = true;
                    return isBoolean;
                }
            }

            return isBoolean;
        }

        public bool isString(string token)
        {
            bool isString = false;

            if (token[0] == '"' && token[token.Length - 1] == '"' && (token.Length - 1) != 0)
            {
                for (int i = 1; i < token.Length; i++)
                {
                    if (token[i] == '"')
                    {
                        if (i == token.Length - 1)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }

            return isString;
        }

        public bool isCharacter(string token)
        {
            bool isCharacter = false;

            if (token.Length == 3)
            {
                if (token[0] == '\'' && token[token.Length - 1] == '\'')
                {
                    isCharacter = true;
                    return isCharacter;
                }
            }

            return isCharacter;
        }

        // The acdepted identifier begins with a letter and then it can have letters, numbers or '_'
        public bool isIdentifier(string token)
        {
            bool acceptedCharacter = false;

            char[] initialCharacter = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'ñ', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'Ñ', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            char[] acceptedCharacters = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'ñ', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'Ñ', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '_' };

            if (token.Length > 0)
            {
                // This step assures we have one letter as the begining character of the variable
                for (int i = 0; i < initialCharacter.Length; i++)
                {
                    if (token[0] == initialCharacter[i])
                    {
                        // Next we test every consecutive character is defined as accepted one
                        for (int j = 1; j < token.Length; j++)
                        {
                            acceptedCharacter = false;

                            for (int k = 0; k < acceptedCharacters.Length; k++)
                            {
                                if (token[j] == acceptedCharacters[k])
                                {
                                    acceptedCharacter = true;
                                    break;
                                }
                            }

                            if (acceptedCharacter == true)
                            {
                                if (j == token.Length - 1)
                                {
                                    return true;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            return false;
        }
    }
}
