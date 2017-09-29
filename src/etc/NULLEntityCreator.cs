using btcp.ECS.core;
using btcp.ECS.utils;

namespace btcp.ECS.etc
{
    public class NULLEntityCreator : IECSEntityFactory
    {
        public Entity CreateEntityFromArchetype(Entity e, string archetype)
        {
            ECSDebug.LogWarning("IECSEntityCreator was not provided.. Using NULLEntityCreator!");
            return e;
        }
    }
}