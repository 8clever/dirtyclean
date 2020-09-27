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

    public static readonly Dictionary<string, string> NipNames = new Dictionary<string, string>()
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

    public static readonly Dictionary<Type, string> TypeNames = new Dictionary<Type, string>()
    {
        { Type.Destroy, "Destroy" },
        { Type.Collect, "Collect" },
        { Type.Create, "Create" },
        { Type.Buy, "Buy" },
        { Type.NotCreateInSteps, "Not create for steps" },
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
            if (GameObject.Find(Nip)) {
                count = 0;
                return;
            };
            count += 1;
        }   
    }
}


