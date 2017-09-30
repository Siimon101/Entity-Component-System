namespace btcp.ECS.core
{
    public interface IECSEntityFactory
    {
        Entity CreateEntity();
        Entity CreateEntity(string archetype);
    }
}