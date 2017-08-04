using System.Collections.Generic;

namespace Simon.ECS.FSM
{
    public class FSMEntity
    {

        private Entity m_entity;
        public Entity Entity { get { return m_entity; } }

        public static FSMState REMOVE_STATE;
        public static FSMState REMAIN_STATE;
        
        private FSMContainer<FSMState> m_stateContainer;
        private Stack<FSMState> m_stateStack;

        public FSMEntity(Entity entity)
        {
            m_entity = entity;
            m_stateStack = new Stack<FSMState>();
            m_stateContainer = new FSMContainer<FSMState>();

            REMOVE_STATE = CreateState("REMOVE_STATE");
            REMAIN_STATE = CreateState("REMAIN_STATE");
        }


        public void Update()
        {
            if (m_currentState != null)
            {
                m_currentState.UpdateState();
            }
        }


        public FSMState CreateState(string id)
        {
            FSMState newState = new FSMState(id, this);
            return newState;
        }

        public void AddState(FSMState state)
        {
            m_stateContainer.Add(state);
        }


        public void RemoveState(string id)
        {
            m_stateContainer.Remove(id);
        }

        public FSMState GetState(string id)
        {
            return m_stateContainer.Get(id);
        }


        private FSMState m_currentState;
        public void ChangeState(string newID)
        {
            FSMState newState = GetState(newID);

            if (GetState(newID) == null)
            {
                ECSDebug.Log("State not found in FSM");
                return;
            }

            ChangeState(newState);
        }

        public void ChangeState(FSMState newState)
        {
            if(newState.ID == REMAIN_STATE.ID)
            {
                return;
            }


            if (m_currentState != null)
            {
                if (m_currentState == newState)
                {
                    return;
                }

                m_currentState.ExitState();
                m_currentState = null;
            }

            if (newState == REMOVE_STATE)
            {
                m_stateStack.Pop();
                newState = m_stateStack.Peek();
            }
            else
            {
                m_stateStack.Push(newState);
            }

            m_currentState = newState;
            m_currentState.EnterState();
        }

        public FSMState GetCurrentState()
        {
            return m_currentState;
        }
    }
}