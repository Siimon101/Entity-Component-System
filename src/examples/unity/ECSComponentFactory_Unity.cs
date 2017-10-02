using System;
using System.Collections.Generic;
using btcp.ECS.core;
using btcp.ECS.examples.unity.common.components;
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


        public virtual int InitializeComponent(int entityID, ECSComponent component)
        {

            if (component.GetType() == typeof(CTransform))
            {
                CTransform cTransform = component as CTransform;

                if (cTransform.GameObject == null)
                {
                    if (cTransform.PrefabPath == null)
                    {
                        cTransform.GameObject = new GameObject(cTransform.Name);
                    }
                    else
                    {
                        GameObject prefab = Resources.Load<GameObject>(cTransform.PrefabPath);

                        if (prefab == null)
                        {
                            OnComponentInitializeFailure(cTransform, "Prefab could not be found " + cTransform.PrefabPath);
                            return 1;
                        }

                        cTransform.GameObject = GameObject.Instantiate(prefab);
                    }
                }
            }

            if (component.GetType() == typeof(CSpriteRenderer))
            {
                CSpriteRenderer cRenderer = component as CSpriteRenderer;


                if (VerifyComponent<CTransform>(cRenderer, entityID) == 1)
                {
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


            if (component.GetType() == typeof(CMeshCollider))
            {
                CMeshCollider collider = m_componentManager.GetComponent<CMeshCollider>(entityID);

                if (VerifyComponent<CTransform>(collider, entityID) == 1)
                {
                    return 1;
                }


            }

            return 0;
        }

        private int VerifyComponent<T>(ECSComponent component, int entityID)
        {
            if (m_componentManager.HasComponent<T>(entityID) == false)
            {
                OnComponentInitializeFailure(component, "Cannot initialize component " + component.GetType().Name + " without component " + typeof(T).Name);
                return 1;
            }

            return 0;
        }

        public virtual int DeInitializeComponent(int entityID, ECSComponent component)
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

        protected void OnComponentInitializeFailure(ECSComponent component, string reason)
        {
            ECSDebug.LogError("Initializing Component " + component.GetType().Name + " failed. Reason: " + reason);
        }

        protected void OnComponentDeInitializeFailure(ECSComponent component, string reason)
        {
            ECSDebug.LogError("DeInitializing Component " + component.GetType().Name + " failed. Reason: " + reason);
        }
    }
}