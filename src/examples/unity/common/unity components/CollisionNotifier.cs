using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionNotifier : MonoBehaviour
{
    public delegate void CollisionAction(CollisionNotifier notifier, Collider collider);
    public CollisionAction OnCollisionBegin;
    public CollisionAction OnCollisionEnd;

    void OnCollisionEnter(Collision collision)
    {
        if (OnCollisionBegin != null)
        {
            OnCollisionBegin(this, collision.collider);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (OnCollisionEnd != null)
        {
            OnCollisionEnd(this, collision.collider);
        }
    }


    void OnTriggerEnter(Collider collider)
    {
        if (OnCollisionBegin != null)
        {
            OnCollisionBegin(this, collider);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (OnCollisionEnd != null)
        {
            OnCollisionEnd(this, collider);
        }
    }

}
