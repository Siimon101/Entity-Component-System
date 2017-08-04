using Simon.Event_System;
using UnityEngine;

namespace Simon.ECS.Systems
{
    public class SEntityDestroy : ECSSystem
    {
        public SEntityDestroy()
        {
            EventDispatcher.s_Instance.RegisterObserver(this, GameEvents.EVENT_ENTITY_DESTROY);
        }

        public override void ReceiveEvent(GameEvent evt)
        {
            if (evt.GetEventType() == GameEvents.EVENT_ENTITY_DESTROY)
            {
                Entity entity = ECSEntityManager.GetEntity(evt.GetRecipient());
                entity.DestroyEntity();
                entity = null;
            }
        }

    }
}
