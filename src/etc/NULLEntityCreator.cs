using btcp.ECS.core;

namespace btcp.ECS.etc
{
    public class NULLEntityCreator : IECSEntityCreator
    {
        public Entity CreateEntityFromArchetype(Entity e, string archetype)
        {
            return e;
        }
    }
}