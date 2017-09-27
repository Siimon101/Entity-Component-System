using System;
using btcp.ECS.utils;

namespace btcp.ECS.core
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

        protected bool IsEntityValid(int entityID)
        {
            return m_queryManager.IsEntityValid(entityID);
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

        protected void Log(string v)
        {
            ECSDebug.Log("[" + GetType().Name.ToString() + "] " + v);
        }

        protected void LogWarning(string v)
        {
            ECSDebug.LogWarning("[" + GetType().Name.ToString() + "] " + v);
        }

        protected void LogError(string v)
        {
            ECSDebug.LogError("[" + GetType().Name.ToString() + "] " + v);
        }
    }
}