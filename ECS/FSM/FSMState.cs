using System.Collections.Generic;
namespace Simon.ECS.FSM
{
    public sealed class FSMState : IFSMQuerable
    {

        private FSMEntity m_fsm;

        public Entity Entity { get { return m_fsm.Entity; } }

        private string m_stateID;
        public string ID { get { return m_stateID; } }
    

        public FSMState(string id, FSMEntity fsm)
        {
            m_stateID = id;
            m_fsm = fsm;
            Initialize();
        }

        private Dictionary<string, ECSComponent> m_componentList;
        private FSMContainer<FSMTransition> m_transitionContainer;
        private List<FSMAction> m_actionList;
        private void Initialize()
        {
            m_componentList = new Dictionary<string, ECSComponent>();
            m_transitionContainer = new FSMContainer<FSMTransition>();
            m_actionList = new List<FSMAction>();
        }


        public void CreateTransition(string id, FSMDecision decision, FSMState trueState, FSMState falseState)
        {
            FSMTransition newTransition = new FSMTransition(id, decision, trueState, falseState);
            m_transitionContainer.Add(newTransition);
        }

        public void AddAction(string id, FSMAction action)
        {
            m_actionList.Add(action);
        }

        public void RemoveTransition(string id)
        {
            m_transitionContainer.Remove(id);
        }

        public void UpdateState()
        {
            FSMState newState = null;

            foreach (FSMTransition transition in m_transitionContainer.GetAll())
            {
                if (transition.GetDecision().DoDecision(m_fsm))
                {
                    newState = transition.GetTrueState();
                }
                else
                {
                    newState = transition.GetFalseState();
                }

                m_fsm.ChangeState(newState);

                if(newState.ID != ID)
                {
                    break;
                }
            }
            
            foreach (FSMAction action in m_actionList)
            {
                action.DoAction(m_fsm);
            }
        }


        public void AddComponent(ECSComponent component)
        {
            string componentType = component.GetType().Name;

            if (m_componentList.ContainsKey(componentType))
            {
                ECSDebug.LogError("FSM already contains component of same type!");
                return;
            }

            m_componentList[componentType] = component;
        }

        public void RemoveComponent<T>()
        {
            string componentType = typeof(T).Name;

            if (m_componentList.ContainsKey(componentType) == false)
            {
                ECSDebug.Log("Component not found in State");
                return;
            }

            m_componentList.Remove(componentType);
        }

        public void EnterState()
        {
            UnityEngine.Debug.Log("Entered state " + m_stateID);
            foreach (KeyValuePair<string, ECSComponent> dict in m_componentList)
            {
                ECSComponent component = dict.Value;
                Entity.AddComponent(component);
            }
        }


        public void ExitState()
        {
            foreach (KeyValuePair<string, ECSComponent> dict in m_componentList)
            {
                ECSComponent component = dict.Value;
                Entity.RemoveComponent(component);
            }
        }


    }
}
