using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace LFP_PROYECTO2_Basic_IDE
{
    // BinaryTree Node
    public class Node
    {
        public Token token;
        public Node previous;
        public Node leftNode;
        public Node rightNode;
        public int level;
        public int annotation;

        // Constructor to create a new node 
        // next and prev is by default initialized as null 
        public Node(Token _token)
        {
            this.token = _token;
            this.previous = null;
            this.leftNode = null;
            this.rightNode = null;
            this.level = 0;
            this.annotation = 0;
        }
    }

    public class BinaryTree
    {
        public Node firstNode; // firstNode of list
        public int maxLevel; // last node of list (to optimize appendOptimized method)
        public int nodesCount = 0;
        public string Log;

        //*******************************************************************    

        public BinaryTree()
        {
            firstNode = null;
            maxLevel = 0;
        }

        //*******************************************************************  
        
        public Node createNode(Token _token)
        {
            Node newNode = new Node(_token);

            if (firstNode == null)
            {
                firstNode = newNode;
                newNode.level = 1;
                maxLevel = 1;
            }

            return newNode;
        }

        public Node appendLeft(Node _node, Token _newToken)
        {
            if (firstNode == null || _node == null)
            {
                return createNode(_newToken);
            }

            if (_node.leftNode == null)
            {
                Node newNode = new Node(_newToken);

                newNode.previous = _node;
                _node.leftNode = newNode;

                refactorLevelsFrom(firstNode);

                return newNode;
            }

            return null;
        }

        public Node appendRight(Node _node, Token _newToken)
        {
            if (firstNode == null || _node == null)
            {
                return createNode(_newToken);
            }

            if (_node.rightNode == null)
            {
                Node newNode = new Node(_newToken);

                newNode.previous = _node;
                _node.rightNode = newNode;

                refactorLevelsFrom(firstNode);

                return newNode;
            }

            return null;
        }

        public Node append(Node _node, Token _newToken)
        {
            if (firstNode == null || _node == null)
            {
                return createNode(_newToken);
            }

            if (_node.leftNode == null)
            {
                Node newNode = new Node(_newToken);

                newNode.previous = _node;
                _node.leftNode = newNode;

                refactorLevelsFrom(firstNode);

                return newNode;
            }
            else if (_node.rightNode == null)
            {
                Node newNode = new Node(_newToken);

                newNode.previous = _node;
                _node.rightNode = newNode;

                refactorLevelsFrom(firstNode);

                return newNode;
            }

            return null;
        }

        public Node pushLeft(Node _node, Token _newToken)
        {
            if (firstNode == null || _node == null)
            {
                return createNode(_newToken);
            }

            if (_node.previous == null)
            {
                Node newNode = new Node(_newToken);

                _node.previous = newNode;
                newNode.leftNode = _node;

                if (firstNode == _node)
                {
                    firstNode = newNode;
                }

                refactorLevelsFrom(firstNode);

                return newNode;
            }

            return null;
        }

        public Node pushRight(Node _node, Token _newToken)
        {
            if (firstNode == null || _node == null)
            {
                return createNode(_newToken);
            }

            if (_node.previous == null)
            {
                Node newNode = new Node(_newToken);

                _node.previous = newNode;
                newNode.rightNode = _node;

                if (firstNode == _node)
                {
                    firstNode = newNode;
                }

                refactorLevelsFrom(firstNode);

                return newNode;
            }

            return null;
        }

        public void remove(Node _node)
        {
            if (_node.previous.leftNode == _node)
            {
                _node.previous.leftNode = null;
            }

            if (_node.previous.rightNode == _node)
            {
                _node.previous.rightNode = null;
            }

            _node.previous = null;
            _node.leftNode = null;
            _node.rightNode = null;
            _node = null;
        }

        public void refactorLevelsFrom(Node _testNode)
        {
            if (_testNode == null)
            {
                return;
            }

            if (_testNode == firstNode)
            {
                _testNode.level = 1;
                maxLevel = 1;
                nodesCount = 1;

                Log = "";
                Log = Convert.ToString(_testNode.level) + "\n"; 
            }

            if (_testNode.leftNode != null)
            {
                _testNode.leftNode.level = _testNode.level + 1;
                if (maxLevel < _testNode.leftNode.level)
                {
                    maxLevel = _testNode.leftNode.level;
                }
                nodesCount++;

                Log += Convert.ToString(_testNode.leftNode.level) + " left" + "\n";
                refactorLevelsFrom(_testNode.leftNode);
            }

            if (_testNode.rightNode != null)
            {
                _testNode.rightNode.level = _testNode.level + 1;
                if (maxLevel < _testNode.rightNode.level)
                {
                    maxLevel = _testNode.rightNode.level;
                }
                nodesCount++;

                Log += Convert.ToString(_testNode.rightNode.level) + " right" + "\n";
                refactorLevelsFrom(_testNode.rightNode);
            }
        }
        
        public void findNodesInLevel(Node _testNode, int level)
        {
            if (_testNode == null)
            {
                return;
            }

            if (_testNode == firstNode)
            {
                if (_testNode.level == level)
                {
                    // Do something here
                    Log += _testNode.level + "\n";
                }
            }

            if (_testNode.leftNode != null)
            {
                if (_testNode.leftNode.level == level)
                {
                    // Do something here
                    Log += _testNode.leftNode.level + "\n";
                }

                findNodesInLevel(_testNode.leftNode, level);
            }

            if (_testNode.rightNode != null)
            {
                if (_testNode.rightNode.level == level)
                {
                    // Do something here
                    Log += _testNode.rightNode.level + "\n";
                }

                findNodesInLevel(_testNode.rightNode, level);
            }
        }

        public void printBinaryTree(Node _testNode)
        {
            if (_testNode == null)
            {
                return;
            }

            if (_testNode == firstNode)
            {
                    // Do something here
                    Log += _testNode.level + " root" + " " + _testNode.token.token + "\n";
            }

            if (_testNode.leftNode != null)
            {
                    // Do something here
                    Log += _testNode.leftNode.level + " " + _testNode.leftNode.token.token + "\n";

                printBinaryTree(_testNode.leftNode);
            }

            if (_testNode.rightNode != null)
            {
                    // Do something here
                    Log += _testNode.rightNode.level + " " + _testNode.rightNode.token.token + "\n";

                printBinaryTree(_testNode.rightNode);
            }
        }
    }
}
