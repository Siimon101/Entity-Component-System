using btcp.ECS.interfaces;
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
            return componentData["ComponentName"];
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