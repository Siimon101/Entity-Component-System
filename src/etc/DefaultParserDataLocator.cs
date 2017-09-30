using SimpleJSON;

namespace btcp.ECS.etc
{
    public class DefaultParserDataLocator : IECSParserDataLocator
    {
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
    }
}