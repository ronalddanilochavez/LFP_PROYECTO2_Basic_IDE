using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;

namespace LFP_PROYECTO2_Basic_IDE
{
    class IDE
    {

        // 1 character
        private string[] tokensPink1 = { "=", ";" }; // Asignation, Semicolon
        private string[] tokensBlue1 = { "+", "-", "*", "/", ">", "<", "!", "(", ")", "{", "}" }; // Arithmetical Operators
        // 2 characters
        private string[] tokensBlue2 = { "++", "--", ">=", "<=", "==", "!=", "||", "&&" }; // Relational Operators, Logical Operators
        private string[] tokensGreen2 = { "SI" }; // Reserved word
        // 4 characters
        private string[] tokensGreen4 = { "SINO", "SEA:", "PARA" }; // Reserved word
        private string[] tokensCadetBlue4 = { "leer" }; // Function
        // 5 characters
        private string[] tokensGreen5 = { "CASO:", "HACER", "DESDE", "HASTA" }; // Reserved word
        // 6 characters
        private string[] tokensGreen6 = { }; // Reserved word
        private string[] tokensPurple6 = { "entero" }; // Integer Type
        private string[] tokensGray6 = { "cadena" }; // String Type
        // 7 characters
        private string[] tokensGreen7 = { "SINO_SI", "EN_CASO" }; // Reserved word
        private string[] tokensCyan7 = { "decimal" }; // Decimal Type
        // 8 characters
        private string[] tokensGreen8 = { "MIENTRAS" };  // Reserved word
        private string[] tokensOrange8 = { "booleano" }; // Boolean Type
        private string[] tokensBrown8 = { "caracter" }; // Character Type
        private string[] tokensCadetBlue8 = { "escribir" }; // Function
        // 9 characters
        private string[] tokensCadetBlue9 = { "principal" };
        // 10 characters
        private string[] tokensGreen10 = { "OTRO CASO:", "INCREMENTO" }; // Reserved word
        // 12 characters
        private string[] tokensGreen12 = { }; // Reserved word
        // 14 characters
        private string[] tokensGreen14 = { "TERMINAR CICLO" }; // Reserved word
        // 15 characters
        private string[] tokensGreen15 = { "CONTINUAR CICLO" }; // Reserved word

        private string[] allTokens = { "=", ";", "+", "-", "*", "/", ">", "<", "!", "(", ")", "{", "}", "++", "--", ">=", "<=", "==", "!=", "||", "&&", "SI", "SINO", "SEA:", "PARA", "CASO:", "HACER", "DESDE", "HASTA", "FIN_SI", "SINO_SI", "EN_CASO", "FIN_CASO", "FIN_PARA", "MIENTRAS", "OTRO CASO:", "INCREMENTO", "FIN_MIENTRAS", "TERMINAR CICLO", "CONTINUAR CICLO", "entero", "cadena", "decimal", "booleano", "caracter" };


        private bool closedString = true;
        private int stringStart = 0;

        private bool closedLongCommentary = true;
        private bool closedShortCommentary = true;

        public string tokenList = "********Think Outside the BOX********";

        private int stringLength = 0;
        private bool isStringIncreasing = false;
        public int row = 1;
        public int column = 0;
        private int lineFirstIndex = 0;
        private int lineLastIndex = 0;

        //***********************************************************************
        //***********************************************************************

        // !
        public void colorString(int start, int length, Color color, RichTextBox myRichTextBox)
        {
            myRichTextBox.SelectionStart = start;
            myRichTextBox.SelectionLength = length;
            myRichTextBox.SelectionColor = color;
        }

        // !*
        public void checkKeyword(string word, Color color, int startIndex, RichTextBox rtb
            )
        {
            if (rtb.Text.Contains(word))
            {
                int index = -1;
                int selectStart = rtb.SelectionStart;

                while ((index = rtb.Text.IndexOf(word, (index + 1))) != -1)
                {
                    rtb.Select((index + startIndex), word.Length);
                    rtb.SelectionColor = color;
                    rtb.Select(selectStart, 0);
                    rtb.SelectionColor = Color.Black;
                }
            }
        }

        // !*
        public int checkKeyword2(string word, Color color, int startIndex, RichTextBox rtb
            )
        {
            if (rtb.Text.Contains(word))
            {
                rtb.Select(startIndex, word.Length);
                rtb.SelectionColor = color;
                rtb.Select(startIndex + word.Length, 0);
                rtb.SelectionColor = Color.Black;
            }

            return startIndex + word.Length;
        }

        // !*
        public void checkKeyword3(string word, Color color, RichTextBox rtb)
        {
            if (rtb.Text.Contains(word))
            {
                rtb.Select(rtb.Text.IndexOf(word), word.Length);
                rtb.SelectionColor = color;
            }
        }

        public int colorText(string word, int start, Color wordColor, Color finalColor, RichTextBox rtb)
        {
            rtb.Select(start, word.Length);
            rtb.SelectionColor = wordColor;

            rtb.Select(start + word.Length, 0);
            rtb.SelectionColor = finalColor;

            //rtb.SelectionStart = start;
            //rtb.SelectionLength = word.Length;
            //rtb.SelectionColor = color;

            //rtb.Select(start + word.Length, 0);
            //rtb.SelectionColor = Color.Black;

            // We return the position of the next index
            return start + word.Length;
        }

        // !*
        /*
        public int colorTextFile(string word, int start, Color wordColor, Color finalColor, RichTextBox rtb)
        {
            rtb.Select(start, word.Length);
            rtb.SelectionColor = wordColor;

            rtb.Select(start + word.Length, 0);
            rtb.SelectionColor = finalColor;

            //myRichTextBox.SelectionStart = start;
            //myRichTextBox.SelectionLength = word.Length;
            //myRichTextBox.SelectionColor = color;

            //myRichTextBox.Select(start + word.Length, 0);
            //myRichTextBox.SelectionColor = Color.Black;

            // We return the position of the next index
            return start + word.Length;
        }
        */

        //public int processText(string text, RichTextBox rtb, int start)
        public int processText(RichTextBox rtb)
        {
            string word = "";

            // To know if the string is increasing or decreasing
            if (rtb.Text.Length > stringLength)
            {
                isStringIncreasing = true;
                stringLength = rtb.Text.Length;
            }
            else
            {
                isStringIncreasing = false;
                stringLength = rtb.Text.Length;
            }

            // To know the number of rows where we are
            try
            {
                if (rtb.Text[rtb.Text.Length - 1] == '\n' && isStringIncreasing == true)
                {
                    row++;
                }
                if (rtb.Text[rtb.Text.Length - 1] == '\n' && isStringIncreasing == false)
                {
                    row--;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            // To know the number of columns where we are
            try
            {
                lineLastIndex = rtb.Text.Length - 1;
                for (int i = rtb.Text.Length - 1; i > 0; i--)
                {
                    if (rtb.Text[i] == '\n')
                    {
                        lineFirstIndex = i + 1;
                        break;
                    }
                }
                column = lineLastIndex - lineFirstIndex + 1;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            // When open a string
            if (rtb.Text.Length >= 1)
            {
                if (rtb.Text[rtb.Text.Length - 1] == '"')
                {
                    closedString = !closedString;
                }
            }

            string token = "";

            //****************
            // To find the main tokens
            for (int i = (rtb.Text.Length - 1); i >= 0; i--)
            {
                word = Convert.ToString(rtb.Text[i]) + word;

                //////////////////////////////////////////////

                if (closedShortCommentary == true && closedLongCommentary == true)
                {
                    // To color the defined tokens

                    // 1 character length
                    if (word.Length == 1)
                    {
                        for (int k = 0; k < tokensPink1.Length; k++)
                        {
                            if (word == tokensPink1[k])
                            {
                                colorText(word, rtb.Text.Length - tokensPink1[k].Length, Color.Magenta, Color.Black, rtb);
                            }
                        }

                        for (int k = 0; k < tokensBlue1.Length; k++)
                        {
                            if (word == tokensBlue1[k])
                            {
                                colorText(word, rtb.Text.Length - tokensBlue1[k].Length, Color.Blue, Color.Black, rtb);
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
                                colorText(word, rtb.Text.Length - tokensBlue2[k].Length, Color.Blue, Color.Black, rtb);
                            }
                        }

                        for (int k = 0; k < tokensGreen2.Length; k++)
                        {
                            if (word == tokensGreen2[k])
                            {
                                colorText(word, rtb.Text.Length - tokensGreen2[k].Length, Color.Green, Color.Black, rtb);
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
                                colorText(word, rtb.Text.Length - tokensGreen4[k].Length, Color.Green, Color.Black, rtb);
                            }
                        }

                        for (int k = 0; k < tokensCadetBlue4.Length; k++)
                        {
                            if (word == tokensCadetBlue4[k])
                            {
                                colorText(word, rtb.Text.Length - tokensCadetBlue4[k].Length, Color.CadetBlue, Color.Black, rtb);
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
                                colorText(word, rtb.Text.Length - tokensGreen5[k].Length, Color.Green, Color.Black, rtb);
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
                                colorText(word, rtb.Text.Length - tokensGreen6[k].Length, Color.Green, Color.Black, rtb);
                            }
                        }

                        for (int k = 0; k < tokensPurple6.Length; k++)
                        {
                            if (word == tokensPurple6[k])
                            {
                                colorText(word, rtb.Text.Length - tokensPurple6[k].Length, Color.Purple, Color.Black, rtb);
                            }
                        }

                        for (int k = 0; k < tokensGray6.Length; k++)
                        {
                            if (word == tokensGray6[k])
                            {
                                colorText(word, rtb.Text.Length - tokensGray6[k].Length, Color.Gray, Color.Black, rtb);
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
                                colorText(word, rtb.Text.Length - tokensGreen7[k].Length, Color.Green, Color.Black, rtb);
                            }
                        }

                        for (int k = 0; k < tokensCyan7.Length; k++)
                        {
                            if (word == tokensCyan7[k])
                            {
                                colorText(word, rtb.Text.Length - tokensCyan7[k].Length, Color.Cyan, Color.Black, rtb);
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
                                colorText(word, rtb.Text.Length - tokensGreen8[k].Length, Color.Green, Color.Black, rtb);
                            }
                        }

                        for (int k = 0; k < tokensOrange8.Length; k++)
                        {
                            if (word == tokensOrange8[k])
                            {
                                colorText(word, rtb.Text.Length - tokensOrange8[k].Length, Color.Orange, Color.Black, rtb);
                            }
                        }

                        for (int k = 0; k < tokensBrown8.Length; k++)
                        {
                            if (word == tokensBrown8[k])
                            {
                                colorText(word, rtb.Text.Length - tokensBrown8[k].Length, Color.Orange, Color.Black, rtb);
                            }
                        }

                        for (int k = 0; k < tokensCadetBlue8.Length; k++)
                        {
                            if (word == tokensCadetBlue8[k])
                            {
                                colorText(word, rtb.Text.Length - tokensCadetBlue8[k].Length, Color.CadetBlue, Color.Black, rtb);
                            }
                        }
                    }

                    // 9 characters length
                    if (word.Length == 9)
                    {
                        for (int k = 0; k < tokensCadetBlue9.Length; k++)
                        {
                            if (word == tokensCadetBlue9[k])
                            {
                                colorText(word, rtb.Text.Length - tokensCadetBlue9[k].Length, Color.CadetBlue, Color.Black, rtb);
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
                                colorText(word, rtb.Text.Length - tokensGreen10[k].Length, Color.Green, Color.Black, rtb);
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
                                colorText(word, rtb.Text.Length - tokensGreen12[k].Length, Color.Green, Color.Black, rtb);
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
                                colorText(word, rtb.Text.Length - tokensGreen14[k].Length, Color.Green, Color.Black, rtb);
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
                                colorText(word, rtb.Text.Length - tokensGreen15[k].Length, Color.Green, Color.Black, rtb);
                            }
                        }
                    }

                    //////////////////////////////////////////////

                    // Testing if a word is boolean
                    if (isBoolean(word))
                    {
                        colorText(word, rtb.Text.Length - word.Length, Color.Orange, Color.Black, rtb);
                        //tokenList += "\nNuevo Token = " + word;
                        //token = word;
                        break;
                    }

                    // Testing if a word is string
                    if (closedString == false /*&& isString(word)*/)
                    {
                        colorText(word, rtb.Text.Length - word.Length, Color.Gray, Color.Black, rtb);
                        //tokenList += "\nNuevo Token = " + word;
                        //token = word;
                        break;
                    }

                    // Testing if a word is character
                    if (isCharacter(word))
                    {
                        colorText(word, rtb.Text.Length - word.Length, Color.Brown, Color.Black, rtb);
                        //tokenList += "\nNuevo Token = " + word;
                        //token = word;
                    }

                    // Testing if a word is decimal before integer
                    if (isDecimal(word))
                    {
                        colorText(word, rtb.Text.Length - word.Length, Color.Cyan, Color.Black, rtb);
                        //tokenList += "\nNuevo Token = " + word;
                        //token = word;
                    }

                    // Testing if a word is integer after decimal
                    if (isInteger(word))
                    {
                        colorText(word, rtb.Text.Length - word.Length, Color.Purple, Color.Black, rtb);
                        //tokenList += "\nNuevo Token = " + word;
                        //token = word;
                    }

                }

                //////////////////////////////////////////////

                // To paint the short commentary //.........
                if (word == "//")
                {
                    closedShortCommentary = false;
                    colorText(word, rtb.Text.Length - word.Length, Color.Red, Color.Red, rtb);
                }
                if (word.Length >= 2 && closedShortCommentary == false && word[0] == '/' && word[1] == '/' && word[word.Length - 1] == '\n')
                {
                    colorText(word, rtb.Text.Length - word.Length, Color.Red, Color.Black, rtb);
                    closedShortCommentary = true;
                    break;
                }

                // To paint the long commentary between /*....*/
                if (word == "/*")
                {
                    closedLongCommentary = false;
                    colorText(word, rtb.Text.Length - word.Length, Color.Red, Color.Red, rtb);
                    break;
                }
                if (closedLongCommentary == false && word == "*/")
                {

                    closedLongCommentary = true;
                    colorText(word, rtb.Text.Length - word.Length, Color.Red, Color.Black, rtb);
                    break;
                }

                //////////////////////////////////////////////

                // This limits the maximun length of "word"
                // 15 is the size of the longest token
                if (i == rtb.Text.Length - 15)
                {
                    break;
                }

                // This limits the word to the size of the actual line
                /*if (rtb.Text[i] == '\n')
                {
                    return rtb.Text.Length;
                }*/
            }

            return rtb.Text.Length;
        }

        public string processSuggestion(RichTextBox rtb)
        {
            string word = "";
            string suggestionList = "";

            //****************
            // To find the main tokens
            for (int i = (rtb.Text.Length - 1); i >= 0; i--)
            {
                word = Convert.ToString(rtb.Text[i]) + word;

                //suggestionList = "";

                //////////////////////////////////////////////


                if (closedShortCommentary == true && closedLongCommentary == true)
                {

                    for (int l = 0; l < allTokens.Length; l++)
                    {
                        string temp = "";
                        for (int m = 0; m < allTokens[l].Length; m++)
                        {
                            temp += Convert.ToString(allTokens[l][m]);

                            if (word == temp)
                            {
                                // Adds some suggestion to the list
                                suggestionList += allTokens[l] + "\n";
                                break;
                            }
                        }
                    }
                }

                //////////////////////////////////////////////

                // This limits the maximun length of "word"
                // 15 is the size of the longest token
                if (i == rtb.Text.Length - 15)
                {
                    break;
                }

                // This limits the word to the size of the actual line
                /*if (rtb.Text[i] == '\n')
                {
                    return rtb.Text.Length;
                }*/
            }

            return suggestionList;
        }

        // !*
        public int cursorColumnPosition(RichTextBox rtb)
        {
            int numberColumn = 0;
            int numberRow = 1;

            for (int i = rtb.Text.Length - 1; i >= 0; i--)
            {
                if (rtb.Text[i] == '\n')
                {
                    numberRow++;
                }
            }

            try
            {
                if (numberRow >= 1 && rtb.Lines[numberRow - 1].Length > 0)
                {
                    numberColumn = rtb.Lines[numberRow - 1].Length;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            /*
            for (int i = rtb.Text.Length - 1; i >= 0; i--)
            {
                if (rtb.Text[i] == '\n')
                {
                    numberColumn = word.Length;
                    break;
                }

                word = rtb.Text[i] + word;
            }
            */

            return numberColumn;
        }

        // !*
        public int cursorRowPosition(RichTextBox rtb)
        {
            int numberRow = 1;

            for (int i = rtb.Text.Length - 1; i >= 0; i--)
            {
                if (rtb.Text[i] == '\n')
                {
                    numberRow++;
                }
            }

            return numberRow;
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

            //char[] initialCharacter = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'ñ', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'Ñ', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            char[] initialCharacter = { '_' };
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
