namespace Simon.ECS.FSM
{
    public class FSMTransition : IFSMQuerable
    {
        public FSMTransition(string id, FSMDecision decision, FSMState trueState, FSMState falseState)
        {
            m_transitionID = id;
            m_decision = decision;
            m_trueState = trueState;
            m_falseState = falseState;
        }


        private FSMDecision m_decision;
        public FSMDecision GetDecision()
        {
            return m_decision;
        }

        private FSMState m_trueState;
        public FSMState GetTrueState()
        {
            return m_trueState;
        }
        private FSMState m_falseState;
        public FSMState GetFalseState()
        {
            return m_falseState;
        }

        private string m_transitionID;
        public string ID { get { return m_transitionID; } }
        

    }
}