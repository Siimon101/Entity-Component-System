
namespace Simon.ECS
{
    public interface IECSUpdateable
    {
        int GetPriority();
        void Update();
        void LateUpdate();
        void FixedUpdate();
    }
}
