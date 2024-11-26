using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DS
{

    #region 堆

    /// <summary>
    /// 小顶堆
    /// </summary>
    public class MinHeap<T> where T : IComparable<T>
    {
        public int Count => heap.Count;

        private List<T> heap;

        public MinHeap()
        {
            heap = new List<T>();
        }

        /// <summary>
        /// 插入元素
        /// </summary>
        /// <param name="value"></param>
        public void Insert(T value)
        {
            heap.Add(value);
            HeapifyUp(heap.Count - 1);
        }

        /// <summary>
        /// 删除最小元素并返回
        /// </summary>
        /// <returns></returns>
        public T Pop()
        {
            if (heap.Count == 0)
            {
                Debug.LogError("Heap is empty.");
                return default(T);
            }

            T min = heap[0];
            heap[0] = heap[heap.Count - 1];
            heap.RemoveAt(heap.Count - 1);
            HeapifyDown(0);
            return min;
        }

        public void Remove(T value)
        {
            int index = heap.IndexOf(value);
            if (index == -1)
            {
                Debug.LogError("Value not found.");
                return;
            }

            heap[index] = heap[heap.Count - 1];
            heap.RemoveAt(heap.Count - 1);
            HeapifyDown(index);
        }

        public bool Contains(T value)
        {
            return heap.Contains(value);
        }

        /// <summary>
        /// 获取最小元素
        /// </summary>
        /// <returns></returns>
        public T Peek()
        {
            if (heap.Count == 0)
            {
                Debug.LogError("Heap is empty.");
                return default(T);
            }

            return heap[0];
        }

        // 堆化向上
        private void HeapifyUp(int index)
        {
            while (index > 0)
            {
                int parentIndex = (index - 1) / 2;
                if (heap[index].CompareTo(heap[parentIndex]) < 0)
                {
                    Swap(index, parentIndex);
                    index = parentIndex;
                }
                else
                {
                    break;
                }
            }
        }

        // 堆化向下
        private void HeapifyDown(int index)
        {
            int lastIndex = heap.Count - 1;
            while (index < lastIndex)
            {
                int leftChildIndex = 2 * index + 1;
                int rightChildIndex = 2 * index + 2;
                int smallestIndex = index;

                if (leftChildIndex <= lastIndex && heap[leftChildIndex].CompareTo(heap[smallestIndex]) < 0)
                {
                    smallestIndex = leftChildIndex;
                }
                if (rightChildIndex <= lastIndex && heap[rightChildIndex].CompareTo(heap[smallestIndex]) < 0)
                {
                    smallestIndex = rightChildIndex;
                }
                if (smallestIndex == index)
                    break;

                Swap(index, smallestIndex);
                index = smallestIndex;
            }
        }

        // 交换元素
        private void Swap(int index1, int index2)
        {
            T temp = heap[index1];
            heap[index1] = heap[index2];
            heap[index2] = temp;
        }

        // 获取当前堆的大小
        public int Size()
        {
            return heap.Count;
        }
    }

    /// <summary>
    /// 大顶堆
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MaxHeap<T> where T : IComparable<T>
    {
        public int Count => heap.Count;

        private List<T> heap;

        public MaxHeap()
        {
            heap = new List<T>();
        }

        /// <summary>
        /// 插入元素
        /// </summary>
        /// <param name="value"></param>
        public void Insert(T value)
        {
            heap.Add(value);
            HeapifyUp(heap.Count - 1);
        }

        /// <summary>
        /// 删除最大元素并返回
        /// </summary>
        /// <returns></returns>
        public T Pop()
        {
            if (heap.Count == 0)
            {
                Debug.LogError("Heap is empty.");
                return default(T);
            }

            T max = heap[0];
            heap[0] = heap[heap.Count - 1];
            heap.RemoveAt(heap.Count - 1);
            HeapifyDown(0);
            return max;
        }

        /// <summary>
        /// 获取最大元素
        /// </summary>
        /// <returns></returns>
        public T Peek()
        {
            if (heap.Count == 0)
            {
                Debug.LogError("Heap is empty.");
                return default(T);
            }

            return heap[0];
        }

        // 堆化向上
        private void HeapifyUp(int index)
        {
            while (index > 0)
            {
                int parentIndex = (index - 1) / 2;
                if (heap[index].CompareTo(heap[parentIndex]) > 0)
                {
                    Swap(index, parentIndex);
                    index = parentIndex;
                }
                else
                {
                    break;
                }
            }
        }

        // 堆化向下
        private void HeapifyDown(int index)
        {
            int lastIndex = heap.Count - 1;
            while (index < lastIndex)
            {
                int leftChildIndex = 2 * index + 1;
                int rightChildIndex = 2 * index + 2;
                int largestIndex = index;

                if (leftChildIndex <= lastIndex && heap[leftChildIndex].CompareTo(heap[largestIndex]) > 0)
                {
                    largestIndex = leftChildIndex;
                }
                if (rightChildIndex <= lastIndex && heap[rightChildIndex].CompareTo(heap[largestIndex]) > 0)
                {
                    largestIndex = rightChildIndex;
                }
                if (largestIndex == index)
                    break;

                Swap(index, largestIndex);
                index = largestIndex;
            }
        }

        // 交换元素
        private void Swap(int index1, int index2)
        {
            T temp = heap[index1];
            heap[index1] = heap[index2];
            heap[index2] = temp;
        }

        // 获取当前堆的大小
        public int Size()
        {
            return heap.Count;
        }
    }

    #endregion

    #region 块状链表

    /// <summary>
    /// 块状链表
    /// 对于随机访问和插入删除操作都有良好的支持
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BlockLinkedList<T>
    {
        public class Node
        {
            public Node Next;
            public Node Prev;
            private T[] _block;
            public T this[int index]
            {
                get
                {
                    if (index < 0 || index >= _length)
                    {
                        throw new Exception("Index out of range.");
                    }

                    return _block[index];
                }
                set
                {
                    if (index < 0 || index >= _length)
                    {
                        throw new Exception("Index out of range.");
                    }

                    _block[index] = value;
                }
            }

            private int _capacity;
            public int Capacity => _capacity;
            private int _length;
            public int Length => _length;
            public bool IsFull => _length == _capacity;
            public bool IsHalf => _length == _capacity / 2;

            public Node(int capacity)
            {
                _capacity = capacity;
                Next = Prev = null;
                _block = new T[_capacity];
                _length = 0;
            }

            public void Add(T data)
            {
                if (_length == _capacity)
                {
                    throw new Exception("Block is full.");
                }

                _block[_length] = data;
                _length++;
            }

            public void Insert(int index, T data)
            {
                if (_length == _capacity)
                {
                    throw new Exception("Block is full.");
                }

                if (index < 0 || index >= _length)
                {
                    throw new Exception("Index out of range.");
                }

                for (int i = _length; i > index; i--)
                {
                    _block[i] = _block[i - 1];
                }

                _block[index] = data;
                _length++;
            }

            public void RemoveAt(int index)
            {
                if (index < 0 || index >= _length)
                {
                    throw new Exception("Index out of range.");
                }

                for (int i = index; i < _length - 1; i++)
                {
                    _block[i] = _block[i + 1];
                }

                _block[_length - 1] = default(T);
                _length--;
            }

            public void Remove(T data)
            {
                int index = -1;
                for (int i = 0; i < _length; i++)
                {
                    if (_block[i].Equals(data))
                    {
                        index = i;
                        break;
                    }
                }

                if (index == -1)
                {
                    Debug.LogWarning("Data not found.");
                    return;
                }

                RemoveAt(index);
            }

            /// <summary>
            /// 只保留前半部分元素
            /// </summary>
            /// <param name="num">保留数量</param>
            public void Shrink(int num)
            {
                for (int i = num; i < _length; i++)
                {
                    _block[i] = default(T);
                }

                _length = num;
            }
        }

        private Node _head;
        private Node _tail;
        private int _blockSize;
        private int _count;
        private int Count => _count;

        public BlockLinkedList(int blockSize = 5)
        {
            _head = null;
            _tail = null;
            _blockSize = blockSize;
            _count = 0;
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= _count)
                {
                    throw new Exception("Index out of range.");
                }

                return FindNode(index, out Node target, out int insertIndex);
            }
            set
            {
                if (index < 0 || index >= _count)
                {
                    throw new Exception("Index out of range.");
                }

                Node current = null;
                int insertIndex;
                FindNode(index, out current, out insertIndex);
                current[insertIndex] = value;
            }
        }

        public void Add(T data)
        {
            if (_head == null)
            {
                _head = new Node(_blockSize);
                _tail = _head;
                _head.Next = _tail;
                _tail.Prev = _head;
            }

            if (_tail.IsHalf)
            {
                Node newNode = new Node(_blockSize);
                _tail.Next = newNode;
                newNode.Prev = _tail;
                _tail = newNode;
            }

            _tail.Add(data);
            _count++;
        }

        /// <summary>
        /// 插入元素
        /// </summary>
        /// <param name="index"></param>
        /// <param name="data"></param>
        public void Insert(int index, T data)
        {
            if (index < 0 || index >= _count)
            {
                throw new Exception("Index out of range.");
            }

            Node current = null;
            int insertIndex;
            FindNode(index, out current, out insertIndex);

            if (current == null)
            {
                throw new Exception("Error.");
            }

            current.Insert(insertIndex, data);
            _count++;

            //判断是否需要分裂
            if (current.IsFull)
            {
                Split(current);
            }
        }

        /// <summary>
        /// 删除元素
        /// </summary>
        /// <param name="index">所在表下标</param>
        /// <returns>要删除的元素</returns>
        /// <exception cref="Exception"></exception>
        public T RemoveAt(int index)
        {
            if (index < 0 || index >= _count)
            {
                throw new Exception("Index out of range.");
            }

            Node current = null;
            int removeIndex;
            FindNode(index, out current, out removeIndex);

            return Remove(current, removeIndex);
        }

        /// <summary>
        /// 删除元素
        /// </summary>
        /// <param name="data">要删除的元素</param>
        public void Remove(T data)
        {
            if (_head == null)
            {
                throw new Exception("BlockLinkedList is Empty.");
            }
            FindNode(data, out Node current, out int removeIndex);
            Remove(current, removeIndex);
        }

        /// <summary>
        /// 删除元素
        /// </summary>
        /// <param name="current">所在节点</param>
        /// <param name="removeIndex">所在节点下标</param>
        /// <returns>要删除的元素</returns>
        /// <exception cref="Exception"></exception>
        private T Remove(Node current, int removeIndex)
        {
            if (current == null)
            {
                throw new Exception("Error.");
            }

            T result = current[removeIndex];
            current.RemoveAt(removeIndex);
            _count--;

            //判断是否需要合并
            if (current.Next != null && current.Length + current.Next.Length <= _blockSize / 2)
            {
                Merge(current);
            }

            return result;
        }

        /// <summary>
        /// 查找节点
        /// </summary>
        /// <param name="data">要查找的数据</param>
        /// <param name="target">所在节点</param>
        /// <param name="nodeIndex">所在节点下标</param>
        /// <returns>所在表下标</returns>
        public int FindNode(T data, out Node target, out int nodeIndex)
        {
            Node current = _head;
            int index = 0;
            target = null;
            nodeIndex = -1;

            while (current != null)
            {
                target = current;
                for (int i = 0; i < current.Length; i++)
                {
                    nodeIndex = i;
                    if (current[i].Equals(data))
                    {
                        return index + i;
                    }
                }

                index += current.Length;
                current = current.Next;
            }

            return -1;
        }

        /// <summary>
        /// 查找节点
        /// </summary>
        /// <param name="index">要查找的表下标</param>
        /// <param name="target">所在节点</param>
        /// <param name="insertIndex">所在节点下标</param>
        /// <returns>查找的数据</returns>
        public T FindNode(int index, out Node target, out int insertIndex)
        {
            if (index < 0 || index >= _count)
            {
                throw new Exception("Index out of range.");
            }

            if (_head == null)
            {
                throw new Exception("BlockLinkedList is Empty.");
            }

            //找到index对应的节点,如果index大于_count/2,从尾部开始查找,否则从头部开始查找
            Node current = index > _count / 2 ? _tail : _head;
            int currentIndex = index > _count / 2 ? _count - 1 : 0;
            insertIndex = 0;
            while (currentIndex != index && current != null)
            {
                if (index > _count / 2)
                {
                    currentIndex -= current.Length;
                    if (currentIndex >= index)
                    {
                        current = current.Prev;
                    }
                    else
                    {
                        insertIndex = index - (currentIndex + 1);
                        break;
                    }
                }
                else
                {
                    currentIndex += current.Length;
                    if (currentIndex <= index)
                    {
                        current = current.Next;
                    }
                    else
                    {
                        insertIndex = index - (currentIndex - current.Length);
                        break;
                    }
                }
            }

            target = current;
            return target[insertIndex];
        }

        /// <summary>
        /// 节点分裂
        /// </summary>
        /// <param name="current"></param>
        private void Split(Node current)
        {
            Node newNode = new Node(_blockSize);
            newNode.Next = current.Next;
            newNode.Prev = current;
            current.Next = newNode;
            if (newNode.Next != null)
            {
                newNode.Next.Prev = newNode;
            }

            for (int i = current.Length / 2; i < current.Length; i++)
            {
                newNode.Add(current[i]);
            }

            current.Shrink(current.Length / 2);
        }

        /// <summary>
        /// 节点合并
        /// </summary>
        /// <param name="current"></param>
        private void Merge(Node current)
        {
            Node next = current.Next;
            for (int i = 0; i < next.Length; i++)
            {
                current.Add(next[i]);
            }

            current.Next = next.Next;
            if (next.Next != null)
            {
                next.Next.Prev = current;
            }
        }

        private void Clear()
        {
            _head = null;
            _tail = null;
            _count = 0;
        }

        public string Print()
        {
            Node current = _head;
            string result = "";
            while (current != null)
            {
                for (int i = 0; i < current.Length; i++)
                {
                    result += current[i] + " ";
                }
                result += "\n";

                current = current.Next;
            }

            Debug.Log(result);
            return result;
        }
    }

    #endregion
}