using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Mission {
    public enum Type
    {
        Destroy,
        Collect,
        Create,
        Buy,
        NotCreateInSteps
    }
    private static readonly Dictionary<string, string> NipNames = new Dictionary<string, string>()
    {
        { "Musorka", "Garbage" },
        { "Dvornik", "Street Worker" },
        { "Buldozer", "Buldozer" },
        { "GreenPeace", "Green Peace" },
        { "President", "President" },
        { "Valun", "Stone" },
        { "Svalka", "Dump" },
        { "Police", "Police" },
        { "Chinese", "Chinese" },
    };
    public int requiredCount;
    public int count;
    public Type type;
    [UnityEngine.SerializeField]
    public string Nip;
    private bool Completed = false;
    public bool IsComplete () {
        if (Completed) return true;
        if (count < requiredCount) return false;
        Completed = true;
        return true;
    }
    public void NotCreateInSteps () {
        if (type == Type.NotCreateInSteps) {
            var nips = GameObject.FindGameObjectsWithTag("nip");
            foreach (var n in nips) {
                if (n.name.Contains(Nip)) {
                    count = 0;
                    return;
                }
            }
            count += 1;
        }   
    }
    public string GetMissionInfo ()
    {
        string nipName = "";
        string humanizeCount = (
            IsComplete() ?
            $"{requiredCount}" :
            $"{count}/{requiredCount}"
        );
        NipNames.TryGetValue(Nip, out nipName);
        switch (type) {
            case Type.Destroy:
                return $"Destroy {nipName} {humanizeCount}";
            case Type.Collect:
                return $"Collect {requiredCount} {nipName} together on map {humanizeCount}";
            case Type.Create:
                return $"Create {nipName} {humanizeCount}";
            case Type.Buy:
                return $"Buy item {nipName} in shop {humanizeCount}";
            case Type.NotCreateInSteps:
                return $"Do not create {nipName} {requiredCount} turns {humanizeCount}";
            default:
                return $"Type '{type.ToString()}' is not assigned";
        }
    }
}


