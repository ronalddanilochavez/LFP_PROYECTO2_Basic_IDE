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

        public List<Token> myIdentifiers = new List<Token>();

        public string LogFinal = "";

        //**********************
        public void listArrayOfTokens(string tokens)
        {
            // To clear myIdentifiers array
            myIdentifiers.Clear();

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
            string type = "";
            for (int i = 0; i < arrayOfTokens.Length; i++)
            {
                // Makes a mark when some defined type happens
                if (arrayOfTokens[i].type == "integer_type" || arrayOfTokens[i].type == "decimal_type" || arrayOfTokens[i].type == "string_type" || arrayOfTokens[i].type == "character_type" || arrayOfTokens[i].type == "boolean_type")
                {
                    type = arrayOfTokens[i].type;
                }

                // Erases the type string
                if (arrayOfTokens[i].token == "=" || arrayOfTokens[i].token == ";")
                {
                    type = "";
                }

                if (type == "integer_type" || type == "decimal_type" || type == "string_type" || type == "character_type" || type == "boolean_type")
                {
                    if (arrayOfTokens[i].type == "identifier")
                    {
                        arrayOfTokens[i].identifierType = type;

                        if (myIdentifiers.Count > 0)
                        {
                            // To see if the identifier already exists
                            bool equals = false;
                            for (int j = 0; j < myIdentifiers.Count; j++)
                            {
                                if (myIdentifiers[j] == arrayOfTokens[i])
                                {
                                    equals = true;
                                }
                            }

                            if (equals == false)
                            {
                                myIdentifiers.Add(arrayOfTokens[i]);
                            }
                        }
                        else
                        {
                            myIdentifiers.Add(arrayOfTokens[i]);
                        }
                    }
                }
            }

            // To fill all myIdentifiers array with values
            if (myIdentifiers.Count > 0)
            {
                for (int i = 0; i < myIdentifiers.Count; i++)
                {
                    // To populate the array myIdentifiers with a starting value
                    if (myIdentifiers[i].identifierType == "integer_type")
                    {
                        myIdentifiers[i].value = "0";
                    }
                    else if (myIdentifiers[i].identifierType == "decimal_type")
                    {
                        myIdentifiers[i].value = "0.0";
                    }
                    else if (myIdentifiers[i].identifierType == "string_type")
                    {
                        myIdentifiers[i].value = "";
                    }
                    else if (myIdentifiers[i].identifierType == "character_type")
                    {
                        myIdentifiers[i].value = "";
                    }
                    else if (myIdentifiers[i].identifierType == "boolean_type")
                    {
                        myIdentifiers[i].value = "verdadero";
                    }
                }
            }

            // To fill the arrayOfTokens identifiers with identifierType
            if (arrayOfTokens.Length > 0 && myIdentifiers.Count > 0)
            {
                for (int i = 0; i < arrayOfTokens.Length; i++)
                {
                    for (int j = 0; j < myIdentifiers.Count; j++)
                    {
                        if (arrayOfTokens[i].token == myIdentifiers[j].token)
                        {
                            arrayOfTokens[i].identifierType = myIdentifiers[j].identifierType;
                        }
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
                    for (int j = 0; j < myIdentifiers.Count; j++)
                    {
                        if (arrayOfTokens[i].token == myIdentifiers[j].token)
                        {
                            arrayOfTokens[i].identifierType = myIdentifiers[j].identifierType;

                            if (myIdentifiers[j].identifierType == "integer_value")
                            {
                                arrayOfTokens[i].value = "0";
                            }
                            else if (myIdentifiers[j].identifierType == "decimal_value")
                            {
                                arrayOfTokens[i].value = "0.0";
                            }
                            else if (myIdentifiers[j].identifierType == "string_value")
                            {
                                arrayOfTokens[i].value = "";
                            }
                            else if (myIdentifiers[j].identifierType == "character_value")
                            {
                                arrayOfTokens[i].value = "";
                            }
                            else if (myIdentifiers[j].identifierType == "boolean_value")
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
            LogFinal = "";

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
            /*if (isBooleanExpression(arrayOfTokens) == false)
            {
                log += "NO es una expresión booleana" + "\n";
            }
            else
            {
                log += "Es una expresión booleana" + "\n";
            }*/

            // To quit if some trouble arises
            if (log.Length > 0)
            {
                return log;
            }

            //***************************

            // To execute the commands of the program
            EXECUTE_COMMANDS();
            log += LogFinal + "\n";

            /*if (arrayOfTokens.Length > 0)
            {
                for (int i = 0; i < arrayOfTokens.Length; i++)
                {
                    if (arrayOfTokens[i].type == "identifier")
                    {
                        log += arrayOfTokens[i].token + ", " + arrayOfTokens[i].type + ", " + arrayOfTokens[i].value + ", " + arrayOfTokens[i].identifierType + "\n";
                    }
                }
            }

            for (int i = 0; i < myIdentifiers.Count; i++)
            {
                log += myIdentifiers[i].token + ", " + myIdentifiers[i].type + ", " + myIdentifiers[i].value + ", " + myIdentifiers[i].identifierType + "\n";
            }

            for (int i = 0; i < arrayOfTokens.Length; i++)
            {
                log += arrayOfTokens[i].token + " " + arrayOfTokens[i].value + "\n"; 
            }*/

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
                else if (myTokens[i].type == "integer_value" || myTokens[i].type == "decimal_value" || myTokens[i].type == "string_value" || myTokens[i].type == "character_value" || (myTokens[i].type == "identifier" && myTokens[i].identifierType != "boolean_type"))
                {
                    _myBinaryTree.append(myNode, myTokens[i]);
                    _myBinaryTree.append(myNode, myTokens[i + 2]);
                    myNode.token = myTokens[i + 1];
                    i += 2;
                }
                else if (myTokens[i].type == "boolean_value" || (myTokens[i].type == "identifier" && myTokens[i].identifierType == "boolean_type"))
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

            string operand1Value = "";
            if (operand1.type == "identifier")
            {
                for (int i = 0; i < myIdentifiers.Count; i++)
                {
                    if (operand1.token == myIdentifiers[i].token)
                    {
                        operand1Value = myIdentifiers[i].value;
                    }
                }
            }

            string operand2Value = "";
            if (operand2.type == "identifier")
            {
                for (int i = 0; i < myIdentifiers.Count; i++)
                {
                    if (operand2.token == myIdentifiers[i].token)
                    {
                        operand2Value = myIdentifiers[i].value;
                    }
                }
            }

            // Binary operation
            if (operand1 != null && operand2 != null)
            {
                if (operand1.type == "integer_value")
                {
                    if (operand2.type == "integer_value" /*|| (operand2.type == "identifier" && operand2.identifierType == "integer_type")*/)
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
                    else if (operand2.type == "identifier" && operand2.identifierType == "integer_type")
                    {
                        if (relationalSymbol.token == "<")
                        {
                            tempBoolean = Convert.ToInt64(operand1.token) < Convert.ToInt64(operand2Value);
                        }
                        else if (relationalSymbol.token == ">")
                        {
                            tempBoolean = Convert.ToInt64(operand1.token) > Convert.ToInt64(operand2Value);
                        }
                        else if (relationalSymbol.token == "<=")
                        {
                            tempBoolean = Convert.ToInt64(operand1.token) <= Convert.ToInt64(operand2Value);
                        }
                        else if (relationalSymbol.token == ">=")
                        {
                            tempBoolean = Convert.ToInt64(operand1.token) >= Convert.ToInt64(operand2Value);
                        }
                        else if (relationalSymbol.token == "==")
                        {
                            tempBoolean = Convert.ToInt64(operand1.token) == Convert.ToInt64(operand2Value);
                        }
                        else if (relationalSymbol.token == "!=")
                        {
                            tempBoolean = Convert.ToInt64(operand1.token) != Convert.ToInt64(operand2Value);
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
                    if (operand2.type == "decimal_value" /*|| (operand2.type == "identifier" && operand2.identifierType == "decimal_type")*/)
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
                    else if (operand2.type == "identifier" && operand2.identifierType == "decimal_type")
                    {
                        if (relationalSymbol.token == "<")
                        {
                            tempBoolean = Convert.ToDouble(operand1.token) < Convert.ToDouble(operand2Value);
                        }
                        else if (relationalSymbol.token == ">")
                        {
                            tempBoolean = Convert.ToDouble(operand1.token) > Convert.ToDouble(operand2Value);
                        }
                        else if (relationalSymbol.token == "<=")
                        {
                            tempBoolean = Convert.ToDouble(operand1.token) <= Convert.ToDouble(operand2Value);
                        }
                        else if (relationalSymbol.token == ">=")
                        {
                            tempBoolean = Convert.ToDouble(operand1.token) >= Convert.ToDouble(operand2Value);
                        }
                        else if (relationalSymbol.token == "==")
                        {
                            tempBoolean = Convert.ToDouble(operand1.token) == Convert.ToDouble(operand2Value);
                        }
                        else if (relationalSymbol.token == "!=")
                        {
                            tempBoolean = Convert.ToDouble(operand1.token) != Convert.ToDouble(operand2Value);
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
                    if (operand2.type == "string_value" /*|| (operand2.type == "identifier" && operand2.identifierType == "string_type")*/)
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
                    else if (operand2.type == "identifier" && operand2.identifierType == "string_type")
                    {
                        if (relationalSymbol.token == "==")
                        {
                            tempBoolean = operand1.token == operand2Value;
                        }
                        else if (relationalSymbol.token == "!=")
                        {
                            tempBoolean = operand1.token != operand2Value;
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
                    if (operand2.type == "character_value" /*|| (operand2.type == "identifier" && operand2.identifierType == "character_type")*/)
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
                    else if (operand2.type == "identifier" && operand2.identifierType == "character_type")
                    {
                        if (relationalSymbol.token == "==")
                        {
                            tempBoolean = operand1.token == operand2Value;
                        }
                        else if (relationalSymbol.token == "!=")
                        {
                            tempBoolean = operand1.token != operand2Value;
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
                    if (operand2.type == "boolean_value" /*|| (operand2.type == "identifier" && operand2.identifierType == "boolean_type")*/)
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
                    else if (operand2.type == "identifier" && operand2.identifierType == "boolean_type")
                    {
                        bool operand1Boolean = operand1.value == "verdadero" ? true : false;
                        bool operand2Boolean = operand2Value == "verdadero" ? true : false;

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
                        if (operand2.type == "integer_value" /*|| (operand2.type == "identifier" && operand2.identifierType == "integer_type")*/)
                        {
                            if (relationalSymbol.token == "<")
                            {
                                tempBoolean = Convert.ToInt64(operand1Value) < Convert.ToInt64(operand2.value);
                            }
                            else if (relationalSymbol.token == ">")
                            {
                                tempBoolean = Convert.ToInt64(operand1Value) > Convert.ToInt64(operand2.value);
                            }
                            else if (relationalSymbol.token == "<=")
                            {
                                tempBoolean = Convert.ToInt64(operand1Value) <= Convert.ToInt64(operand2.value);
                            }
                            else if (relationalSymbol.token == ">=")
                            {
                                tempBoolean = Convert.ToInt64(operand1Value) >= Convert.ToInt64(operand2.value);
                            }
                            else if (relationalSymbol.token == "==")
                            {
                                tempBoolean = Convert.ToInt64(operand1Value) == Convert.ToInt64(operand2.value);
                            }
                            else if (relationalSymbol.token == "!=")
                            {
                                tempBoolean = Convert.ToInt64(operand1Value) != Convert.ToInt64(operand2.value);
                            }

                            newToken.token = tempBoolean == true ? "verdadero" : "falso";
                        }
                        else if (operand2.type == "identifier" && operand2.identifierType == "integer_type")
                        {
                            if (relationalSymbol.token == "<")
                            {
                                tempBoolean = Convert.ToInt64(operand1Value) < Convert.ToInt64(operand2Value);
                            }
                            else if (relationalSymbol.token == ">")
                            {
                                tempBoolean = Convert.ToInt64(operand1Value) > Convert.ToInt64(operand2Value);
                            }
                            else if (relationalSymbol.token == "<=")
                            {
                                tempBoolean = Convert.ToInt64(operand1Value) <= Convert.ToInt64(operand2Value);
                            }
                            else if (relationalSymbol.token == ">=")
                            {
                                tempBoolean = Convert.ToInt64(operand1Value) >= Convert.ToInt64(operand2Value);
                            }
                            else if (relationalSymbol.token == "==")
                            {
                                tempBoolean = Convert.ToInt64(operand1Value) == Convert.ToInt64(operand2Value);
                            }
                            else if (relationalSymbol.token == "!=")
                            {
                                tempBoolean = Convert.ToInt64(operand1Value) != Convert.ToInt64(operand2Value);
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
                                tempBoolean = Convert.ToDouble(operand1Value) < Convert.ToDouble(operand2.value);
                            }
                            else if (relationalSymbol.token == ">")
                            {
                                tempBoolean = Convert.ToDouble(operand1Value) > Convert.ToDouble(operand2.value);
                            }
                            else if (relationalSymbol.token == "<=")
                            {
                                tempBoolean = Convert.ToDouble(operand1Value) <= Convert.ToDouble(operand2.value);
                            }
                            else if (relationalSymbol.token == ">=")
                            {
                                tempBoolean = Convert.ToDouble(operand1Value) >= Convert.ToDouble(operand2.value);
                            }
                            else if (relationalSymbol.token == "==")
                            {
                                tempBoolean = Convert.ToDouble(operand1Value) == Convert.ToDouble(operand2.value);
                            }
                            else if (relationalSymbol.token == "!=")
                            {
                                tempBoolean = Convert.ToDouble(operand1Value) != Convert.ToDouble(operand2.value);
                            }

                            newToken.token = tempBoolean == true ? "verdadero" : "falso";
                        }
                        else if (operand2.type == "identifier" && operand2.identifierType == "decimal_type")
                        {
                            if (relationalSymbol.token == "<")
                            {
                                tempBoolean = Convert.ToDouble(operand1Value) < Convert.ToDouble(operand2Value);
                            }
                            else if (relationalSymbol.token == ">")
                            {
                                tempBoolean = Convert.ToDouble(operand1Value) > Convert.ToDouble(operand2Value);
                            }
                            else if (relationalSymbol.token == "<=")
                            {
                                tempBoolean = Convert.ToDouble(operand1Value) <= Convert.ToDouble(operand2Value);
                            }
                            else if (relationalSymbol.token == ">=")
                            {
                                tempBoolean = Convert.ToDouble(operand1Value) >= Convert.ToDouble(operand2Value);
                            }
                            else if (relationalSymbol.token == "==")
                            {
                                tempBoolean = Convert.ToDouble(operand1Value) == Convert.ToDouble(operand2Value);
                            }
                            else if (relationalSymbol.token == "!=")
                            {
                                tempBoolean = Convert.ToDouble(operand1Value) != Convert.ToDouble(operand2Value);
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
                        if (operand2.type == "string_value" /*|| (operand2.type == "identifier" && operand2.identifierType == "string_type")*/)
                        {
                            if (relationalSymbol.token == "==")
                            {
                                tempBoolean = operand1Value == operand2.value;
                            }
                            else if (relationalSymbol.token == "!=")
                            {
                                tempBoolean = operand1Value != operand2.value;
                            }

                            newToken.token = tempBoolean == true ? "verdadero" : "falso";
                        }
                        else if (operand2.type == "identifier" && operand2.identifierType == "string_type")
                        {
                            if (relationalSymbol.token == "==")
                            {
                                tempBoolean = operand1Value == operand2Value;
                            }
                            else if (relationalSymbol.token == "!=")
                            {
                                tempBoolean = operand1Value != operand2Value;
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
                        if (operand2.type == "character_value" /*|| (operand2.type == "identifier" && operand2.identifierType == "character_type")*/)
                        {
                            if (relationalSymbol.token == "==")
                            {
                                tempBoolean = operand1Value == operand2.value;
                            }
                            else if (relationalSymbol.token == "!=")
                            {
                                tempBoolean = operand1Value != operand2.value;
                            }

                            newToken.token = tempBoolean == true ? "verdadero" : "falso";
                        }
                        else if (operand2.type == "identifier" && operand2.identifierType == "character_type")
                        {
                            if (relationalSymbol.token == "==")
                            {
                                tempBoolean = operand1Value == operand2Value;
                            }
                            else if (relationalSymbol.token == "!=")
                            {
                                tempBoolean = operand1Value != operand2Value;
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
                        if (operand2.type == "boolean_value" /*|| (operand2.type == "identifier" && operand2.identifierType == "boolean_type")*/)
                        {
                            bool operand1Boolean = operand1Value == "verdadero" ? true : false;
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
                        else if (operand2.type == "identifier" && operand2.identifierType == "boolean_type")
                        {
                            bool operand1Boolean = operand1Value == "verdadero" ? true : false;
                            bool operand2Boolean = operand2Value == "verdadero" ? true : false;

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

            newToken.value = newToken.token;

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
                                    _testNode.token.value = _testNode.token.token;
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
                                _testNode.token.value = _testNode.token.token;
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
                                    _testNode.token.value = _testNode.token.token;
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
                                _testNode.token.value = _testNode.token.token;
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

        //**************************** OPERATION METHODS**************************************
        //**********************************************************************************

        private Token doMathematicalOperation(List<Token> _tokens)
        {
            Token result = new Token();

            if (_tokens.Count > 0 && _tokens.Count < 4)
            {
                // To assure that our identifiers have the correct value
                for (int i = 0; i < myIdentifiers.Count; i++)
                {
                    if (_tokens[0].token == myIdentifiers[i].token)
                    {
                        _tokens[0].value = myIdentifiers[i].value;
                    }

                    if (_tokens[2].token == myIdentifiers[i].token)
                    {
                        _tokens[2].value = myIdentifiers[i].value;
                    }
                }

                if ((_tokens[0].type == "integer_value" || _tokens[0].identifierType == "integer_type") && (_tokens[2].type == "integer_value" || _tokens[2].identifierType == "integer_type"))
                {
                    result.type = _tokens[0].type;
                    result.identifierType = _tokens[0].identifierType;
                    string value = "";
                    if (_tokens[1].token == "+")
                    {
                        value = Convert.ToString(Convert.ToInt64(_tokens[0].value) + Convert.ToInt64(_tokens[2].value));
                        result.value = value;
                    }
                    else if (_tokens[1].token == "-")
                    {
                        value = Convert.ToString(Convert.ToInt64(_tokens[0].value) - Convert.ToInt64(_tokens[2].value));
                        result.value = value;
                    }
                    if (_tokens[1].token == "*")
                    {
                        value = Convert.ToString(Convert.ToInt64(_tokens[0].value) * Convert.ToInt64(_tokens[2].value));
                        result.value = value;
                    }
                    if (_tokens[1].token == "/")
                    {
                        value = Convert.ToString(Convert.ToInt64(_tokens[0].value) / Convert.ToInt64(_tokens[2].value));
                        result.value = value;
                    }
                }
                else if ((_tokens[0].type == "decimal_value" || _tokens[0].identifierType == "decimal_type") && (_tokens[2].type == "decimal_value" || _tokens[2].identifierType == "decimal_type"))
                {
                    result.type = _tokens[0].type;
                    result.identifierType = _tokens[0].identifierType;
                    string value = "";
                    if (_tokens[1].token == "+")
                    {
                        value = Convert.ToString(Convert.ToDouble(_tokens[0].value) + Convert.ToDouble(_tokens[2].value));
                        result.value = value;
                    }
                    else if (_tokens[1].token == "-")
                    {
                        value = Convert.ToString(Convert.ToDouble(_tokens[0].value) - Convert.ToDouble(_tokens[2].value));
                        result.value = value;
                    }
                    if (_tokens[1].token == "*")
                    {
                        value = Convert.ToString(Convert.ToDouble(_tokens[0].value) * Convert.ToDouble(_tokens[2].value));
                        result.value = value;
                    }
                    if (_tokens[1].token == "/")
                    {
                        value = Convert.ToString(Convert.ToDouble(_tokens[0].value) / Convert.ToDouble(_tokens[2].value));
                        result.value = value;
                    }
                }
                else if ((_tokens[0].type == "string_value" || _tokens[0].identifierType == "string_type") && (_tokens[2].type == "string_value" || _tokens[2].identifierType == "string_type"))
                {
                    result.type = _tokens[0].type;
                    string value = "";
                    if (_tokens[1].token == "+")
                    {
                        value = _tokens[0].value + _tokens[2].value;
                        result.value = value;
                    }
                }
                else if ((_tokens[0].type == "character_value" || _tokens[0].identifierType == "character_type") && (_tokens[2].type == "character_value" || _tokens[2].identifierType == "character_type"))
                {
                    result.type = _tokens[0].type;
                    string value = "";
                    if (_tokens[1].token == "+")
                    {
                        value = _tokens[0].value + _tokens[2].value;
                        result.value = value;
                    }
                }
            }
            else
            {
                result.type = _tokens[0].type;
                if (_tokens[0].type == "integer_value")
                {
                    result.token = "0";
                    result.value = "0";
                }
                else if (_tokens[0].type == "decimal_value")
                {
                    result.token = "0.0";
                    result.value = "0.0";
                }
                else if (_tokens[0].type == "string_value")
                {
                    result.token = "";
                    result.value = "";
                }
                else if (_tokens[0].type == "character_value")
                {
                    result.token = "''";
                    result.value = "''";
                }
                else if (_tokens[0].type == "boolean_value")
                {
                    result.token = "verdadero";
                    result.value = "verdadero";
                }
                else if (_tokens[0].type == "identifier")
                {
                    if (_tokens[0].identifierType == "integer_type")
                    {
                        result.token = "0";
                        result.value = "0";
                    }
                    else if (_tokens[0].identifierType == "decimal_type")
                    {
                        result.token = "0.0";
                        result.value = "0.0";
                    }
                    else if (_tokens[0].identifierType == "string_type")
                    {
                        result.token = "";
                        result.value = "";
                    }
                    else if (_tokens[0].identifierType == "character_type")
                    {
                        result.token = "";
                        result.value = "";
                    }
                    else if (_tokens[0].identifierType == "boolean_type")
                    {
                        result.token = "verdadero";
                        result.value = "verdadero";
                    }
                }
            }

            return result;
        }

        //**************************** OPERATION METHODS**************************************
        //**********************************************************************************

        //**********************
        public void populateMyNTree(Token[] myTokens)
        {
            int treeLevel = 0;
            string tokenType = "";
            List<Token> tokenList = new List<Token>();
            NNode myNNode = new NNode("", tokenList);
            bool booleanSwitch = true;

            myNTree = new NTree();

            for (int i = 0; i < myTokens.Length; i++)
            {
                if (booleanSwitch == true)
                {
                    if (myTokens[i].token == "principal" || myTokens[i].token == "escribir" || myTokens[i].token == "leer" || myTokens[i].token == "SI" || myTokens[i].token == "SINO" || myTokens[i].token == "SINO_SI" || myTokens[i].token == "PARA" || myTokens[i].token == "MIENTRAS" || myTokens[i].token == "HACER" || myTokens[i].token == "EN_CASO" || myTokens[i].token == "CASO:" || myTokens[i].token == "OTRO CASO:")
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
                            tokenType = "while";
                        }
                        else if (myTokens[i].token == "HACER")
                        {
                            tokenType = "do_while";
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
                        else if (myTokens[i].token == "leer")
                        {
                            tokenType = "method";
                        }
                        else if (myTokens[i].token == "escribir")
                        {
                            tokenType = "method";
                        }
                        else if (myTokens[i].token == "principal")
                        {
                            tokenType = "method";
                        }
                    }
                    else if (myTokens[i].type == "integer_type" || myTokens[i].type == "decimal_type" || myTokens[i].type == "string_type" || myTokens[i].type == "character_type" || myTokens[i].type == "boolean_type" || myTokens[i].type == "identifier")
                    {
                        tokenType = myTokens[i].type;
                    }

                    // Needed to deativate the switch
                    // We need to test only the first words that appears
                    booleanSwitch = false;
                }



                if (myTokens[i].token == "{")
                {
                    treeLevel++;
                    myNNode = myNTree.append(myNNode, tokenType, tokenList);
                    tokenList.Clear();
                    booleanSwitch = true;
                }
                else if (myTokens[i].token == "}")
                {
                    treeLevel--;
                    tokenType = "";
                    myNNode = myNNode.previous;
                    tokenList.Clear();
                    booleanSwitch = true;
                }
                else if (myTokens[i].token == ";")
                {
                    myNNode = myNTree.append(myNNode, tokenType, tokenList);
                    myNNode = myNNode.previous;
                    tokenList.Clear();
                    booleanSwitch = true;
                }
                else
                {
                    // This adds every token in the commands
                    tokenList.Add(myTokens[i]);
                }
            }
        }

        //**********************
        public void executeCommand(NNode _myNNode)
        {
            // If is a type declaration: entero _a = 3;
            if (_myNNode.type == "integer_type" || _myNNode.type == "decimal_type" || _myNNode.type == "string_type" || _myNNode.type == "character_type" || _myNNode.type == "boolean_type")
            {
                bool assignationSignExists = false;
                bool operationSignExists = false;

                // To know if there is an equal sign or operation sign
                for (int i = 0; i < _myNNode.command.Count; i++)
                {
                    if (_myNNode.command[i].token == "=" || _myNNode.command[i].token == "+" || _myNNode.command[i].token == "-" || _myNNode.command[i].token == "*" || _myNNode.command[i].token == "/" || _myNNode.command[i].token == "||" || _myNNode.command[i].token == "&&")
                    {
                        if (_myNNode.command[i].token == "=")
                        {
                            assignationSignExists = true;
                        }
                        else if (_myNNode.command[i].token == "+" || _myNNode.command[i].token == "-" || _myNNode.command[i].token == "*" || _myNNode.command[i].token == "/" || _myNNode.command[i].token == "||" || _myNNode.command[i].token == "&&")
                        {
                            operationSignExists = true;
                        }
                    }
                }

                if (assignationSignExists == true)
                {
                    if (operationSignExists == false)
                    {
                        // To search for identifiers and values in command expression of tokens
                        List<Token> identifiers = new List<Token>();
                        List<Token> values = new List<Token>();
                        for (int i = 0; i < _myNNode.command.Count; i++)
                        {
                            if (_myNNode.command[i].type == "identifier")
                            {
                                // We can have many identifiers in a row: _a, _b, _c
                                identifiers.Add(_myNNode.command[i]);
                            }

                            if (_myNNode.command[i].type == "integer_value" || _myNNode.command[i].type == "decimal_value" || _myNNode.command[i].type == "string_value" || _myNNode.command[i].type == "character_value" || _myNNode.command[i].type == "boolean_value")
                            {
                                // We can have many values in a row: 1, 2
                                values.Add(_myNNode.command[i]);
                            }
                        }

                        // To assign values to the global myIdentifiers array
                        // The number of values is the limit, so: entero _a, _b, _c = 1, 2  -> _a = 1, _b= 2, _c = 0
                        for (int i = 0; i < values.Count; i++)
                        {
                            for (int j = 0; j < myIdentifiers.Count; j++)
                            {
                                if (identifiers[i].token == myIdentifiers[j].token)
                                {
                                    myIdentifiers[j].value = values[i].token;
                                }
                            }
                        }

                        identifiers = null;
                        values = null;
                    }
                    else
                    {
                        // To search for identifiers and values in command expression of tokens
                        List<Token> identifiers = new List<Token>();
                        List<Token> operation = new List<Token>();
                        bool assignation = false;
                        bool booleanOperation = false;

                        for (int i = 0; i < _myNNode.command.Count; i++)
                        {
                            if (_myNNode.command[i].token == "=")
                            {
                                assignation = true;
                                continue;
                            }

                            if (assignation == false)
                            {
                                if (_myNNode.command[i].type == "identifier")
                                {
                                    // We can have many identifiers in a row: _a, _b, _c
                                    identifiers.Add(_myNNode.command[i]);
                                }
                            }
                            else
                            {
                                if (_myNNode.command[i].type == "integer_value" || _myNNode.command[i].type == "decimal_value" || _myNNode.command[i].type == "string_value" || _myNNode.command[i].type == "character_value" || _myNNode.command[i].type == "boolean_value" || _myNNode.command[i].type == "identifier" || _myNNode.command[i].token == "+" || _myNNode.command[i].token == "-" || _myNNode.command[i].token == "*" || _myNNode.command[i].token == "/" || _myNNode.command[i].token == "||" || _myNNode.command[i].token == "&&" || _myNNode.command[i].token == "(" || _myNNode.command[i].token == ")" || _myNNode.command[i].token == "!" || _myNNode.command[i].token == "==" || _myNNode.command[i].token == "!=")
                                {
                                    // We can have many values in a row: 1, 2
                                    operation.Add(_myNNode.command[i]);

                                    if (_myNNode.command[i].type == "boolean_value" || _myNNode.command[i].identifierType == "boolean_type" || _myNNode.command[i].token == "||" || _myNNode.command[i].token == "&&" || _myNNode.command[i].token == "(" || _myNNode.command[i].token == ")" || _myNNode.command[i].token == "!" || _myNNode.command[i].token == "==" || _myNNode.command[i].token == "!=")
                                    {
                                        booleanOperation = true;
                                    }
                                }
                            }
                        }

                        // To assign values to the global myIdentifiers array
                        // The number of values is the limit, so: entero _a = 1 + _a;
                        if (identifiers.Count > 0)
                        {
                            Token tempToken = new Token();
                            
                            if (booleanOperation == true)
                            {
                                Token[] myOperands = new Token[operation.Count];
                                
                                for (int j = 0; j < myOperands.Length; j++)
                                {
                                    myOperands[j] = operation[j];
                                }

                                tempToken.token = evaluateBooleanExpression(myOperands) == true ? "verdadero" : "falso";
                                tempToken.value = tempToken.token;
                                tempToken.type = "boolean_value";
                            }
                            else
                            {
                                tempToken = doMathematicalOperation(operation);
                            }

                            for (int j = 0; j < myIdentifiers.Count; j++)
                            {
                                if (identifiers[0].token == myIdentifiers[j].token)
                                {
                                    myIdentifiers[j].value = tempToken.value;
                                }
                            }
                        }

                        identifiers = null;
                        operation = null;
                    }
                    
                }
            }
            // If is an identifier: _a = 3;
            else if (_myNNode.type == "identifier")
            {
                bool assignationSignExists = false;
                bool operationSignExists = false;

                // To know if there is an equal sign or operation sign
                for (int i = 0; i < _myNNode.command.Count; i++)
                {
                    if (_myNNode.command[i].token == "=" || _myNNode.command[i].token == "+" || _myNNode.command[i].token == "-" || _myNNode.command[i].token == "*" || _myNNode.command[i].token == "/" || _myNNode.command[i].token == "||" || _myNNode.command[i].token == "&&")
                    {
                        if (_myNNode.command[i].token == "=")
                        {
                            assignationSignExists = true;
                        }
                        else if (_myNNode.command[i].token == "+" || _myNNode.command[i].token == "-" || _myNNode.command[i].token == "*" || _myNNode.command[i].token == "/" || _myNNode.command[i].token == "||" || _myNNode.command[i].token == "&&")
                        {
                            operationSignExists = true;
                        }
                    }
                }

                if (assignationSignExists == true)
                {
                    if (operationSignExists == false)
                    {
                        // To search for identifiers and values in command expression of tokens
                        List<Token> identifiers = new List<Token>();
                        List<Token> values = new List<Token>();
                        for (int i = 0; i < _myNNode.command.Count; i++)
                        {
                            if (_myNNode.command[i].type == "identifier")
                            {
                                // We can have many identifiers in a row: _a, _b, _c
                                identifiers.Add(_myNNode.command[i]);
                            }

                            if (_myNNode.command[i].type == "integer_value" || _myNNode.command[i].type == "decimal_value" || _myNNode.command[i].type == "string_value" || _myNNode.command[i].type == "character_value" || _myNNode.command[i].type == "boolean_value")
                            {
                                // We can have many values in a row: 1, 2
                                values.Add(_myNNode.command[i]);
                            }
                        }

                        // To assign values to the global myIdentifiers array
                        // The number of values is the limit, so: _a, _b, _c = 1, 2  -> _a = 1, _b= 2, _c = 0
                        for (int i = 0; i < values.Count; i++)
                        {
                            for (int j = 0; j < myIdentifiers.Count; j++)
                            {
                                if (identifiers[i].token == myIdentifiers[j].token)
                                {
                                    myIdentifiers[j].value = values[i].token;
                                }
                            }
                        }

                        identifiers = null;
                        values = null;
                    }
                    else
                    {
                        // To search for identifiers and values in command expression of tokens
                        List<Token> identifiers = new List<Token>();
                        List<Token> operation = new List<Token>();
                        bool assignation = false;
                        bool booleanOperation = false;

                        for (int i = 0; i < _myNNode.command.Count; i++)
                        {
                            if (_myNNode.command[i].token == "=")
                            {
                                assignation = true;
                                continue;
                            }

                            if (assignation == false)
                            {
                                if (_myNNode.command[i].type == "identifier")
                                {
                                    // We can have many identifiers in a row: _a, _b, _c
                                    identifiers.Add(_myNNode.command[i]);
                                }
                            }
                            else
                            {
                                if (_myNNode.command[i].type == "integer_value" || _myNNode.command[i].type == "decimal_value" || _myNNode.command[i].type == "string_value" || _myNNode.command[i].type == "character_value" || _myNNode.command[i].type == "boolean_value" || _myNNode.command[i].type == "identifier" || _myNNode.command[i].token == "+" || _myNNode.command[i].token == "-" || _myNNode.command[i].token == "*" || _myNNode.command[i].token == "/" || _myNNode.command[i].token == "||" || _myNNode.command[i].token == "&&" || _myNNode.command[i].token == "(" || _myNNode.command[i].token == ")" || _myNNode.command[i].token == "!" || _myNNode.command[i].token == "==" || _myNNode.command[i].token == "!=")
                                {
                                    // We can have many values in a row: 1, 2
                                    operation.Add(_myNNode.command[i]);

                                    if (_myNNode.command[i].type == "boolean_value" || _myNNode.command[i].identifierType == "boolean_type" || _myNNode.command[i].token == "||" || _myNNode.command[i].token == "&&" || _myNNode.command[i].token == "(" || _myNNode.command[i].token == ")" || _myNNode.command[i].token == "!" || _myNNode.command[i].token == "==" || _myNNode.command[i].token == "!=")
                                    {
                                        booleanOperation = true;
                                    }
                                }
                            }
                        }

                        // To assign values to the global myIdentifiers array
                        // The number of values is the limit, so: entero _a = 1 + _a;
                        if (identifiers.Count > 0)
                        {
                            Token tempToken = new Token();
                            if (booleanOperation == true)
                            {
                                Token[] myOperands = new Token[operation.Count];

                                for (int j = 0; j < myOperands.Length; j++)
                                {
                                    myOperands[j] = operation[j];
                                }

                                tempToken.token = evaluateBooleanExpression(myOperands) == true ? "verdadero" : "falso";
                                tempToken.value = tempToken.token;
                                tempToken.type = "boolean_value";
                            }
                            else
                            {
                                tempToken = doMathematicalOperation(operation);
                            }

                            for (int j = 0; j < myIdentifiers.Count; j++)
                            {
                                if (identifiers[0].token == myIdentifiers[j].token)
                                {
                                    myIdentifiers[j].value = tempToken.value;
                                }
                            }
                        }

                        identifiers = null;
                        operation = null;
                    }
                }
                else if (_myNNode.command[0].identifierType == "integer_type")
                {
                    // This is the traditional identifier++ an increment
                    if (_myNNode.command.Count > 1)
                    {
                        if (_myNNode.command[1].token == "++")
                        {
                            for (int i = 0; i < myIdentifiers.Count; i++)
                            {
                                if (_myNNode.command[0].token == myIdentifiers[i].token)
                                {
                                    myIdentifiers[i].value = Convert.ToString(Convert.ToInt64(myIdentifiers[i].value) + 1);
                                    break;
                                }
                            }
                        }
                        // This is the traditional identifier-- a decrement
                        else if (_myNNode.command[1].token == "--")
                        {
                            for (int i = 0; i < myIdentifiers.Count; i++)
                            {
                                if (_myNNode.command[0].token == myIdentifiers[i].token)
                                {
                                    myIdentifiers[i].value = Convert.ToString(Convert.ToInt64(myIdentifiers[i].value) - 1);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            else if (_myNNode.type == "method")
            {
                if (_myNNode.command[0].token == "escribir")
                {
                    List<Token> tokens = new List<Token>();
                    bool open = false;
                    string value = "";

                    // To make a list of enclosed tokens between "(" and ")"                    
                    for (int i = 0; i < _myNNode.command.Count; i++)
                    {
                        if (_myNNode.command[i].token == ")")
                        {
                            open = false;
                        }

                        if (open == true)
                        {
                            if (_myNNode.command[i].type == "integer_value" || _myNNode.command[i].type == "decimal_value" || _myNNode.command[i].type == "string_value" || _myNNode.command[i].type == "character_value" || _myNNode.command[i].type == "boolean_value")
                            {
                                tokens.Add(_myNNode.command[i]);
                            }

                            if (_myNNode.command[i].type == "identifier")
                            {
                                for (int j = 0; j < myIdentifiers.Count; j++)
                                {
                                    if (myIdentifiers[j].token == _myNNode.command[i].token)
                                    {
                                        tokens.Add(myIdentifiers[j]);
                                    }
                                }
                            }
                        }

                        if (_myNNode.command[i].token == "(")
                        {
                            open = true;
                        }
                    }

                    // To concatenate every token in tokens List
                    for (int i = 0; i < tokens.Count; i++)
                    {
                        if (tokens[i].type == "string_type" || tokens[i].type == "character_type" || tokens[i].identifierType == "string_type" || tokens[i].identifierType == "character_type")
                        {
                            // We do not want " characters in our final string ("hola")
                            string newValue = "";
                            if (tokens[i].value.Length > 0)
                            {
                                for (int j = 0; j < tokens[i].value.Length; j++)
                                {
                                    if (tokens[i].value[j] != '\"' && tokens[i].value[j] != '"' && tokens[i].value[j] != '\'')
                                    {
                                        //newValue += Convert.ToString(tokens[i].value[j]);
                                        newValue += tokens[i].value[j].ToString();
                                    }
                                }
                            }

                            value += newValue;
                        }
                        else
                        {
                            value += tokens[i].value;
                        }
                    }

                    // We write the result in the global LogFinal
                    LogFinal += value + "\n";
                }
            }
        }

        public void EXECUTE_COMMANDS()
        {
            // A recursive function
            void runTree(NNode _myNNode)
            {
                if (_myNNode == null)
                {
                    return;
                }

                if (_myNNode == myNTree.firstNNode)
                {
                    // Do something here
                    
                }

                if (_myNNode.NextNNodes.Count != 0)
                {
                    // For loop ********
                    bool forLoopActivated = false;
                    List<int> counterFromTo = new List<int>();
                    bool ascending = false;
                    bool descending = false;
                    int step = 1;
                    int index = 0;
                    bool DontQuit = true;
                    // *****************

                    bool else_Expression = false;
                    bool else_if_Expression = false;
                    bool switch_Expression = false;
                    bool switch_default_Expression = false;
                    for (int i = 0; i < _myNNode.NextNNodes.Count; i++)
                    {
                        // Do something here

                        // IF EXPRESSION
                        if (_myNNode.NextNNodes[i].type == "if")
                        {
                            List<Token> booleanExpression = new List<Token>();
                            bool openParenthesis = false;

                            // To fill the booleanExpression List
                            for (int j = 0; j < _myNNode.NextNNodes[i].command.Count; j++)
                            {
                                if (_myNNode.NextNNodes[i].command[j].token == "(")
                                {
                                    openParenthesis = true;
                                }

                                if (openParenthesis == true)
                                {
                                    booleanExpression.Add(_myNNode.NextNNodes[i].command[j]);
                                }

                                if (_myNNode.NextNNodes[i].command[j].token == ")")
                                {
                                    openParenthesis = false;
                                }
                            }

                            // We convert the booleanExpression to array booleanExpressionArray
                            Token[] booleanExpressionArray = new Token[booleanExpression.Count];
                            for (int j = 0; j < booleanExpressionArray.Length; j++)
                            {
                                booleanExpressionArray[j] = booleanExpression[j];
                            }

                            if (evaluateBooleanExpression(booleanExpressionArray) == true)
                            {
                                booleanExpression = null;
                                booleanExpressionArray = null;
                                else_Expression = false;
                                else_if_Expression = false;

                                runTree(_myNNode.NextNNodes[i]);
                            }
                            else
                            {
                                else_Expression = true;
                                else_if_Expression = true;
                            }
                        }
                        // ELSE_IF EXPRESSION
                        else if (_myNNode.NextNNodes[i].type == "else_if")
                        {
                            if (else_if_Expression == true)
                            {
                                List<Token> booleanExpression = new List<Token>();
                                bool openParenthesis = false;

                                // To fill the booleanExpression List
                                for (int j = 0; j < _myNNode.NextNNodes[i].command.Count; j++)
                                {
                                    if (_myNNode.NextNNodes[i].command[j].token == "(")
                                    {
                                        openParenthesis = true;
                                    }

                                    if (openParenthesis == true)
                                    {
                                        booleanExpression.Add(_myNNode.NextNNodes[i].command[j]);
                                    }

                                    if (_myNNode.NextNNodes[i].command[j].token == ")")
                                    {
                                        openParenthesis = false;
                                    }
                                }

                                // We convert the booleanExpression to array booleanExpressionArray
                                Token[] booleanExpressionArray = new Token[booleanExpression.Count];
                                for (int j = 0; j < booleanExpressionArray.Length; j++)
                                {
                                    booleanExpressionArray[j] = booleanExpression[j];
                                }

                                if (evaluateBooleanExpression(booleanExpressionArray) == true)
                                {
                                    booleanExpression = null;
                                    booleanExpressionArray = null;
                                    else_Expression = false;
                                    else_if_Expression = false;

                                    runTree(_myNNode.NextNNodes[i]);
                                }
                                else
                                {
                                    else_Expression = true;
                                    else_if_Expression = true;
                                }
                            }
                        }
                        // ELSE EXPRESSION
                        else if (_myNNode.NextNNodes[i].type == "else")
                        {
                            if (else_Expression == true)
                            {
                                else_Expression = false;
                                else_if_Expression = false;

                                runTree(_myNNode.NextNNodes[i]);
                            }
                        }
                        // FOR EXPRESSION
                        else if (_myNNode.NextNNodes[i].type == "for")
                        {
                            int integers = 0;
                            for (int j = 0; j < _myNNode.NextNNodes[i].command.Count; j++)
                            {
                                if (_myNNode.NextNNodes[i].command[j].type == "integer_value" && integers < 3)
                                {
                                    counterFromTo.Add(Convert.ToInt32(_myNNode.NextNNodes[i].command[j].value));
                                    integers++;
                                }
                            }

                            if (counterFromTo.Count == 2)
                            {
                                if (counterFromTo[0] < counterFromTo[1])
                                {
                                    for (int j = counterFromTo[0]; j <= counterFromTo[1]; j++)
                                    {
                                        runTree(_myNNode.NextNNodes[i]);
                                    }
                                }
                                else if (counterFromTo[0] > counterFromTo[1])
                                {
                                    for (int j = counterFromTo[0]; j >= counterFromTo[1]; j--)
                                    {
                                        runTree(_myNNode.NextNNodes[i]);
                                    }
                                }
                                else if (counterFromTo[0] == counterFromTo[1])
                                {
                                    runTree(_myNNode.NextNNodes[i]);
                                }
                            }
                            else if (counterFromTo.Count == 3)
                            {
                                step = counterFromTo[2];

                                if (counterFromTo[0] < counterFromTo[1])
                                {
                                    for (int j = counterFromTo[0]; j <= counterFromTo[1]; j += step)
                                    {
                                        runTree(_myNNode.NextNNodes[i]);
                                    }
                                }
                                else if (counterFromTo[0] > counterFromTo[1])
                                {
                                    for (int j = counterFromTo[0]; j >= counterFromTo[1]; j -= step)
                                    {
                                        runTree(_myNNode.NextNNodes[i]);
                                    }
                                }
                                else if (counterFromTo[0] == counterFromTo[1])
                                {
                                    runTree(_myNNode.NextNNodes[i]);
                                }
                            }

                            counterFromTo = new List<int>();
                        }
                        // WHILE EXPRESSION
                        else if (_myNNode.NextNNodes[i].type == "while")
                        {
                            List<Token> booleanExpression = new List<Token>();
                            bool openParenthesis = false;

                            // To fill the booleanExpression List
                            for (int j = 0; j < _myNNode.NextNNodes[i].command.Count; j++)
                            {
                                if (_myNNode.NextNNodes[i].command[j].token == "(")
                                {
                                    openParenthesis = true;
                                }

                                if (openParenthesis == true)
                                {
                                    booleanExpression.Add(_myNNode.NextNNodes[i].command[j]);
                                }

                                if (_myNNode.NextNNodes[i].command[j].token == ")")
                                {
                                    openParenthesis = false;
                                }
                            }

                            // We convert the booleanExpression to array booleanExpressionArray
                            Token[] booleanExpressionArray = new Token[booleanExpression.Count];
                            for (int j = 0; j < booleanExpressionArray.Length; j++)
                            {
                                booleanExpressionArray[j] = booleanExpression[j];
                            }

                            while (evaluateBooleanExpression(booleanExpressionArray) == true)
                            {
                                runTree(_myNNode.NextNNodes[i]);
                            }
                        }
                        // DO WHILE EXPRESSION
                        else if (_myNNode.NextNNodes[i].type == "do_while")
                        {
                            if (i + 1 < _myNNode.NextNNodes.Count)
                            {
                                if (_myNNode.NextNNodes[i + 1].type == "while")
                                {
                                    List<Token> booleanExpression = new List<Token>();
                                    bool openParenthesis = false;

                                    // To fill the booleanExpression List
                                    for (int j = 0; j < _myNNode.NextNNodes[i + 1].command.Count; j++)
                                    {
                                        if (_myNNode.NextNNodes[i + 1].command[j].token == "(")
                                        {
                                            openParenthesis = true;
                                        }

                                        if (openParenthesis == true)
                                        {
                                            booleanExpression.Add(_myNNode.NextNNodes[i + 1].command[j]);
                                        }

                                        if (_myNNode.NextNNodes[i + 1].command[j].token == ")")
                                        {
                                            openParenthesis = false;
                                        }
                                    }

                                    // We convert the booleanExpression to array booleanExpressionArray
                                    Token[] booleanExpressionArray = new Token[booleanExpression.Count];
                                    for (int j = 0; j < booleanExpressionArray.Length; j++)
                                    {
                                        booleanExpressionArray[j] = booleanExpression[j];
                                    }

                                    do
                                    {
                                        runTree(_myNNode.NextNNodes[i]);
                                    }
                                    while (evaluateBooleanExpression(booleanExpressionArray) == true);

                                    // To advance one unit avoiding the evaluation of the "while" estatement
                                    i++;
                                }
                            }
                        }
                        // SWITCH EXPRESSION
                        else if (_myNNode.NextNNodes[i].type == "switch")
                        {

                        }
                        // CASE EXPRESSION
                        else if (_myNNode.NextNNodes[i].type == "case")
                        {

                        }
                        // DEFAULT EXPRESSION
                        else if (_myNNode.NextNNodes[i].type == "default")
                        {

                        }
                        else
                        {
                            //LogFinal += _myNNode.NextNNodes[i].command[0].token + "\n";
                            executeCommand(_myNNode.NextNNodes[i]);

                            runTree(_myNNode.NextNNodes[i]);
                        }
                    }
                }
            }

            // To fill the tree of tokens
            populateMyNTree(arrayOfTokens);

            List<Token> myTokenList = new List<Token>();
            NNode myNNode = new NNode("", myTokenList);

            myNNode = myNTree.firstNNode;

            runTree(myNNode);

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
