using System.Collections.Generic;
using Simon.Event_System;

namespace Simon.ECS
{
    public static class ECSEntityManager
    {

        public static List<Entity> GetAllEntities()
        {
            return ECSManager.GetAll<Entity>();
        }

        public static List<Entity> GetEntitiesWithComponents(params System.Type[] types)
        {
            List<Entity> returnedEntities = GetAllEntities();

            foreach (System.Type type in types)
            {
                returnedEntities = GetEntitiesWithComponent(returnedEntities, type);
            }

            return returnedEntities;
        }

        private static List<Entity> GetEntitiesWithComponent(List<Entity> entityList, System.Type componentType)
        {
            List<Entity> returnedEntities = new List<Entity>();

            foreach (Entity entity in entityList)
            {
                if (entity != null)
                {
                    if (entity.HasComponent(componentType))
                    {
                        returnedEntities.Add(entity);
                    }
                }
            }

            return returnedEntities;
        }

        public static Entity GetEntity(int id)
        {
            List<Entity> allEntities = GetAllEntities();
            Entity returnedEntity = null;

            foreach (Entity entity in allEntities)
            {
                if (entity != null)
                {
                    if (entity.GetID() == id)
                    {
                        returnedEntity = entity;
                        break;
                    }
                }
            }

            return returnedEntity;

        }


        public static Entity GetEntityByGameObject(UnityEngine.GameObject go)
        {
            List<Entity> allEntities = GetAllEntities();
            Entity returnedEntity = null;

            foreach (Entity entity in allEntities)
            {
                if (entity != null)
                {
                    if (entity.GameObject == go)
                    {
                        returnedEntity = entity;
                        break;
                    }
                }
            }

            return returnedEntity;
        }

        private static List<int> m_destroyedEntities = new List<int>();
        public static bool IsDestroyed(Entity entity)
        {
            return IsDestroyed(entity.GetID());
        }

        public static bool IsDestroyed(int entityID)
        {
            return (m_destroyedEntities.IndexOf(entityID) > -1);
        }

        public static void DestroyEntity(Entity entity, float delay = 0)
        {
            if (m_destroyedEntities.IndexOf(entity.GetID()) > -1)
            {
                return;
            }


            GameEvent evnt = new GameEvent(GameEventID.EVENT_ENTITY_DESTROY, entity.GetID(), 105);
            EventDispatcher.s_Instance.SendEvent(evnt, delay);
            m_destroyedEntities.Add(entity.GetID());
        }

    }
}