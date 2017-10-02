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
                    if (cTransform.PrefabID == null)
                    {
                        cTransform.GameObject = new GameObject(cTransform.Name);
                    }
                    else
                    {
                        GameObject prefab = Resources.Load<GameObject>(cTransform.PrefabID);

                        if (prefab == null)
                        {
                            OnComponentInitializeFailure(cTransform, "Prefab could not be found " + cTransform.PrefabID);
                            return 1;
                        }

                        cTransform.GameObject = GameObject.Instantiate(prefab);
                    }

                }
                cTransform.GameObject.layer = cTransform.LayerID;
            }

            if (component.GetType() == typeof(CSpriteRenderer))
            {
                CSpriteRenderer cRenderer = component as CSpriteRenderer;

                CTransform cTransform = VerifyTransform(cRenderer, entityID);

                if (cTransform == null)
                {
                    return 1;
                }

                cRenderer.SpriteRenderer = AddOrGetUnityComponent<SpriteRenderer>(cTransform);
            }

            if (component.GetType() == typeof(CRigidbody))
            {
                CRigidbody cRigidbody = component as CRigidbody;

                CTransform cTransform = VerifyTransform(cRigidbody, entityID);

                if (cTransform == null)
                {
                    return 1;
                }

                cRigidbody.RigidBody = AddOrGetUnityComponent<Rigidbody>(cTransform);
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

                if (collider.Collider == null)
                {
                    collider.Collider = AddOrGetUnityComponent<MeshCollider>(cTransform);

                    collider.Collider.sharedMesh = null;
                    collider.Collider.sharedMesh = cMeshRenderer.MeshFilter.mesh;

                    if (collider.IsConvex)
                    {
                        collider.Collider.convex = true;
                    }
                }
            }

            if (component.GetType() == typeof(CSphereCollider))
            {
                CSphereCollider cSphereCollider = component as CSphereCollider;

                CTransform cTransform = VerifyTransform(cSphereCollider, entityID);


                if (cTransform == null)
                {
                    return 1;
                }

                if (cSphereCollider.Collider == null)
                {
                    cSphereCollider.Collider = AddOrGetUnityComponent<SphereCollider>(cTransform);

                    if(cSphereCollider.PhysicsMaterialID != null && (cSphereCollider.Collider.material == null || cSphereCollider.Collider.material.name != cSphereCollider.PhysicsMaterialID))
                    {
                        cSphereCollider.Collider.material = ResourceManager.GetInstance().Get<PhysicMaterial>(cSphereCollider.PhysicsMaterialID);
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
                    meshRenderer.MeshRenderer = AddOrGetUnityComponent<MeshRenderer>(cTransform);

                    if (meshRenderer.MaterialID != null && (meshRenderer.MeshRenderer.material == null || meshRenderer.MeshRenderer.material.name != meshRenderer.MaterialID))
                    {
                        meshRenderer.MeshRenderer.material = ResourceManager.GetInstance().Get<Material>(meshRenderer.MaterialID);
                    }

                }


                if (meshRenderer.MeshFilter == null)
                {
                    meshRenderer.MeshFilter = AddOrGetUnityComponent<MeshFilter>(cTransform);

                    if (meshRenderer.MeshID != null && (meshRenderer.MeshFilter.mesh == null || meshRenderer.MeshFilter.mesh.name != meshRenderer.MeshID))
                    {
                        Mesh mesh = ResourceManager.GetInstance().Get<Mesh>(meshRenderer.MeshID);
                        meshRenderer.MeshFilter.mesh = mesh;
                    }
                }
            }

            return 0;
        }

        private T AddOrGetUnityComponent<T>(CTransform cTransform) where T : Component
        {
            T component = cTransform.GameObject.GetComponent<T>();

            if (component == null)
            {
                component = cTransform.GameObject.AddComponent<T>();
            }

            return component;
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
                DestroyUnityComponent<Transform>(entityID, component);
            }

            if (component.GetType() == typeof(CSpriteRenderer))
            {
                CSpriteRenderer cRenderer = component as CSpriteRenderer;
                DestroyUnityComponent<SpriteRenderer>(entityID, cRenderer);
            }


            if (component.GetType() == typeof(CMeshCollider))
            {
                CMeshCollider cMeshCollider = component as CMeshCollider;
                DestroyUnityComponent<MeshCollider>(entityID, cMeshCollider);
                GameObject.Destroy(cMeshCollider.Collider);
            }



            if (component.GetType() == typeof(CRigidbody))
            {
                CRigidbody cRigidbody = component as CRigidbody;
                DestroyUnityComponent<Rigidbody>(entityID, cRigidbody);
            }



            if (component.GetType() == typeof(CMeshRenderer))
            {
                CMeshRenderer cMeshRenderer = component as CMeshRenderer;
                DestroyUnityComponent<MeshRenderer>(entityID, cMeshRenderer);
                DestroyUnityComponent<MeshFilter>(entityID, cMeshRenderer);
            }


            return 0;
        }


        private int DestroyUnityComponent<T>(int entityID, ECSComponent component) where T : Component
        {
            CTransform cTransform = VerifyTransform(component, entityID);

            if (cTransform == null)
            {
                OnComponentDeInitializeFailure(component, "Transform not found.");
                return 1;
            }

            T unityComponent = cTransform.GameObject.GetComponent<T>();

            if (component == null)
            {
                OnComponentDeInitializeFailure(component, "Unity component not found on transform " + typeof(T));
                return 1;
            }

            GameObject.Destroy(unityComponent);
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