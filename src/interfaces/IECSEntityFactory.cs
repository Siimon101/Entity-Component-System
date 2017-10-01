using btcp.ECS.core;

namespace btcp.ECS.interfaces
{
    public interface IECSEntityFactory
    {
        ECSEntity CreateEntity();
        ECSEntity CreateEntity(string archetype);
    }
}