using System;

namespace Heap
{
    class Node<T>
    {
        protected T m_info;
        protected int m_heapValue;
        public Node(T info, int heapValue)
        {
            this.m_info = info;
            this.m_heapValue = heapValue;
        }
        public void SetInfo(T info)
        {
            this.m_info = info;
        }
        public T GetInfo()
        {
            return this.m_info;
        }
        public void SetHeapValue(int heapValue)
        {
            this.m_heapValue = heapValue;
        }
        public int GetHeapValue()
        {
            return this.m_heapValue;
        }
    }
    class Heap<T>
    {
        //Members
        protected Node<T>[] m_array;
        protected int m_capacity;
        protected int m_counter;
        //Constructor
        public Heap(int size = 100)//Size is the estimated length
        {
            this.m_array = new Node<T>[size + 1];
            this.m_counter = 0;
            this.m_capacity = size;
        }
        //Methods
        protected int GetLeftChildIndex(int index)
        {
            return 2 * index + 1;
        }
        protected int GetRightChildIndex(int index)
        {
            return 2 * index + 2;
        }
        protected int GetParentIndex(int index)
        {
            return (index - 1) / 2;
        }
        protected bool HasLeftChild(int index)
        {
            if(index*2 + 1 < this.m_counter - 1)
            {
                return true;
            }
            return false;
        }
        protected bool HasRightChild(int index)
        {
            if (index * 2 + 1 < this.m_counter - 1)
            {
                return true;
            }
            return false;
        }
        protected bool HasParent(int index)
        {
            if (index > 0)
            {
                return true;
            }
            return false;
        }
        protected Node<T> LeftChild(int index)
        {
            return this.m_array[index * 2 + 1];
        }
        protected Node<T> RightChild(int index)
        {
            return this.m_array[index * 2 + 2];
        }
        protected Node<T> Parent(int index)
        {
            return this.m_array[this.GetParentIndex(index)];
        }
        public void Resize()
        {
            if(this.m_capacity == this.m_counter)
            {
                this.m_capacity = this.m_capacity * 2;
                Array.Resize(ref this.m_array, this.m_capacity+1);
            }
            if(this.m_capacity < this.m_counter)
            {
                this.m_capacity = this.m_capacity / 2;
                Array.Resize(ref this.m_array, this.m_capacity+1);
            }
        }
        public Node<T> Peek()
        {
            if(this.m_counter == 0)
            {
                throw new System.InvalidOperationException("The heap is empty");
            }
            else
            {
                return this.m_array[0];
            }
        }
        protected void Swap(int index1, int index2)
        {
            Node<T> temp = this.m_array[index1];
            this.m_array[index1] = this.m_array[index2];
            this.m_array[index2] = temp;
        }
        public void AddNode(T node, int heapValue)
        {
            this.Resize();
            this.m_array[this.m_counter] = new Node<T>(node, heapValue);
            this.m_counter++;
            this.HeapifyUp();
        }
        public virtual void HeapifyUp()
        { }
        public virtual void HeapifyDown()
        { }
    }
    class MaxHeap<T> : Heap<T>     // Implements HeapifyUp() and HeapifyDown()
    {
        //Members
        private int m_indexUp;  //index is used by heapifyUp()
        private int m_indexDown; //used in heapifyDown()
                                 //Constructor
        public MaxHeap(int size = 100)  //Estimated size
        {
            this.m_array = new Node<T>[size + 1];
            this.m_counter = 0;
            this.m_indexUp = -1;        //index is used by heapifyUp() 
            this.m_indexDown = -1;        //index is used by heapifyDown()
        }
        public Node<T> Poll()
        {
            /*
            * Cuts the binary tree by swaping last leaf with the root and heapify down
            *
            */
            if (this.m_counter == 0)
            {
                throw new System.InvalidOperationException("The heap is empty");
            }
            else
            {
                var element = this.m_array[0];
                this.Swap(0, this.m_counter - 1);
                this.m_counter--;
                this.HeapifyDown();
                return element;
            }
        }
        //Methods
        public override void HeapifyUp()
        {
            /*
             * Compares current node with it's parent.
             *If child is bigger than parent then swap them
             */
            if (this.m_indexUp == -1)
            {
                this.m_indexUp = this.m_counter - 1;
            }
            if (!this.HasParent(this.m_indexUp) && this.Parent(this.m_indexUp).GetHeapValue() > this.m_array[this.m_indexUp].GetHeapValue())
            {
                this.m_indexUp = -1;
                return;
            }
            if (this.HasParent(this.m_indexUp) && this.Parent(this.m_indexUp).GetHeapValue() < this.m_array[this.m_indexUp].GetHeapValue())
            {
                this.Swap(this.GetParentIndex(this.m_indexUp), this.m_indexUp);
                this.m_indexUp = this.GetParentIndex(this.m_indexUp);
                this.HeapifyUp();
                return;
            }
            this.m_indexUp = -1;

        }
        public override void HeapifyDown()
        {
            if (this.m_indexDown == -1)
            {
                this.m_indexDown = 0;
            }
            if (!this.HasLeftChild(this.m_indexDown))
            {
                this.m_indexDown = -1;
                return;
            }
            if (this.HasRightChild(this.m_indexDown))
            {
                if (this.LeftChild(this.m_indexDown).GetHeapValue() >= this.RightChild(this.m_indexDown).GetHeapValue())
                {
                    if (this.m_array[this.m_indexDown].GetHeapValue() < this.LeftChild(this.m_indexDown).GetHeapValue())
                    {
                        this.Swap(this.m_indexDown, this.GetLeftChildIndex(this.m_indexDown));
                        this.m_indexDown = this.GetLeftChildIndex(this.m_indexDown);
                        this.HeapifyDown();
                        return;
                    }
                }
                if (this.LeftChild(this.m_indexDown).GetHeapValue() < this.RightChild(this.m_indexDown).GetHeapValue())
                {
                    if (this.m_array[this.m_indexDown].GetHeapValue() < this.RightChild(this.m_indexDown).GetHeapValue())
                    {
                        this.Swap(this.m_indexDown, this.GetRightChildIndex(this.m_indexDown));
                        this.m_indexDown = this.GetRightChildIndex(this.m_indexDown);
                        this.HeapifyDown();
                        return;
                    }
                }
            }
            if (this.HasLeftChild(this.m_indexDown))
            {
                if (this.m_array[this.m_indexDown].GetHeapValue() < this.LeftChild(this.m_indexDown).GetHeapValue())
                {
                    this.Swap(this.m_indexDown, this.GetLeftChildIndex(this.m_indexDown));
                    this.m_indexDown = this.GetLeftChildIndex(this.m_indexDown);
                    this.HeapifyDown();
                    return;
                }
            }
            /*
            if (!this.HasLeftChild(this.m_indexDown))
            {
                this.m_indexDown = -1;
                return;
            }
            if (this.HasLeftChild(this.m_indexDown))
            {
                int smallerChildIndex = this.GetLeftChildIndex(this.m_indexDown);
                if (this.HasRIghtChild(this.m_indexDown) && this.RightChild(this.m_indexDown).GetHeapValue() < this.LeftChild(smallerChildIndex).GetHeapValue())
                {
                    smallerChildIndex = this.GetRightChildIndex(this.m_indexDown);
                }
                if (this.m_array[this.m_indexDown].GetHeapValue() < this.m_array[smallerChildIndex].GetHeapValue())
                {
                    this.Swap(this.m_indexDown, smallerChildIndex);
                }
                else
                {
                    this.m_indexDown = smallerChildIndex;
                }
                this.m_indexDown = smallerChildIndex;
            }*/
        }
    }
    class MinHeap<T> : Heap <T>     // Implements HeapifyUp() and HeapifyDown()
    {
        //Members
        private int m_indexUp;  //index is used by heapifyUp()
        private int m_indexDown; //used in heapifyDown()
                                 //Constructor
        public MinHeap(int size = 100)  //Estimated size
        {
            this.m_array = new Node<T>[size + 1];
            this.m_counter = 0;
            this.m_indexUp = -1;        //index is used by heapifyUp() 
            this.m_indexDown = -1;        //index is used by heapifyDown()
        }
        public Node<T> Poll()
        {
            /*
            * Cuts the binary tree by swaping last leaf with the root and heapify down
            *
            */
            if (this.m_counter == 0)
            {
                throw new System.InvalidOperationException("The heap is empty");
            }
            else
            {
                var element = this.m_array[0];
                this.Swap(0, this.m_counter-1);
                this.m_counter--;
                this.HeapifyDown();
                return element;
            }
        }
        //Methods
        public override void HeapifyUp()
        {
            /*
             * Compares current node with it's parent.
             *If child is bigger than parent then swap them
             */
            if (this.m_indexUp == -1)
            {
                this.m_indexUp = this.m_counter - 1;
            }
            if(!this.HasParent(this.m_indexUp) && this.Parent(this.m_indexUp).GetHeapValue() < this.m_array[this.m_indexUp].GetHeapValue())
            {
                this.m_indexUp = -1;
                return;
            }
            if(this.HasParent(this.m_indexUp) && this.Parent(this.m_indexUp).GetHeapValue() > this.m_array[this.m_indexUp].GetHeapValue())
            {
                this.Swap(this.GetParentIndex(this.m_indexUp), this.m_indexUp);
                this.m_indexUp = this.GetParentIndex(this.m_indexUp);
                this.HeapifyUp();
            }
            this.m_indexUp = -1;

        }
        public override void HeapifyDown()
        {
            if (this.m_indexDown == -1)
            {
                this.m_indexDown = 0;
            }
            if (!this.HasLeftChild(this.m_indexDown))
            {
                this.m_indexDown = -1;
                return;
            }
            if (this.HasRightChild(this.m_indexDown))
            {
                if(this.LeftChild(this.m_indexDown).GetHeapValue() <= this.RightChild(this.m_indexDown).GetHeapValue())
                {
                    if(this.m_array[this.m_indexDown].GetHeapValue() > this.LeftChild(this.m_indexDown).GetHeapValue())
                    {
                        this.Swap(this.m_indexDown, this.GetLeftChildIndex(this.m_indexDown));
                        this.m_indexDown = this.GetLeftChildIndex(this.m_indexDown);
                        this.HeapifyDown();
                        return;
                    }
                }
                if (this.LeftChild(this.m_indexDown).GetHeapValue() > this.RightChild(this.m_indexDown).GetHeapValue())
                {
                    if (this.m_array[this.m_indexDown].GetHeapValue() > this.RightChild(this.m_indexDown).GetHeapValue())
                    {
                        this.Swap(this.m_indexDown, this.GetRightChildIndex(this.m_indexDown));
                        this.m_indexDown = this.GetRightChildIndex(this.m_indexDown);
                        this.HeapifyDown();
                        return;
                    }
                }
            }
            if(this.HasLeftChild(this.m_indexDown))
            {
                if (this.m_array[this.m_indexDown].GetHeapValue() > this.LeftChild(this.m_indexDown).GetHeapValue())
                {
                    this.Swap(this.m_indexDown, this.GetLeftChildIndex(this.m_indexDown));
                    this.m_indexDown = this.GetLeftChildIndex(this.m_indexDown);
                    this.HeapifyDown();
                    return;
                }
            }
            /*
            if (!this.HasLeftChild(this.m_indexDown))
            {
                this.m_indexDown = -1;
                return;
            }
            if (this.HasLeftChild(this.m_indexDown))
            {
                int smallerChildIndex = this.GetLeftChildIndex(this.m_indexDown);
                if (this.HasRIghtChild(this.m_indexDown) && this.RightChild(this.m_indexDown).GetHeapValue() < this.LeftChild(smallerChildIndex).GetHeapValue())
                {
                    smallerChildIndex = this.GetRightChildIndex(this.m_indexDown);
                }
                if (this.m_array[this.m_indexDown].GetHeapValue() < this.m_array[smallerChildIndex].GetHeapValue())
                {
                    this.Swap(this.m_indexDown, smallerChildIndex);
                }
                else
                {
                    this.m_indexDown = smallerChildIndex;
                }
                this.m_indexDown = smallerChildIndex;
            }*/
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            MaxHeap<int> a = new MaxHeap<int>();

            a.AddNode(3999, 3999);
            a.AddNode(2, 2);
            a.AddNode(46, 46);
            a.AddNode(11, 11);
            a.AddNode(666, 666);
            a.AddNode(11, 11);
            a.AddNode(7777, 7777);
            a.AddNode(999999, 999999);

            var b = a.Poll().GetInfo();
            Console.WriteLine(b);
            b = a.Poll().GetInfo();
            Console.WriteLine(b);
            b = a.Poll().GetInfo();
            Console.WriteLine(b);
            b = a.Poll().GetInfo();
            Console.WriteLine(b);
            b = a.Poll().GetInfo();
            Console.WriteLine(b);
            b = a.Poll().GetInfo();
            Console.WriteLine(b);
            b = a.Poll().GetInfo();
            Console.WriteLine(b);
            b = a.Poll().GetInfo();
            Console.WriteLine(b);
        }
    }
}
