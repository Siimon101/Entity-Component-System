using btcp.ECS.core;

namespace btcp.ECS.interfaces
{
    public interface IECSEntityFactory
    {
        void Initialize(ECSComponentManager componentManager);
        ECSEntity CreateEntity();
        ECSEntity SetupEntity(ECSEntity e, string archetype);
        void DestroyEntity(ECSEntity entity);
    }
}