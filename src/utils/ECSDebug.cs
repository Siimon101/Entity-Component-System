using System;
using UnityEngine;

namespace btcp.ECS.utils
{
    public static class ECSDebug
    {

        public enum DebugLevel
        {
            None,
            Assertions,
            Errors,
            Warnings,
            All
        }


        private static DebugLevel m_debugLevel = DebugLevel.None;
        private const string DEBUG_PREFIX = "[ECSDebug] ";

        public static void Log(object v)
        {
            if (m_debugLevel < DebugLevel.All)
            {
                return;
            }

            v = DEBUG_PREFIX + v;
            Debug.Log(v);
        }

        public static void LogWarning(object v)
        {
            if (m_debugLevel < DebugLevel.Warnings)
            {
                return;
            }

            v = DEBUG_PREFIX + v;
            Debug.LogWarning(v);
        }

        public static void LogError(object v)
        {
            if (m_debugLevel < DebugLevel.Errors)
            {
                return;
            }

            v = DEBUG_PREFIX + v;
            Debug.LogError(v);
        }

        internal static void Assert(bool condition, object v)
        {
            if (m_debugLevel < DebugLevel.Assertions)
            {
                return;
            }

            v = DEBUG_PREFIX + v;
            Debug.Assert(condition, v);
        }

        internal static void LogForce(object v)
        {
            v = DEBUG_PREFIX + v;
            Debug.Log(v);
        }
    }
}