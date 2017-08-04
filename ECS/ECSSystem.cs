using Simon.ECS.Manager;
using Simon.Event_System;
using System.Collections.Generic;

namespace Simon.ECS
{
    public abstract class ECSSystem : EventHandler, IECSQuerable, IECSUpdateable
    {
        protected List<Entity> m_entityList;
        public virtual void Update() { }
        public virtual void LateUpdate() { }
        public virtual void FixedUpdate() { }

        private int m_id;
        public int GetID()
        {
            return (int)m_id;
        }


        private int m_priority;
        public int GetPriority()
        {
            return m_priority;
        }

        private UpdateType m_updateThread;
        public UpdateType GetUpdateThread()
        {
            return m_updateThread;
        }
        public void SetThread(UpdateType type)
        {
            m_updateThread = type;
        }

        public virtual ECSSystem Init(int priority = 100)
        {
            m_id = s_systemsCreated;
            m_priority = priority;
            m_eventPriority = priority;
            m_updateThread = UpdateType.TYPE_GAME;
            AddSystem();
            return this;
        }

        private static int s_systemsCreated = 0;

        public void AddSystem()
        {
            ECSContainerManager.Add(this);
            s_systemsCreated++;
        }

        public void RemoveSystem()
        {
            ECSContainerManager.Remove(this);
            s_systemsCreated--;
        }

        protected List<Entity> GetEntitiesWithComponents(params System.Type[] types)
        {
            return ECSEntityManager.GetEntitiesWithComponents(types);
        }


        public enum UpdateType
        {
            TYPE_GAME,
            TYPE_UI,
            TYPE_MENU
        }

    }



}