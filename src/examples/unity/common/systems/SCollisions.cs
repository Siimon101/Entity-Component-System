using System;
using System.Collections.Generic;
using btcp.ECS.core;
using btcp.ECS.examples.unity.common.components;
using btcp.ECS.utils;
using UnityEngine;

namespace btcp.halloweenpumpkin.src.core.Systems
{
    public class SCollisions : ECSSystem
    {

        internal override void Update()
        {
            int[] allColliders = GetEntitiesWithComponents(typeof(CCollider));
            CacheColliders(allColliders);


            int[] entities = GetEntitiesWithComponents(typeof(CBoxCollider));
            foreach (int eID in entities)
            {
                CBoxCollider cBoxCollider = GetComponent<CBoxCollider>(eID);
                BoxCollider collider = cBoxCollider.BoxCollider;

                List<int> collisions = new List<int>();

                Vector3 size = cBoxCollider.BoxCollider.transform.TransformVector(cBoxCollider.BoxCollider.size);
                size.z = Mathf.Abs(size.z);
                RaycastHit[] hits = Physics.BoxCastAll(cBoxCollider.BoxCollider.transform.TransformPoint(cBoxCollider.BoxCollider.transform.position), size / 2, Vector3.up);

                foreach (RaycastHit hit in hits)
                {
                    if (hit.collider == null)
                    {
                        continue;
                    }

                    if (hit.collider == collider)
                    {
                        continue;
                    }

                    int entityID = m_colliderBag.Has(hit.collider);
                    if (entityID > 0)
                        collisions.Add(entityID);
                }

                CCollider cCollider = GetComponent<CCollider>(eID);
                cCollider.Collisions = collisions;
            }
        }

        private Bag<Collider> m_colliderBag;

        private void CacheColliders(int[] entities)
        {
            m_colliderBag = new Bag<Collider>(entities.Length);

            foreach (int eID in entities)
            {
                CCollider collider = GetComponent<CCollider>(eID);
                m_colliderBag.Set(eID, collider.Collider);
            }
        }
    }
}