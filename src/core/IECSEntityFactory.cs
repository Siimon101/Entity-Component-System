namespace btcp.ECS.core
{
    public interface IECSEntityFactory
    {
        Entity CreateEntityFromArchetype(Entity e, string archetype);
    }
}