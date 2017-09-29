namespace btcp.ECS.core
{
    public interface IECSEntityCreator
    {
        Entity CreateEntityFromArchetype(Entity e, string archetype);
    }
}