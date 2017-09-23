using System;
using ECS.Interfaces;

namespace ECS.Core
{
    public sealed class Entity : IQuerable
    {
        private int m_entityID;
        public int QueryID { get { return m_entityID; } set { m_entityID = value; } }

        internal void Destroy()
        {
            throw new NotImplementedException();
        }
    }
}