using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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

            JSONNode mergedJson = null;

            if (HasBaseArchetype(archeTypeData))
            {
                string baseArchetypeID = archeTypeData["base_archetype"];
                JSONNode baseArchetypeData = m_jsonParsedData[baseArchetypeID];

                if (baseArchetypeData == null)
                {
                    ECSDebug.LogError("Base Archetype not found or not supported -> " + archeTypeData["base_archetype"].ToString());
                    return null;
                }

                mergedJson = MergeJSONData(archetypeID, baseArchetypeID);
            }

            archeTypeData["Components"] = mergedJson;

            CreateComponents(e, archeTypeData);

            return e;
        }

        private JSONNode MergeJSONData(string archetypeID, string baseID)
        {
            JSONNode baseComponents = JSON.Parse(m_jsonData)[baseID]["Components"];
            JSONNode newComponents = JSON.Parse(m_jsonData)[archetypeID]["Components"];

            JSONNode returnedComponentData = baseComponents;

            //Add missing components
            bool foundComponent = false;

            foreach (JSONObject componentA in newComponents.Children)
            {
                foundComponent = false;

                foreach (JSONObject componentB in baseComponents.Children)
                {
                    if (componentA["ComponentName"] == componentB["ComponentName"])
                    {
                        foundComponent = true;
                    }
                }

                if (foundComponent == false)
                {
                    returnedComponentData.Add(componentA);
                }

            }


            List<JSONObject> toAdd = new List<JSONObject>();

            bool hasValue = false;
            // Add/Overwrite missing values
            foreach (JSONObject componentA in newComponents.Children)
            {
                foreach (JSONObject componentB in baseComponents.Children)
                {
                    if (componentA["ComponentName"] == componentB["ComponentName"])
                    {
                        ArrayList valueKeysA = componentA.AsObject.GetKeys();
                        ArrayList valueKeysB = componentB.AsObject.GetKeys();

                        foreach (string valueA in valueKeysA)
                        {
                            hasValue = false;
                            foreach (string valueB in valueKeysB)
                            {
                                if (valueA == valueB)
                                {
                                    componentB.GetDictionary()[valueB] = componentA.GetDictionary()[valueA];
                                    hasValue = true;
                                }
                            }

                            if (hasValue == false)
                            {
                                componentB.AsObject.GetDictionary().Add(valueA, componentA.GetDictionary()[valueA]);
                            }
                        }
                    }
                }
            }

            return returnedComponentData;
        }

        private bool HasBaseArchetype(JSONNode archeTypeData)
        {
            return archeTypeData["base_archetype"] != null;
        }

        private void CreateComponents(Entity e, JSONNode archeTypeData)
        {
            foreach (var data in archeTypeData["Components"].Children)
            {
                string componentName = data["ComponentName"];

                ECSComponent component = null;

                foreach (System.Type type in m_cachedComponentReferences)
                {
                    if (type.Name == componentName)
                    {
                        component = JsonUtility.FromJson(data.ToString(), type) as ECSComponent;
                    }
                }

                if (component == null)
                {
                    ECSDebug.LogError("Tried to create component " + componentName + " but it failed, does it even exist?");
                    return;
                }

                m_ecsManager.InstanceManager.AddComponent(e, component);
            }

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