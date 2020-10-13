﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFP_PROYECTO2_Basic_IDE
{
    // N-aryTree Node
    public class NNode
    {
        public string type;
        public List<Token> command;
        public NNode previous;
        public List<NNode> NextNNodes;
        public int level;
        public int annotation;
        public int index;

        // Constructor to create a new node 
        // next and prev is by default initialized as null 
        public NNode(string _type, List<Token> _command)
        {
            this.type = "";
            this.command = _command;
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
            firstNNode = null;
            maxLevel = 0;
        }

        //*******************************************************************  

        public NNode createNNode(string _type, List<Token> _command)
        {
            NNode newNode = new NNode(_type, _command);

            if (firstNNode == null)
            {
                firstNNode = newNode;
                newNode.level = 1;
                maxLevel = 1;
            }

            return newNode;
        }

        public NNode append(NNode _NNode, string _type, List<Token> _command)
        {
            if (firstNNode == null)
            {
                return createNNode(_type, _command);
            }

            NNode newNNode = new NNode(_type, _command);

            newNNode.previous = _NNode;
            _NNode.NextNNodes.Add(newNNode);
            newNNode.index = _NNode.NextNNodes.Count - 1;

            newNNode.level = _NNode.level + 1;
            //refactorLevelsFrom(firstNNode);

            return newNNode;
        }

        public NNode push(NNode _NNode, string _type, List<Token> _command)
        {
            if (firstNNode == null)
            {
                return createNNode(_type, _command);
            }

            if (_NNode.previous == null)
            {
                NNode newNode = new NNode(_type, _command);

                _NNode.previous = newNode;
                newNode.NextNNodes.Add(_NNode);

                if (firstNNode == _NNode)
                {
                    firstNNode = newNode;
                }

                refactorLevelsFrom(firstNNode);

                return newNode;
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

        public void printBinaryTree(NNode _testNNode)
        {
            if (_testNNode == null)
            {
                return;
            }

            if (_testNNode == firstNNode)
            {
                // Do something here
                Log += _testNNode.level + " root" + " " + _testNNode.type + "\n";
            }

            if (_testNNode.NextNNodes.Count != 0)
            {
                for (int i = 0; i < _testNNode.NextNNodes.Count; i++)
                {
                    // Do something here
                    Log += _testNNode.NextNNodes[i].level + " " + _testNNode.NextNNodes[i].type + ", ";

                    printBinaryTree(_testNNode.NextNNodes[i]);
                }

                Log += "\n";
            }
        }
    }
}