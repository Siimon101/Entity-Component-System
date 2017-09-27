using System;
using com.btcp.ECS.utils;

namespace com.btcp.ECS.core
{
    public class ECSEntityManager
    {

        private Bag<Entity> m_entityBag;


        public ECSEntityManager()
        {
            m_entityBag = new Bag<Entity>();
        }


        public Entity AddEntity(Entity entity)
        {
            m_entityBag.Add(entity);
            entity.EntityID = m_entityBag.GetSize();
            return entity;
        }

        public Entity CreateEntity()
        {
            Entity entity = new Entity();
            return AddEntity(entity);
        }

        public Entity GetEntity(int entityID)
        {
            return m_entityBag.Get(entityID);
        }

        public void RemoveEntity(Entity e)
        {
            m_entityBag.Set(e.EntityID, null);
        }

        public int[] GetEntityIdentifiers()
        {
            int[] identifiers = new int[m_entityBag.GetSize()];

            Entity e = null;
            for (int i = 0; i < identifiers.Length; i++)
            {
                e = m_entityBag.Get(i);

                if (e != null)
                {
                    identifiers[i] = e.EntityID;
                }
            }

            return identifiers;
        }
    }
}