using System;
using btcp.ECS.core;
using btcp.ECS.interfaces;
using btcp.ECS.utils;

namespace btcp.ECS.etc
{
    public class ECSComponentFactory_NULL : IECSComponentFactory
    {
        public void Initialize(ECSComponentManager componentManager)
        {

        }

        public ECSComponent CreateComponent<T>()
        {
            LogWarning();
            return (Activator.CreateInstance<T>() as ECSComponent);
        }

        public int DeInitializeComponent(int entityID, ECSComponent component)
        {
            LogWarning();
            return 0;
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