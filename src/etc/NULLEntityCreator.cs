using btcp.ECS.core;
using btcp.ECS.utils;

namespace btcp.ECS.etc
{
    public class NULLEntityFactory : IECSEntityFactory
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