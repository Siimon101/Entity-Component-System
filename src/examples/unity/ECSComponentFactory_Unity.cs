using System;
using System.Collections.Generic;
using btcp.ECS.core;
using btcp.ECS.examples.unity.components;
using btcp.ECS.interfaces;
using btcp.ECS.utils;
using UnityEngine;

namespace btcp.ECS.examples.unity
{
    public class ECSComponentFactory_Unity : IECSComponentFactory
    {

        private ECSComponentManager m_componentManager;
        public ECSComponentFactory_Unity(ECSComponentManager componentManager)
        {
            m_componentManager = componentManager;
        }

        public ECSComponent CreateComponent<T>()
        {
            return Activator.CreateInstance<T>() as ECSComponent;
        }


        public int InitializeComponent(int entityID, ECSComponent component)
        {

            if (component.GetType() == typeof(CTransform))
            {
                CTransform cTransform = component as CTransform;

                if (cTransform.GameObject == null)
                {
                    cTransform.GameObject = new GameObject(cTransform.Name);
                }

            }

            if (component.GetType() == typeof(CSpriteRenderer))
            {
                CSpriteRenderer cRenderer = component as CSpriteRenderer;

                if (m_componentManager.HasComponent<CTransform>(entityID) == false)
                {
                    OnComponentInitializeFailure(component, "Cannot create sprite renderer without CTransform component!");
                    return 1;
                }


                CTransform cTransform = m_componentManager.GetComponent<CTransform>(entityID);

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

        public int DeInitializeComponent(int entityID, ECSComponent component)
        {


            if (component.GetType() == typeof(CTransform))
            {
                CTransform cTransform = component as CTransform;

                if (cTransform.GameObject)
                {
                    GameObject.Destroy(cTransform.GameObject);
                }
            }

            if (component.GetType() == typeof(CSpriteRenderer))
            {
                CSpriteRenderer cRenderer = component as CSpriteRenderer;

                if (m_componentManager.HasComponent<CTransform>(entityID) == false)
                {
                    OnComponentDeInitializeFailure(component, "Cannot remove sprite renderer without CTransform component!");
                    return 1;
                }

                CTransform cTransform = m_componentManager.GetComponent<CTransform>(entityID);

                if (cTransform.GameObject == null)
                {
                    OnComponentDeInitializeFailure(component, "Entity does not have GameObject!");
                    return 1;
                }

                if (cRenderer.SpriteRenderer == null)
                {
                    OnComponentDeInitializeFailure(component, "Sprite Renderer not found!");
                    return 1;
                }

                GameObject.Destroy(cRenderer.SpriteRenderer);
            }

            return 0;
        }

        private void OnComponentInitializeFailure(ECSComponent component, string reason)
        {
            ECSDebug.LogError("Initializing Component " + component.GetType().Name + " failed. Reason: " + reason);
        }

        private void OnComponentDeInitializeFailure(ECSComponent component, string reason)
        {
            ECSDebug.LogError("DeInitializing Component " + component.GetType().Name + " failed. Reason: " + reason);
        }
    }
}