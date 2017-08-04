using Simon.ECS.Manager;
using System.Collections.Generic;

namespace Simon.ECS
{
    public static class ECSManager
    {

        public static List<T> GetAll<T>() where T : IECSQuerable
        {
            return ECSContainerManager.GetAll<T>();
        }

        public static T Get<T>(int id) where T : IECSQuerable
        {
            return ECSContainerManager.Get<T>(id);
        }


        public static void Update()
        {
            ECSContainerManager.Update<ECSSystem>();
        }

        public static void LateUpdate()
        {
            ECSContainerManager.LateUpdate<ECSSystem>();
        }

        public static void FixedUpdate()
        {
            ECSContainerManager.FixedUpdate<ECSSystem>();
        }


    }


}