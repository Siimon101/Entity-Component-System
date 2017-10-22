using System;
using System.Collections;
using System.Collections.Generic;
using btcp.ECS.core;
using UnityEngine;

namespace btcp.ECS.utils
{
    public class Bag<T> : IEnumerable, IDebuggable
    {
        public T NULL_VALUE = default(T);
        private T[] m_data;
        protected int m_size = 0;

        private BagIterator m_iterator;

        public bool IsDebugOn { get; set; }

        public Bag() : this(64)
        {
        }

        public Bag(int capacity)
        {
            m_data = new T[capacity];
        }


        internal int GetSize()
        {
            return m_size;
        }

        public int Has(T obj)
        {
            if (obj == null)
            {
                LogError("Null object passed!");
                return -1;
            }
            else
            {

                for (int i = 0; i < m_size; i++)
                {

                    if (m_data[i] == null)
                    {
                        continue;
                    }

                    if (EqualityComparer<T>.Default.Equals(m_data[i], obj))
                    {
                        return i;
                    }
                }

                LogWarning("Bag does not have an item " + obj.ToString());
                return -1;
            }
        }

        public void Add(T obj)
        {
            if (m_size == m_data.Length)
            {
                IncreaseCapacity(m_size + 1);
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

        internal T[] GetAll()
        {
            return m_data;
        }

        internal T SafeGet(int index)
        {
            if (index >= m_data.Length)
            {
                IncreaseCapacity(Mathf.Max(index + 1, (m_data.Length * 2)));
            }

            return m_data[index];
        }

        public T Get(int index)
        {
            if (index < 0 || index > m_size)
            {
                return default(T);
            }

            return m_data[index];
        }

        public void Set(int index, T obj)
        {
            if (index >= m_data.Length)
            {
                IncreaseCapacity(index + 1);
            }

            m_size = Mathf.Max(index + 1, m_size);
            m_data[index] = obj;
        }

        public T RemoveIndex(int index)
        {
            if (index <= m_size)
            {
                T toRemove = m_data[index];
                m_data[index] = m_data[--m_size];
                m_data[m_size] = default(T);

                return toRemove;
            }

            return default(T);
        }

        internal void Clear()
        {
            for (int i = 0; i < m_size; i++)
            {
                m_data[i] = default(T);
            }

            m_size = 0;
        }


        ///  Sets capacity and clears bag 
        /// <param name="capacity"> The new capacity </param>
        internal void Reset(int capacity)
        {
            m_data = new T[capacity];
            m_size = 0;
        }


        ///  Sets capacity and keeps bag items 
        /// <param name="capacity"> The new capacity </param>
        internal void IncreaseCapacity(int capacity)
        {
            T[] oldData = m_data;
            m_data = new T[capacity];
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

        internal void ResizeToFit()
        {
            List<T> validItems = GetValid();
            Reset(validItems.Count);
            Add(validItems);
        }


        internal List<T> GetValid()
        {
            List<T> validItems = new List<T>();

            for (int i = 0; i < m_size; i++)
            {
                if (m_data[i] != null && m_data[i].Equals(NULL_VALUE) == false)
                {
                    validItems.Add(m_data[i]);
                }
            }
            return validItems;
        }

        public void Log(object v)
        {
            if (IsDebugOn)
            {
                ECSDebug.Log(v);
            }
        }

        public void LogError(object v)
        {
            if (IsDebugOn)
            {
                ECSDebug.LogError(v);
            }
        }

        public void LogWarning(object v)
        {
            if (IsDebugOn)
            {
                ECSDebug.LogWarning(v);
            }
        }

        public void Assert(bool condition, object v)
        {
            ECSDebug.Assert(condition, v);
        }

        internal Bag<T> Clone()
        {
            Bag<T> newBag = new Bag<T>(this.GetSize());
            newBag.Add(this.m_data);
            return newBag;
        }
    }
}