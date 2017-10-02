using System;
using System.Collections.Generic;
using btcp.ECS.core;
using btcp.ECS.examples.unity.common.components;
using btcp.ECS.interfaces;
using btcp.ECS.utils;
using btcp.src.utils;
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

                CTransform cTransform = VerifyTransform(cRenderer, entityID);

                if (cTransform == null)
                {
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
                CMeshCollider collider = component as CMeshCollider;

                CTransform cTransform = VerifyTransform(collider, entityID);

                CMeshRenderer cMeshRenderer = GetComponent<CMeshRenderer>(collider, entityID);

                if (cMeshRenderer == null)
                {
                    return 1;
                }

                if (cMeshRenderer.MeshFilter == null)
                {
                    OnComponentInitializeFailure(collider, "Cannot create mesh collider without mesh filter");
                    return 1;
                }

                if (collider.MeshCollider == null)
                {
                    collider.MeshCollider = cTransform.GameObject.GetComponent<MeshCollider>();

                    if (collider.MeshCollider == null)
                    {
                        collider.MeshCollider = cTransform.GameObject.AddComponent<MeshCollider>();
                    }

                    collider.MeshCollider.sharedMesh = null;
                    collider.MeshCollider.sharedMesh = cMeshRenderer.MeshFilter.mesh;

                    if (collider.IsConvex)
                    {
                        collider.MeshCollider.convex = true;
                    }
                }
            }

            if (component.GetType() == typeof(CMeshRenderer))
            {
                CMeshRenderer meshRenderer = component as CMeshRenderer;

                CTransform cTransform = VerifyTransform(meshRenderer, entityID);

                if (cTransform == null)
                {
                    return 1;
                }

                if (meshRenderer.MeshRenderer == null)
                {
                    meshRenderer.MeshRenderer = cTransform.GameObject.GetComponent<MeshRenderer>();

                    if (meshRenderer.MeshRenderer == null)
                    {
                        meshRenderer.MeshRenderer = cTransform.GameObject.AddComponent<MeshRenderer>();
                    }

                    if (meshRenderer.MaterialID != null && (meshRenderer.MeshRenderer.material == null || meshRenderer.MeshRenderer.material.name != meshRenderer.MaterialID))
                    {
                        meshRenderer.MeshRenderer.material = ResourceManager.GetInstance().Get<Material>(meshRenderer.MaterialID);
                    }
                }


                if (meshRenderer.MeshFilter == null)
                {
                    meshRenderer.MeshFilter = cTransform.GameObject.GetComponent<MeshFilter>();

                    if (meshRenderer.MeshFilter == null)
                    {
                        meshRenderer.MeshFilter = cTransform.GameObject.AddComponent<MeshFilter>();
                    }

                    if (meshRenderer.MeshID != null && (meshRenderer.MeshFilter.mesh == null || meshRenderer.MeshFilter.mesh.name != meshRenderer.MeshID))
                    {
                        Mesh mesh = ResourceManager.GetInstance().Get<Mesh>(meshRenderer.MeshID);
                        meshRenderer.MeshFilter.mesh = mesh;
                    }
                }
            }

            return 0;
        }

        private CTransform VerifyTransform(ECSComponent component, int entityID)
        {
            CTransform cTransform = GetComponent<CTransform>(component, entityID);

            if (cTransform == null)
            {
                return null;
            }

            if (cTransform.GameObject == null)
            {
                OnComponentInitializeFailure(component, "GameObject does not exist!");
                return null;
            }

            return cTransform;
        }

        private T GetComponent<T>(ECSComponent component, int entityID) where T : ECSComponent
        {
            if (m_componentManager.HasComponent<T>(entityID) == false)
            {
                OnComponentInitializeFailure(component, "Cannot initialize component " + component.GetType().Name + " without component " + typeof(T).Name);
                return null;
            }

            return m_componentManager.GetComponent<T>(entityID);
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