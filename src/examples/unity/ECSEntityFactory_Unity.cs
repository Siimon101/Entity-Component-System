using System;
using System.Collections.Generic;
using btcp.ECS.core;
using btcp.ECS.etc;
using btcp.ECS.examples.unity.common.components;
using btcp.ECS.interfaces;
using btcp.ECS.utils;
using SimpleJSON;
using UnityEngine;

namespace btcp.ECS.examples.unity
{
    public class ECSEntityFactory_Unity : IECSEntityFactory
    {
        private ECSComponentManager m_componentManager;
        private ECSParserJSON m_parser;
        private IECSParserDataLocator m_dataLocator;

        public ECSEntityFactory_Unity()
        {
            m_dataLocator = new ECSParserDataLocator_Default();
        }

        public void Initialize(ECSComponentManager componentManager)
        {
            m_componentManager = componentManager;
        }

        public void ProvideJSON(string json)
        {
            m_parser = new ECSParserJSON(m_dataLocator);
            m_parser.Provide(json);
        }

        public ECSEntity CreateEntity()
        {
            return new ECSEntity();
        }

        public void DestroyEntity(ECSEntity entity)
        {
            m_componentManager.RemoveAllComponents(entity.EntityID);
        }

        public ECSEntity SetupEntity(ECSEntity e, string archetype)
        {
            ECSDebug.Assert(m_parser != null, "Cannot create Entity from Archetype > JSON not provided!");
            
            JSONNode archetypeData = m_parser.GetArchetypeData(archetype);

            string baseArchetype = m_dataLocator.GetBaseArchetype(archetypeData);

            if (baseArchetype != null)
            {
                archetypeData = m_parser.OverwriteBaseArchetypeData(m_parser.GetArchetypeData(baseArchetype).AsObject, archetypeData.AsObject);
            }

            Bag<ECSComponent> components = m_parser.ParseComponentData(archetypeData);
            ComponentPreProcessing(archetype, components);
            m_componentManager.AddComponents(e.EntityID, components);

            return e;
        }

        private void ComponentPreProcessing(string archetype, Bag<ECSComponent> components)
        {
            foreach (ECSComponent component in components)
            {
                if (component.GetType() == typeof(CTransform))
                {
                    CTransform cTransform = (component as CTransform);
                    if (cTransform.Name == null)
                    {
                        cTransform.Name = archetype;
                    }
                }
            }
        }
    }
}