using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFP_PROYECTO2_Basic_IDE
{
    // N-aryTree Node
    public class NNode
    {
        public string type;
        public Token[] booleanExpression;
        public List<Token> command = new List<Token>();
        public NNode previous;
        public List<NNode> NextNNodes = new List<NNode>();
        public int level;
        public int annotation;
        public int index;

        // Constructor to create a new node 
        // next and prev is by default initialized as null 
        public NNode(string _type, List<Token> _command)
        {
            this.type = _type;
            //this.command = _command;
            if (_command.Count > 0)
            {
                for (int i = 0; i < _command.Count; i++)
                {
                    this.command.Add(_command[i]);
                }
            }

            this.previous = null;
            this.NextNNodes = new List<NNode>();
            this.level = 0;
            this.annotation = 0;
            this.index = 0;
        }
    }
    class NTree
    {
        public NNode firstNNode; // firstNode of list
        public int maxLevel; // last node of list (to optimize appendOptimized method)
        public int NNodesCount = 0;
        public string Log;

        //*******************************************************************    

        public NTree()
        {
            this.firstNNode = null;
            this.maxLevel = 0;
            this.Log = "";
        }

        //*******************************************************************  

        public NNode createNNode(string _type, List<Token> _command)
        {
            NNode newNNode = new NNode(_type, _command);
            //newNNode.type = _type;
            //newNNode.command = _command;

            if (firstNNode == null)
            {
                firstNNode = newNNode;
                newNNode.level = 1;
                maxLevel = 1;
            }

            return newNNode;
        }

        public NNode append(NNode _NNode, string _type, List<Token> _command)
        {
            if (firstNNode == null || _NNode == null)
            {
                return createNNode(_type, _command);
            }

            NNode newNNode = new NNode(_type, _command);
            //newNNode.type = _type;
            //newNNode.command = _command;

            newNNode.previous = _NNode;
            _NNode.NextNNodes.Add(newNNode);
            newNNode.index = _NNode.NextNNodes.Count - 1;

            newNNode.level = _NNode.level + 1;
            //refactorLevelsFrom(firstNNode);

            return newNNode;
        }

        public NNode push(NNode _NNode, string _type, List<Token> _command)
        {
            if (firstNNode == null || _NNode == null)
            {
                return createNNode(_type, _command);
            }

            if (_NNode.previous == null)
            {
                NNode newNNode = new NNode(_type, _command);
                //newNNode.type = _type;
                //newNNode.command = _command;

                _NNode.previous = newNNode;
                newNNode.NextNNodes.Add(_NNode);

                if (firstNNode == _NNode)
                {
                    firstNNode = newNNode;
                }

                refactorLevelsFrom(firstNNode);

                return newNNode;
            }

            return null;
        }

        public void remove(NNode _NNode)
        {
            _NNode.previous.NextNNodes.Remove(_NNode);
            _NNode.previous = null;
            _NNode.NextNNodes.Clear();
        }

        public void refactorLevelsFrom(NNode _testNNode)
        {
            if (_testNNode == null)
            {
                return;
            }

            if (_testNNode == firstNNode)
            {
                _testNNode.level = 1;
                maxLevel = 1;
                NNodesCount = 1;

                Log = "";
                Log = Convert.ToString(_testNNode.level) + "\n";
            }

            if (_testNNode.NextNNodes.Count != 0)
            {
                for (int i = 0; i < _testNNode.NextNNodes.Count; i++)
                {
                    _testNNode.NextNNodes[i].level = _testNNode.level + 1;
                    if (maxLevel < _testNNode.NextNNodes[i].level)
                    {
                        maxLevel = _testNNode.NextNNodes[i].level;
                    }
                    NNodesCount++;

                    Log += Convert.ToString(_testNNode.NextNNodes[i].level) + Convert.ToString(_testNNode.NextNNodes[i].index) + "\n";
                    refactorLevelsFrom(_testNNode.NextNNodes[i]);
                }
            }
        }

        public void findNodesInLevel(NNode _testNNode, int level)
        {
            if (_testNNode == null)
            {
                return;
            }

            if (_testNNode == firstNNode)
            {
                if (_testNNode.level == level)
                {
                    // Do something here
                    Log += _testNNode.level + "\n";
                }
            }

            if (_testNNode.NextNNodes.Count != 0)
            {
                for (int i = 0; i < _testNNode.NextNNodes.Count; i++)
                {
                    if (_testNNode.NextNNodes[i].level == level)
                    {
                        // Do something here
                        Log += _testNNode.NextNNodes[i].level + "\n";

                        findNodesInLevel(_testNNode.NextNNodes[i], level);
                    }
                }
            }
        }

        public void printNTree(NNode _testNNode)
        {
            if (_testNNode == null)
            {
                return;
            }

            if (_testNNode == firstNNode)
            {
                // Do something here
                Log += _testNNode.level + " root" + " " + _testNNode.type;
                printNNodeCommands(_testNNode);
                /*for (int j = 0; j < _testNNode.command.Count; j++)
                {
                    Log += _testNNode.command[j].value;
                }*/
            }

            if (_testNNode.NextNNodes.Count != 0)
            {
                Log += "\n";
                for (int i = 0; i < _testNNode.NextNNodes.Count; i++)
                {
                    // Do something here
                    //Log += _testNNode.NextNNodes[i].level + " " + _testNNode.NextNNodes[i].type + ", ";
                    Log += _testNNode.NextNNodes[i].type + ", ";
                    //printNNodeCommands(_testNNode.NextNNodes[i]);
                    /*for (int j = 0; j < _testNNode.NextNNodes[i].command.Count; j++)
                    {
                        Log += _testNNode.NextNNodes[i].command[j].value;
                    }*/

                    printNTree(_testNNode.NextNNodes[i]);
                }
            }
        }

        public void printNNodeCommands(NNode _testNNode)
        {
            for (int i = 0; i < _testNNode.command.Count; i++)
            {
                Log += _testNNode.command[i].value + ", ";
            }
        }
    }
}
