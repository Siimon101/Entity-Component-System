namespace btcp.ECS.utils
{
    public interface IDebuggable
    {
        bool IsDebugOn { get; set; }

        void Log(object v);
        void LogError(object v);
        void LogWarning(object v);
        void Assert(bool condition, object v);

    }
}