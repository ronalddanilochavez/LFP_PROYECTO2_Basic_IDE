using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFP_PROYECTO2_Basic_IDE
{
    class StackAutomaton
    {
        public List<string> expression = new List<string>();
        public List<string> stack = new List<string>();
        public string[,] Table = new string[5, 7];
        private int index = 0;
        public string token = "";
        public string state = "setting";

        // Temp List
        List<string> myList = new List<string>();

        public StackAutomaton()
        {
            stack.Add("$");
            stack.Add("E");
        }

        //***********
        public void fillExpressionList (string myExpression)
        {
            for (int i = 0; i < myExpression.Length; i++)
            {
                string myNUMERO = "numero";
                string myID = "id";
                bool match = false;

                if (myExpression[i] == '+' || myExpression[i] == '*' || myExpression[i] == '(' || myExpression[i] == ')' || myExpression[i] == '$')
                {
                    expression.Add(Convert.ToString(myExpression[i]));
                }
                else if (myExpression[i] == 'n')
                {
                    if (i + 5 < myExpression.Length)
                    {
                        match = true;
                        for (int j = i; j <= i + 5; j++)
                        {
                            if (myExpression[j] != myNUMERO[j - i])
                            {
                                match = false;
                            }
                        }

                        if (match == true)
                        {
                            expression.Add("numero");
                            // To go to the next word
                            i += 5;
                        }
                    }
                }
                else if (myExpression[i] == 'i')
                {
                    if (i + 1 < myExpression.Length)
                    {
                        match = true;
                        for (int j = i; j <= i + 1; j++)
                        {
                            if (myExpression[j] != myID[j - i])
                            {
                                match = false;
                            }
                        }

                        if (match == true)
                        {
                            expression.Add("id");
                            // To go to the next word
                            i += 1;
                        }
                    }
                }
            }

            if (expression[expression.Count - 1] != "$")
            {
                expression.Add("$");
            }
        }

        //*************
        private string reverse(string word)
        {
            string result = "";

            for (int i = word.Length - 1; i >= 0; i--)
            {
                result += Convert.ToString(word[i]);
            }

            return result;
        }

        //*************
        private void addToStack(string word)
        {
            List<string> tempList = new List<string>();

            for (int i = 0; i < word.Length; i++)
            {
                string myNUMERO = "numero";
                string myID = "id";
                bool match = false;

                if (word[i] == 'E' || word[i] == 'F' || word[i] == 'T')
                {
                    if (i + 1 < word.Length)
                    {
                        if (word[i + 1] != '\'')
                        {
                            tempList.Add(Convert.ToString(word[i]));
                        }
                        else
                        {
                            tempList.Add(Convert.ToString(word[i]) + "\'");
                        }
                    }
                }
                else if (word[i] == '+' || word[i] == '*' || word[i] == '(' || word[i] == ')' || word[i] == 'e' || word[i] == '$')
                {
                    tempList.Add(Convert.ToString(word[i]));
                }
                else if (word[i] == 'n')
                {
                    if (i + 5 < word.Length)
                    {
                        match = true;
                        for (int j = i; j <= i + 5; j++)
                        {
                            if (word[j] != myNUMERO[j - i])
                            {
                                match = false;
                            }
                        }

                        if (match == true)
                        {
                            tempList.Add("numero");
                            // To go to the next word
                            i += 5;

                            match = false;
                        }
                    }
                }
                else if (word[i] == 'i')
                {
                    if (i + 1 < word.Length)
                    {
                        match = true;
                        for (int j = i; j <= i + 1; j++)
                        {
                            if (word[j] != myID[j - i])
                            {
                                match = false;
                            }
                        }

                        if (match == true)
                        {
                            tempList.Add("id");
                            // To go to the next word
                            i += 1;

                            match = false;
                        }
                    }
                }
            }

            // To reverse the result
            myList = new List<string>();
            for (int i = tempList.Count - 1; i >= 0; i--)
            {
                myList.Add(tempList[i]);
            }

            // Adding to stack
            for (int i = 0; i < myList.Count; i++)
            {
                stack.Add(myList[i]);
            }
        }

        //*************

        public void start()
        {
            stack = new List<string>();

            stack.Add("$");
            stack.Add("E");

            index = 0;
            token = expression[index];
            state = "start";
        }

        //*************
        public void step()
        {
            if (state == "acceptation")
            {
                state = "setting";
            }
            else if (state == "setting")
            {
                state = "start";
            }
            else if (stack.Count != 0)
            {
                // To make a reduce
                if (stack[stack.Count - 1] == token || stack[stack.Count - 1] == "e")
                {
                    // The "token" is removed and we call for next "token"
                    if (stack[stack.Count - 1] == token)
                    {
                        stack.RemoveAt(stack.Count - 1);
                        state = "reduce";

                        if (stack.Count == 0)
                        {
                            state = "acceptation";
                            token = "";
                        }
                        else
                        {
                            if (index + 1 < expression.Count)
                            {
                                index++;
                                token = expression[index];
                            }
                        }
                    }
                    // The "e" only is removed
                    else if (stack[stack.Count - 1] == "e")
                    {
                        stack.RemoveAt(stack.Count - 1);
                        state = "reduce";

                        if (stack.Count == 0)
                        {
                            state = "acceptation";
                            token = "";
                        }
                    }
                }
                // To make a shift
                else
                {
                    state = "shift";

                    if (stack[stack.Count - 1] == "E")
                    {
                        stack.RemoveAt(stack.Count - 1);

                        if (token == "+")
                        {
                            addToStack(Table[0, 0]);
                        }
                        else if (token == "*")
                        {
                            addToStack(Table[0, 1]);
                        }
                        else if (token == "(")
                        {
                            addToStack(Table[0, 2]);
                        }
                        else if (token == ")")
                        {
                            addToStack(Table[0, 3]);
                        }
                        else if (token == "numero")
                        {
                            addToStack(Table[0, 4]);
                        }
                        else if (token == "id")
                        {
                            addToStack(Table[0, 5]);
                        }
                        else if (token == "$")
                        {
                            addToStack(Table[0, 6]);
                        }
                    }
                    else if (stack[stack.Count - 1] == "E'")
                    {
                        stack.RemoveAt(stack.Count - 1);

                        if (token == "+")
                        {
                            addToStack(Table[1, 0]);
                        }
                        else if (token == "*")
                        {
                            addToStack(Table[1, 1]);
                        }
                        else if (token == "(")
                        {
                            addToStack(Table[1, 2]);
                        }
                        else if (token == ")")
                        {
                            addToStack(Table[1, 3]);
                        }
                        else if (token == "numero")
                        {
                            addToStack(Table[1, 4]);
                        }
                        else if (token == "id")
                        {
                            addToStack(Table[1, 5]);
                        }
                        else if (token == "$")
                        {
                            addToStack(Table[1, 6]);
                        }
                    }
                    else if (stack[stack.Count - 1] == "T")
                    {
                        stack.RemoveAt(stack.Count - 1);

                        if (token == "+")
                        {
                            addToStack(Table[2, 0]);
                        }
                        else if (token == "*")
                        {
                            addToStack(Table[2, 1]);
                        }
                        else if (token == "(")
                        {
                            addToStack(Table[2, 2]);
                        }
                        else if (token == ")")
                        {
                            addToStack(Table[2, 3]);
                        }
                        else if (token == "numero")
                        {
                            addToStack(Table[2, 4]);
                        }
                        else if (token == "id")
                        {
                            addToStack(Table[2, 5]);
                        }
                        else if (token == "$")
                        {
                            addToStack(Table[2, 6]);
                        }
                    }
                    else if (stack[stack.Count - 1] == "T'")
                    {
                        stack.RemoveAt(stack.Count - 1);

                        if (token == "+")
                        {
                            addToStack(Table[3, 0]);
                        }
                        else if (token == "*")
                        {
                            addToStack(Table[3, 1]);
                        }
                        else if (token == "(")
                        {
                            addToStack(Table[3, 2]);
                        }
                        else if (token == ")")
                        {
                            addToStack(Table[3, 3]);
                        }
                        else if (token == "numero")
                        {
                            addToStack(Table[3, 4]);
                        }
                        else if (token == "id")
                        {
                            addToStack(Table[3, 5]);
                        }
                        else if (token == "$")
                        {
                            addToStack(Table[3, 6]);
                        }
                    }
                    else if (stack[stack.Count - 1] == "F")
                    {
                        stack.RemoveAt(stack.Count - 1);

                        if (token == "+")
                        {
                            addToStack(Table[4, 0]);
                        }
                        else if (token == "*")
                        {
                            addToStack(Table[4, 1]);
                        }
                        else if (token == "(")
                        {
                            addToStack(Table[4, 2]);
                        }
                        else if (token == ")")
                        {
                            addToStack(Table[4, 3]);
                        }
                        else if (token == "numero")
                        {
                            addToStack(Table[4, 4]);
                        }
                        else if (token == "id")
                        {
                            addToStack(Table[4, 5]);
                        }
                        else if (token == "$")
                        {
                            addToStack(Table[4, 6]);
                        }
                    }
                }
            }
        }
    }
}
