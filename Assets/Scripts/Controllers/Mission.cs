using System.Collections.Generic;

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

    public Mission (System.Type nip, Type type, int requiredCount) {
        this.nip = nip;
        this.type = type;
        this.requiredCount = requiredCount;
    }
    public string name;

    public int requiredCount;

    public int count;

    public Type type;

    public System.Type nip;

    public bool IsComplete () {
        return count >= requiredCount;
    }
}


