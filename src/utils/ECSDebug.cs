using System;
using UnityEngine;

namespace btcp.ECS.utils
{
    public static class ECSDebug
    {

        public enum DebugLevel
        {
            None,
            Errors,
            Warnings,
            All
        }


        private static DebugLevel m_debugLevel = DebugLevel.All;
        private const string DEBUG_PREFIX = "[ECSDebug] ";

        public static void Log(string v)
        {
            if (m_debugLevel < DebugLevel.All)
            {
                return;
            }

            v = DEBUG_PREFIX + v;
            Debug.Log(v);
        }

        public static void LogWarning(string v)
        {
            if (m_debugLevel < DebugLevel.Warnings)
            {
                return;
            }

            v = DEBUG_PREFIX + v;
            Debug.LogWarning(v);
        }

        public static void LogError(string v)
        {
            if (m_debugLevel < DebugLevel.Errors)
            {
                return;
            }

            v = DEBUG_PREFIX + v;
            Debug.LogError(v);
        }

        internal static void Assert(bool condition, string v)
        {
            v = DEBUG_PREFIX + v;
            Debug.Assert(condition, v);
        }
    }
}