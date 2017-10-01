using System;
using btcp.ECS.core;
using btcp.ECS.interfaces;
using btcp.ECS.utils;

namespace btcp.ECS.etc
{
    public class ECSComponentFactory_NULL : IECSComponentFactory
    {

        public ECSComponent CreateComponent<T>()
        {
            LogWarning();
            return (Activator.CreateInstance<T>() as ECSComponent);
        }

        public int InitializeComponent(int entityID, ECSComponent component)
        {
            LogWarning();
            return 0;
        }

        private void LogWarning()
        {
            ECSDebug.LogWarning("ECSComponentFactory not provided.. using " + GetType().Name);
        }
    }
}