
namespace Simon.ECS
{
    public static class ECSDebug
    {
        public static DebugMode m_debugMode = DebugMode.Warnings;

        public enum DebugMode
        {
            Off,
            Any,
            Errors,
            Warnings
        }

        public static void Log(string s)
        {
            if (m_debugMode != DebugMode.Any)
            {
                return;
            }

            UnityEngine.Debug.Log(s);
        }

        public static void LogError(string s)
        {
            if (m_debugMode == DebugMode.Off)
            {
                return;
            }

            UnityEngine.Debug.LogError(s);
        }

        public static void LogWarning(string s)
        {
            if (m_debugMode == DebugMode.Warnings)
            {
                return;
            }

            UnityEngine.Debug.LogWarning(s);
        }

    }
}