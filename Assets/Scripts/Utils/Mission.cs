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
        NotCreateInSteps,
        Move,
        PlayTurns
    }
    private static readonly Dictionary<string, string> NipNames = new Dictionary<string, string>()
    {
        { "Musorka", "Dump" },
        { "Dvornik", "Janitor" },
        { "Buldozer", "Bulldozer" },
        { "GreenPeace", "Green Peace" },
        { "President", "President" },
        { "Valun", "Stone" },
        { "Svalka", "Trash can" },
        { "Police", "Police" },
        { "Chinese", "Chinese" },
        { "StreetDog", "Dog" }
    };
    public int requiredCount;
    public int count;
    public Type type;
    [UnityEngine.SerializeField]
    public string Nip;
    public string CustomMissionText = string.Empty;
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
        if (CustomMissionText != string.Empty) {
            return $"{CustomMissionText} {humanizeCount}";
        }
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
            case Type.Move:
                return $"Move {nipName} {humanizeCount}";
            case Type.PlayTurns:
                return $"Play turns {humanizeCount}";
            default:
                return $"Type '{type.ToString()}' is not assigned";
        }
    }
}


