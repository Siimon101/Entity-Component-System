using btcp.ECS.interfaces;
using btcp.ECS.utils;
using SimpleJSON;

namespace btcp.ECS.examples
{
    public class ECSParserDataLocator_Default : IECSParserDataLocator
    {
        public string GetBaseArchetype(JSONNode archetypeData)
        {
            return archetypeData["BaseArchetype"];
        }

        public JSONNode GetComponentData(JSONNode entityNode)
        {
            return entityNode["Components"];
        }

        public string GetComponentName(JSONNode componentData)
        {
            string componentName = componentData["ComponentName"];
            ECSDebug.Assert(componentName != null, "Validate JSON file, might have misspelt 'ComponentName'");
            return componentName;
        }


        public JSONNode GetEntityContainer(JSONNode parentNode)
        {
            return parentNode["Entities"];
        }

        public void SetComponentData(JSONNode archetypeData, JSONNode replacedValues)
        {
            archetypeData["Components"] = replacedValues;
        }
    }
}