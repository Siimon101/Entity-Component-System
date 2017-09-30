using SimpleJSON;

namespace btcp.ECS.interfaces
{
    public interface IECSParserDataLocator
    {
        JSONNode GetEntityContainer(JSONNode mainData);
        string GetBaseArchetype(JSONNode archetypeData);
        JSONNode GetComponentData(JSONNode archetypeData);
        string GetComponentName(JSONNode componentData);
        void SetComponentData(JSONNode archetypeData, JSONNode replacedValues);
    }
}