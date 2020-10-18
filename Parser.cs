using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFP_PROYECTO2_Basic_IDE
{
    class Parser
    {
        public Token[] arrayOfTokens = new Token[0];

        private Lexer myLexer = new Lexer();

        public NTree myNTree = new NTree();

        //**********************
        public void listArrayOfTokens(string tokens)
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
             
            // To make an array of accepted identifiers
            List<Token> identifiersList = new List<Token>();
            for (int i = 0; i < arrayOfTokens.Length; i++)
            {
                if (arrayOfTokens[i].type == "identifier")
                {
                    if (arrayOfTokens[i - 1].type == "integer_type" || arrayOfTokens[i - 1].type == "decimal_type" || arrayOfTokens[i - 1].type == "string_type" || arrayOfTokens[i - 1].type == "character_type" || arrayOfTokens[i - 1].type == "boolean_type")
                    {
                        arrayOfTokens[i].identifierType = arrayOfTokens[i - 1].type;
                        identifiersList.Add(arrayOfTokens[i]);
                    }
                }
            }

            // To populate the array of tokens with a starting value
            for (int i = 0; i < arrayOfTokens.Length; i++)
            {
                if (arrayOfTokens[i].type == "integer_value" || arrayOfTokens[i].type == "decimal_value" || arrayOfTokens[i].type == "string_value" || arrayOfTokens[i].type == "character_value" || arrayOfTokens[i].type == "boolean_value")
                {
                    arrayOfTokens[i].value = arrayOfTokens[i].token;
                }
                else if (arrayOfTokens[i].type == "identifier")
                {
                    for (int j = 0; j < identifiersList.Count; j++)
                    {
                        if (arrayOfTokens[i].token == identifiersList[j].token)
                        {
                            arrayOfTokens[i].identifierType = identifiersList[j].identifierType;

                            if (identifiersList[j].identifierType == "integer_value")
                            {
                                arrayOfTokens[i].value = "0";
                            }
                            else if (identifiersList[j].identifierType == "decimal_value")
                            {
                                arrayOfTokens[i].value = "0.0";
                            }
                            else if (identifiersList[j].identifierType == "string_value")
                            {
                                arrayOfTokens[i].value = "";
                            }
                            else if (identifiersList[j].identifierType == "character_value")
                            {
                                arrayOfTokens[i].value = "''";
                            }
                            else if (identifiersList[j].identifierType == "boolean_value")
                            {
                                arrayOfTokens[i].value = "verdadero";
                            }
                        }
                    }
                }
            }
        }

        //************************************************************************************************************************************
        //************************************************************************************************************************************
        //************************************************************************************************************************************
        // The parser main method

        //**********************
        public string parseArrayOfTokens (string lexerString) // Here we use a string of tokens procesed by the lexer
        {
            string log = "";

            //log = lexerString;
            //log += "\n";
            listArrayOfTokens(lexerString);

            //***************************
            // TROUBLES AREA

            // To test if the count of brakets and its order are right
            if (isIdentifiersRight(arrayOfTokens) == false)
            {
                log += "Algunos identificadores están mal y No son válidos" + "\n";
            }

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
            else
            {
                log += "Es una expresión booleana" + "\n";
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
        // To test if there are identifers Not valid
        private bool isIdentifiersRight(Token[] tokens)
        {
            int counter = 0;
            if (tokens.Length > 0)
            {
                for (int i = 0; i < tokens.Length; i++)
                {
                    if (tokens[i].type == "identifier_NOT_Valid")
                    {
                        counter++;
                    }

                    if (counter > 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

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

        //**********************************************************************************
        //**************************** BOOLEAN METHODS**************************************

        //**********************
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

        //**********************
        // 5 STARS FUNCTION!!
        public void populateBinaryTree(BinaryTree _myBinaryTree, Token[] myTokens)
        {
            Node myNode = null;

            for (int i = 0; i < myTokens.Length; i++)
            {
                if (myTokens[i].token == "!")
                {
                    // If we have a lot of "!" symbols
                    int count = 0;
                    for (int j = i; j < myTokens.Length; j++)
                    {
                        if (myTokens[j].token == "!")
                        {
                            count++;
                        }
                    }

                    i += count;
                    myNode = _myBinaryTree.append(myNode, myTokens[i]);
                    myNode.annotation = count;  // number of negation symbols
                }
                else if (myTokens[i].token == "(")
                {
                    myNode = _myBinaryTree.append(myNode, myTokens[i]);
                }
                else if (myTokens[i].token == ")")
                {
                    myNode = myNode.previous;

                    if (i + 1 < myTokens.Length)
                    {
                        if (myNode == _myBinaryTree.firstNode && (myTokens[i + 1].token == "||" || myTokens[i + 1].token == "&&"))
                        {
                            myNode = _myBinaryTree.pushLeft(_myBinaryTree.firstNode, myTokens[i + 1]);
                            i++;
                        }
                    }
                }
                else if (myTokens[i].token == "||" || myTokens[i].token == "&&")
                {
                    if (myNode == null)
                    {
                        myNode = _myBinaryTree.pushLeft(_myBinaryTree.firstNode, myTokens[i]);
                    }

                    myNode.token = myTokens[i];
                }
                else if (myTokens[i].type == "integer_value" || myTokens[i].type == "decimal_value" || myTokens[i].type == "string_value" || myTokens[i].type == "character_value" || (myTokens[i].type == "identifier" && myTokens[i].value != "boolean_value"))
                {
                    _myBinaryTree.append(myNode, myTokens[i]);
                    _myBinaryTree.append(myNode, myTokens[i + 2]);
                    myNode.token = myTokens[i + 1];
                    i += 2;
                }
                else if (myTokens[i].type == "boolean_value" || (myTokens[i].type == "identifier" && myTokens[i].value == "boolean_value"))
                {
                    myNode.token = myTokens[i];
                }
            }
        }

        //**********************
        public Token evaluateBinaryUnaryOperation(Token operand1, Token operand2, Token relationalSymbol)
        {
            Token newToken = new Token();
            newToken.token = "verdadero";
            newToken.type = "boolean_value";
            bool tempBoolean = false;

            if (operand1 != null && operand2 != null)
            {
                if (operand1.type == "integer_value")
                {
                    if (operand2.type == "integer_value" || (operand2.type == "identifier" && operand2.identifierType == "integer_type"))
                    {
                        if (relationalSymbol.token == "<")
                        {
                            tempBoolean = Convert.ToInt64(operand1.token) < Convert.ToInt64(operand2.value);
                        }
                        else if (relationalSymbol.token == ">")
                        {
                            tempBoolean = Convert.ToInt64(operand1.token) > Convert.ToInt64(operand2.value);
                        }
                        else if (relationalSymbol.token == "<=")
                        {
                            tempBoolean = Convert.ToInt64(operand1.token) <= Convert.ToInt64(operand2.value);
                        }
                        else if (relationalSymbol.token == ">=")
                        {
                            tempBoolean = Convert.ToInt64(operand1.token) >= Convert.ToInt64(operand2.value);
                        }
                        else if (relationalSymbol.token == "==")
                        {
                            tempBoolean = Convert.ToInt64(operand1.token) == Convert.ToInt64(operand2.value);
                        }
                        else if (relationalSymbol.token == "!=")
                        {
                            tempBoolean = Convert.ToInt64(operand1.token) != Convert.ToInt64(operand2.value);
                        }

                        newToken.token = tempBoolean == true ? "verdadero" : "falso";
                    }
                    else
                    {
                        newToken.token = "falso";
                    }
                }
                else if (operand1.type == "decimal_value")
                {
                    if (operand2.type == "decimal_value" || (operand2.type == "identifier" && operand2.identifierType == "decimal_type"))
                    {
                        if (relationalSymbol.token == "<")
                        {
                            tempBoolean = Convert.ToDouble(operand1.token) < Convert.ToDouble(operand2.value);
                        }
                        else if (relationalSymbol.token == ">")
                        {
                            tempBoolean = Convert.ToDouble(operand1.token) > Convert.ToDouble(operand2.value);
                        }
                        else if (relationalSymbol.token == "<=")
                        {
                            tempBoolean = Convert.ToDouble(operand1.token) <= Convert.ToDouble(operand2.value);
                        }
                        else if (relationalSymbol.token == ">=")
                        {
                            tempBoolean = Convert.ToDouble(operand1.token) >= Convert.ToDouble(operand2.value);
                        }
                        else if (relationalSymbol.token == "==")
                        {
                            tempBoolean = Convert.ToDouble(operand1.token) == Convert.ToDouble(operand2.value);
                        }
                        else if (relationalSymbol.token == "!=")
                        {
                            tempBoolean = Convert.ToDouble(operand1.token) != Convert.ToDouble(operand2.value);
                        }

                        newToken.token = tempBoolean == true ? "verdadero" : "falso";
                    }
                    else
                    {
                        newToken.token = "falso";
                    }
                }
                else if (operand1.type == "string_value")
                {
                    if (operand2.type == "string_value" || (operand2.type == "identifier" && operand2.identifierType == "string_type"))
                    {
                        if (relationalSymbol.token == "==")
                        {
                            tempBoolean = operand1.token == operand2.value;
                        }
                        else if (relationalSymbol.token == "!=")
                        {
                            tempBoolean = operand1.token != operand2.value;
                        }

                        newToken.token = tempBoolean == true ? "verdadero" : "falso";
                    }
                    else
                    {
                        newToken.token = "falso";
                    }
                }
                else if (operand1.type == "character_value")
                {
                    if (operand2.type == "character_value" || (operand2.type == "identifier" && operand2.identifierType == "character_type"))
                    {
                        if (relationalSymbol.token == "==")
                        {
                            tempBoolean = operand1.token == operand2.value;
                        }
                        else if (relationalSymbol.token == "!=")
                        {
                            tempBoolean = operand1.token != operand2.value;
                        }

                        newToken.token = tempBoolean == true ? "verdadero" : "falso";
                    }
                    else
                    {
                        newToken.token = "falso";
                    }
                }
                else if (operand1.type == "boolean_value")
                {
                    if (operand2.type == "boolean_value" || (operand2.type == "identifier" && operand2.identifierType == "boolean_type"))
                    {
                        bool operand1Boolean = operand1.token == "verdadero" ? true : false;
                        bool operand2Boolean = operand2.token == "verdadero" ? true : false;

                        if (relationalSymbol.token == "==")
                        {
                            tempBoolean = operand1Boolean == operand2Boolean;
                        }
                        else if (relationalSymbol.token == "!=")
                        {
                            tempBoolean = operand1Boolean != operand2Boolean;
                        }
                        else if (relationalSymbol.token == "||")
                        {
                            tempBoolean = operand1Boolean || operand2Boolean;
                        }
                        else if (relationalSymbol.token == "&&")
                        {
                            tempBoolean = operand1Boolean && operand2Boolean;
                        }

                        newToken.token = tempBoolean == true ? "verdadero" : "falso";
                    }
                    else
                    {
                        newToken.token = "falso";
                    }
                }
                else if (operand1.type == "identifier")
                {
                    if (operand1.identifierType == "integer_type")
                    {
                        if (operand2.type == "integer_value" || (operand2.type == "identifier" && operand2.identifierType == "integer_type"))
                        {
                            if (relationalSymbol.token == "<")
                            {
                                tempBoolean = Convert.ToInt64(operand1.value) < Convert.ToInt64(operand2.value);
                            }
                            else if (relationalSymbol.token == ">")
                            {
                                tempBoolean = Convert.ToInt64(operand1.value) > Convert.ToInt64(operand2.value);
                            }
                            else if (relationalSymbol.token == "<=")
                            {
                                tempBoolean = Convert.ToInt64(operand1.value) <= Convert.ToInt64(operand2.value);
                            }
                            else if (relationalSymbol.token == ">=")
                            {
                                tempBoolean = Convert.ToInt64(operand1.value) >= Convert.ToInt64(operand2.value);
                            }
                            else if (relationalSymbol.token == "==")
                            {
                                tempBoolean = Convert.ToInt64(operand1.value) == Convert.ToInt64(operand2.value);
                            }
                            else if (relationalSymbol.token == "!=")
                            {
                                tempBoolean = Convert.ToInt64(operand1.value) != Convert.ToInt64(operand2.value);
                            }

                            newToken.token = tempBoolean == true ? "verdadero" : "falso";
                        }
                        else
                        {
                            newToken.token = "falso";
                        }
                    }
                    else if (operand1.identifierType == "decimal_type")
                    {
                        if (operand2.type == "decimal_value" || (operand2.type == "identifier" && operand2.identifierType == "decimal_type"))
                        {
                            if (relationalSymbol.token == "<")
                            {
                                tempBoolean = Convert.ToDouble(operand1.value) < Convert.ToDouble(operand2.value);
                            }
                            else if (relationalSymbol.token == ">")
                            {
                                tempBoolean = Convert.ToDouble(operand1.value) > Convert.ToDouble(operand2.value);
                            }
                            else if (relationalSymbol.token == "<=")
                            {
                                tempBoolean = Convert.ToDouble(operand1.value) <= Convert.ToDouble(operand2.value);
                            }
                            else if (relationalSymbol.token == ">=")
                            {
                                tempBoolean = Convert.ToDouble(operand1.value) >= Convert.ToDouble(operand2.value);
                            }
                            else if (relationalSymbol.token == "==")
                            {
                                tempBoolean = Convert.ToDouble(operand1.value) == Convert.ToDouble(operand2.value);
                            }
                            else if (relationalSymbol.token == "!=")
                            {
                                tempBoolean = Convert.ToDouble(operand1.value) != Convert.ToDouble(operand2.value);
                            }

                            newToken.token = tempBoolean == true ? "verdadero" : "falso";
                        }
                        else
                        {
                            newToken.token = "falso";
                        }
                    }
                    else if (operand1.identifierType == "string_type")
                    {
                        if (operand2.type == "string_value" || (operand2.type == "identifier" && operand2.identifierType == "string_type"))
                        {
                            if (relationalSymbol.token == "==")
                            {
                                tempBoolean = operand1.value == operand2.value;
                            }
                            else if (relationalSymbol.token == "!=")
                            {
                                tempBoolean = operand1.value != operand2.value;
                            }

                            newToken.token = tempBoolean == true ? "verdadero" : "falso";
                        }
                        else
                        {
                            newToken.token = "falso";
                        }
                    }
                    else if (operand1.identifierType == "character_type")
                    {
                        if (operand2.type == "character_value" || (operand2.type == "identifier" && operand2.identifierType == "character_type"))
                        {
                            if (relationalSymbol.token == "==")
                            {
                                tempBoolean = operand1.value == operand2.value;
                            }
                            else if (relationalSymbol.token == "!=")
                            {
                                tempBoolean = operand1.value != operand2.value;
                            }

                            newToken.token = tempBoolean == true ? "verdadero" : "falso";
                        }
                        else
                        {
                            newToken.token = "falso";
                        }
                    }
                    else if (operand1.identifierType == "boolean_type")
                    {
                        if (operand2.type == "boolean_value" || (operand2.type == "identifier" && operand2.identifierType == "boolean_type"))
                        {
                            bool operand1Boolean = operand1.value == "verdadero" ? true : false;
                            bool operand2Boolean = operand2.value == "verdadero" ? true : false;

                            if (relationalSymbol.token == "==")
                            {
                                tempBoolean = operand1Boolean == operand2Boolean;
                            }
                            else if (relationalSymbol.token == "!=")
                            {
                                tempBoolean = operand1Boolean != operand2Boolean;
                            }
                            else if (relationalSymbol.token == "||")
                            {
                                tempBoolean = operand1Boolean || operand2Boolean;
                            }
                            else if (relationalSymbol.token == "&&")
                            {
                                tempBoolean = operand1Boolean && operand2Boolean;
                            }

                            newToken.token = tempBoolean == true ? "verdadero" : "falso";
                        }
                        else
                        {
                            newToken.token = "falso";
                        }
                    }
                }
            }
            else // If some of the opeerands are null, a unary operation
            {
                if (operand1 == null)
                {
                    if(operand2.type == "boolean_value")
                    {
                        newToken.token = operand2.token;
                    }
                    else if (operand2.type == "identifier" && isBoolean(operand2.value))
                    {
                        newToken.token = operand2.value;
                    }
                    else
                    {
                        newToken.token = "verdadero";
                    }
                }
                if (operand2 == null)
                {
                    if (operand1.type == "boolean_value")
                    {
                        newToken.token = operand1.token;
                    }
                    else if (operand1.type == "identifier" && isBoolean(operand1.value))
                    {
                        newToken.token = operand1.value;
                    }
                    else
                    {
                        newToken.token = "verdadero";
                    }
                }
            }

            return newToken;
        }

        //**********************
        public bool evaluateBooleanBinaryTree (BinaryTree _myBinaryTree)
        {
            void findNodesInLevelAndEvaluate(Node _testNode, int level)
            {
                if (_testNode == null)
                {
                    return;
                }

                if (_testNode == _myBinaryTree.firstNode)
                {
                    if (_testNode.level == level)
                    {
                        // Do something here
                        
                    }
                }

                if (_testNode.leftNode != null)
                {
                    if (_testNode.leftNode.level == level)
                    {
                        // If we have a binary operation or unary operation in case the left or right nodes are null
                        if (_testNode.rightNode != null)
                        {
                            if (_testNode.leftNode.token.type == "integer_value" || _testNode.leftNode.token.type == "decimal_value" || _testNode.leftNode.token.type == "string_value" || _testNode.leftNode.token.type == "character_value" || _testNode.leftNode.token.type == "boolean_value" || _testNode.leftNode.token.type == "identifier")
                            {
                                _testNode.token = evaluateBinaryUnaryOperation(_testNode.leftNode.token, _testNode.rightNode.token, _testNode.token);
                                // We have "!" symbol codified in "annotation"
                                if (_testNode.annotation % 2 == 1)
                                {
                                    _testNode.token.token = _testNode.token.token == "verdadero" ? "falso" : "verdadero";
                                    _testNode.token.value = _testNode.token.value == "verdadero" ? "falso" : "verdadero";
                                }
                                _testNode.leftNode = null;
                                _testNode.rightNode = null;
                            }
                        }
                        else
                        {
                            _testNode.token = evaluateBinaryUnaryOperation(_testNode.leftNode.token, null, _testNode.token);
                            // We have "!" symbol codified in "annotation"
                            if (_testNode.annotation % 2 == 1)
                            {
                                _testNode.token.token = _testNode.token.token == "verdadero" ? "falso" : "verdadero";
                                _testNode.token.value = _testNode.token.value == "verdadero" ? "falso" : "verdadero";
                            }
                            _testNode.leftNode = null;
                            _testNode.rightNode = null;
                        }
                    }

                    findNodesInLevelAndEvaluate(_testNode.leftNode, level);
                }

                if (_testNode.rightNode != null)
                {
                    if (_testNode.rightNode.level == level)
                    {
                        // If we have a binary operation or unary operation in case the left or right nodes are null
                        if (_testNode.leftNode != null)
                        {
                            if (_testNode.rightNode.token.type == "integer_value" || _testNode.rightNode.token.type == "decimal_value" || _testNode.rightNode.token.type == "string_value" || _testNode.rightNode.token.type == "character_value" || _testNode.rightNode.token.type == "boolean_value" || _testNode.rightNode.token.type == "identifier")
                            {
                                _testNode.token = evaluateBinaryUnaryOperation(_testNode.leftNode.token, _testNode.rightNode.token, _testNode.token);
                                // We have "!" symbol codified in "annotation"
                                if (_testNode.annotation % 2 == 1)
                                {
                                    _testNode.token.token = _testNode.token.token == "verdadero" ? "falso" : "verdadero";
                                    _testNode.token.value = _testNode.token.value == "verdadero" ? "falso" : "verdadero";
                                }
                                _testNode.leftNode = null;
                                _testNode.rightNode = null;
                            }
                        }
                        else
                        {
                            _testNode.token = evaluateBinaryUnaryOperation(null, _testNode.leftNode.token, _testNode.token);
                            // We have "!" symbol codified in "annotation"
                            if (_testNode.annotation % 2 == 1)
                            {
                                _testNode.token.token = _testNode.token.token == "verdadero" ? "falso" : "verdadero";
                                _testNode.token.value = _testNode.token.value == "verdadero" ? "falso" : "verdadero";
                            }
                            _testNode.leftNode = null;
                            _testNode.rightNode = null;
                        }
                    }

                    findNodesInLevelAndEvaluate(_testNode.rightNode, level);
                }
            }

            for (int i = _myBinaryTree.maxLevel; i > 0; i--)
            {
                findNodesInLevelAndEvaluate(_myBinaryTree.firstNode,i);
            }

            if (_myBinaryTree.firstNode.token.token == "verdadero")
            {
                return true;
            }
            else if (_myBinaryTree.firstNode.token.token == "falso")
            {
                return false;
            }

            return false;
        }

        //**********************
        public bool evaluateBooleanExpression(Token[] tokens)
        {
            int start = 0;
            int end = 0;
            string tempBooleanString = "falso";
            BinaryTree myBinaryTree = new BinaryTree();

            populateBinaryTree(myBinaryTree, tokens);

            return evaluateBooleanBinaryTree(myBinaryTree);
        }

        //**************************** BOOLEAN METHODS**************************************
        //**********************************************************************************

        //**********************
        public void populateMyNTree(Token[] myTokens)
        {
            int treeLevel = 0;
            string tokenType = "";
            List<Token> tokenList = new List<Token>();
            NNode myNNode = new NNode("", tokenList);
            List<Token> commandTokens = new List<Token>();

            myNTree = new NTree();

            for (int i = 0; i < myTokens.Length; i++)
            {
                if (myTokens[i].token == "principal" || myTokens[i].token == "SI" || myTokens[i].token == "SINO" || myTokens[i].token == "SINO_SI" || myTokens[i].token == "PARA" || myTokens[i].token == "MIENTRAS" || myTokens[i].token == "HACER" || myTokens[i].token == "EN_CASO" || myTokens[i].token == "CASO:" || myTokens[i].token == "OTRO CASO:")
                {
                    if (myTokens[i].token == "principal")
                    {
                        tokenType = "principal_function";
                    }
                    else if (myTokens[i].token == "SI")
                    {
                        tokenType = "if";
                    }
                    else if (myTokens[i].token == "SINO")
                    {
                        tokenType = "else";
                    }
                    else if (myTokens[i].token == "SINO_SI")
                    {
                        tokenType = "else_if";
                    }
                    else if (myTokens[i].token == "PARA")
                    {
                        tokenType = "for";
                    }
                    else if (myTokens[i].token == "MIENTRAS")
                    {
                        tokenType = "for";
                    }
                    else if (myTokens[i].token == "HACER")
                    {
                        tokenType = "do";
                    }
                    else if (myTokens[i].token == "EN_CASO")
                    {
                        tokenType = "switch";
                    }
                    else if (myTokens[i].token == "CASO:")
                    {
                        tokenType = "case";
                    }
                    else if (myTokens[i].token == "OTRO CASO:")
                    {
                        tokenType = "default";
                    }

                    /*for (int j = i; j < myTokens.Length; j++)
                    {
                        if (myTokens[j].token != "{")
                        {
                            commandTokens.Add(myTokens[j]);
                        }
                        else
                        {
                            i += j - i - 1;
                        }
                    }*/
                }
                else if (myTokens[i].type == "integer_type" || myTokens[i].type == "decimal_type" || myTokens[i].type == "string_type" || myTokens[i].type == "character_type" || myTokens[i].type == "boolean_type" || myTokens[i].type == "identifier")
                {
                    tokenType = myTokens[i].type;

                    /*for (int j = i; j < myTokens.Length; j++)
                    {
                        if (myTokens[j].token != ";")
                        {
                            tokenList.Add(myTokens[j]);
                        }
                        else
                        {
                            i += j - i - 1;
                        }
                    }*/
                }



                if (myTokens[i].token == "{")
                {
                    treeLevel++;
                    myNNode = myNTree.append(myNNode, tokenType, tokenList/*commandTokens*/);
                    tokenList.Clear();
                }
                else if (myTokens[i].token == "}")
                {
                    treeLevel--;
                    tokenType = "";
                    myNNode = myNNode.previous;
                    //commandTokens.Clear();
                    tokenList.Clear();
                }
                else if (myTokens[i].token == ";")
                {
                    myNNode = myNTree.append(myNNode, tokenType, tokenList);
                    myNNode = myNNode.previous;
                    tokenList.Clear();
                }
                else
                {
                    tokenList.Add(myTokens[i]);
                }
            }
        }

        //**********************
        public void executeCommand(Token[] tokens, string commandType)
        {
            if (commandType == "asignation")
            {

            }
            else if (commandType == "if_else")
            {

            }
        }

        //**********************
        //**********************
        //**********************
        // Type test

        //**********************
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

        //**********************
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

        //**********************
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

        //**********************
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

        //**********************
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
