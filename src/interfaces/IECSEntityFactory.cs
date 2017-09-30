using btcp.ECS.core;

namespace btcp.ECS.interfaces
{
    public interface IECSEntityFactory
    {
        Entity CreateEntity();
        Entity CreateEntity(string archetype);
    }
}