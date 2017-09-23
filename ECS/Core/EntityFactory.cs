using System;
using System.Collections.Generic;
using System.Reflection;
using ECS.Core;
using Game.Components.Common;
using SimpleJSON;
using UnityEngine;

namespace ECS.Core
{
    public class EntityFactory
    {

        private string m_jsonData;
        private JSONNode m_jsonParsedData;
        private ECSManager m_ecsManager;

        private System.Type[] m_cachedComponentReferences;

        public EntityFactory(ECSManager ecsManager, string json)
        {
            m_ecsManager = ecsManager;
            m_jsonData = json;

            m_cachedComponentReferences = GetTypesByName("ECSComponent");
            m_jsonParsedData = JSON.Parse(m_jsonData);
        }

        public Entity CreateEntity(string archetypeID)
        {
            Entity e = m_ecsManager.InstanceManager.CreateEntity();

            JSONNode archeTypeData = m_jsonParsedData[archetypeID];

            if (archeTypeData == null)
            {
                Debug.LogError("Archetype not found or not supported -> " + archetypeID);
                return null;
            }

            JSONNode data = null;
            for (int i = 0; i < archeTypeData["Components"].Count; i++)
            {
                ECSComponent component = null;

                data = archeTypeData["Components"][i];

                string componentName = data["component_name"];

                foreach (System.Type type in m_cachedComponentReferences)
                {
                    if (type.Name == componentName)
                    {
                        component = JsonUtility.FromJson(data.ToString(), type) as ECSComponent;
                    }
                }

                m_ecsManager.InstanceManager.AddComponent(e, component);
            }

            return e;
        }

        private static Type[] GetTypesByName(string className)
        {
            List<Type> returnVal = new List<Type>();

            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type[] assemblyTypes = a.GetTypes();
                for (int j = 0; j < assemblyTypes.Length; j++)
                {
                    if (assemblyTypes[j].Name == className)
                    {
                        returnVal.Add(assemblyTypes[j]);
                    }
                    else
                        if (assemblyTypes[j].BaseType != null && assemblyTypes[j].BaseType.Name == className)
                    {
                        returnVal.Add(assemblyTypes[j]);
                    }

                }
            }

            return returnVal.ToArray();
        }
    }

}