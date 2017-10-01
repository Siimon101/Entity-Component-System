using System;
using System.Collections.Generic;
using btcp.ECS.core;
using btcp.ECS.etc;
using btcp.ECS.examples.unity.components;
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

        public ECSEntityFactory_Unity(string json, ECSComponentManager componentManager)
        {
            m_componentManager = componentManager;
            m_dataLocator = new ECSParserDataLocator_Default();
            m_parser = new ECSParserJSON(m_dataLocator);
            m_parser.Provide(json);
        }

        public ECSEntity CreateEntity()
        {
            return new ECSEntity();
        }

        public ECSEntity CreateEntity(string archetype)
        {
            ECSEntity e = CreateEntity();

            JSONNode archetypeData = m_parser.GetArchetypeData(archetype);

            string baseArchetype = m_dataLocator.GetBaseArchetype(archetypeData);

            if (baseArchetype != null)
            {
                archetypeData = m_parser.OverwriteBaseArchetypeData(m_parser.GetArchetypeData(baseArchetype).AsObject, archetypeData.AsObject);
            }

            Bag<ECSComponent> components = m_parser.ParseComponentData(archetypeData);
            m_componentManager.AddComponents(e.EntityID, components);

            return e;
        }


    }
}