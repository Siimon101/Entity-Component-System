using System;

namespace com.btcp.ECS.core
{
    public class ECSSystem
    {

        private ECSQueryManager m_queryManager;
        internal void Provide(ECSQueryManager queryManager)
        {
            m_queryManager = queryManager;
        }

        protected int[] GetEntitiesWithComponents(params Type[] args)
        {
            return m_queryManager.GetEntitiesWithComponents(args);
        }

        protected T GetComponent<T>(int entityID) where T : ECSComponent
        {
            return m_queryManager.GetComponent<T>(entityID);
        }

        internal virtual void Update()
        {
        }

        internal virtual void LateUpdate()
        {
        }

        internal virtual void FixedUpdate()
        {
        }
    }
}