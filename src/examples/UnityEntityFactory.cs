using System;
using System.Collections.Generic;
using btcp.ECS.core;
using btcp.ECS.etc;
using btcp.ECS.interfaces;
using btcp.ECS.utils;
using btcp.halloweenpumpkin.src.core.Components;
using btcp.halloweenpumpkin.src.utils;
using SimpleJSON;
using UnityEngine;

namespace btcp.ECS.examples
{
    public class EntityFactory_Unity : IECSEntityFactory
    {
        private ECSComponentManager m_componentManager;
        private ECSParserJSON m_parser;
        private IECSParserDataLocator m_dataLocator;

        public EntityFactory_Unity(string json, ECSComponentManager componentManager)
        {
            m_componentManager = componentManager;
            m_dataLocator = new ECSParserDataLocator_Default();
            m_parser = new ECSParserJSON(m_dataLocator);
            m_parser.Provide(json);
        }
        
        public Entity CreateEntity()
        {
            return new Entity();
        }

        public Entity CreateEntity(string archetype)
        {
            Entity e = CreateEntity();

            JSONNode archetypeData = m_parser.GetArchetypeData(archetype);

            string baseArchetype = m_dataLocator.GetBaseArchetype(archetypeData);

            if (baseArchetype != null)
            {
                archetypeData = m_parser.OverwriteBaseArchetypeData(m_parser.GetArchetypeData(baseArchetype).AsObject, archetypeData.AsObject);
            }

            Bag<ECSComponent> components = m_parser.ParseComponentData(archetypeData);
            InitializeComponents(e, components);

            return e;
        }

        private void InitializeComponents(Entity entity, Bag<ECSComponent> components)
        {
            List<ECSComponent> toInit = new List<ECSComponent>(components.GetAll());
            ECSComponent component = null;

            int attemptThreshold = 10;


            while (toInit.Count > 0 && attemptThreshold > 0)
            {
                for (int i = toInit.Count - 1; i >= 0; i--)
                {
                    component = toInit[i];

                    if (InitializeComponent(entity, component) == 0)
                    {
                        ECSDebug.Log("Initialized Componet " + component);
                        m_componentManager.AddComponent(entity, component);
                        toInit.RemoveAt(i);
                    }

                    attemptThreshold--;
                }
            }
        }


        private int InitializeComponent(Entity entity, ECSComponent component)
        {

            if (component.GetType() == typeof(CTransform))
            {
                CTransform cTransform = component as CTransform;
                cTransform.GameObject = new GameObject(cTransform.Name);
            }

            if (component.GetType() == typeof(CSpriteRenderer))
            {
                CSpriteRenderer cRenderer = component as CSpriteRenderer;

                if (m_componentManager.HasComponent<CTransform>(entity.EntityID) == false)
                {
                    OnComponentInitializeFailure(component, "Cannot create sprite renderer without CTransform component!");
                    return 1;
                }

                CTransform cTransform = m_componentManager.GetComponent<CTransform>(entity.EntityID);

                if (cTransform.GameObject == null)
                {
                    OnComponentInitializeFailure(component, "GameObject does not exist!");
                    return 1;
                }

                cRenderer.SpriteRenderer = cTransform.GameObject.GetComponentInChildren<SpriteRenderer>();

                if (cRenderer.SpriteRenderer == null)
                {
                    cRenderer.SpriteRenderer = cTransform.GameObject.AddComponent<SpriteRenderer>();
                }

            }

            return 0;
        }

        private void OnComponentInitializeFailure(ECSComponent component, string reason)
        {
            ECSDebug.LogError("Initializing Component " + component.GetType().Name + " failed. Reason: " + reason);
        }
    }
}