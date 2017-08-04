using Simon.Action_List;
using System.Collections.Generic;

namespace Simon.ECS.Actions
{
    public class ECSAction : BaseAction
    {
        private List<ECSComponent> m_componentsAdded;
        protected Entity m_entity;
        

        public ECSAction(Entity entity)
        {
            m_entity = entity;
            m_componentsAdded = new List<ECSComponent>();
        }


        public override void OnEnd()
        {
            base.OnEnd();
            Reset();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            HasRanAlready = true;
        }


        protected void AddComponent(ECSComponent component)
        {
            m_entity.AddComponent(component);
            m_componentsAdded.Add(component);
        }

        protected void RemoveComponent(ECSComponent component)
        {
            m_entity.RemoveComponent(component);
            m_componentsAdded.Remove(component);
        }

        private void RemoveAddedComponents()
        {
            for (int i = m_componentsAdded.Count - 1; i >= 0; i--)
            {
                m_entity.RemoveComponent(m_componentsAdded[i]);
            }

            m_componentsAdded = new List<ECSComponent>();
        }

        protected void Reset()
        {
            RemoveAddedComponents();
        }

    }
}
