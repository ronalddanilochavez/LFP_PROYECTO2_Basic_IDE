using System;
using System.Collections.Generic;
using System.Drawing.Text;
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

            // To create the empty array of tokens
            arrayOfTokens = new Token[size];
            for (int i = 0; i < arrayOfTokens.Length; i++)
            {
                arrayOfTokens[i] = new Token();
            }

            // To populate the array of tokens with data
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

            // To populate the array of tokens with a starting value
            for (int i = 0; i < arrayOfTokens.Length; i++)
            {
                if (arrayOfTokens[i].type == "identifier")
                {
                    if (arrayOfTokens[i - 1].type == "integer_type")
                    {
                        arrayOfTokens[i].value = "0";
                    }
                    if (arrayOfTokens[i - 1].type == "decimal_type")
                    {
                        arrayOfTokens[i].value = "0.0";
                    }
                    if (arrayOfTokens[i - 1].type == "string_type")
                    {
                        arrayOfTokens[i].value = "";
                    }
                    if (arrayOfTokens[i - 1].type == "character_type")
                    {
                        arrayOfTokens[i].value = "''";
                    }
                    if (arrayOfTokens[i - 1].type == "boolean_type")
                    {
                        arrayOfTokens[i].value = "verdadero";
                    }
                }
            }
        }

        //************************************************************************************************************************************
        //************************************************************************************************************************************
        //************************************************************************************************************************************
        // The parser main method
        public string parseArrayOfTokens (string lexerString) // Here we use a string of tokens procesed by the lexer
        {
            string log = "";

            //log = lexerString;
            //log += "\n";
            listArrayOfTokens(lexerString);

            //***************************
            // TROUBLES AREA

            // To test if the count of brakets and its order are right
            if (isBraketRight(arrayOfTokens) == false)
            {
                log += "Los brakets están mal, no están cerrados o su cuenta es incorrecta" + "\n";
            }

            // To test if the count of parenthesis and its order are right
            if (isParenthesisRight(arrayOfTokens) == false)
            {
                log += "Los paréntesis están mal, no están cerrados o su cuenta es incorrecta" + "\n";
            }

            // To verify if the principal structure is right: principal(){}
            if (isPrincipalMethodRight() == false)
            {
                log += "El método principal está mal" + "\n";
            }

            // To find NOT valid identifiers
            for (int i = 0; i < arrayOfTokens.Length; i++)
            {
                if (arrayOfTokens[i].type == "identifier_NOT_Valid")
                {
                    log += arrayOfTokens[i].token + ", " + "identificador NO Válido" + "\n";
                }
            }

            // To verify if is a boolean expression
            if (isBooleanExpression(arrayOfTokens) == false)
            {
                log += "NO es una expresión booleana" + "\n";
            }

            // To verify if is a boolean expression
            if (isBooleanExpression(arrayOfTokens) == true)
            {
                log += "Verdadero" + "\n";
            }
            else
            {
                log += "Falso" + "\n";
            }

            // To quit if some trouble arises
            if (log.Length > 0)
            {
                return log;
            }

            //***************************



            return log;
        }

        //************************************************************************************************************************************
        //************************************************************************************************************************************
        //************************************************************************************************************************************

        //**********************
        // To test if the count of brakets and its order are right
        private bool isBraketRight(Token[] tokens)
        {
            int counter = 0;
            if (tokens.Length > 0)
            {
                for (int i = 0; i < tokens.Length; i++)
                {
                    if (tokens[i].token == "{")
                    {
                        counter++;
                    }
                    else if (tokens[i].token == "}")
                    {
                        counter--;
                    }

                    if (counter == 0 && i == tokens.Length - 1)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        //**********************
        // To test if the count of parenthesis and its order are right
        private bool isParenthesisRight(Token[] tokens)
        {
            int counter = 0;
            if (tokens.Length > 0)
            {
                for (int i = 0; i < tokens.Length; i++)
                {
                    if (tokens[i].token == "(")
                    {
                        counter++;
                    }
                    else if (tokens[i].token == ")")
                    {
                        counter--;
                    }

                    if (counter == 0 && i == tokens.Length - 1)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        //**********************
        // To verify if the principal structure is right: principal(){}
        private bool isPrincipalMethodRight()
        {
            if (arrayOfTokens.Length >= 5)
            {
                if (arrayOfTokens[0].token == "principal" && arrayOfTokens[1].token == "(" && arrayOfTokens[2].token == ")" && arrayOfTokens[3].token == "{" && arrayOfTokens[arrayOfTokens.Length - 1].token == "}")
                {
                    return true;
                }
            }

            return false;
        }

        private string swapBooleanValue(Token token)
        {
            if (token.value == "verdadero")
            {
                return "falso";
            }

            return "verdadero";
        }

        private bool isBooleanExpression(Token[] tokens)
        {
            for (int i = 0; i < tokens.Length; i++)
            {
                if (tokens[i].type != "opened_parenthesis" && tokens[i].type != "closed_parenthesis" && tokens[i].type != "relational_operator" && tokens[i].type != "logical_operator" && tokens[i].type != "identifier" && tokens[i].type != "integer_value" && tokens[i].type != "decimal_value" && tokens[i].type != "string_value" && tokens[i].type != "character_value" && tokens[i].type != "boolean_value")
                {
                    return false;
                }
            }

            if (!isParenthesisRight(tokens))
            {
                return false;
            }

            bool not_expected = true;
            bool open_parenthesis_expected = true;
            bool closed_parenthesis_expected = false;
            bool logical_operator_expected = false;
            bool relational_operator_expected = false;
            bool identifier_expected = false;
            string identifier_value = "";
            bool integer_expected = false;
            bool decimal_expected = false;
            bool string_expected = false;
            bool character_expected = false;
            bool boolean_expected = false;

            
            for (int i = 0; i < tokens.Length; i++)
            {
                if (tokens[i].token == "!" && not_expected == true)
                {
                    not_expected = true;
                    open_parenthesis_expected = true;
                    closed_parenthesis_expected = false;
                    logical_operator_expected = false;
                    relational_operator_expected = false;
                    identifier_expected = true;
                    identifier_value = "boolean_value";
                    integer_expected = false;
                    decimal_expected = false;
                    string_expected = false;
                    character_expected = false;
                    boolean_expected = true;
                }
                else if (tokens[i].token == "(" && open_parenthesis_expected == true)
                {
                    not_expected = true;
                    open_parenthesis_expected = true;
                    closed_parenthesis_expected = false;
                    logical_operator_expected = false;
                    relational_operator_expected = false;
                    identifier_value = "";
                    identifier_expected = true;
                    integer_expected = true;
                    decimal_expected = true;
                    string_expected = true;
                    character_expected = true;
                    boolean_expected = true;
                }
                else if (tokens[i].token == ")" && closed_parenthesis_expected == true)
                {
                    not_expected = false;
                    open_parenthesis_expected = false;
                    closed_parenthesis_expected = true;
                    logical_operator_expected = true;
                    relational_operator_expected = false;
                    identifier_value = "";
                    identifier_expected = false;
                    integer_expected = false;
                    decimal_expected = false;
                    string_expected = false;
                    character_expected = false;
                    boolean_expected = false;
                }
                else if (tokens[i].type == "logical_operator" && logical_operator_expected == true)
                {
                    not_expected = true;
                    open_parenthesis_expected = true;
                    closed_parenthesis_expected = false;
                    logical_operator_expected = true;
                    relational_operator_expected = false;
                    identifier_value = "boolean_value";
                    identifier_expected = true;
                    integer_expected = true;
                    decimal_expected = true;
                    string_expected = true;
                    character_expected = true;
                    boolean_expected = true;
                }
                else if (tokens[i].type == "relational_operator" && relational_operator_expected == true)
                {
                    not_expected = false;
                    open_parenthesis_expected = false;
                    closed_parenthesis_expected = false;
                    logical_operator_expected = true;
                    relational_operator_expected = false;
                    identifier_value = "any";
                    identifier_expected = false;
                    integer_expected = true;
                    decimal_expected = true;
                    string_expected = true;
                    character_expected = true;
                    boolean_expected = true;

                    if (i - 1 >= 0 && i + 1 < tokens.Length)
                    {
                        // Integers compares with integers, Decimal compares with decimal and so on...
                        if (tokens[i - 1].type == "integer_value" || tokens[i - 1].type == "decimal_value" || tokens[i - 1].type == "string_value" || tokens[i - 1].type == "character_value" || tokens[i - 1].type == "boolean_value")
                        {
                            if (tokens[i - 1].type != tokens[i + 1].type)
                            {
                                return false;
                            }
                        }

                        // Identifiers compares with identifiers of the same kind
                        if (tokens[i - 1].type == "identifier")
                        {
                            if (tokens[i - 1].type != tokens[i + 1].type)
                            {
                                return false;
                            }
                            if ((tokens[i].value == "==" || tokens[i].value == "!=") && isBoolean(tokens[i - 1].value) != isBoolean(tokens[i + 1].value))
                            {
                                return false;
                            }
                            if (isInteger(tokens[i - 1].value) != isInteger(tokens[i + 1].value) || isDecimal(tokens[i - 1].value) != isDecimal(tokens[i + 1].value) || isString(tokens[i - 1].value) != isString(tokens[i + 1].value) || isCharacter(tokens[i - 1].value) != isCharacter(tokens[i + 1].value))
                            {
                                return false;
                            }
                        }
                    }
                }
                else if (tokens[i].type == "identifier" && identifier_expected == true)
                {
                    not_expected = false;
                    open_parenthesis_expected = false;
                    closed_parenthesis_expected = true;
                    logical_operator_expected = true;
                    relational_operator_expected = true;
                    identifier_value = "";
                    identifier_expected = false;
                    integer_expected = false;
                    decimal_expected = false;
                    string_expected = false;
                    character_expected = false;
                    boolean_expected = false;

                    // Checks if the identifier is a boolean
                    if (identifier_value == "boolean_value")
                    {
                        if (isBoolean(tokens[i].value))
                        {
                            not_expected = false;
                            open_parenthesis_expected = false;
                            closed_parenthesis_expected = true;
                            logical_operator_expected = true;
                            relational_operator_expected = false;
                            identifier_value = "";
                            identifier_expected = false;
                            integer_expected = false;
                            decimal_expected = false;
                            string_expected = false;
                            character_expected = false;
                            boolean_expected = false;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                else if (tokens[i].type == "integer_value" && integer_expected == true)
                {
                    not_expected = false;
                    open_parenthesis_expected = false;
                    closed_parenthesis_expected = true;
                    logical_operator_expected = true;
                    relational_operator_expected = true;
                    identifier_value = "";
                    identifier_expected = false;
                    integer_expected = false;
                    decimal_expected = false;
                    string_expected = false;
                    character_expected = false;
                    boolean_expected = false;
                }
                else if (tokens[i].type == "decimal_value" && decimal_expected == true)
                {

                    not_expected = false;
                    open_parenthesis_expected = false;
                    closed_parenthesis_expected = true;
                    logical_operator_expected = true;
                    relational_operator_expected = true;
                    identifier_value = "";
                    identifier_expected = false;
                    integer_expected = false;
                    decimal_expected = false;
                    string_expected = false;
                    character_expected = false;
                    boolean_expected = false;
                }
                else if (tokens[i].type == "string_value" && string_expected == true)
                {
                    not_expected = false;
                    open_parenthesis_expected = false;
                    closed_parenthesis_expected = true;
                    logical_operator_expected = true;
                    relational_operator_expected = true;
                    identifier_value = "";
                    identifier_expected = false;
                    integer_expected = false;
                    decimal_expected = false;
                    string_expected = false;
                    character_expected = false;
                    boolean_expected = false;
                }
                else if (tokens[i].type == "character_value" && character_expected == true)
                {
                    not_expected = false;
                    open_parenthesis_expected = false;
                    closed_parenthesis_expected = true;
                    logical_operator_expected = true;
                    relational_operator_expected = true;
                    identifier_value = "";
                    identifier_expected = false;
                    integer_expected = false;
                    decimal_expected = false;
                    string_expected = false;
                    character_expected = false;
                    boolean_expected = false;
                }
                else if (tokens[i].type == "boolean_value" && boolean_expected == true)
                {
                    not_expected = false;
                    open_parenthesis_expected = false;
                    closed_parenthesis_expected = true;
                    logical_operator_expected = true;
                    if (i < tokens.Length - 1)
                    {
                        if (tokens[i + 1].token == "==" || tokens[i + 1].token == "!=")
                        {
                            relational_operator_expected = true;
                        }
                        else
                        {
                            relational_operator_expected = false;
                        }
                    }
                    else
                    {
                        relational_operator_expected = false;
                    }
                    identifier_value = "";
                    identifier_expected = false;
                    integer_expected = false;
                    decimal_expected = false;
                    string_expected = false;
                    character_expected = false;
                    boolean_expected = false;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        private bool evaluateBooleanExpression(Token[] tokens)
        {
            int start = 0;
            int end = 0;
            string tempBooleanString = "falso";

            string evaluateBoolean(Token[] tempTokens)
            {
                bool tempBooleanValue = false;

                bool stringToBoolean(string booleanString)
                {
                    if (booleanString == "verdadero")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                string booleanToString(bool boolean)
                {
                    if (boolean == true)
                    {
                        return "verdadero";
                    }
                    else
                    {
                        return "falso";
                    }
                }

                if (tempTokens.Length > 2)
                {
                    while (tempTokens.Length > 1)
                    {
                        if (tempTokens[0].type == "integer_value" || tempTokens[0].type == "decimal_value" || tempTokens[0].type == "string_value" || tempTokens[0].type == "character_value")
                        {
                            if (tempTokens[0].type == "integer_value")
                            {
                                if (tempTokens[2].type == "integer_value")
                                {
                                    if (tempTokens[1].token == ">")
                                    {
                                        tempBooleanValue = Convert.ToInt32(tempTokens[0].token) > Convert.ToInt32(tempTokens[2].token);
                                    }
                                    else if (tempTokens[1].token == "<")
                                    {
                                        tempBooleanValue = Convert.ToInt32(tempTokens[0].token) < Convert.ToInt32(tempTokens[2].token);
                                    }
                                    else if (tempTokens[1].token == ">=")
                                    {
                                        tempBooleanValue = Convert.ToInt32(tempTokens[0].token) >= Convert.ToInt32(tempTokens[2].token);
                                    }
                                    else if (tempTokens[1].token == "<=")
                                    {
                                        tempBooleanValue = Convert.ToInt32(tempTokens[0].token) <= Convert.ToInt32(tempTokens[2].token);
                                    }
                                    else if (tempTokens[1].token == "==")
                                    {
                                        tempBooleanValue = Convert.ToInt32(tempTokens[0].token) == Convert.ToInt32(tempTokens[2].token);
                                    }
                                    else if (tempTokens[1].token == "!=")
                                    {
                                        tempBooleanValue = Convert.ToInt32(tempTokens[0].token) != Convert.ToInt32(tempTokens[2].token);
                                    }
                                }
                                else if (tempTokens[2].type == "identifier")
                                {
                                    if (tempTokens[1].token == ">")
                                    {
                                        tempBooleanValue = Convert.ToInt32(tempTokens[0].token) > Convert.ToInt32(tempTokens[2].value);
                                    }
                                    else if (tempTokens[1].token == "<")
                                    {
                                        tempBooleanValue = Convert.ToInt32(tempTokens[0].token) < Convert.ToInt32(tempTokens[2].value);
                                    }
                                    else if (tempTokens[1].token == ">=")
                                    {
                                        tempBooleanValue = Convert.ToInt32(tempTokens[0].token) >= Convert.ToInt32(tempTokens[2].value);
                                    }
                                    else if (tempTokens[1].token == "<=")
                                    {
                                        tempBooleanValue = Convert.ToInt32(tempTokens[0].token) <= Convert.ToInt32(tempTokens[2].value);
                                    }
                                    else if (tempTokens[1].token == "==")
                                    {
                                        tempBooleanValue = Convert.ToInt32(tempTokens[0].token) == Convert.ToInt32(tempTokens[2].value);
                                    }
                                    else if (tempTokens[1].token == "!=")
                                    {
                                        tempBooleanValue = Convert.ToInt32(tempTokens[0].token) != Convert.ToInt32(tempTokens[2].value);
                                    }
                                }

                                if (tempTokens.Length > 3)
                                {
                                    Token[] temp = new Token[tempTokens.Length - 2];
                                    temp[0].token = booleanToString(tempBooleanValue);
                                    temp[0].type = "boolean_value";
                                    temp[0].value = booleanToString(tempBooleanValue);
                                    for (int i = 1; i < tempTokens.Length; i++)
                                    {
                                        temp[i] = tempTokens[i + 2];
                                    }
                                    tempTokens = temp;
                                }
                                else
                                {
                                    Token[] temp = new Token[1];
                                    temp[0].token = booleanToString(tempBooleanValue);
                                    temp[0].type = "boolean_value";
                                    temp[0].value = booleanToString(tempBooleanValue);
                                }
                            }
                            else if (tempTokens[0].type == "decimal_value")
                            {
                                if (tempTokens[2].type == "decimal_value")
                                {
                                    if (tempTokens[1].token == ">")
                                    {
                                        tempBooleanValue = Convert.ToDecimal(tempTokens[0].token) > Convert.ToDecimal(tempTokens[2].token);
                                    }
                                    else if (tempTokens[1].token == "<")
                                    {
                                        tempBooleanValue = Convert.ToDecimal(tempTokens[0].token) < Convert.ToDecimal(tempTokens[2].token);
                                    }
                                    else if (tempTokens[1].token == ">=")
                                    {
                                        tempBooleanValue = Convert.ToDecimal(tempTokens[0].token) >= Convert.ToDecimal(tempTokens[2].token);
                                    }
                                    else if (tempTokens[1].token == "<=")
                                    {
                                        tempBooleanValue = Convert.ToDecimal(tempTokens[0].token) <= Convert.ToDecimal(tempTokens[2].token);
                                    }
                                    else if (tempTokens[1].token == "==")
                                    {
                                        tempBooleanValue = Convert.ToDecimal(tempTokens[0].token) == Convert.ToDecimal(tempTokens[2].token);
                                    }
                                    else if (tempTokens[1].token == "!=")
                                    {
                                        tempBooleanValue = Convert.ToDecimal(tempTokens[0].token) != Convert.ToDecimal(tempTokens[2].token);
                                    }
                                }
                                else if (tempTokens[2].type == "identifier")
                                {
                                    if (tempTokens[1].token == ">")
                                    {
                                        tempBooleanValue = Convert.ToDecimal(tempTokens[0].token) > Convert.ToDecimal(tempTokens[2].value);
                                    }
                                    else if (tempTokens[1].token == "<")
                                    {
                                        tempBooleanValue = Convert.ToDecimal(tempTokens[0].token) < Convert.ToDecimal(tempTokens[2].value);
                                    }
                                    else if (tempTokens[1].token == ">=")
                                    {
                                        tempBooleanValue = Convert.ToDecimal(tempTokens[0].token) >= Convert.ToDecimal(tempTokens[2].value);
                                    }
                                    else if (tempTokens[1].token == "<=")
                                    {
                                        tempBooleanValue = Convert.ToDecimal(tempTokens[0].token) <= Convert.ToDecimal(tempTokens[2].value);
                                    }
                                    else if (tempTokens[1].token == "==")
                                    {
                                        tempBooleanValue = Convert.ToDecimal(tempTokens[0].token) == Convert.ToDecimal(tempTokens[2].value);
                                    }
                                    else if (tempTokens[1].token == "!=")
                                    {
                                        tempBooleanValue = Convert.ToDecimal(tempTokens[0].token) != Convert.ToDecimal(tempTokens[2].value);
                                    }
                                }

                                if (tempTokens.Length > 3)
                                {
                                    Token[] temp = new Token[tempTokens.Length - 2];
                                    temp[0].token = booleanToString(tempBooleanValue);
                                    temp[0].type = "boolean_value";
                                    temp[0].value = booleanToString(tempBooleanValue);
                                    for (int i = 1; i < tempTokens.Length; i++)
                                    {
                                        temp[i] = tempTokens[i + 2];
                                    }
                                    tempTokens = temp;
                                }
                                else
                                {
                                    Token[] temp = new Token[1];
                                    temp[0].token = booleanToString(tempBooleanValue);
                                    temp[0].type = "boolean_value";
                                    temp[0].value = booleanToString(tempBooleanValue);
                                }
                            }
                            else if (tempTokens[0].type == "string_value")
                            {
                                if (tempTokens[2].type == "string_value")
                                {
                                    if (tempTokens[1].token == "==")
                                    {
                                        tempBooleanValue = tempTokens[0].token == tempTokens[2].token;
                                    }
                                    if (tempTokens[1].token == "!=")
                                    {
                                        tempBooleanValue = tempTokens[0].token != tempTokens[2].token;
                                    }
                                }
                                else if (tempTokens[2].type == "identifier")
                                {
                                    if (tempTokens[1].token == "==")
                                    {
                                        tempBooleanValue = tempTokens[0].token == tempTokens[2].value;
                                    }
                                    if (tempTokens[1].token == "!=")
                                    {
                                        tempBooleanValue = tempTokens[0].token != tempTokens[2].value;
                                    }
                                }

                                if (tempTokens.Length > 3)
                                {
                                    Token[] temp = new Token[tempTokens.Length - 2];
                                    temp[0].token = booleanToString(tempBooleanValue);
                                    temp[0].type = "boolean_value";
                                    temp[0].value = booleanToString(tempBooleanValue);
                                    for (int i = 1; i < tempTokens.Length; i++)
                                    {
                                        temp[i] = tempTokens[i + 2];
                                    }
                                    tempTokens = temp;
                                }
                                else
                                {
                                    Token[] temp = new Token[1];
                                    temp[0].token = booleanToString(tempBooleanValue);
                                    temp[0].type = "boolean_value";
                                    temp[0].value = booleanToString(tempBooleanValue);
                                }
                            }
                            else if (tempTokens[0].type == "character_value")
                            {
                                if (tempTokens[2].type == "character_value")
                                {
                                    if (tempTokens[1].token == "==")
                                    {
                                        tempBooleanValue = tempTokens[0].token == tempTokens[2].token;
                                    }
                                    if (tempTokens[1].token == "!=")
                                    {
                                        tempBooleanValue = tempTokens[0].token != tempTokens[2].token;
                                    }
                                }
                                else if (tempTokens[2].type == "identifier")
                                {
                                    if (tempTokens[1].token == "==")
                                    {
                                        tempBooleanValue = tempTokens[0].token == tempTokens[2].value;
                                    }
                                    if (tempTokens[1].token == "!=")
                                    {
                                        tempBooleanValue = tempTokens[0].token != tempTokens[2].value;
                                    }
                                }

                                if (tempTokens.Length > 3)
                                {
                                    Token[] temp = new Token[tempTokens.Length - 2];
                                    temp[0].token = booleanToString(tempBooleanValue);
                                    temp[0].type = "boolean_value";
                                    temp[0].value = booleanToString(tempBooleanValue);
                                    for (int i = 1; i < tempTokens.Length; i++)
                                    {
                                        temp[i] = tempTokens[i + 2];
                                    }
                                    tempTokens = temp;
                                }
                                else
                                {
                                    Token[] temp = new Token[1];
                                    temp[0].token = booleanToString(tempBooleanValue);
                                    temp[0].type = "boolean_value";
                                    temp[0].value = booleanToString(tempBooleanValue);
                                }
                            }
                        }
                        else if (tempTokens[0].type == "boolean_value")
                        {
                            if (tempTokens[2].type == "boolean_value")
                            {
                                if (tempTokens[1].token == "||")
                                {
                                    tempBooleanValue = stringToBoolean(tempTokens[0].token) || stringToBoolean(tempTokens[2].token);
                                }
                                else if (tempTokens[1].token == "&&")
                                {
                                    tempBooleanValue = stringToBoolean(tempTokens[0].token) && stringToBoolean(tempTokens[2].token);
                                }

                                if (tempTokens.Length > 3)
                                {
                                    Token[] temp = new Token[tempTokens.Length - 2];
                                    temp[0].token = booleanToString(tempBooleanValue);
                                    temp[0].type = "boolean_value";
                                    temp[0].value = booleanToString(tempBooleanValue);
                                    for (int i = 1; i < tempTokens.Length; i++)
                                    {
                                        temp[i] = tempTokens[i + 2];
                                    }
                                    tempTokens = temp;
                                }
                                else
                                {
                                    Token[] temp = new Token[1];
                                    temp[0].token = booleanToString(tempBooleanValue);
                                    temp[0].type = "boolean_value";
                                    temp[0].value = booleanToString(tempBooleanValue);
                                }
                            }
                            else if (tempTokens[2].type == "identifier")
                            {
                                if (tempTokens[2].value == "verdadero" || tempTokens[2].value == "falso")
                                {
                                    if (tempTokens[1].token == "||")
                                    {
                                        tempBooleanValue = stringToBoolean(tempTokens[0].token) || stringToBoolean(tempTokens[2].value);
                                    }
                                    else if (tempTokens[1].token == "&&")
                                    {
                                        tempBooleanValue = stringToBoolean(tempTokens[0].token) && stringToBoolean(tempTokens[2].value);
                                    }

                                    if (tempTokens.Length > 3)
                                    {
                                        Token[] temp = new Token[tempTokens.Length - 2];
                                        temp[0].token = booleanToString(tempBooleanValue);
                                        temp[0].type = "boolean_value";
                                        temp[0].value = booleanToString(tempBooleanValue);
                                        for (int i = 1; i < tempTokens.Length; i++)
                                        {
                                            temp[i] = tempTokens[i + 2];
                                        }
                                        tempTokens = temp;
                                    }
                                    else
                                    {
                                        Token[] temp = new Token[1];
                                        temp[0].token = booleanToString(tempBooleanValue);
                                        temp[0].type = "boolean_value";
                                        temp[0].value = booleanToString(tempBooleanValue);
                                    }
                                }
                                else
                                {
                                    tempBooleanValue = stringToBoolean(tempTokens[0].token);

                                    if (tempTokens.Length > 3)
                                    {
                                        Token[] temp = new Token[tempTokens.Length - 2];
                                        temp[0].token = booleanToString(tempBooleanValue);
                                        temp[0].type = "boolean_value";
                                        temp[0].value = booleanToString(tempBooleanValue);
                                        for (int i = 1; i < tempTokens.Length; i++)
                                        {
                                            temp[i] = tempTokens[i + 2];
                                        }
                                        tempTokens = temp;
                                    }
                                    else
                                    {
                                        Token[] temp = new Token[1];
                                        temp[0].token = booleanToString(tempBooleanValue);
                                        temp[0].type = "boolean_value";
                                        temp[0].value = booleanToString(tempBooleanValue);
                                    }
                                }
                            }
                            else if (tempTokens[2].type == "integer_value" || tempTokens[2].type == "decimal_value" || tempTokens[2].type == "string_value" || tempTokens[2].type == "character_value")
                            {
                                if (tempTokens[2].type == "integer_value")
                                {
                                    if (tempTokens[4].type == "integer_value")
                                    {
                                        if (tempTokens[3].token == ">")
                                        {
                                            tempBooleanValue = Convert.ToInt32(tempTokens[2].token) > Convert.ToInt32(tempTokens[4].token);
                                        }
                                        else if (tempTokens[3].token == "<")
                                        {
                                            tempBooleanValue = Convert.ToInt32(tempTokens[2].token) < Convert.ToInt32(tempTokens[4].token);
                                        }
                                        else if (tempTokens[3].token == ">=")
                                        {
                                            tempBooleanValue = Convert.ToInt32(tempTokens[2].token) >= Convert.ToInt32(tempTokens[4].token);
                                        }
                                        else if (tempTokens[3].token == "<=")
                                        {
                                            tempBooleanValue = Convert.ToInt32(tempTokens[2].token) <= Convert.ToInt32(tempTokens[4].token);
                                        }
                                        else if (tempTokens[3].token == "==")
                                        {
                                            tempBooleanValue = Convert.ToInt32(tempTokens[2].token) == Convert.ToInt32(tempTokens[4].token);
                                        }
                                        else if (tempTokens[3].token == "!=")
                                        {
                                            tempBooleanValue = Convert.ToInt32(tempTokens[2].token) != Convert.ToInt32(tempTokens[4].token);
                                        }
                                    }
                                    else if (tempTokens[4].type == "identifier")
                                    {
                                        if (tempTokens[3].token == ">")
                                        {
                                            tempBooleanValue = Convert.ToInt32(tempTokens[2].token) > Convert.ToInt32(tempTokens[4].value);
                                        }
                                        else if (tempTokens[3].token == "<")
                                        {
                                            tempBooleanValue = Convert.ToInt32(tempTokens[2].token) < Convert.ToInt32(tempTokens[4].value);
                                        }
                                        else if (tempTokens[3].token == ">=")
                                        {
                                            tempBooleanValue = Convert.ToInt32(tempTokens[2].token) >= Convert.ToInt32(tempTokens[4].value);
                                        }
                                        else if (tempTokens[3].token == "<=")
                                        {
                                            tempBooleanValue = Convert.ToInt32(tempTokens[2].token) <= Convert.ToInt32(tempTokens[4].value);
                                        }
                                        else if (tempTokens[3].token == "==")
                                        {
                                            tempBooleanValue = Convert.ToInt32(tempTokens[2].token) == Convert.ToInt32(tempTokens[4].value);
                                        }
                                        else if (tempTokens[3].token == "!=")
                                        {
                                            tempBooleanValue = Convert.ToInt32(tempTokens[2].token) != Convert.ToInt32(tempTokens[4].value);
                                        }
                                    }
                                }
                                else if (tempTokens[2].type == "decimal_value")
                                {
                                    if (tempTokens[4].type == "decimal_value")
                                    {
                                        if (tempTokens[3].token == ">")
                                        {
                                            tempBooleanValue = Convert.ToDecimal(tempTokens[2].token) > Convert.ToDecimal(tempTokens[4].token);
                                        }
                                        else if (tempTokens[3].token == "<")
                                        {
                                            tempBooleanValue = Convert.ToDecimal(tempTokens[2].token) < Convert.ToDecimal(tempTokens[4].token);
                                        }
                                        else if (tempTokens[3].token == ">=")
                                        {
                                            tempBooleanValue = Convert.ToDecimal(tempTokens[2].token) >= Convert.ToDecimal(tempTokens[4].token);
                                        }
                                        else if (tempTokens[3].token == "<=")
                                        {
                                            tempBooleanValue = Convert.ToDecimal(tempTokens[2].token) <= Convert.ToDecimal(tempTokens[4].token);
                                        }
                                        else if (tempTokens[3].token == "==")
                                        {
                                            tempBooleanValue = Convert.ToDecimal(tempTokens[2].token) == Convert.ToDecimal(tempTokens[4].token);
                                        }
                                        else if (tempTokens[3].token == "!=")
                                        {
                                            tempBooleanValue = Convert.ToDecimal(tempTokens[2].token) != Convert.ToDecimal(tempTokens[4].token);
                                        }
                                    }
                                    else if (tempTokens[4].type == "identifier")
                                    {
                                        if (tempTokens[3].token == ">")
                                        {
                                            tempBooleanValue = Convert.ToDecimal(tempTokens[2].token) > Convert.ToDecimal(tempTokens[4].value);
                                        }
                                        else if (tempTokens[3].token == "<")
                                        {
                                            tempBooleanValue = Convert.ToDecimal(tempTokens[2].token) < Convert.ToDecimal(tempTokens[4].value);
                                        }
                                        else if (tempTokens[3].token == ">=")
                                        {
                                            tempBooleanValue = Convert.ToDecimal(tempTokens[2].token) >= Convert.ToDecimal(tempTokens[4].value);
                                        }
                                        else if (tempTokens[3].token == "<=")
                                        {
                                            tempBooleanValue = Convert.ToDecimal(tempTokens[2].token) <= Convert.ToDecimal(tempTokens[4].value);
                                        }
                                        else if (tempTokens[3].token == "==")
                                        {
                                            tempBooleanValue = Convert.ToDecimal(tempTokens[2].token) == Convert.ToDecimal(tempTokens[4].value);
                                        }
                                        else if (tempTokens[3].token == "!=")
                                        {
                                            tempBooleanValue = Convert.ToDecimal(tempTokens[2].token) != Convert.ToDecimal(tempTokens[4].value);
                                        }
                                    }
                                }
                                else if (tempTokens[2].type == "string_value")
                                {
                                    if (tempTokens[4].type == "string_value")
                                    {
                                        if (tempTokens[3].token == "==")
                                        {
                                            tempBooleanValue = tempTokens[2].token == tempTokens[4].token;
                                        }
                                        if (tempTokens[3].token == "!=")
                                        {
                                            tempBooleanValue = tempTokens[2].token != tempTokens[4].token;
                                        }
                                    }
                                    else if (tempTokens[4].type == "identifier")
                                    {
                                        if (tempTokens[3].token == "==")
                                        {
                                            tempBooleanValue = tempTokens[2].token == tempTokens[4].value;
                                        }
                                        if (tempTokens[3].token == "!=")
                                        {
                                            tempBooleanValue = tempTokens[2].token != tempTokens[4].value;
                                        }
                                    }
                                }
                                else if (tempTokens[2].type == "character_value")
                                {
                                    if (tempTokens[4].type == "character_value")
                                    {
                                        if (tempTokens[3].token == "==")
                                        {
                                            tempBooleanValue = tempTokens[2].token == tempTokens[4].token;
                                        }
                                        if (tempTokens[3].token == "!=")
                                        {
                                            tempBooleanValue = tempTokens[2].token != tempTokens[4].token;
                                        }
                                    }
                                    else if (tempTokens[4].type == "identifier")
                                    {
                                        if (tempTokens[3].token == "==")
                                        {
                                            tempBooleanValue = tempTokens[2].token == tempTokens[4].value;
                                        }
                                        if (tempTokens[3].token == "!=")
                                        {
                                            tempBooleanValue = tempTokens[2].token != tempTokens[4].value;
                                        }
                                    }
                                }

                                if (tempTokens[1].token == "||")
                                {
                                    tempBooleanValue = stringToBoolean(tempTokens[0].token) || tempBooleanValue;
                                }
                                else if (tempTokens[1].token == "&&")
                                {
                                    tempBooleanValue = stringToBoolean(tempTokens[0].token) && tempBooleanValue;
                                }

                                if (tempTokens.Length > 3)
                                {
                                    Token[] temp = new Token[tempTokens.Length - 4];
                                    temp[0].token = booleanToString(tempBooleanValue);
                                    temp[0].type = "boolean_value";
                                    temp[0].value = booleanToString(tempBooleanValue);
                                    for (int i = 1; i < tempTokens.Length; i++)
                                    {
                                        temp[i] = tempTokens[i + 4];
                                    }
                                    tempTokens = temp;
                                }
                                else
                                {
                                    Token[] temp = new Token[1];
                                    temp[0].token = booleanToString(tempBooleanValue);
                                    temp[0].type = "boolean_value";
                                    temp[0].value = booleanToString(tempBooleanValue);
                                }
                            }
                        }
                        else if (tempTokens[0].type == "identifier")
                        {
                            if (tempTokens[0].value == "integer_value" || tempTokens[0].value == "decimal_value" || tempTokens[0].value == "string_value" || tempTokens[0].value == "character_value" || tempTokens[0].value == "boolean_value")
                            {
                                if (tempTokens[0].value == "integer_value")
                                {
                                    if (tempTokens[2].type == "integer_value")
                                    {
                                        if (tempTokens[1].token == ">")
                                        {
                                            tempBooleanValue = Convert.ToInt32(tempTokens[0].value) > Convert.ToInt32(tempTokens[2].token);
                                        }
                                        else if (tempTokens[1].token == "<")
                                        {
                                            tempBooleanValue = Convert.ToInt32(tempTokens[0].value) < Convert.ToInt32(tempTokens[2].token);
                                        }
                                        else if (tempTokens[1].token == ">=")
                                        {
                                            tempBooleanValue = Convert.ToInt32(tempTokens[0].value) >= Convert.ToInt32(tempTokens[2].token);
                                        }
                                        else if (tempTokens[1].token == "<=")
                                        {
                                            tempBooleanValue = Convert.ToInt32(tempTokens[0].value) <= Convert.ToInt32(tempTokens[2].token);
                                        }
                                        else if (tempTokens[1].token == "==")
                                        {
                                            tempBooleanValue = Convert.ToInt32(tempTokens[0].value) == Convert.ToInt32(tempTokens[2].token);
                                        }
                                        else if (tempTokens[1].token == "!=")
                                        {
                                            tempBooleanValue = Convert.ToInt32(tempTokens[0].value) != Convert.ToInt32(tempTokens[2].token);
                                        }
                                    }
                                    else if (tempTokens[2].type == "identifier")
                                    {
                                        if (tempTokens[1].token == ">")
                                        {
                                            tempBooleanValue = Convert.ToInt32(tempTokens[0].value) > Convert.ToInt32(tempTokens[2].value);
                                        }
                                        else if (tempTokens[1].token == "<")
                                        {
                                            tempBooleanValue = Convert.ToInt32(tempTokens[0].value) < Convert.ToInt32(tempTokens[2].value);
                                        }
                                        else if (tempTokens[1].token == ">=")
                                        {
                                            tempBooleanValue = Convert.ToInt32(tempTokens[0].value) >= Convert.ToInt32(tempTokens[2].value);
                                        }
                                        else if (tempTokens[1].token == "<=")
                                        {
                                            tempBooleanValue = Convert.ToInt32(tempTokens[0].value) <= Convert.ToInt32(tempTokens[2].value);
                                        }
                                        else if (tempTokens[1].token == "==")
                                        {
                                            tempBooleanValue = Convert.ToInt32(tempTokens[0].value) == Convert.ToInt32(tempTokens[2].value);
                                        }
                                        else if (tempTokens[1].token == "!=")
                                        {
                                            tempBooleanValue = Convert.ToInt32(tempTokens[0].value) != Convert.ToInt32(tempTokens[2].value);
                                        }
                                    }

                                    if (tempTokens.Length > 3)
                                    {
                                        Token[] temp = new Token[tempTokens.Length - 2];
                                        temp[0].token = booleanToString(tempBooleanValue);
                                        temp[0].type = "boolean_value";
                                        temp[0].value = booleanToString(tempBooleanValue);
                                        for (int i = 1; i < tempTokens.Length; i++)
                                        {
                                            temp[i] = tempTokens[i + 2];
                                        }
                                        tempTokens = temp;
                                    }
                                    else
                                    {
                                        Token[] temp = new Token[1];
                                        temp[0].token = booleanToString(tempBooleanValue);
                                        temp[0].type = "boolean_value";
                                        temp[0].value = booleanToString(tempBooleanValue);
                                    }
                                }
                                else if (tempTokens[0].value == "decimal_value")
                                {
                                    if (tempTokens[2].type == "decimal_value")
                                    {
                                        if (tempTokens[1].token == ">")
                                        {
                                            tempBooleanValue = Convert.ToDecimal(tempTokens[0].value) > Convert.ToDecimal(tempTokens[2].token);
                                        }
                                        else if (tempTokens[1].token == "<")
                                        {
                                            tempBooleanValue = Convert.ToDecimal(tempTokens[0].value) < Convert.ToDecimal(tempTokens[2].token);
                                        }
                                        else if (tempTokens[1].token == ">=")
                                        {
                                            tempBooleanValue = Convert.ToDecimal(tempTokens[0].value) >= Convert.ToDecimal(tempTokens[2].token);
                                        }
                                        else if (tempTokens[1].token == "<=")
                                        {
                                            tempBooleanValue = Convert.ToDecimal(tempTokens[0].value) <= Convert.ToDecimal(tempTokens[2].token);
                                        }
                                        else if (tempTokens[1].token == "==")
                                        {
                                            tempBooleanValue = Convert.ToDecimal(tempTokens[0].value) == Convert.ToDecimal(tempTokens[2].token);
                                        }
                                        else if (tempTokens[1].token == "!=")
                                        {
                                            tempBooleanValue = Convert.ToDecimal(tempTokens[0].value) != Convert.ToDecimal(tempTokens[2].token);
                                        }
                                    }
                                    else if (tempTokens[2].type == "identifier")
                                    {
                                        if (tempTokens[1].token == ">")
                                        {
                                            tempBooleanValue = Convert.ToDecimal(tempTokens[0].value) > Convert.ToDecimal(tempTokens[2].value);
                                        }
                                        else if (tempTokens[1].token == "<")
                                        {
                                            tempBooleanValue = Convert.ToDecimal(tempTokens[0].value) < Convert.ToDecimal(tempTokens[2].value);
                                        }
                                        else if (tempTokens[1].token == ">=")
                                        {
                                            tempBooleanValue = Convert.ToDecimal(tempTokens[0].value) >= Convert.ToDecimal(tempTokens[2].value);
                                        }
                                        else if (tempTokens[1].token == "<=")
                                        {
                                            tempBooleanValue = Convert.ToDecimal(tempTokens[0].value) <= Convert.ToDecimal(tempTokens[2].value);
                                        }
                                        else if (tempTokens[1].token == "==")
                                        {
                                            tempBooleanValue = Convert.ToDecimal(tempTokens[0].value) == Convert.ToDecimal(tempTokens[2].value);
                                        }
                                        else if (tempTokens[1].token == "!=")
                                        {
                                            tempBooleanValue = Convert.ToDecimal(tempTokens[0].value) != Convert.ToDecimal(tempTokens[2].value);
                                        }
                                    }

                                    if (tempTokens.Length > 3)
                                    {
                                        Token[] temp = new Token[tempTokens.Length - 2];
                                        temp[0].token = booleanToString(tempBooleanValue);
                                        temp[0].type = "boolean_value";
                                        temp[0].value = booleanToString(tempBooleanValue);
                                        for (int i = 1; i < tempTokens.Length; i++)
                                        {
                                            temp[i] = tempTokens[i + 2];
                                        }
                                        tempTokens = temp;
                                    }
                                    else
                                    {
                                        Token[] temp = new Token[1];
                                        temp[0].token = booleanToString(tempBooleanValue);
                                        temp[0].type = "boolean_value";
                                        temp[0].value = booleanToString(tempBooleanValue);
                                    }
                                }
                                else if (tempTokens[0].value == "string_value")
                                {
                                    if (tempTokens[2].type == "string_value")
                                    {
                                        if (tempTokens[1].token == "==")
                                        {
                                            tempBooleanValue = tempTokens[0].value == tempTokens[2].token;
                                        }
                                        if (tempTokens[1].token == "!=")
                                        {
                                            tempBooleanValue = tempTokens[0].value != tempTokens[2].token;
                                        }
                                    }
                                    else if (tempTokens[2].type == "identifier")
                                    {
                                        if (tempTokens[1].token == "==")
                                        {
                                            tempBooleanValue = tempTokens[0].value == tempTokens[2].value;
                                        }
                                        if (tempTokens[1].token == "!=")
                                        {
                                            tempBooleanValue = tempTokens[0].value != tempTokens[2].value;
                                        }
                                    }

                                    if (tempTokens.Length > 3)
                                    {
                                        Token[] temp = new Token[tempTokens.Length - 2];
                                        temp[0].token = booleanToString(tempBooleanValue);
                                        temp[0].type = "boolean_value";
                                        temp[0].value = booleanToString(tempBooleanValue);
                                        for (int i = 1; i < tempTokens.Length; i++)
                                        {
                                            temp[i] = tempTokens[i + 2];
                                        }
                                        tempTokens = temp;
                                    }
                                    else
                                    {
                                        Token[] temp = new Token[1];
                                        temp[0].token = booleanToString(tempBooleanValue);
                                        temp[0].type = "boolean_value";
                                        temp[0].value = booleanToString(tempBooleanValue);
                                    }
                                }
                                else if (tempTokens[0].value == "character_value")
                                {
                                    if (tempTokens[2].type == "character_value")
                                    {
                                        if (tempTokens[1].token == "==")
                                        {
                                            tempBooleanValue = tempTokens[0].value == tempTokens[2].token;
                                        }
                                        if (tempTokens[1].token == "!=")
                                        {
                                            tempBooleanValue = tempTokens[0].value != tempTokens[2].token;
                                        }
                                    }
                                    else if (tempTokens[2].type == "identifier")
                                    {
                                        if (tempTokens[1].token == "==")
                                        {
                                            tempBooleanValue = tempTokens[0].value == tempTokens[2].value;
                                        }
                                        if (tempTokens[1].token == "!=")
                                        {
                                            tempBooleanValue = tempTokens[0].value != tempTokens[2].value;
                                        }
                                    }

                                    if (tempTokens.Length > 3)
                                    {
                                        Token[] temp = new Token[tempTokens.Length - 2];
                                        temp[0].token = booleanToString(tempBooleanValue);
                                        temp[0].type = "boolean_value";
                                        temp[0].value = booleanToString(tempBooleanValue);
                                        for (int i = 1; i < tempTokens.Length; i++)
                                        {
                                            temp[i] = tempTokens[i + 2];
                                        }
                                        tempTokens = temp;
                                    }
                                    else
                                    {
                                        Token[] temp = new Token[1];
                                        temp[0].token = booleanToString(tempBooleanValue);
                                        temp[0].type = "boolean_value";
                                        temp[0].value = booleanToString(tempBooleanValue);
                                    }
                                }
                                else if (tempTokens[0].value == "boolean_value")
                                {
                                    if (tempTokens[2].type == "boolean_value")
                                    {
                                        if (tempTokens[1].token == "||")
                                        {
                                            tempBooleanValue = stringToBoolean(tempTokens[0].value) || stringToBoolean(tempTokens[2].token);
                                        }
                                        else if (tempTokens[1].token == "&&")
                                        {
                                            tempBooleanValue = stringToBoolean(tempTokens[0].value) && stringToBoolean(tempTokens[2].token);
                                        }

                                        if (tempTokens.Length > 3)
                                        {
                                            Token[] temp = new Token[tempTokens.Length - 2];
                                            temp[0].token = booleanToString(tempBooleanValue);
                                            temp[0].type = "boolean_value";
                                            temp[0].value = booleanToString(tempBooleanValue);
                                            for (int i = 1; i < tempTokens.Length; i++)
                                            {
                                                temp[i] = tempTokens[i + 2];
                                            }
                                            tempTokens = temp;
                                        }
                                        else
                                        {
                                            Token[] temp = new Token[1];
                                            temp[0].token = booleanToString(tempBooleanValue);
                                            temp[0].type = "boolean_value";
                                            temp[0].value = booleanToString(tempBooleanValue);
                                        }
                                    }
                                    else if (tempTokens[2].type == "identifier")
                                    {
                                        if (tempTokens[2].value == "verdadero" || tempTokens[2].value == "falso")
                                        {
                                            if (tempTokens[1].token == "||")
                                            {
                                                tempBooleanValue = stringToBoolean(tempTokens[0].value) || stringToBoolean(tempTokens[2].value);
                                            }
                                            else if (tempTokens[1].token == "&&")
                                            {
                                                tempBooleanValue = stringToBoolean(tempTokens[0].value) && stringToBoolean(tempTokens[2].value);
                                            }

                                            if (tempTokens.Length > 3)
                                            {
                                                Token[] temp = new Token[tempTokens.Length - 2];
                                                temp[0].token = booleanToString(tempBooleanValue);
                                                temp[0].type = "boolean_value";
                                                temp[0].value = booleanToString(tempBooleanValue);
                                                for (int i = 1; i < tempTokens.Length; i++)
                                                {
                                                    temp[i] = tempTokens[i + 2];
                                                }
                                                tempTokens = temp;
                                            }
                                            else
                                            {
                                                Token[] temp = new Token[1];
                                                temp[0].token = booleanToString(tempBooleanValue);
                                                temp[0].type = "boolean_value";
                                                temp[0].value = booleanToString(tempBooleanValue);
                                            }
                                        }
                                        else
                                        {
                                            tempBooleanValue = stringToBoolean(tempTokens[0].value);

                                            if (tempTokens.Length > 3)
                                            {
                                                Token[] temp = new Token[tempTokens.Length - 2];
                                                temp[0].token = booleanToString(tempBooleanValue);
                                                temp[0].type = "boolean_value";
                                                temp[0].value = booleanToString(tempBooleanValue);
                                                for (int i = 1; i < tempTokens.Length; i++)
                                                {
                                                    temp[i] = tempTokens[i + 2];
                                                }
                                                tempTokens = temp;
                                            }
                                            else
                                            {
                                                Token[] temp = new Token[1];
                                                temp[0].token = booleanToString(tempBooleanValue);
                                                temp[0].type = "boolean_value";
                                                temp[0].value = booleanToString(tempBooleanValue);
                                            }
                                        }
                                    }
                                    else if (tempTokens[2].type == "integer_value" || tempTokens[2].type == "decimal_value" || tempTokens[2].type == "string_value" || tempTokens[2].type == "character_value")
                                    {
                                        if (tempTokens[2].type == "integer_value")
                                        {
                                            if (tempTokens[4].type == "integer_value")
                                            {
                                                if (tempTokens[3].token == ">")
                                                {
                                                    tempBooleanValue = Convert.ToInt32(tempTokens[2].token) > Convert.ToInt32(tempTokens[4].token);
                                                }
                                                else if (tempTokens[3].token == "<")
                                                {
                                                    tempBooleanValue = Convert.ToInt32(tempTokens[2].token) < Convert.ToInt32(tempTokens[4].token);
                                                }
                                                else if (tempTokens[3].token == ">=")
                                                {
                                                    tempBooleanValue = Convert.ToInt32(tempTokens[2].token) >= Convert.ToInt32(tempTokens[4].token);
                                                }
                                                else if (tempTokens[3].token == "<=")
                                                {
                                                    tempBooleanValue = Convert.ToInt32(tempTokens[2].token) <= Convert.ToInt32(tempTokens[4].token);
                                                }
                                                else if (tempTokens[3].token == "==")
                                                {
                                                    tempBooleanValue = Convert.ToInt32(tempTokens[2].token) == Convert.ToInt32(tempTokens[4].token);
                                                }
                                                else if (tempTokens[3].token == "!=")
                                                {
                                                    tempBooleanValue = Convert.ToInt32(tempTokens[2].token) != Convert.ToInt32(tempTokens[4].token);
                                                }
                                            }
                                            else if (tempTokens[4].type == "identifier")
                                            {
                                                if (tempTokens[3].token == ">")
                                                {
                                                    tempBooleanValue = Convert.ToInt32(tempTokens[2].token) > Convert.ToInt32(tempTokens[4].value);
                                                }
                                                else if (tempTokens[3].token == "<")
                                                {
                                                    tempBooleanValue = Convert.ToInt32(tempTokens[2].token) < Convert.ToInt32(tempTokens[4].value);
                                                }
                                                else if (tempTokens[3].token == ">=")
                                                {
                                                    tempBooleanValue = Convert.ToInt32(tempTokens[2].token) >= Convert.ToInt32(tempTokens[4].value);
                                                }
                                                else if (tempTokens[3].token == "<=")
                                                {
                                                    tempBooleanValue = Convert.ToInt32(tempTokens[2].token) <= Convert.ToInt32(tempTokens[4].value);
                                                }
                                                else if (tempTokens[3].token == "==")
                                                {
                                                    tempBooleanValue = Convert.ToInt32(tempTokens[2].token) == Convert.ToInt32(tempTokens[4].value);
                                                }
                                                else if (tempTokens[3].token == "!=")
                                                {
                                                    tempBooleanValue = Convert.ToInt32(tempTokens[2].token) != Convert.ToInt32(tempTokens[4].value);
                                                }
                                            }
                                        }
                                        else if (tempTokens[2].type == "decimal_value")
                                        {
                                            if (tempTokens[4].type == "decimal_value")
                                            {
                                                if (tempTokens[3].token == ">")
                                                {
                                                    tempBooleanValue = Convert.ToDecimal(tempTokens[2].token) > Convert.ToDecimal(tempTokens[4].token);
                                                }
                                                else if (tempTokens[3].token == "<")
                                                {
                                                    tempBooleanValue = Convert.ToDecimal(tempTokens[2].token) < Convert.ToDecimal(tempTokens[4].token);
                                                }
                                                else if (tempTokens[3].token == ">=")
                                                {
                                                    tempBooleanValue = Convert.ToDecimal(tempTokens[2].token) >= Convert.ToDecimal(tempTokens[4].token);
                                                }
                                                else if (tempTokens[3].token == "<=")
                                                {
                                                    tempBooleanValue = Convert.ToDecimal(tempTokens[2].token) <= Convert.ToDecimal(tempTokens[4].token);
                                                }
                                                else if (tempTokens[3].token == "==")
                                                {
                                                    tempBooleanValue = Convert.ToDecimal(tempTokens[2].token) == Convert.ToDecimal(tempTokens[4].token);
                                                }
                                                else if (tempTokens[3].token == "!=")
                                                {
                                                    tempBooleanValue = Convert.ToDecimal(tempTokens[2].token) != Convert.ToDecimal(tempTokens[4].token);
                                                }
                                            }
                                            else if (tempTokens[4].type == "identifier")
                                            {
                                                if (tempTokens[3].token == ">")
                                                {
                                                    tempBooleanValue = Convert.ToDecimal(tempTokens[2].token) > Convert.ToDecimal(tempTokens[4].value);
                                                }
                                                else if (tempTokens[3].token == "<")
                                                {
                                                    tempBooleanValue = Convert.ToDecimal(tempTokens[2].token) < Convert.ToDecimal(tempTokens[4].value);
                                                }
                                                else if (tempTokens[3].token == ">=")
                                                {
                                                    tempBooleanValue = Convert.ToDecimal(tempTokens[2].token) >= Convert.ToDecimal(tempTokens[4].value);
                                                }
                                                else if (tempTokens[3].token == "<=")
                                                {
                                                    tempBooleanValue = Convert.ToDecimal(tempTokens[2].token) <= Convert.ToDecimal(tempTokens[4].value);
                                                }
                                                else if (tempTokens[3].token == "==")
                                                {
                                                    tempBooleanValue = Convert.ToDecimal(tempTokens[2].token) == Convert.ToDecimal(tempTokens[4].value);
                                                }
                                                else if (tempTokens[3].token == "!=")
                                                {
                                                    tempBooleanValue = Convert.ToDecimal(tempTokens[2].token) != Convert.ToDecimal(tempTokens[4].value);
                                                }
                                            }
                                        }
                                        else if (tempTokens[2].type == "string_value")
                                        {
                                            if (tempTokens[4].type == "string_value")
                                            {
                                                if (tempTokens[3].token == "==")
                                                {
                                                    tempBooleanValue = tempTokens[2].token == tempTokens[4].token;
                                                }
                                                if (tempTokens[3].token == "!=")
                                                {
                                                    tempBooleanValue = tempTokens[2].token != tempTokens[4].token;
                                                }
                                            }
                                            else if (tempTokens[4].type == "identifier")
                                            {
                                                if (tempTokens[3].token == "==")
                                                {
                                                    tempBooleanValue = tempTokens[2].token == tempTokens[4].value;
                                                }
                                                if (tempTokens[3].token == "!=")
                                                {
                                                    tempBooleanValue = tempTokens[2].token != tempTokens[4].value;
                                                }
                                            }
                                        }
                                        else if (tempTokens[2].type == "character_value")
                                        {
                                            if (tempTokens[4].type == "character_value")
                                            {
                                                if (tempTokens[3].token == "==")
                                                {
                                                    tempBooleanValue = tempTokens[2].token == tempTokens[4].token;
                                                }
                                                if (tempTokens[3].token == "!=")
                                                {
                                                    tempBooleanValue = tempTokens[2].token != tempTokens[4].token;
                                                }
                                            }
                                            else if (tempTokens[4].type == "identifier")
                                            {
                                                if (tempTokens[3].token == "==")
                                                {
                                                    tempBooleanValue = tempTokens[2].token == tempTokens[4].value;
                                                }
                                                if (tempTokens[3].token == "!=")
                                                {
                                                    tempBooleanValue = tempTokens[2].token != tempTokens[4].value;
                                                }
                                            }
                                        }

                                        if (tempTokens[1].token == "||")
                                        {
                                            tempBooleanValue = stringToBoolean(tempTokens[0].value) || tempBooleanValue;
                                        }
                                        else if (tempTokens[1].token == "&&")
                                        {
                                            tempBooleanValue = stringToBoolean(tempTokens[0].value) && tempBooleanValue;
                                        }

                                        if (tempTokens.Length > 3)
                                        {
                                            Token[] temp = new Token[tempTokens.Length - 4];
                                            temp[0].token = booleanToString(tempBooleanValue);
                                            temp[0].type = "boolean_value";
                                            temp[0].value = booleanToString(tempBooleanValue);
                                            for (int i = 1; i < tempTokens.Length; i++)
                                            {
                                                temp[i] = tempTokens[i + 4];
                                            }
                                            tempTokens = temp;
                                        }
                                        else
                                        {
                                            Token[] temp = new Token[1];
                                            temp[0].token = booleanToString(tempBooleanValue);
                                            temp[0].type = "boolean_value";
                                            temp[0].value = booleanToString(tempBooleanValue);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                return tokens[0].value;
            }

            if (tokens.Length > 2)
            {
                // To eliminate parenthesis and evaluate each of it reducing the expression to a pure logical one
                while (tokens.Length > 1)
                {
                    start = 0;
                    end = 0;

                    for (int i = 0; i < tokens.Length; i++)
                    {
                        if (tokens[i].token == "(")
                        {
                            start = i;
                        }
                        if (tokens[i].token == ")")
                        {
                            end = i;
                        }
                    }

                    if (end > 0)
                    {
                        // To fill a sub array of tokens between parenthesis ()
                        Token[] tempTokens = new Token[end - start - 1];
                        for (int j = start + 1; j < end; j++)
                        {
                            tempTokens[j - start - 1] = tokens[j];
                        }

                        // Filling the new array
                        Token[] newTokens = new Token[tokens.Length - end + start];
                        if (start > 0)
                        {
                            for (int j = 0; j < start; j++)
                            {
                                newTokens[j] = tokens[j];
                            }
                        }

                        tempBooleanString = evaluateBoolean(tempTokens);

                        newTokens[start] = new Token();
                        newTokens[start].token = tempBooleanString;
                        newTokens[start].type = "boolean_value";
                        newTokens[start].value = tempBooleanString;
                        if (end != tokens.Length - 1)
                        {
                            for (int j = start + 1; j < newTokens.Length; j++)
                            {
                                newTokens[j] = tokens[j + end - start];
                            }
                        }

                        tokens = newTokens;
                    }
                    else
                    {
                        tempBooleanString = evaluateBoolean(tokens);
                    }
                }
            }

            if (tokens.Length == 1)
            {
                if (tokens[0].token == "verdadero")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        private bool evaluateBooleanExpression2(Token[] tokens)
        {
            int start = 0;
            int end = 0;

            bool evaluateBoolean(Token[] tempTokens)
            {
                while (tempTokens.Length > 2)
                {
                    if (tempTokens[0].type == "integer_value" || tempTokens[0].type == "decimal_value" || tempTokens[0].type == "string_value" || tempTokens[0].type == "character_value")
                    {
                        bool tempBooleanValue;
                        if (tempTokens[0].type == "integer_value")
                        {
                            if (tempTokens[1].token == ">")
                            {
                                tempBooleanValue = Convert.ToInt32(tempTokens[0].token) > Convert.ToInt32(tempTokens[2].token);
                            }
                            if (tempTokens[1].token == "<")
                            {
                                tempBooleanValue = Convert.ToInt32(tempTokens[0].token) < Convert.ToInt32(tempTokens[2].token);
                            }
                            if (tempTokens[1].token == ">=")
                            {
                                tempBooleanValue = Convert.ToInt32(tempTokens[0].token) >= Convert.ToInt32(tempTokens[2].token);
                            }
                            if (tempTokens[1].token == "<=")
                            {
                                tempBooleanValue = Convert.ToInt32(tempTokens[0].token) <= Convert.ToInt32(tempTokens[2].token);
                            }
                            if (tempTokens[1].token == "==")
                            {
                                tempBooleanValue = Convert.ToInt32(tempTokens[0].token) == Convert.ToInt32(tempTokens[2].token);
                            }
                            if (tempTokens[1].token == "!=")
                            {
                                tempBooleanValue = Convert.ToInt32(tempTokens[0].token) != Convert.ToInt32(tempTokens[2].token);
                            }
                        }

                        if (tempTokens[0].type == "decimal_value")
                        {
                            if (tempTokens[1].token == ">")
                            {
                                tempBooleanValue = Convert.ToDecimal(tempTokens[0].token) > Convert.ToDecimal(tempTokens[2].token);
                            }
                            if (tempTokens[1].token == "<")
                            {
                                tempBooleanValue = Convert.ToDecimal(tempTokens[0].token) < Convert.ToDecimal(tempTokens[2].token);
                            }
                            if (tempTokens[1].token == ">=")
                            {
                                tempBooleanValue = Convert.ToDecimal(tempTokens[0].token) >= Convert.ToDecimal(tempTokens[2].token);
                            }
                            if (tempTokens[1].token == "<=")
                            {
                                tempBooleanValue = Convert.ToDecimal(tempTokens[0].token) <= Convert.ToDecimal(tempTokens[2].token);
                            }
                            if (tempTokens[1].token == "==")
                            {
                                tempBooleanValue = Convert.ToDecimal(tempTokens[0].token) == Convert.ToDecimal(tempTokens[2].token);
                            }
                            if (tempTokens[1].token == "!=")
                            {
                                tempBooleanValue = Convert.ToDecimal(tempTokens[0].token) != Convert.ToDecimal(tempTokens[2].token);
                            }
                        }

                        if (tempTokens[0].type == "string_value")
                        {
                            if (tempTokens[1].token == "==")
                            {
                                tempBooleanValue = tempTokens[0].token == tempTokens[2].token;
                            }
                            if (tempTokens[1].token == "!=")
                            {
                                tempBooleanValue = tempTokens[0].token != tempTokens[2].token;
                            }
                        }

                        if (tempTokens[0].type == "character_value")
                        {
                            if (tempTokens[1].token == "==")
                            {
                                tempBooleanValue = tempTokens[0].token == tempTokens[2].token;
                            }
                            if (tempTokens[1].token == "!=")
                            {
                                tempBooleanValue = tempTokens[0].token != tempTokens[2].token;
                            }
                        }

                        //*****



                    }
                    if (tempTokens[0].type == "boolean_value")
                    {

                    }
                    if (tempTokens[0].type == "identifier")
                    {

                    }
                }

                return false;
            }


            // To eliminate parenthesis and evaluate each of it reducing the expression to a pure logical one
            while (tokens.Length > 1)
            {
                start = 0;
                end = 0;

                for (int i = 0; i < tokens.Length; i++)
                {
                    if (tokens[i].token == "(")
                    {
                        start = i;
                    }
                    if (tokens[i].token == ")")
                    {
                        end = i;
                    }
                    if (end > 0)
                    {
                        // To fill a sub array of tokens between parenthesis ()
                        Token[] tempTokens = new Token[end - start - 1];
                        for (int j = start + 1; j < end; j++)
                        {
                            tempTokens[j - start - 1] = tokens[j];
                        }

                        bool tempBooleanValue = evaluateBoolean(tempTokens);
                        // Filling the new array
                        Token[] newTokens = new Token[tokens.Length - end + start];
                        if (start > 0)
                        {
                            for (int j = 0; j < start; j++)
                            {
                                newTokens[j] = tokens[j];
                            }
                        }
                        newTokens[start] = new Token();
                        newTokens[start].token = Convert.ToString(tempBooleanValue);
                        newTokens[start].type = "boolean_value";
                        newTokens[start].value = Convert.ToString(tempBooleanValue);
                        if (end != tokens.Length - 1)
                        {
                            for (int j = start + 1; j < newTokens.Length; j++)
                            {
                                newTokens[j] = tokens[j + end - start];
                            }
                        }

                        tokens = newTokens;

                        break;
                    }
                }
            }

            return Convert.ToBoolean(tokens[0].value);
            /*
            for (int i = 0; i < tokens.Length; i++)
            {
                if (tokens[i].token == "!")
                {

                }
                else if (tokens[i].token == "(")
                {

                }
                else if (tokens[i].token == ")")
                {

                }
                else if (tokens[i].type == "logical_operator")
                {

                }
                else if (tokens[i].type == "relational_operator")
                {

                }
                else if (tokens[i].type == "identifier")
                {

                }
                else if (tokens[i].type == "integer_value")
                {

                }
                else if (tokens[i].type == "decimal_value")
                {

                }
                else if (tokens[i].type == "string_value")
                {

                }
                else if (tokens[i].type == "character_value")
                {

                }
                else if (tokens[i].type == "boolean_value")
                {

                }
                else
                {
                    return false;
                }
            }*/

            return false;
        }

        //**********************
        //**********************
        //**********************
        // Type test
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
    }
}
