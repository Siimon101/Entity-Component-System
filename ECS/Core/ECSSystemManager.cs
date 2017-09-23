using System;
using System.Collections;
using System.Collections.Generic;
using ECS.Interfaces;
using UnityEngine;

namespace ECS.Core
{
    public class ECSSystemManager : IUpdateable
    {

        private Dictionary<string, ECSSystem> m_systems;

        private int m_systemsCreated = 0;

        public ECSSystemManager()
        {
            m_systems = new Dictionary<string, ECSSystem>();
        }

        private void AddSystem(ECSSystem system)
        {
            if (m_systems.ContainsKey(system.SystemID))
            {
                ECSDebug.LogError("Tried to add System [" + system.SystemID + "] but it already exists.");
                return;
            }

            m_systems.Add(system.SystemID, system);
        }

        private void RemoveSystem(ECSSystem system)
        {
            if (m_systems.ContainsKey(system.SystemID) == false)
            {
                ECSDebug.LogError("Tried to destroy System [" + system.SystemID + "] but it does not exist.");
                return;
            }

            m_systems.Remove(system.SystemID);
        }


        internal ECSSystem GetSystem(string id)
        {
            if (m_systems.ContainsKey(id) == false)
            {
                ECSDebug.LogError("Tried to get System [" + id + "] but it does not exist.");
                return null;
            }

            return m_systems[id];
        }

        internal ECSSystem GetSystem<T>() where T : ECSSystem
        {
            return GetSystem(GetSystemID<T>());
        }

        private string GetSystemID<T>() where T : ECSSystem
        {
            return typeof(T).ToString();
        }

        internal T CreateSystem<T>(ECSQueryManager queryManager) where T : ECSSystem
        {
            T system = Activator.CreateInstance<T>();
            system.QueryManager = queryManager;
            system.SystemID = GetSystemID<T>();
            system.QueryID = m_systemsCreated;

            m_systemsCreated++;
            AddSystem(system);
            return system;
        }


        internal void DestroySystem(ECSSystem system)
        {
            if(system == null)
            {
                return;
            }

            RemoveSystem(system);
            m_systemsCreated--;
            system.Destroy();
        }

        internal void DestroySystem<T>() where T : ECSSystem
        {
            DestroySystem(GetSystem<T>());
        }

        public void FixedUpdate()
        {
            foreach (var pair in m_systems)
            {
                pair.Value.FixedUpdate();
            }
        }
        public void LateUpdate()
        {
            foreach (var pair in m_systems)
            {
                pair.Value.LateUpdate();
            }
        }

        public void Update()
        {
            foreach (var pair in m_systems)
            {
                pair.Value.Update();
            }
        }


    }
}