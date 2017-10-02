using btcp.ECS.core;

namespace btcp.ECS.interfaces
{
    public interface IECSEntityFactory
    {
        ECSEntity CreateEntity();
        ECSEntity SetupEntity(ECSEntity e, string archetype);
    }
}