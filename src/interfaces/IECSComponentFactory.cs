using btcp.ECS.core;

namespace btcp.ECS.interfaces
{
    public interface IECSComponentFactory
    {   
        void Initialize(ECSComponentManager componentManager);
        ECSComponent CreateComponent<T>();
        int InitializeComponent(int entityID, ECSComponent component);
        int DeInitializeComponent(int entityID, ECSComponent component);
    }
}