using System;
using System.Collections.Generic;
using ECS.Interfaces;

namespace ECS.Core
{
    public abstract class ECSSystem : IUpdateable, IQuerable
    {

        private string m_systemID;
        private int m_queryID;
        private ECSQueryManager m_queryManager;

        public string SystemID { get { return m_systemID; } set { m_systemID = value; } }
        public int QueryID { get { return m_queryID; } set { m_queryID = value; } }
        public ECSQueryManager QueryManager { set { m_queryManager = value; } }

        protected List<Entity> GetEntitiesWithComponents(params Type[] args)
        {
            return m_queryManager.GetEntitiesWithComponents(args);
        }

        protected T GetComponent<T>(Entity entity) where T : ECSComponent
        {
            return m_queryManager.GetComponent<T>(entity);
        }

        public virtual void OnEnable() { }
        public virtual void OnDisable() { }
        public virtual void FixedUpdate() { }
        public virtual void LateUpdate() { }
        public virtual void Update() { }

        public virtual void Initialize() { }
        public virtual void Shutdown() { }
    }
}