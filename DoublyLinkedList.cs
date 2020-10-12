using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFP_PROYECTO2_Basic_IDE
{
    // Class for Doubly Linked List 
    public class DoublyLinkedList
    {
        private Node firstNode; // firstNode of list 
        private Node lastNode; // last node of list (to optimize appendOptimized method)
        private int listLength;  // Size of the linked list

        //*******************************************************************    

        public DoublyLinkedList()
        {
            firstNode = null;
            lastNode = null;
            listLength = 0;
        }

        //*******************************************************************    

        // Doubly Linked list Node
        public class Node
        {
            public String name;
            public Node prev;
            public Node next;

            // Constructor to create a new node 
            // next and prev is by default initialized as null 
            public Node(String name)
            {
                this.name = name;
            }
        }

        //*******************************************************************    
        
        // Returns the first node
        public Node getFirstNode()
        {
            return firstNode;
        }

        // Returns the last node
        public Node getLasttNode()
        {
            return lastNode;
        }   

        // Returns count of nodes in linked list
        //Iterative solution
        public int getLength()
        {
            return listLength;
        }

        //******************************************************************* 

        // Adding a node at the front of the list 
        public void push(String name)
        {
            Node newNode = new Node(name);

            if (firstNode == null)
            {
                firstNode = new Node(name);
                firstNode.prev = null;
                firstNode.next = null;
                listLength++;
                return;
            }

            newNode.next = firstNode;
            newNode.prev = null;

            if (firstNode != null)
                firstNode.prev = newNode;

            firstNode = newNode;

            listLength++;
        }

        //*******************************************************************    

        // Given a node as prevNode, insert a new node after the given node
        public void InsertAfter(Node node, String name)
        {
            if (node == null && listLength > 0)
            {
                if (node == firstNode.prev)
                {
                    push(name);
                }

                return;
            }
            else if (node == null && listLength == 0)
            {
                append(name);
            }

            Node newNode = new Node(name);

            newNode.next = node.next;

            node.next = newNode;

            newNode.prev = node;

            if (newNode.next != null)
                newNode.next.prev = newNode;

            if (newNode.prev == lastNode)
            {
                lastNode = newNode;
            }

            listLength++;
        }

        //*******************************************************************    

        // Given a node as prevNode, insert a new node after the given node
        public void ReplaceFromStartToEnd(Node start, Node end, String name)
        {
            if (start == null && listLength > 0)
            {
                if (start == firstNode.prev)
                {
                    push(name);
                }

                return;
            }
            else if (start == null && listLength == 0)
            {
                append(name);
            }

            Node newNode = new Node(name);

            newNode = start;
            newNode.next = end.next;

            if (newNode.next != null)
                newNode.next.prev = newNode;

            if (start == lastNode)
            {
                lastNode = newNode;
                newNode.next = null;
            }

            // If start is firstNode
            if (firstNode == start)
            {
                firstNode = newNode;
                newNode.prev = null;
            }

            // If end is lastNode
            if (lastNode == end)
            {
                lastNode = newNode;
                newNode.next = null;
            }

            if (firstNode == start && lastNode == end)
            {
                firstNode = newNode;
                newNode.prev = null;
                lastNode = newNode;
                newNode.next = null;
            }

            // To find the list's length
            if (firstNode != null)
            {
                Node node = firstNode;
                int i = 1;
                while (node.next != null)
                {
                    //node.name = Convert.ToString(i);
                    node = node.next;
                    i++;
                }
                listLength = i;
            }
            else
            {
                listLength = 0;
            }
        }

        //*******************************************************************    

        void append(String name)
        {
            Node newNode = new Node(name);

            newNode.next = null;

            if (firstNode == null)
            {
                newNode.prev = null;
                newNode.next = null;
                firstNode = newNode;
                lastNode = newNode;
                listLength++;
                return;
            }

            lastNode.next = newNode;
            newNode.prev = lastNode;

            lastNode = newNode;
            lastNode.next = null;

            listLength++;
        }

        //*******************************************************************    

        void append(Node newNode)
        {
            newNode.next = null;

            if (firstNode == null)
            {
                newNode.prev = null;
                newNode.next = null;
                firstNode = newNode;
                lastNode = newNode;
                listLength++;
                return;
            }

            lastNode.next = newNode;
            newNode.prev = lastNode;

            lastNode = newNode;
            lastNode.next = null;

            listLength++;
        }

        //*******************************************************************  

        public void deleteNodeKey(int key)
        {
            if (key >= 1 && key <= listLength)
            {
                // if key is firstNode
                if (key == 1)
                {
                    if (listLength > 1)
                    {
                        firstNode = firstNode.next;
                        firstNode.prev = null;
                        listLength--;
                        return;
                    }
                    else
                    {
                        firstNode = null;
                        lastNode = null;
                        listLength = 0;
                    }
                }

                // if key is lastNode
                if (key == listLength)
                {
                    if (listLength > 1)
                    {
                        lastNode = lastNode.prev;
                        lastNode.next = null;
                        listLength--;
                        return;
                    }
                    else
                    {
                        firstNode = null;
                        lastNode = null;
                        listLength = 0;
                    }
                }

                // if key is in between
                Node temp = firstNode.next;
                for (int i = 2; i <= key; i++)
                {
                    if (i == key)
                    {
                        temp.prev.next = temp.next;
                        temp.next.prev = temp.prev;
                        listLength--;
                    }
                    else
                    {
                        temp = temp.next;
                    }
                }
            }
        }

        //*******************************************************************  

        public void deleteNodeData(String name)
        {
            if (listLength > 1)
            {
                Node temp = firstNode;
                while (temp != null)
                {
                    if (temp == firstNode && temp.name == name)
                    {
                        temp.next.prev = null;
                        firstNode = temp.next;
                        temp = temp.next;
                        listLength--;
                    }
                    if (temp.next != null)
                    {
                        if (temp.next.name == name)
                        {
                            temp.next = temp.next.next;
                            if (temp.next != null)
                            {
                                temp.next.prev = temp;
                            }
                            temp = temp.next;
                            listLength--;
                        }
                        else
                        {
                            temp = temp.next;
                        }
                    }
                    else
                    {
                        temp = temp.next;
                    }
                }
            }
            else
            {
                if (firstNode.name == name)
                {
                    firstNode = null;
                    lastNode = null;
                    listLength = 0;
                }
            }
        }

        //******************************************************************* 

        public Node[] nodeArray(DoublyLinkedList list)
        {
            Node[] nodeArray = new Node[getLength()];
            Node node = list.firstNode;
            for (int i = 0; i < getLength(); i++)
            {
                nodeArray[i] = node;
                node = node.next;
            }

            return nodeArray;
        }

        //*******************************************************************  

        public Node node(int key)
        {
            Node node = firstNode;
            for (int i = 1; i <= getLength(); i++)
            {
                if (key == i)
                {
                    return node;
                }
                node = node.next;
            }
            return null;
        }

        //*******************************************************************  

        public Node node(String name)
        {
            Node node = firstNode;
            for (int i = 0; i < getLength(); i++)
            {
                if (node.name == name)
                {
                    return node;
                }
                node = node.next;
            }
            return null;
        }

        //*******************************************************************    

        // This function prints contents of linked list starting from the given node 
        public void printlist()
        {
            Node node = firstNode;
            if (node == null)
            {
                Console.WriteLine();
                return;
            }
            for (int i = 1; i <= getLength(); i++)
            {
                Console.Write(node.name + " ");
                node = node.next;
            }
            Console.WriteLine();
        }

        //*******************************************************************    

        // This function prints contents of linked list starting from the given node 
        public void printlistReversed()
        {
            Node node = lastNode;
            if (node == null)
            {
                Console.WriteLine();
                return;
            }
            for (int i = getLength(); i >= 1; i--)
            {
                Console.Write(node.name + " ");
                node = node.prev;
            }
            Console.WriteLine();
        }

        //*******************************************************************    

        public static void main(String[] args)
        {
            DoublyLinkedList dll = new DoublyLinkedList();

            dll.append("Mary");

            dll.push("John");

            dll.push("Marcos");

            dll.append("Lisa");

            dll.InsertAfter(dll.firstNode.next, "Skully");

            dll.append("John");

            dll.InsertAfter(dll.lastNode, "TEST");

            Console.WriteLine("Created DoublyLinkedList is: ");
            dll.printlist();
            /*System.out.println(""); 
            dll.printlistReversed();
            System.out.println(""); 

            System.out.println();
            dll.deleteNodeData("John");
            dll.printlist();
            System.out.println(dll.getLength());*/
        }
    }
}
