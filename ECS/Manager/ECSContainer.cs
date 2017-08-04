using System.Collections.Generic;

namespace Simon.ECS.Manager
{
    public class ECSContainer<T> : IECSContainer where T : IECSQuerable
    {
        public ECSContainer()
        {
            ECSDebug.Log("System created for " + typeof(T));
        }

        public System.Type GetContainerType()
        {
            return typeof(T).BaseType;
        }

        private List<T> m_List = new List<T>();
        internal List<T> GetAll()
        {
            return m_List;
        }

        internal T Get(int id)
        {
            T returnedObject = default(T);

            foreach (IECSQuerable queryObj in m_List)
            {
                if (queryObj.GetID() == id)
                {
                    returnedObject = (T)queryObj;
                    break;
                }
            }

            return returnedObject;
        }


        internal void Add(T obj)
        {
            m_List.Add(obj);
            ECSDebug.Log("Added " + obj + " to " + this);
        }

        internal void Remove(T obj)
        {
            m_List.Remove(obj);
        }

    }
}