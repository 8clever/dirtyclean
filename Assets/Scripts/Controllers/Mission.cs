using System.Collections.Generic;

public class Mission {

    public enum Association
    {
        DestroyMusor,
        CollectDvornik,
        CreateBuldozers,
        CreateGreenPeace,
        CreatePresident
    }

    public Mission (string name) {
        this.name = name;
    }
    public string name;

    public class Info {
        public int requiredCount;
        public int count = 0;
    }

    public Dictionary<Association, Info> mission = new Dictionary<Association, Info>();

    public Mission AddMission (Association a, int requiredCount) {
        var info = new Info();
        info.requiredCount = requiredCount;
        mission.Add(a, info);
        return this;
    }

    public bool IsComplete () {
        foreach (var m in mission) {
            var info = m.Value;
            if (info.count < info.requiredCount) 
                return false;
        }
        return true;
    }
}
