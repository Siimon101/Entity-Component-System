using System;
using System.Collections;
using System.Collections.Generic;
using com.btcp.ECS.core;
using UnityEngine;

namespace com.btcp.ECS.utils
{
    public class Bag<T> : IEnumerable
    {

        private T[] m_data;
        protected int m_size = 0;

        private BagIterator m_iterator;

        public Bag() : this(64)
        {
        }

        public Bag(int capacity)
        {
            SetCapacity(capacity);
        }

        internal int GetSize()
        {
            return m_size;
        }

        public int GetIndex(T obj)
        {
            for (int i = 0; i < m_size; i++)
            {
                if (m_data[i].Equals(obj))
                {
                    return i;
                }
            }

            return -1;
        }

        public void Add(T obj)
        {
            if (m_size == m_data.Length)
            {
                IncreaseSize(m_size * 2);
            }

            m_data[m_size++] = obj;
        }

        public void Add(IEnumerable<T> list)
        {
            foreach (T obj in list)
            {
                Add(obj);
            }
        }

        internal T SafeGet(int index)
        {
            if (index >= m_data.Length)
            {
                IncreaseSize(Mathf.Max(index + 1, (m_data.Length * 2)));
            }

            return m_data[index];
        }

        internal T[] GetAll()
        {
            return m_data;
        }

        internal T Get(int index)
        {
            return m_data[index];
        }

        public void Set(int index, T obj)
        {
            if (index >= m_data.Length)
            {
                IncreaseSize();
            }

            m_size = Mathf.Max(index, m_size + 1);
            m_data[index] = obj;
        }

        public T Remove(int index)
        {
            if (index < m_size)
            {
                T toRemove = m_data[index];
                m_data[index] = m_data[--m_size];
                m_data[m_size] = default(T);

                return toRemove;
            }

            return default(T);
        }

        internal void Clear(int newCapacity)
        {
            Clear(newCapacity);
        }

        ///  Clears bag and sets capacity 
        /// <param name="capacity"> The new capacity </param>
        /// <returns> Returns itself for chaining </returns>
        internal Bag<T> SetCapacity(int capacity)
        {
            m_data = new T[capacity];
            return this;
        }

        internal void Clear()
        {
            for (int i = 0; i < m_size; i++)
            {
                m_data[i] = default(T);
            }

            m_size = 0;
        }

        public void Remove(T obj)
        {
            Remove(GetIndex(obj));
        }

        private void IncreaseSize()
        {
            IncreaseSize((m_size * 3) / 2 + 1);
        }

        private void IncreaseSize(int amount)
        {
            T[] oldData = m_data;
            m_data = new T[amount];
            oldData.CopyTo(m_data, 0);
        }

        public IEnumerator GetEnumerator()
        {
            if (m_iterator == null)
            {
            }
            m_iterator = new BagIterator(this);

            return m_iterator;
        }


        private sealed class BagIterator : IEnumerator<T>
        {
            private int m_current = -1;
            private Bag<T> m_bag;

            public BagIterator(Bag<T> bag)
            {
                m_bag = bag;
            }

            public T Current
            {
                get
                {
                    return m_bag.Get(m_current);
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    return Current;
                }
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                m_current++;
                return m_current < m_bag.GetSize();
            }

            public void Reset()
            {
                m_current = -1;
            }
        }
    }
}