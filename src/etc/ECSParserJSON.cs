using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using btcp.ECS.core;
using btcp.ECS.interfaces;
using btcp.ECS.utils;
using SimpleJSON;
using UnityEngine;

namespace btcp.ECS.etc
{
    public class ECSParserJSON
    {
        private JSONNode m_parsedJSON;

        private Dictionary<string, Type> m_componentClasses;
        private Dictionary<Assembly, Type[]> m_assemblyTypes;

        private IECSParserDataLocator m_dataLocator;

        public ECSParserJSON(IECSParserDataLocator dataLocator)
        {
            m_dataLocator = dataLocator;
            CacheAssemblyTypes();
            m_componentClasses = new Dictionary<string, Type>();
        }

        private void CacheAssemblyTypes()
        {
            m_assemblyTypes = new Dictionary<Assembly, Type[]>();
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                m_assemblyTypes.Add(assembly, assembly.GetTypes());
            }
        }

        public void Provide(string json)
        {
            m_parsedJSON = JSONNode.Parse(json);
            ECSDebug.Assert(json != "" && json != null && m_parsedJSON != null, "JSON is not Valid");

            Bag<string> componentsChecked = GetAllComponentNames(m_parsedJSON);
            CacheComponentTypesFromJSON(componentsChecked.GetAll());
        }

        private Bag<string> GetAllComponentNames(JSONNode mainData)
        {
            Bag<string> componentsChecked = new Bag<string>();
            componentsChecked.IsDebugOn = false;

            string componentName = "";

            foreach (JSONNode entityData in m_dataLocator.GetEntityContainer(m_parsedJSON).Children)
            {
                foreach (JSONNode componentData in m_dataLocator.GetComponentData(entityData).AsArray)
                {
                    componentName = m_dataLocator.GetComponentName(componentData);

                    if (componentsChecked.Has(componentName) == -1)
                    {
                        componentsChecked.Add(componentName);
                    }
                }
            }

            componentsChecked.ResizeToFit();
            return componentsChecked;
        }

        private Dictionary<string, Type> CacheComponentTypesFromJSON(string[] componentNames)
        {
            Dictionary<string, Type> returnedTypes = new Dictionary<string, Type>();

            foreach (string componentName in componentNames)
            {
                foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    Type[] allTypes = m_assemblyTypes[assembly];

                    foreach (Type type in allTypes)
                    {
                        if (type.Name == componentName)
                        {
                            m_componentClasses.Add(componentName, type);
                            continue;
                        }
                    }
                }

                ECSDebug.Assert(m_componentClasses.ContainsKey(componentName), "Could not find Component " + componentName);
            }

            return returnedTypes;
        }

        public Bag<ECSComponent> ParseComponentData(JSONNode archetypeData)
        {
            Bag<ECSComponent> components = new Bag<ECSComponent>();
            JSONNode data = null;
            JSONNode componentDataArray = m_dataLocator.GetComponentData(archetypeData);
            Type componentType = null;

            for (int i = 0; i < componentDataArray.Count; i++)
            {
                data = componentDataArray[i];

                componentType = GetComponentTypeByName(m_dataLocator.GetComponentName(data));

                if (componentType == null)
                {
                    continue;
                }

                ECSComponent component = JsonUtility.FromJson(data.ToString(), componentType) as ECSComponent;
                ECSDebug.Assert(component != null, "Failed to create Component " + m_dataLocator.GetComponentName(data));

                components.Add(component);
            }

            components.ResizeToFit();

            return components;
        }

        private Type GetComponentTypeByName(string componentName)
        {
            if (m_componentClasses.ContainsKey(componentName) == false)
            {
                ECSDebug.LogError("Component Type not found " + componentName);
                return null;
            }

            return m_componentClasses[componentName];
        }

        public JSONNode OverwriteBaseArchetypeData(JSONNode baseArchetypeData, JSONNode newArchetypeData)
        {
            JSONNode originalData = JSONNode.Parse(newArchetypeData.ToString());

            baseArchetypeData = m_dataLocator.GetComponentData(baseArchetypeData);
            newArchetypeData = m_dataLocator.GetComponentData(newArchetypeData);

            JSONNode replacedValues = JSONObject.Parse(baseArchetypeData.ToString());
            List<string> valueKeys = new List<string>();

            bool hasComponent = false;

            foreach (JSONObject newComponent in newArchetypeData.Children)
            {
                hasComponent = false;
                foreach (JSONObject baseComponent in replacedValues.Children)
                {
                    if (m_dataLocator.GetComponentName(baseComponent) != m_dataLocator.GetComponentName(newComponent))
                    {
                        continue;
                    }

                    hasComponent = true;
                    valueKeys = newComponent.GetKeys();

                    foreach (var newValueKey in valueKeys)
                    {
                        baseComponent.Add(newValueKey, newComponent[newValueKey]);
                    }
                }

                if (hasComponent == false)
                {
                    replacedValues.Add(newComponent);
                }
            }

            m_dataLocator.SetComponentData(originalData, replacedValues);

            return originalData;
        }

        public JSONNode GetArchetypeData(string v)
        {
            JSONNode data = m_dataLocator.GetEntityContainer(m_parsedJSON)[v];

            ECSDebug.Assert(data != null, "Archetype not found " + v);

            return data;
        }

    }
}