using System.Collections.Generic;

namespace Simon.ECS.FSM
{
    public class FSMContainer<T> where T : IFSMQuerable
    {

        private List<T> m_container;

        public FSMContainer()
        {
            m_container = new List<T>();
        }

        public void Add(T item)
        {
            m_container.Add(item);
        }

        public void Remove(string id)
        {
            T item = Get(id);
            m_container.Remove(item);
        }

        public T Get(string id)
        {
            T returnedItem = default(T);

            foreach (T item in m_container)
            {
                if (item.ID == id)
                {
                    returnedItem = item;
                }
            }

            return returnedItem;
        }

        public List<T> GetAll()
        {
            return m_container;
        }
    }

}