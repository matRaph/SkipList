using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkipList
{
    internal class SkipList
    { 
        public class Node
        {
            public Node Above { get; set; }
            public Node Below { get; set; }
            public Node Next { get; set; }
            public Node Prev { get; set; }
            public int Key { get; set; }

            public Node(int key)
            {
                Key = key;
                this.Above = null;
                this.Below = null;
                this.Next = null;
                this.Prev = null;
            }

            public override string ToString()
            {
                return this.Key.ToString();
            }
        }

        private Node Head { get; set; }
        private Node Tail { get; set; }

        private int Neg_Infinity = int.MinValue;
        private int Pos_Infinity = int.MaxValue;

        public int Height { get; set; }

        public Random Random = new Random();
        public SkipList()
        {
            Head = new Node(Neg_Infinity);
            Tail = new Node(Pos_Infinity);
            Head.Next = Tail;
            Tail.Prev = Head;
            Height = 0;
        }

        public Node Search(int key)
        {
            Node node = Head;
            while (node.Below != null)
            {
                node = node.Below;
                while (key >= node.Next.Key)
                {
                    node = node.Next;
                }
            }
            return node;
        }
        
        public Node Insert(int key)
        {
            Node posi = Search(key);
            Node q;

            int level = -1;
            int numHeads = -1;

            if (posi.Key == key) return posi;

            do
            {
                numHeads++;
                level++;

                CanIncreaseLevel(level);
                q = posi;
                while (posi.Above == null)
                {
                    posi = posi.Prev;
                }
                posi = posi.Above;
                q = InsertAfterAbove(posi, q, key);
            } while (Random.Next(2) == 1);
            
            return q;
        }

        public Node Remove(int key)
        {
            Node NodeToBeRemoved = Search(key);
            if (NodeToBeRemoved.Key != key)
            {
                return null;
            }
            
            RemoveReferencesToNode(NodeToBeRemoved);
            
            while (NodeToBeRemoved != null)
            {
                RemoveReferencesToNode(NodeToBeRemoved);
                
                if (NodeToBeRemoved.Above != null)
                {
                    NodeToBeRemoved = NodeToBeRemoved.Above;
                }
                else { break; }
            }

            return NodeToBeRemoved;
        }
        
        private void RemoveReferencesToNode(Node NodeToBeRemoved)
        {
            Node after = NodeToBeRemoved.Next;
            Node before = NodeToBeRemoved.Prev;

            before.Next = after;
            after.Prev = before;
        }
        private void CanIncreaseLevel(int level)
        {
            if (level >= this.Height)
            {
                this.Height++;
                AddNewLevel();
            }
        }
        
        private void AddNewLevel()
        {
            Node newHead = new Node(Neg_Infinity);
            Node newTail = new Node(Pos_Infinity);
            newHead.Next = newTail;
            newHead.Below = Head;
            newTail.Prev = newHead;
            newTail.Below = Tail;

            Head.Above = newHead;
            Tail.Above = newTail;

            Head = newHead;
            Tail = newTail;
        }
        private Node InsertAfterAbove(Node posi, Node q, int key)
        {
            Node newNode = new Node(key);
            Node nodeBefore = posi.Below.Below;

            SetBeforeAndAfterReferences(q, newNode);
            SetAboveAndBelowReferences(posi, key, newNode, nodeBefore);
            
            return newNode;
        }
        private void SetBeforeAndAfterReferences(Node q, Node newNode)
        {
            newNode.Next = q.Next;
            newNode.Prev = q;
            q.Next.Prev = newNode;
            q.Next = newNode;
        }
        private void SetAboveAndBelowReferences(Node posi, int key, Node newNode, Node nodeBeforeNewNode)
        {
            if (nodeBeforeNewNode != null)
            {
                while (true)
                {
                    if (nodeBeforeNewNode.Next.Key != key)
                    {
                        nodeBeforeNewNode = nodeBeforeNewNode.Next;
                    }
                    else { break; }
                }
                newNode.Below = nodeBeforeNewNode.Next;
                nodeBeforeNewNode.Next.Above = newNode;
            }

            if (posi != null)
            {
                if (posi.Next.Key == key)
                {
                    newNode.Above = posi.Next;
                }
            }
        }
        
    }
}
