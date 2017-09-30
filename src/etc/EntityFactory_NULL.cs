using btcp.ECS.core;
using btcp.ECS.interfaces;
using btcp.ECS.utils;

namespace btcp.ECS.etc
{
    public class EntityFactory_NULL : IECSEntityFactory
    {

        public Entity CreateEntity(string archetype)
        {
            return CreateEntity();
        }

        public Entity CreateEntity()
        {
            ECSDebug.LogWarning("IECSEntityCreator was not provided.. Using NULLEntityCreator!");
            return new Entity();
        }

    }
}