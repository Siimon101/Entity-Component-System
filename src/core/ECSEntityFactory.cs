using System;
using btcp.halloweenpumpkin.src.core.Components;
using btcp.halloweenpumpkin.src.utils;
using UnityEngine;

namespace btcp.ECS.core
{
    public class ECSEntityFactory
    {
        private ECSComponentManager m_componentManager;
        public ECSEntityFactory(ECSComponentManager componentManager)
        {
            m_componentManager = componentManager;
        }

        public int LoadArchetype(int entityID, string archetype)
        {

            if (archetype == "pumpkin_normal_001")
            {
                AddPumpkinComponents(entityID, archetype);
            }

            if (archetype == "pumpkin_rotten_001")
            {
                AddPumpkinComponents(entityID, archetype);
            }

            if (archetype == "pumpkin_special_001")
            {
                AddPumpkinComponents(entityID, archetype);
            }

            return entityID;
        }

        private void AddPumpkinComponents(int entityID, string archetype)
        {
            GameObject go = GameObject.Instantiate(ResourceManager.Get<GameObject>("Prefabs/" + archetype));
            m_componentManager.AddComponent(entityID, new CTransform(go));
            m_componentManager.AddComponent(entityID, new CSpriteRenderer(go.GetComponentInChildren<SpriteRenderer>()));
            m_componentManager.AddComponent(entityID, new CMovement());
            m_componentManager.AddComponent(entityID, new CConveyorItem());
        }

    }
}