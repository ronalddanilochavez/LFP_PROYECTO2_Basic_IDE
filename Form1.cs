﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LFP_PROYECTO2_Basic_IDE
{
    public partial class Form1 : Form
    {
        private IDE myIDE = new IDE();
        private Lexer myLexer = new Lexer();
        private Parser myParser = new Parser();
        private File myFile = new File();
        private Lexer.Token myToken = new Lexer.Token();
        private List<Lexer.Token> tokenList = new List<Lexer.Token>();

        private Automaton myAutomaton = new Automaton();
        private string[] myStates = { "Q0", "Q1", "Q2" };
        private string[] myAlphabet = { "a", "b" };
        private string[,] myTransitionFunction = { { "Q1", "Q2" }, { "Q1", "Q2" }, { "Q1", "Q2" } };
        private string myInitialState = "Q0";
        private string[] myFinalStates = { "Q1" };

        private string actualState = "Q0";

        // fileProject[0] is the project's file and fileProject[1] is the myIDE's file
        private string[] fileProject = new string[2];

        private int letterPos = 0;

        //***********************************************
        public Form1()
        {
            InitializeComponent();

            // To initialize the automaton object
            myAutomaton.states = new string[3];
            myAutomaton.states[0] = "Q0";
            myAutomaton.states[1] = "Q1";
            myAutomaton.states[2] = "Q2";
            myAutomaton.alphabet = new string[2];
            myAutomaton.alphabet[0] = "a";
            myAutomaton.alphabet[1] = "b";
            myAutomaton.transitionFunction = new string[3, 2];
            myAutomaton.transitionFunction[0, 0] = textBoxQ0a.Text;
            myAutomaton.transitionFunction[0, 1] = textBoxQ0b.Text;
            myAutomaton.transitionFunction[1, 0] = textBoxQ1a.Text;
            myAutomaton.transitionFunction[1, 1] = textBoxQ1b.Text;
            myAutomaton.transitionFunction[2, 0] = textBoxQ2a.Text;
            myAutomaton.transitionFunction[2, 1] = textBoxQ2b.Text;
            myAutomaton.initialState = "Q0";
            myAutomaton.finalStates = new string[1];
            myAutomaton.finalStates[0] = "Q1";

            myAutomaton.actualState = "Q0";
            myAutomaton.actualLetter = "a";

            // To clear the text
            AutomatonLog.Clear();
            AutomatonStrings.Clear();
        }

        public void start(string[] args)
        {
            // To initialize the automaton object
            myAutomaton.states = new string[3];
            myAutomaton.states[0] = "Q0";
            myAutomaton.states[1] = "Q1";
            myAutomaton.states[2] = "Q2";
            myAutomaton.alphabet = new string[2];
            myAutomaton.alphabet[0] = "a";
            myAutomaton.alphabet[1] = "b";
            myAutomaton.transitionFunction = new string[3, 2];
            myAutomaton.transitionFunction[0, 0] = textBoxQ0a.Text;
            myAutomaton.transitionFunction[0, 1] = textBoxQ0b.Text;
            myAutomaton.transitionFunction[1, 0] = textBoxQ1a.Text;
            myAutomaton.transitionFunction[1, 1] = textBoxQ1b.Text;
            myAutomaton.transitionFunction[2, 0] = textBoxQ2a.Text;
            myAutomaton.transitionFunction[2, 1] = textBoxQ2b.Text;
            myAutomaton.initialState = "Q0";
            myAutomaton.finalStates = new string[1];
            myAutomaton.finalStates[0] = "Q1";

            myAutomaton.actualState = "Q0";
            myAutomaton.actualLetter = "a";

            // To clear the text
            AutomatonLog.Clear();
            AutomatonLog.AppendText("\n" + "Q0" + " Estado inicial");
            AutomatonStrings.Clear();
        }

        public static Color[] ParseString(string raw)
        {
            int len = 0;
            foreach (char chr in raw)
            {
                len++;
            }
            Color[] ret = new Color[len];
            string[] parsed = Regex.Split(raw, " ");
            for (int i = 0; i < len; i++)
            {
                ret[i] = Color.Green;
            }
            return ret;
        }

        private void myIDELexer_KeyPress(object sender, KeyPressEventArgs e)
        {
            /*
            // Check for an illegal character in the KeyDown event.
            // Only this characters are all allowed
            if (System.Text.RegularExpressions.Regex.IsMatch(e.KeyChar.ToString(), @"[^a-z^A-Z^0-9^+^\-^\/^\*^\(^\)^\>^\<^\!^\|^\&^\;^\=]"))
            {
                // Stop the character from being entered into the control since it is illegal.
                e.Handled = true;
            }
            */
        }

        /*
        myIDE.processFile(myIDELexer.Text, myIDELexer);

        RowColumn.Text = myIDE.cursorRowPosition(myIDELexer) + "," + myIDE.cursorColumnPosition(myIDELexer);
        */

        private void IDELexer_TextChanged(object sender, EventArgs e)
        {
            // We need to asign the return of the function to some integer that counts the last colored index
            letterPos = myIDE.processText(IDELexer);

            // To process the suggestions or InteliCode
            Suggestion.Text = myIDE.processSuggestion(IDELexer);

            //myIDE.processFile(myIDELexer.Text, myIDELexer);

            // To see the row and column
            //RowColumn.Text = myIDE.cursorRowPosition(IDELexer) + "," + myIDE.cursorColumnPosition(IDELexer);
            RowColumn.Text = myIDE.row + "," + myIDE.column;

            // To log the tokens found
            Log.Text = myIDE.tokenList;
        }

        private void ButtonCompile_Click(object sender, EventArgs e)
        {
            //Log.Text = myLexer.lexer(IDELexer.Text);

            Log.Text = "";
            Log.AppendText(myParser.parseArrayOfTokens(myLexer.lexer(IDELexer.Text)));

            Token myToken = new Token();
            Node myNode = new Node(myToken);
            BinaryTree myBinaryTree = new BinaryTree();
            NTree myNTree = new NTree();

            /*myNode = myBinaryTree.append(myNode, myToken);
            myBinaryTree.append(myNode, myToken);
            myBinaryTree.append(myNode, myToken);
            myBinaryTree.append(myNode, myToken);


            myBinaryTree.refactorLevelsFrom(myBinaryTree.firstNode);
            Log.AppendText(Convert.ToString(myBinaryTree.nodesCount));
            Log.AppendText("\n" + myBinaryTree.Log);
            Log.AppendText("\n" + myBinaryTree.maxLevel + " MaxLevel");

            myBinaryTree.findNodesInLevel(myBinaryTree.firstNode, 3);
            Log.AppendText("\n" + myBinaryTree.Log);*/

            /*
            myParser.listArrayOfTokens(myLexer.lexer(IDELexer.Text));
            myParser.evaluateBooleanExpression(myParser.arrayOfTokens);
            myParser.populateBinaryTree(myBinaryTree, myParser.arrayOfTokens);
            myBinaryTree.printBinaryTree(myBinaryTree.firstNode);
            Log.AppendText(myBinaryTree.Log);*/



            //Log.AppendText(Convert.ToString(myParser.evaluateBooleanExpression(myParser.arrayOfTokens)));

            //++
            /*Log.AppendText("\n\n");
            myParser.parseArrayOfTokens(myLexer.lexer(IDELexer.Text));
            myParser.populateMyNTree(myParser.arrayOfTokens);
            myParser.myNTree.printNTree(myParser.myNTree.firstNNode);
            Log.AppendText(myParser.myNTree.Log);*/

            /*
            myParser.listArrayOfTokens(myLexer.lexer(IDELexer.Text));
            Token[] myToken = myParser.arrayOfTokens;
            for (int i = 0; i < myToken.Length; i++)
            {
                Log.AppendText(myToken[i].token + "\n");
            }*/


            /*tokenList = myLexer.lexer2(IDELexer.Text);
            Log.AppendText(Convert.ToString(tokenList.Count));

            for (int i = 0; i < tokenList.Count; i++)
            {
                myToken = tokenList[i];
                Log.AppendText("Hola");
                Log.AppendText("\nToken = \"" + myToken.token + "\"" + ", " + myToken.type);
            }*/
        }

        //********************************************************
        //********************************************************
        // Menu

        private void crearProyectoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fileProject = myFile.createFileProjectGTP(IDELexer);
        }

        private void abrirProyectoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fileProject = myFile.openFileProjectGTP(IDELexer);
        }

        private void guardarProyectoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            myFile.saveFileProjectGTP(IDELexer, fileProject[0], fileProject[1]);
            Log.AppendText("\nProyecto guardado exitosamente");
        }

        private void cerrarProyectoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IDELexer.Clear();
            Log.Clear();
        }

        private void abrirArchivoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            myFile.openFileIDEGT(IDELexer);
        }

        private void guardarArchivoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            myFile.saveFileIDEGT(IDELexer);
            Log.AppendText("\nArchivo guardado exitosamente");
        }

        private void cerrarArchivoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IDELexer.Clear();
            Log.Clear();
        }

        private void crearArchivoLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            myFile.saveFileLogGTE(Log);
        }

        private void abrirArchivoLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            myFile.openFileLogGTE(IDELexer);
        }

        private void cerrarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //********************************************************
        //********************************************************
        // Automaton AFD

        private void buttonReset_Click(object sender, EventArgs e)
        {
            // To initialize the automaton object
            myAutomaton.states = new string[3];
            myAutomaton.states[0] = "Q0";
            myAutomaton.states[1] = "Q1";
            myAutomaton.states[2] = "Q2";
            myAutomaton.alphabet = new string[2];
            myAutomaton.alphabet[0] = "a";
            myAutomaton.alphabet[1] = "b";
            myAutomaton.transitionFunction = new string[3, 2];
            myAutomaton.transitionFunction[0, 0] = textBoxQ0a.Text;
            myAutomaton.transitionFunction[0, 1] = textBoxQ0b.Text;
            myAutomaton.transitionFunction[1, 0] = textBoxQ1a.Text;
            myAutomaton.transitionFunction[1, 1] = textBoxQ1b.Text;
            myAutomaton.transitionFunction[2, 0] = textBoxQ2a.Text;
            myAutomaton.transitionFunction[2, 1] = textBoxQ2b.Text;
            myAutomaton.initialState = "Q0";
            myAutomaton.finalStates = new string[1];
            myAutomaton.finalStates[0] = "Q1";

            myAutomaton.actualState = "Q0";
            myAutomaton.actualLetter = "a";

            // To clear the text
            AutomatonLog.Clear();
            AutomatonLog.AppendText("********Think Outside the BOX *********" + "\n" + "Q0" + " Estado inicial");
            AutomatonStrings.Clear();
        }

        private void buttonA_Click(object sender, EventArgs e)
        {
            myAutomaton.actualLetter = "a";
            string myNextState = myAutomaton.AFD(myAutomaton.actualState, myAutomaton.actualLetter, myAutomaton.finalStates);

            // When reaches the acceptation state
            for (int i = 0; i < myAutomaton.finalStates.Length; i++)
            {
                if (myNextState == myAutomaton.finalStates[i])
                {
                    AutomatonLog.AppendText("\n" + "d(" + myAutomaton.actualState + "," + myAutomaton.actualLetter + ") = " + myNextState + " Estado de aceptación");
                    myAutomaton.actualState = myNextState;
                    AutomatonStrings.AppendText(myAutomaton.actualLetter);
                    return;
                }
            }

            // When reaches the NO acceptation state
            for (int i = 0; i < myAutomaton.states.Length; i++)
            {
                if (myNextState == myAutomaton.states[i])
                {
                    AutomatonLog.AppendText("\n" + "d(" + myAutomaton.actualState + "," + myAutomaton.actualLetter + ") = " + myNextState + " Estado de NO aceptación");
                    myAutomaton.actualState = myNextState;
                    AutomatonStrings.AppendText(myAutomaton.actualLetter);
                    return;
                }
            }

            // When there is not an state
            AutomatonLog.AppendText("\nNo hay estado");
            myAutomaton.actualState = myNextState;
            AutomatonStrings.AppendText("");
        }

        private void buttonB_Click(object sender, EventArgs e)
        {
            myAutomaton.actualLetter = "b";
            string myNextState = myAutomaton.AFD(myAutomaton.actualState, myAutomaton.actualLetter, myAutomaton.finalStates);

            // When reaches the acceptation state
            for (int i = 0; i < myAutomaton.finalStates.Length; i++)
            {
                if (myNextState == myAutomaton.finalStates[i])
                {
                    AutomatonLog.AppendText("\n" + "d(" + myAutomaton.actualState + "," + myAutomaton.actualLetter + ") = " + myNextState + " Estado de aceptación");
                    myAutomaton.actualState = myNextState;
                    AutomatonStrings.AppendText(myAutomaton.actualLetter);
                    return;
                }
            }

            // When reaches the NO acceptation state
            for (int i = 0; i < myAutomaton.states.Length; i++)
            {
                if (myNextState == myAutomaton.states[i])
                {
                    AutomatonLog.AppendText("\n" + "d(" + myAutomaton.actualState + "," + myAutomaton.actualLetter + ") = " + myNextState + " Estado de NO aceptación");
                    myAutomaton.actualState = myNextState;
                    AutomatonStrings.AppendText(myAutomaton.actualLetter);
                    return;
                }
            }

            // When there is not an state
            AutomatonLog.AppendText("\nNo hay estado");
            myAutomaton.actualState = myNextState;
            AutomatonStrings.AppendText("");
        }

        private void Form1_Enter(object sender, EventArgs e)
        {
            // To initialize the automaton object
            myAutomaton.states = new string[3];
            myAutomaton.states[0] = "Q0";
            myAutomaton.states[1] = "Q1";
            myAutomaton.states[2] = "Q2";
            myAutomaton.alphabet = new string[2];
            myAutomaton.alphabet[0] = "a";
            myAutomaton.alphabet[1] = "b";
            myAutomaton.transitionFunction = new string[3, 2];
            myAutomaton.transitionFunction[0, 0] = textBoxQ0a.Text;
            myAutomaton.transitionFunction[0, 1] = textBoxQ0b.Text;
            myAutomaton.transitionFunction[1, 0] = textBoxQ1a.Text;
            myAutomaton.transitionFunction[1, 1] = textBoxQ1b.Text;
            myAutomaton.transitionFunction[2, 0] = textBoxQ2a.Text;
            myAutomaton.transitionFunction[2, 1] = textBoxQ2b.Text;
            myAutomaton.initialState = "Q0";
            myAutomaton.finalStates = new string[1];
            myAutomaton.finalStates[0] = "Q1";

            myAutomaton.actualState = "Q0";
            myAutomaton.actualLetter = "a";

            // To clear the text
            AutomatonLog.Clear();
            AutomatonLog.AppendText("\n" + "Q0" + " Estado inicial");
            AutomatonStrings.Clear();
        }
    }
}
