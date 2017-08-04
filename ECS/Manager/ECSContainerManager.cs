using System.Collections.Generic;

namespace Simon.ECS.Manager
{
    public static class ECSContainerManager
    {
        internal static List<IECSContainer> s_containerList = new List<IECSContainer>();

        internal static void AddContainer<T>(ECSContainer<T> container) where T : IECSQuerable
        {
            s_containerList.Add(container);
        }

        internal static void RemoveContainer<T>(ECSContainer<T> container) where T : IECSQuerable
        {
            s_containerList.Remove(container as ECSContainer<IECSQuerable>);
        }

        internal static ECSContainer<T> CreateContainer<T>() where T : IECSQuerable
        {
            ECSContainer<T> container = new ECSContainer<T>();
            AddContainer(container);
            return container;
        }

        private static ECSContainer<T> GetContainer<T>() where T : IECSQuerable
        {
            System.Type classType = typeof(T).BaseType;
            ECSContainer<T> returnedContainer = null;


            foreach (IECSContainer container in s_containerList)
            {
                if (container.GetContainerType() == classType)
                {
                    returnedContainer = container as ECSContainer<T>;
                    break;
                }
            }

            if (returnedContainer == null)
            {
                returnedContainer = CreateContainer<T>();
            }


            return returnedContainer;
        }

        internal static void Add<T>(T obj) where T : IECSQuerable
        {
            ECSContainer<T> container = GetContainer<T>();
            container.Add(obj);
        }


        internal static void Remove<T>(T obj) where T : IECSQuerable
        {
            ECSContainer<T> container = GetContainer<T>();
            container.Remove(obj);
        }

        internal static List<T> GetAll<T>() where T : IECSQuerable
        {
            ECSContainer<T> container = GetContainer<T>();
            return container.GetAll();
        }

        internal static T Get<T>(int id) where T : IECSQuerable
        {
            ECSContainer<T> container = GetContainer<T>();
            return container.Get(id);
        }

        internal static void Update<T>() where T : IECSQuerable
        {
            List<T> objects = GetContainer<T>().GetAll();

            List<IECSUpdateable> updateableObjects = GetUpdateableObjectsFromList(objects);

            foreach (IECSUpdateable obj in updateableObjects)
            {
                obj.Update();
            }
        }

        private static List<IECSUpdateable> GetUpdateableObjectsFromList<T>(List<T> objects, bool sortByPriority = true)
        {
            List<IECSUpdateable> returnedList = new List<IECSUpdateable>();

            IECSUpdateable updateableObj = null;
            foreach (T obj in objects)
            {
                updateableObj = obj as IECSUpdateable;

                if (updateableObj != null)
                {
                    returnedList.Add(updateableObj);
                }
            }

            if (sortByPriority)
            {
                returnedList.Sort(new SortByPriority());
            }

            return returnedList;
        }


        internal static void LateUpdate<T>() where T : IECSQuerable
        {
            List<T> objects = GetContainer<T>().GetAll();
            List<IECSUpdateable> updateableObjects = GetUpdateableObjectsFromList(objects);

            foreach (IECSUpdateable obj in updateableObjects)
            {
                obj.LateUpdate();
            }
        }

        internal static void FixedUpdate<T>() where T : IECSQuerable
        {
            List<T> objects = GetContainer<T>().GetAll();
            List<IECSUpdateable> updateableObjects = GetUpdateableObjectsFromList(objects);

            foreach (IECSUpdateable obj in updateableObjects)
            {
                obj.FixedUpdate();
            }

        }

        public class SortByPriority : IComparer<IECSUpdateable>
        {
            public int Compare(IECSUpdateable x, IECSUpdateable y)
            {
                if (x.GetPriority() > y.GetPriority())
                {
                    return -1;
                }

                return 1;
            }
        }
    }


}