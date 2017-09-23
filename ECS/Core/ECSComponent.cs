using ECS.Interfaces;

namespace ECS.Core
{
    public abstract class ECSComponent : IQuerable
    {
        private int m_queryID;
        private float m_gitTest = 25;
        public int QueryID { get { return m_queryID; } set { m_queryID = value; } }

    }
}