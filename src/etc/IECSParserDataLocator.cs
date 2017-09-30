using SimpleJSON;

namespace btcp.ECS.etc
{
    public interface IECSParserDataLocator
    {
        JSONNode GetEntityContainer(JSONNode parentNode);
        JSONNode GetComponentData(JSONNode entityNode);
        string GetComponentName(JSONNode componentData);
    }
}