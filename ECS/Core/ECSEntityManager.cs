using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ECS.Core
{
    public class ECSEntityManager
    {

        private Dictionary<int, Entity> m_entities;

        private int m_entitiesCreated;

        public ECSEntityManager()
        {
            m_entities = new Dictionary<int, Entity>();
            m_entitiesCreated = 0;
        }

        internal Entity CreateEntity()
        {
            Entity e = new Entity();
            e.QueryID = m_entitiesCreated;
            m_entitiesCreated++;
            AddEntity(e);
            return e;
        }

        internal void DestroyEntity(Entity e)
        {
            m_entities.Remove(e.QueryID);
            e.Destroy();
            m_entitiesCreated--;
        }

        internal Entity GetEntity(int id)
        {
            return m_entities[id];
        }

        private void AddEntity(Entity e)
        {
            m_entities.Add(e.QueryID, e);
        }

        private void RemoveEntity(Entity e)
        {
            m_entities.Remove(e.QueryID);
        }

        internal List<Entity> GetAll()
        {
            return new List<Entity>(m_entities.Values);
        }
    }
}