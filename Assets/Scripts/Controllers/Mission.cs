using System.Collections.Generic;

[System.Serializable]
public class Mission {

    public enum Type
    {
        Destroy,
        Collect,
        Create
    }

    public static readonly Dictionary<System.Type, string> NipNames = new Dictionary<System.Type, string>()
    {
        { typeof(Musorka), "Garbage" },
        { typeof(Dvornik), "Street Worker" },
        { typeof(Buldozer), "Buldozer" },
        { typeof(GreenPeace), "Green Peace" },
        { typeof(President), "President" }
    };
    public int requiredCount;

    public int count;

    public Type type;

    [UnityEngine.SerializeField]
    private string s_nip;
    public System.Type nip {
        get {
            return System.Type.GetType(s_nip);
        }
        set {
            s_nip = value.ToString();
        }
    }

    public bool IsComplete () {
        return count >= requiredCount;
    }
}


