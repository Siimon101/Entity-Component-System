using UnityEngine;

namespace ECS
{
    public static class ECSDebug
    {

        private enum LogType
        {
            NONE,
            ERRORS,
            WARNINGS,
            ALL
        }

        private static LogType m_currentLogType = LogType.ALL;

        private const string DEBUG_PREFIX = "[ECSDEBUG] ";

        public static void LogError(string log)
        {
            if (m_currentLogType < LogType.ERRORS)
            {
                return;
            }

            Debug.LogError(DEBUG_PREFIX + log);
        }

        public static void LogWarning(string log)
        {
            if (m_currentLogType < LogType.WARNINGS)
            {
                return;
            }

            Debug.LogWarning(DEBUG_PREFIX + log);
        }

        
        public static void Log(string log)
        {
            if (m_currentLogType < LogType.ALL)
            {
                return;
            }

            Debug.Log(DEBUG_PREFIX + log);
        }


    }
}