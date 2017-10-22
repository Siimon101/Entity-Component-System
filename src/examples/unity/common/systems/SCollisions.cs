using System;
using System.Collections.Generic;
using btcp.ECS.core;
using btcp.ECS.examples.unity.common.components;
using btcp.ECS.utils;
using UnityEngine;

namespace btcp.ECS.examples.unity.common.systems
{
    public class SCollisions : ECSSystem
    {
        private Bag<CollisionNotifier> m_collisioNotifiers;
        public SCollisions()
        {
            m_collisioNotifiers = new Bag<CollisionNotifier>();
        }

        public override void OnComponentAdded(int entityID, ECSComponent component)
        {
            UpdateCollisionNotifiers();
        }

        public override void OnComponentRemovedPre(int entityID, ECSComponent component)
        {
            if (component.GetType() == typeof(CCollider))
            {
                RemoveEntity(entityID);
            }
        }

        public override void OnEntityDestroyedPre(ECSEntity entity)
        {
            if (HasComponent<CCollider>(entity.EntityID))
            {
                RemoveEntity(entity.EntityID);
            }
        }

        private void AddEntity(int entityID)
        {
            CCollider cCollider = GetComponent<CCollider>(entityID);

            if(cCollider.Collider == null)
            {
                return;
            }
            
            CollisionNotifier notifier = cCollider.Collider.GetComponent<CollisionNotifier>();

            if (notifier == null)
            {
                notifier = cCollider.Collider.gameObject.AddComponent<CollisionNotifier>();
            }

            if (m_collisioNotifiers.Has(notifier) == -1)
            {
                m_collisioNotifiers.Set(entityID, notifier);
                notifier.OnCollisionBegin += OnCollisionBegin;
                notifier.OnCollisionEnd += OnCollisionEnd;
            }

        }

        private void RemoveEntity(int entityID)
        {
            CCollider cCollider = GetComponent<CCollider>(entityID);
            CollisionNotifier notifier = cCollider.Collider.GetComponent<CollisionNotifier>();

            if (m_collisioNotifiers.Has(notifier) > -1 - 1)
            {
                m_collisioNotifiers.Set(entityID, null);
                notifier.OnCollisionBegin -= OnCollisionBegin;
                notifier.OnCollisionEnd -= OnCollisionEnd;
                GameObject.Destroy(notifier);
            }
        }

        public void UpdateCollisionNotifiers()
        {
            int[] allColliders = GetEntitiesWithComponents(typeof(CCollider));

            foreach (int eID in allColliders)
            {
                AddEntity(eID);
            }
        }

        private int GetEntityByCollider(CollisionNotifier collisionNotifier)
        {
            return m_collisioNotifiers.Has(collisionNotifier);
        }

        private void OnCollisionBegin(CollisionNotifier collisionNotifier, Collider collider)
        {
            int entityID = m_collisioNotifiers.Has(collisionNotifier);
            AddCollision(entityID, collider);
        }

        private void AddCollision(int entityID, Collider collider)
        {
            CCollider cCollider = GetComponent<CCollider>(entityID);
            int collidedEntity = GetEntityByCollider(collider.GetComponent<CollisionNotifier>());

            if (cCollider.Collisions.Contains(collidedEntity) == false)
            {
                cCollider.Collisions.Add(collidedEntity);
            }
        }

        private void RemoveCollision(int entityID, Collider collider)
        {
            CCollider cCollider = GetComponent<CCollider>(entityID);
            int collidedEntity = GetEntityByCollider(collider.GetComponent<CollisionNotifier>());

            if (cCollider.Collisions.Contains(collidedEntity))
            {
                cCollider.Collisions.Remove(collidedEntity);
            }
        }

        private void OnCollisionEnd(CollisionNotifier collisionNotifier, Collider collider)
        {
            int entityID = m_collisioNotifiers.Has(collisionNotifier);
            RemoveCollision(entityID, collider);
        }

        internal override void Update()
        {
        }
    }
}